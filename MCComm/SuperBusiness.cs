using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;

namespace MCComm
{
    ///// <summary>
    ///// 业务通用接口
    ///// </summary>
    //public interface ISuperBusiness
    //{
    //    string GetBusinessInfo();

    //    /// <summary>
    //    /// 拼装自动生成费用条件具体内容
    //    /// </summary>
    //    /// <param name="isMergeFee"></param>
    //    /// <returns></returns>
    //    List<FeeRateCondition> GetFeeRateCondition(ref bool isMergeFee);//1零件2设备3其他（金杯）



    //}

    /// <summary>
    /// 业务通用类
    /// </summary>
    public class SuperBusiness
    {



        /// <summary>
        /// 按类型自动创建业务实体
        /// </summary>
        /// <param name="businessSubTypeID"></param>
        /// <param name="businessID"></param>
        /// <returns></returns>
        //public static ISuperBusiness GetSuperNewBusiness(int businessSubTypeID)
        //{


        //    BLL.ISuperBusiness IBusiness = null;
        //    //int bussinessTypeID = BLL.BusinessSubType.GetMainTypeID(businessSubTypeID);
        //    //switch (bussinessTypeID)
        //    //{
        //    //    case 1:
        //    //    case 2:
        //    //    case 3:
        //    //        IBusiness = new BLL.Contract();
        //    //        break;
        //    //    case 4:
        //    //    case 5:
        //    //    case 6:
        //    //        IBusiness = new BLL.Ocean();
        //    //        break;
        //    //    case 7:
        //    //        IBusiness = new BLL.Road();
        //    //        break;
        //    //    case 8:
        //    //        IBusiness = new BLL.Railway();
        //    //        break;
        //    //    case 9:
        //    //    case 10:
        //    //    case 11:
        //    //    case 12:
        //    //        IBusiness = new BLL.StockInOut();
        //    //        break;
        //    //    case 13:
        //    //    case 14:
        //    //        IBusiness = new BLL.ContainerCasing();
        //    //        break;
        //    //    case 15:
        //    //        IBusiness = new BLL.GoodsReceiving();
        //    //        break;
        //    //    //case 16://货损货差
        //    //    //    IBusiness = BLL..TryFind(businessID);
        //    //    //    break;
        //    //    case 17://报关
        //    //    case 18:
        //    //        IBusiness = new BLL.CustomsInfo();
        //    //        break;
        //    //    case 19://报检
        //    //    case 20:
        //    //        IBusiness = new BLL.InspectionInfo();
        //    //        break;
        //    //    case 25://船代
        //    //        IBusiness = new BLL.ShipAgency();
        //    //        break;
        //    //    case 26://船代报价单
        //    //        IBusiness = new BLL.ShipAgencyOffer();
        //    //        break;
        //    //    case 27://船代预受理
        //    //        IBusiness = new BLL.ShipAgencyPrepare();
        //    //        break;
        //    //    case 28://库存调整
        //    //        IBusiness = new BLL.StockAdjust();
        //    //        break;
        //    //    case 30://车队作业
        //    //        IBusiness = new BLL.Road();
        //    //        break;
        //    //    case 33://空运出口
        //    //    case 34://空运进口
        //    //    case 35://空运国内
        //    //        IBusiness = new BLL.Air();
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //    return IBusiness;

        //}

