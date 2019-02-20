using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


#pragma warning disable
namespace MCComm
{
    public class DBHelper
    {
        public static string ConnectionString
        {
            get;
            set;
        }

        public static void GetWebSqlConnection()
        {
            ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SCMConnection"] == null ? "" :
                System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SCMConnection"].ConnectionString;
        }


        #region 运行SQL语句的方法


        public static int ExecuteNonQuery(string sql)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql);
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql, commandParameters);
        }

        public static int ExecuteNonQuery(string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, spName, parameterValues);
        }

        ///// <summary>
        ///// 执行带事务处理的SQL文
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="conn"></param>
        ///// <param name="IsProc"></param>
        ///// <param name="trans"></param>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public static int ExecuteTranSql(string sql, SqlTransaction trans, params SqlParameter[] param)
        //{
        //    return SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, param);

        //}

        ///// <summary>
        ///// 执行带事务处理的储存过程
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="conn"></param>
        ///// <param name="IsProc"></param>
        ///// <param name="trans"></param>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public static int ExecuteTranSqlForSP(string sql, SqlTransaction trans, params SqlParameter[] param)
        //{
        //    return SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sql, param);

        //}

        public static int ExecuteNonQueryForSP(string spName, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable ExecuteDatatableForSP(string sql)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, sql).Tables[0];
        }



        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDatatableForSP(string sql, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, sql, commandParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetForSP(string sql)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, sql);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetForSP(string sql, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, sql, commandParameters);
        }
        public static DataTable ExecuteDatatable(string sql)
        {
            return ExecuteDataset(sql).Tables[0];
        }

        public static DataTable ExecuteDatatable(string sql, params SqlParameter[] commandParameters)
        {
            return ExecuteDataset(sql, commandParameters).Tables[0];
        }

        public static DataTable ExecuteDatatable(string spName, params object[] parameterValues)
        {
            return ExecuteDataset(spName, parameterValues).Tables[0];
        }

        public static DataSet ExecuteDataset(string sql)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql);
        }

        public static DataSet ExecuteDataset(string sql, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql, commandParameters);
        }

        public static DataSet ExecuteDataset(string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, spName, parameterValues);
        }


        public static DataSet ExecuteDataset(string spName, CommandType commandType, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, commandType, spName, commandParameters);
        }

        public static SqlDataReader ExecuteReader(string sql)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql);
        }

        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql, commandParameters);
        }

        public static SqlDataReader ExecuteReader(string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteReader(ConnectionString, spName, parameterValues);
        }

        public static object ExecuteScalar(string sql)
        {
            return SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, sql);
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, sql, commandParameters);
        }

        public static object ExecuteScalar(string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteScalar(ConnectionString, spName, parameterValues);
        }


        ///// <summary>
        ///// 获取记录数
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="where"></param>
        ///// <param name="parameterValues"></param>
        ///// <returns></returns>
        //public static int GetCount(string tableName, string where, params SqlParameter[] parameterValues)
        //{
        //    string sql = "select count(*) from " + tableName + " where " + where;
        //    return (int)SqlHelper.ExecuteScalar(ConnectionString, sql, parameterValues);
        //}
        ///// <summary>
        ///// 获取记录数
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="where"></param>
        ///// <returns></returns>
        //public static int GetCount(string tableName, string where)
        //{
        //    return GetCount(tableName, where, null);
        //}
        ///// <summary>
        ///// 获取记录数
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <returns></returns>
        //public static int GetCount(string tableName)
        //{
        //    return GetCount(tableName, "1=1");
        //}


        /// <summary>
        /// 通过事务处理来执行sql语句
        /// </summary>
        /// <param name="sqlList">sql语句列表</param>
        /// <returns></returns>
        public static bool ExecuteSqlByTransaction(List<string> sqlList, Dictionary<string, object> param=null)
        {
            bool result = false;
            try
            {


                if (sqlList.Count > 0)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = connection.CreateCommand();
                        SqlTransaction transaction;
                        transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.Parameters.Clear();
                        if (param != null)
                        {
                            foreach (var item in param.Keys)
                            {
                                command.Parameters.AddWithValue("@" + item, param[item]);
                                //注意：
                                //1、如果存储过程中使用字符串拼接sql的话，上面的参数化将不会起作用，单引号必须经过判断并替换，
                                //在数据库中，用2个单引号代表1个实际的单引号。所以，如果是拼接sql字符串的方式，
                                //需要用Replace(@para, '''', '''''')来替换一下，将1个单引号替换为2个就没有问题了。
                            }
                        }
                        try
                        {
                            foreach (string sql in sqlList)
                            {
                                //Console.Write(sqlList);
                                command.CommandText = sql;
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            result = true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            result = false;
                            throw;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception dbErr)
            {
               
            }

            return result;
        }

        /// <summary>
        /// 通过事务处理来执行sql语句
        /// </summary>
        /// <param name="sqlAndParamList">sql语句和参数列表</param>
        /// <returns></returns>
        public static bool ExecuteSqlByTransaction(List<SqlAndParam> sqlAndParams)
        {
            bool result = false;
            try
            {
                if (sqlAndParams.Count > 0)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = connection.CreateCommand();
                        SqlTransaction transaction;
                        transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.Parameters.Clear();
                        try
                        {
                            foreach (SqlAndParam sap in sqlAndParams)
                            {
                                //Console.Write(sqlList);
                                command.CommandText = sap.sql;
                                command.Parameters.Clear();
                                foreach (var item in sap.Param.Keys)
                                {
                                    command.Parameters.AddWithValue("@" + item, sap.Param[item]);
                                    //注意：
                                    //1、如果存储过程中使用字符串拼接sql的话，上面的参数化将不会起作用，单引号必须经过判断并替换，
                                    //在数据库中，用2个单引号代表1个实际的单引号。所以，如果是拼接sql字符串的方式，
                                    //需要用Replace(@para, '''', '''''')来替换一下，将1个单引号替换为2个就没有问题了。
                                }
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            result = true;
                        }
                        catch(Exception e)
                        {
                            FileHelper.WriteLog("ExecuteSqlByTransaction", e.Message);
                            transaction.Rollback();
                            result = false;
                            throw;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception dbErr)
            {
                //MessageBox.Show("数据回写失败" + ex.Message);
            }

            return result;
        }

        #endregion

    }

    public class SqlAndParam
    {
        public string sql { get; set; }
        public Dictionary<string, object> Param { get; set; }
    }
}
