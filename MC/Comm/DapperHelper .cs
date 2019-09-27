using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Web;

namespace MC.Comm
{
    public enum Providers
    {
        SqlClient,
        SQLite
    }
    /// <summary>
    /// Dapper帮助类
    /// </summary>
    public class DapperHelper : IDisposable
    {
        public IDbConnection Connection { get; }

        public DapperHelper(IDbConnection connection)
        {
            Connection = connection;
        }

        public DapperHelper(string connectionString, Providers providers = Providers.SQLite)
        {
            switch (providers)
            {
                case Providers.SqlClient:
                    Connection = new SqlConnection(connectionString);
                    break;
                case Providers.SQLite:
                    Connection = new SQLiteConnection(connectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(providers), providers, null);
            }
        }
        public void Dispose() => Connection.Dispose();

        #region 通用方法
        public IDbTransaction BeginTransaction() => Connection.BeginTransaction();
        public IDbTransaction BeginTransaction(IsolationLevel il) => Connection.BeginTransaction(il);
        public void ChangeDatabase(string databaseName) => Connection.ChangeDatabase(databaseName);
        public void Close() => Connection.Close();
        public void Open() => Connection.Open();
        public IDbCommand CreateCommand() => Connection.CreateCommand();
        /// <summary>
        /// 执行查询，返回影响记录行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param = null) => Connection.Execute(sql, param);
        /// <summary>
        /// 执行查询，返回IDataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, object param = null) => Connection.ExecuteReader(sql, param);
        /// <summary>
        /// 执行查询，返回单个结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, object param = null) => Connection.ExecuteScalar(sql, param);
        public SqlMapper.GridReader QueryMultiple(string sql, object param = null) => Connection.QueryMultiple(sql, param);
        /// <summary>
        /// 执行查询，返回一个可遍历的匿名对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string sql, object param = null) => Connection.Query(sql, param);
        /// <summary>
        /// 执行查询并将第一个结果映射到匿名对象；
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns>无结果抛出异常；一个结果返回一个；多个结果返回首个</returns>
        public dynamic QueryFirst(string sql, object param = null) => Connection.QueryFirst(sql, param);
        /// <summary>
        /// 执行查询并返回结果，若结果为空，则返回默认值，若返回多个结果，则将首个结果映射给匿名对象或强类型对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic QueryFirstOrDefault(string sql, object param = null) => Connection.QueryFirstOrDefault(sql, param);
        /// <summary>
        /// 执行查询并返回结果，若结果为空或者返回多个结果，将抛出异常，反之映射给匿名对象或强类型对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic QuerySingle(string sql, object param = null) => Connection.QuerySingle(sql, param);
        /// <summary>
        /// 执行查询并返回结果，若结果为空或者返回多个结果，将抛出异常，反之映射给匿名对象或强类型对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic QuerySingleOrDefault(string sql, object param = null) => Connection.QuerySingleOrDefault(sql, param);
        /// <summary>
        ///  执行查询，返回一个可遍历的匿名对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null) => Connection.Query<T>(sql, param);
        /// <summary>
        /// 执行查询并将第一个结果映射到强类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns>无结果抛出异常；一个结果返回一个；多个结果返回首个</returns>
        public T QueryFirst<T>(string sql, object param = null) => Connection.QueryFirst<T>(sql, param);
        /// <summary>
        ///  执行查询，返回一个列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<T> QueryList<T>(string sql, object param = null) => Connection.Query<T>(sql, param) as List<T>;
        /// <summary>
        /// 执行查询并返回结果，若结果为空，则返回默认值，若返回多个结果，则将首个结果映射给匿名对象或强类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, object param = null) => Connection.QueryFirstOrDefault<T>(sql, param);
        /// <summary>
        /// 执行查询并返回结果，若结果为空或者返回多个结果，将抛出异常，反之映射给匿名对象或强类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, object param = null) => Connection.QuerySingle<T>(sql, param);
        /// <summary>
        /// 执行查询并返回结果，若结果为空或者返回多个结果，将抛出异常，反之映射给匿名对象或强类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingleOrDefault<T>(string sql, object param = null) => Connection.QuerySingleOrDefault<T>(sql, param);
        /// <summary>
        /// 更新指定列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dicData"></param>
        /// <param name="colCond"></param>
        /// <param name="varCond"></param>
        /// <returns></returns>
        public int Update<T>(Dictionary<string, object> dicData, string colCond, object varCond)
        {
            string sql = $"update {ClassName<T>()} set {ParamJoint(",", dicData.Keys.ToList<string>())} where {ParamJoint(" and ", new List<string> { colCond })}";
            dicData[colCond] = varCond;
            DynamicParameters parms = new DynamicParameters();
            foreach (KeyValuePair<string, object> kvp in dicData)
            {
                parms.Add(kvp.Key, kvp.Value);
            }
            return this.Execute(sql, parms);
        }