        /// <summary>
        /// 通过实体类获取到插入语句
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetSqlInsertByObject(object info, string newTableName)
        {
            #region
            StringBuilder sbFileds = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            if (info != null)
            {
                PropertyInfo[] list = info.GetType().GetProperties();

                if (list.Length == 0)
                {
                    return string.Empty;
                }

                string tableName = newTableName;

                object[] objs = info.GetType().GetCustomAttributes(false);


                foreach (PropertyInfo pro in list)
                {
                    if (pro.GetCustomAttributes(false).Length == 0)
                    {
                        //判断如果不是DB字段，则不生成sql语句
                        continue;
                    }
                    if (pro.PropertyType == typeof(decimal))
                    {
                        sbFileds.Append(string.Format("{0},", pro.Name));
                        sbValues.Append(string.Format("{0},", pro.GetValue(info, null)));
                    }
                    else if (pro.PropertyType == typeof(bool))
                    {
                        sbFileds.Append(string.Format("{0},", pro.Name));
                        sbValues.Append(string.Format("{0},", pro.GetValue(info, null).ToString().ToBoolean() ? 1 : 0));
                    }
                    else if (pro.PropertyType == typeof(DateTime?))
                    {
                        if (pro.GetValue(info, null) != null && pro.GetValue(info, null).ToString().ToDateTimeNull().HasValue)
                        {
                            sbFileds.Append(string.Format("{0},", pro.Name));
                            sbValues.Append(string.Format("'{0}',", pro.GetValue(info, null).ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")));
                        }
                    }
                    else if (pro.PropertyType == typeof(string))
                    {
                        if (pro.GetValue(info, null) != null && pro.GetValue(info, null).ToString().IsNotEmpty())
                        {
                            sbFileds.Append(string.Format("{0},", pro.Name));
                            sbValues.Append(string.Format("'{0}',", pro.GetValue(info, null).ToString().Replace("'", "''")));
                        }
                    }
                    else
                    {
                        sbFileds.Append(string.Format("{0},", pro.Name));
                        sbValues.Append(string.Format("'{0}',", pro.GetValue(info, null)));
                    }

                }

                if (tableName.IsNotEmpty())
                {
                    if (newTableName.IsNotEmpty())
                    {
                        tableName = newTableName;
                    }
                    return string.Format("INSERT INTO {0} ({1}) VALUES  ({2})", tableName, sbFileds.ToString().TrimEnd(','), sbValues.ToString().TrimEnd(','));
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }

            #endregion
        }

        /// <summary>
        /// 通过实体类获取到插入语句
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetSqlInsertByObjectWithParam(object info, string newTableName, out Dictionary<string, object> valuePairs)
        {
            #region
            valuePairs = new Dictionary<string, object>();
            StringBuilder sbFileds = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            if (info != null)
            {
                PropertyInfo[] list = info.GetType().GetProperties();

                if (list.Length == 0)
                {
                    valuePairs = null;
                    return string.Empty;
                }

                string tableName = newTableName;

                object[] objs = info.GetType().GetCustomAttributes(false);


                foreach (PropertyInfo pro in list)
                {
                    if (pro.GetCustomAttributes(false).Length == 0)
                    {
                        //判断如果不是DB字段，则不生成sql语句
                        continue;
                    }


                    object sqlv = null;

                    if (pro.PropertyType == typeof(decimal))
                    {
                        sbFileds.Append(string.Format("{0},", pro.Name));
                        sbValues.Append(string.Format("@{0},", pro.Name));
                        sqlv = pro.GetValue(info, null);
                    }
                    else if (pro.PropertyType == typeof(bool))
                    {
                        sbFileds.Append(string.Format("{0},", pro.Name));
                        sbValues.Append(string.Format("@{0},", pro.Name));
                        sqlv = pro.GetValue(info, null).ToString().ToBoolean() ? 1 : 0;
                    }
                    else if (pro.PropertyType == typeof(DateTime?))
                    {
                        if (pro.GetValue(info, null) != null && pro.GetValue(info, null).ToString().ToDateTimeNull().HasValue)
                        {
                            sbFileds.Append(string.Format("{0},", pro.Name));
                            sbValues.Append(string.Format("@{0},", pro.Name));
                            sqlv = pro.GetValue(info, null).ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    else if (pro.PropertyType == typeof(string))
                    {
                        if (pro.GetValue(info, null) != null && pro.GetValue(info, null).ToString().IsNotEmpty())
                        {
                            sbFileds.Append(string.Format("{0},", pro.Name));
                            sbValues.Append(string.Format("@{0},", pro.Name));
                            sqlv = pro.GetValue(info, null).ToString().Replace("'", "''");
                        }
                    }
                    else
                    {
                        sbFileds.Append(string.Format("{0},", pro.Name));
                        sbValues.Append(string.Format("@{0},", pro.Name));
                        sqlv = pro.GetValue(info, null);
                    }

                    if (sqlv != null)
                        valuePairs.Add(pro.Name, sqlv);

                }

                if (tableName.IsNotEmpty())
                {
                    if (newTableName.IsNotEmpty())
                    {
                        tableName = newTableName;
                    }
                    return string.Format("INSERT INTO {0} ({1}) VALUES  ({2})", tableName, sbFileds.ToString().TrimEnd(','), sbValues.ToString().TrimEnd(','));
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }

            #endregion
        }


        /// <summary>
        /// 通过AR实体类获取Update语句
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="fieldsList">逗号分隔的字段列表，需要更新的字段列表，区分大小写</param>
        /// <returns></returns>
        public static string GetSqlUpdateByObject(object info, string newTableName, string PrimaryKey, string PrimaryKeyVal)
        {
            #region
            StringBuilder sbSet = new StringBuilder();

            if (info != null)
            {
                object[] list = info.GetType().GetCustomAttributes(false);

                if (list.Length > 0)
                {
                    string tableName = newTableName;
                    string sqlWhere = string.Empty;//where条件
                    string primaryKey = string.Empty;
                    sqlWhere = string.Format("{0} = '{1}'", PrimaryKey, PrimaryKeyVal);
                    foreach (PropertyInfo pro in list)
                    {

                        if (pro.PropertyType == typeof(decimal))
                        {
                            sbSet.Append(string.Format("{0} = {1},", pro.Name, pro.GetValue(info, null)));
                        }
                        else if (pro.PropertyType == typeof(bool))
                        {
                            sbSet.Append(string.Format("{0} = {1},", pro.Name, pro.GetValue(info, null).ToString().ToBoolean() ? 1 : 0));
                        }
                        else if (pro.PropertyType == typeof(DateTime?))
                        {
                            if (pro.GetValue(info, null) != null && pro.GetValue(info, null).ToString().ToDateTimeNull().HasValue)
                            {
                                sbSet.Append(string.Format("{0} = '{1}',", pro.Name, pro.GetValue(info, null).ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")));
                            }
                        }
                        else
                        {
                            if (pro.GetValue(info, null) != null)
                            {
                                sbSet.Append(string.Format("{0} = '{1}',", pro.Name, pro.GetValue(info, null).ToString().Replace("'", "''")));
                            }

                        }

                    }

                    if (tableName.IsNotEmpty() && sqlWhere.IsNotEmpty())
                    {
                        return string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, sbSet.ToString().TrimEnd(','), sqlWhere);
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
            #endregion
        }

        /// <summary>
        /// 通过AR实体类获取Update语句
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="fieldsList">逗号分隔的字段列表，需要更新的字段列表，区分大小写</param>
        /// <returns></returns>
        public static string GetSqlUpdateByObjectWithParam(object info, string newTableName, string PrimaryKey, out Dictionary<string, object> valueParam)
        {
            #region
            StringBuilder sbSet = new StringBuilder();
            valueParam = new Dictionary<string, object>();
            if (info != null)
            {
                PropertyInfo[] list = info.GetType().GetProperties();

                if (list.Length == 0)
                {
                    return string.Empty;
                }

                object[] objs = info.GetType().GetCustomAttributes(false);



                string tableName = newTableName;
                string sqlWhere = string.Empty;//where条件
                string primaryKey = string.Empty;
                sqlWhere = string.Format("{0} = @{0}", PrimaryKey);
                foreach (PropertyInfo pro in list)
                {
                    if (pro.GetCustomAttributes(false).Length == 0)
                    {
                        //判断如果不是DB字段，则不生成sql语句
                        continue;
                    }

                    object sqlv = null;
                    if (pro.PropertyType == typeof(decimal))
                    {
                        sbSet.Append(string.Format("{0} = @{0},", pro.Name));
                        sqlv = pro.GetValue(info, null);
                    }
                    else if (pro.PropertyType == typeof(bool))
                    {
                        sbSet.Append(string.Format("{0} = @{0},", pro.Name));
                        sqlv = pro.GetValue(info, null).ToString().ToBoolean() ? 1 : 0;
                    }
                    else if (pro.PropertyType == typeof(DateTime?))
                    {
                        if (pro.GetValue(info, null) != null && pro.GetValue(info, null).ToString().ToDateTimeNull().HasValue)
                        {
                            sbSet.Append(string.Format("{0} = @{0},", pro.Name));
                            sqlv = pro.GetValue(info, null).ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    else
                    {
                        if (pro.GetValue(info, null) != null&& pro.GetValue(info, null).ToString()!= "00000000-0000-0000-0000-000000000000")
                        {
                            sbSet.Append(string.Format("{0} = @{0},", pro.Name));
                            sqlv = pro.GetValue(info, null).ToString().Replace("'", "''");
                        }
                    }
                    if (sqlv != null)
                        valueParam.Add(pro.Name, sqlv);

                }

                if (tableName.IsNotEmpty() && sqlWhere.IsNotEmpty())
                {
                    return string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, sbSet.ToString().TrimEnd(','), sqlWhere);
                }
                else
                {
                    return string.Empty;
                }

            }
            else
            {
                return string.Empty;
            }

            #endregion
        }

        /// <summary>
        /// 通过AR实体类获取Update语句
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="fieldsList">逗号分隔的字段列表，需要更新的字段列表，区分大小写</param>
        /// <returns></returns>
        public static string GetSqlDeleteByTableNameAndPrimaryKey(string tablName, string PrimaryKeyName, string PrimaryKeyValue)
        {
            return string.Format("DELETE FROM {0} WHERE {1}='{2}'", tablName, PrimaryKeyName, PrimaryKeyValue);
        }




        /// <summary>
        /// 将List转化为带有指定分隔符的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="separator">默认逗号 ，</param>
        /// <returns></returns>
        public static string ListConvertSeparatorString<T>(List<T> list, char separator = ',')
        {
            string str = "";
            if (list == null || list.Count == 0)
                return "";
            try
            {
                foreach (T item in list)
                {
                    if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
                    {
                        str += item.ToString() + separator;
                    }
                    else
                    {
                        str += "'" + item + "'" + separator;
                    }
                }
                str = str.TrimEnd(separator);
                return str;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 时间计算
        /// </summary>
        /// <param name="dateBegin">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>返回(秒)单位，比如: 0.00239秒</returns>
        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            return ts3.TotalMilliseconds.ToString();
        }
    }
}
