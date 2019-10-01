using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MC.Comm
{
    /// <summary>
    /// 转换辅助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConvertHelper<T> where T : new()
    {
        /// <summary>
        /// DataTable-->List
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<T> DataTableToList(DataTable dt)
        {
            List<T> ts = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
                return ts;
            return RowsToList(dt.Select());
        }

        /// <summary>
        /// DataRow[]-->List
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static List<T> RowsToList(DataRow[] rows)
        {
            List<T> ts = new List<T>();
            if (rows == null || rows.Length == 0)
            {
                return ts;
            }

            // 取得泛型的类型
            Type type = typeof(T);

            // 反射取得类型实例的属性数组
            PropertyInfo[] propertys = type.GetProperties();

            // 反射取得类型实例的字段数组
            FieldInfo[] fields = type.GetFields();

            foreach (DataRow dr in rows)
            {

                // 创建类型的对象（用于赋值用）
                T outputObj = new T();

                // 遍历字段（公共字段）
                foreach (FieldInfo fi in fields)
                {
                    // 如果DataTable的数据列中包含有对应的字段
                    if (dr.Table.Columns.Contains(fi.Name))
                    {
                        // 取得字段的值
                        object value = dr[fi.Name];

                        if (value != DBNull.Value)
                        {
                            // 将对应字段的值赋给创建的类型实例的对应的字段
                            fi.SetValue(outputObj, value);
                        }
                    }
                }

                // 遍历属性
                foreach (PropertyInfo pi in propertys)
                {
                    // 如果DataTable的数据列中包含有对应的属性
                    if (dr.Table.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }

                        // 取得属性的值
                        object value = dr[pi.Name];

                        if (value != DBNull.Value)
                        {
                            // 将对应属性的值赋给创建的类型实例的对应的属性
                            pi.SetValue(outputObj, value, null);
                        }
                    }
                }

                // 添加到List中
                ts.Add(outputObj);
            }

            return ts;
        }

        /// <summary>
        /// Hashtable-->T
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static T ConvertToModel(Hashtable ht)
        {
            T m = new T();
            if (ht == null)
                return m;
            Type type = typeof(T);
            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                if (ht.ContainsKey(pi.Name))
                {
                    if (!pi.CanWrite)
                    {
                        continue;
                    }
                    object value = ht[pi.Name];
                    if (value != null)
                    {
                        // 将对应属性的值赋给创建的类型实例的对应的属性
                        pi.SetValue(m, value, null);
                    }
                }
            }
            return m;
        }

        /// <summary>
        /// T-->DataRow
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static DataRow ModelToRow(T entity)
        {
            if (entity == null)
                return null;
            Type entityType = entity.GetType();
            //  PropertyInfo[] entityProperties = entityType.GetProperties();
            //排除未映射字段
            PropertyInfo[] entityProperties = entityType.GetProperties().Where(pi => !Attribute.IsDefined(pi, typeof(NotMappedAttribute))).ToArray();// entityType.GetProperties();

            DataTable dt = new DataTable("dt");
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
            }
            object[] entityValues = new object[entityProperties.Length];
            for (int i = 0; i < entityProperties.Length; i++)
            {
                entityValues[i] = entityProperties[i].GetValue(entity, null);
            }
            dt.Rows.Add(entityValues);
            return dt.Rows[0];
        }
        /// <summary>
        /// List--> Table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ListToTable(List<T> list)
        {
            if (list == null)
                return null;
            Type entityType = typeof(T);
            PropertyInfo[] entityProperties = entityType.GetProperties();
            DataTable dt = new DataTable(entityType.Name);
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
            }
            foreach (T entity in list)
            {
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        /// <summary>
        /// T-->Dictionary
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ModelToDic(T entity)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (entity == null) return dic;
            Type entityType = entity.GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dic.Add(entityProperties[i].Name, entityProperties[i].GetValue(entity, null));
            }
            return dic;
        }

        /// <summary>
        /// Row--> T
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T RowToModel(DataRow row)
        {
            if (row == null) return default(T);
            T item = new T();
            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                if (row.Table.Columns.Contains(pi.Name))
                {
                    if (!pi.CanWrite)
                    {
                        continue;
                    }
                    // 取得属性的值
                    object value = row[pi.Name];

                    if (value != DBNull.Value)
                    {
                        // 将对应属性的值赋给创建的类型实例的对应的属性
                        pi.SetValue(item, value, null);
                    }
                }
            }
            return item;
        }
        /// <summary>
        /// Rows--> T
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static T RowsToModel(DataRow[] rows)
        {
            if (rows == null || rows.Length==0) return default(T);
            T item = RowToModel(rows[0]);
            return item;
        }

        /// <summary>
        /// 修改缓存某一行数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="row">需修改的行</param>
        /// <param name="item">最新数据</param>
        /// <param name="Fields">仅要修改的列名(多个|分割)</param>
        public static void ModifyModel(DataRow row, T item, String Fields = "")
        {
            PropertyInfo[] pi = typeof(T).GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                //验证是否存在不删除的字段
                if (!String.IsNullOrEmpty(Fields) && !("|" + Fields + "|").Contains("|" + pi[i].Name + "|"))
                {
                    continue;
                }
                if (row.Table.Columns.Contains(pi[i].Name))
                {
                    if (row[pi[i].Name] != pi[i].GetValue(item, null))
                    {
                        row[pi[i].Name] = pi[i].GetValue(item, null);
                    }
                }
            }
        }



    }
}