        #endregion

        #region 参数拼接
        /// <summary>
        /// 获取类名字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ClassName<T>() => typeof(T).ToString().Split('.').Last();

        /// <summary>
        /// 属性名称拼接并附加连接符
        /// </summary>
        /// <param name="separator">分隔符</param>
        /// <param name="param">要拼接的动态类型</param>
        /// <param name="ignore">忽略属性名称</param>
        /// <param name="isParam">是否为参数,即是否增加前缀'@'</param>
        /// <returns></returns>
        public static string Joint(string separator, object param, string ignore = null, bool isParam = false)
        {
            var prefix = isParam ? "@" : string.Empty;
            var joint = new StringBuilder();
            var propertys = param.GetType().GetProperties().Where(e => string.IsNullOrEmpty(ignore) ? true: e.Name != ignore).Select(t => $"{prefix}{t.Name}").ToArray();
            for (var i = 0; i < propertys.Length; i++)
            {
                joint.Append(i != 0 ? $"{separator}{propertys[i]}" : propertys[i]);
            }
            return joint.ToString();
        }

        /// <summary>
        /// 以"param=@param"格式拼接属性名称并附加连接符
        /// </summary>
        /// <param name="separator">分隔符</param>
        /// <param name="param">要拼接的动态类型</param>
        /// <returns></returns>
        public static string ParamJoint(string separator, object param)
        {
            var propertys = param.GetType().GetProperties().Where(t => t.GetValue(param) != null).Select(t => t.Name).Select(t => $"{t}=@{t}").ToArray();
            var joint = new StringBuilder();
            for (var i = 0; i < propertys.Length; i++)
            {
                joint.Append(i != 0 ? $"{separator}{propertys[i]}" : propertys[i]);
            }
            return joint.ToString();
        }
        public static string ParamJoint(string separator, List<string> pms)
        {
            var joint = new StringBuilder();
            for (var i = 0; i < pms.Count; i++)
            {
                joint.Append(i != 0 ? (separator + pms[i] + "=@" + pms[i]) : (pms[i] + "=@" + pms[i]));
            }
            return joint.ToString();
        }

        /// <summary>
        /// 将参数名和参数值拼接并附加连接符,用于where语句拼接
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ValueJoint(string separator, object param)
        {
            var joint = new StringBuilder();
            var count = 0;
            foreach (var item in param.GetType().GetProperties())
            {
                var value = item.GetValue(param, null);
                if (value == null) continue;
                var slice = $"{item.Name}=\'{value}\'";
                joint.Append(count != 0 ? $"{separator}{slice}" : slice);
                count++;
            }
            return joint.ToString();
        }

        #endregion

        #region 语句拼接 ;SELECT last_insert_rowid()  ;select @@IDENTITY
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="pk">主键</param>
        /// <param name="joinsql">获取插入记录的数据库类型 sl-[sqllite];ms-[mysql]</param>
        /// <returns></returns>
        public static string CompileInsert<T>(object param, string pk = null, string joinsql = null)
        {
            switch (joinsql)
            {
                //mysql
                case "ms":
                    joinsql = ";select @@IDENTITY";
                    break;
                //sqllite
                case "sl":
                    joinsql = ";SELECT last_insert_rowid()";
                    break;
                //sql server
                case "ss":
                    break;
                default:
                    joinsql = "";
                    break;
            }
            return $"insert into {ClassName<T>()}({Joint(",", param, pk)}) values ({Joint(",", param, pk, true)})" + joinsql;
        }

        public static string CompileDelete<T>(object param)
        {
            return $"delete from {ClassName<T>()} where {ParamJoint(" and ", param)}";
        }

        public static string CompileUpdate<T>(object setParam, object whereParam)
        {
            return $"update {ClassName<T>()} set {ValueJoint(",", setParam)} where {ValueJoint(" and ", whereParam)}";
        }

        public static string CompileSelect<T>(object param)
        {
            return $"select {Joint(",", param)} from {ClassName<T>()}";
        }

        public static string CompileSelectAndWhere<T>(object param = null,string where = null,bool total = false)
        {
            return $"select { (total ? "*" : (Joint(",", param)))} from {ClassName<T>()}" + (string.IsNullOrEmpty(where) ? "":" where " + where);
        }

        #endregion

        #region 便捷查询
        public static T GetQuery<T>(DapperHelper conn, dynamic param)
        {
            return conn.QueryFirstOrDefault<T>($"select * from {ClassName<T>()} where {ParamJoint(" and ", param)}", param);
        }
        #endregion

    }
}