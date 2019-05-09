using System;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace MCComm
{
    public static class Extensions
    {

        /// <summary>
        /// 格式化日期类型的字符串yyyy-MM-dd
        /// </summary>
        public const string FormatDate = "yyyy-MM-dd";

        /// <summary>
        /// 格式化日期类型的字符串为带时间的yyyy-MM-dd HH:mm
        /// </summary>
        public const string FormatDateTime = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// 格式化日期类型的字符串为月份 yyyy-MM
        /// </summary>
        public const string FormatMonth = "yyyy-MM";

        #region GUID扩展
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static int ToInt(this Enum s)
        //{
        //    return int.Parse(s);
        //}

        /// <summary>
        /// 扩展Guid类型，判断Guid是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this Guid s)
        {
            return s != Guid.Empty;
        }



        /// <summary>
        /// 扩展Guid类型，判断Guid是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid s)
        {
            return s == Guid.Empty;
        }



        #endregion

        #region 日期扩展
        /// <summary>
        /// 可空日期yyyy-MM-dd
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToShortFormat(this DateTime? s)
        {
            if (s == null)
            {
                return "";
            }
            else
            {
                return s.GetValueOrDefault().ToString(FormatDate);
            }
        }
        /// <summary>
        /// 可空日期yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToLongFormat(this DateTime? s)
        {
            if (s == null)
            {
                return "";
            }
            else
            {
                return s.GetValueOrDefault().ToString(FormatDateTime);
            }
        }

        /// <summary>
        /// 可空日期yyyy-MM
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToMonthFormat(this DateTime? s)
        {
            if (s == null)
            {
                return string.Empty;
            }
            else
            {
                return s.GetValueOrDefault().ToString(FormatMonth);
            }
        }
        /// <summary>
        /// 非空日期yyyy-MM-dd
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToShortFormat(this DateTime s)
        {
            return s.ToString(FormatDate);
        }
        /// <summary>
        /// 非空日期yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToLongFormat(this DateTime s)
        {
            return s.ToString(FormatDateTime);
        }
        /// <summary>
        /// 非空日期yyyy-MM
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToMonthFormat(this DateTime s)
        {
            return s.ToString(FormatMonth);
        }
        /// <summary>
        /// 验证是否为日期类型.2011年4月7日 14:43:29 xyl 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>是否为日期,true-是,false-不是</returns>
        public static bool IsDateTime(this string str)
        {
            DateTime dtTemp = new DateTime();
            return DateTime.TryParse(str, out dtTemp);
        }


        #endregion


        #region Decimal金额扩展

        /// <summary>
        /// 格式化金钱类型的字符串,保留两位小数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToMoney(this Decimal s)
        {
            return s.ToString("f2");
        }


        /// <summary>
        /// 格式化金钱类型的字符串
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="pointDigit">小数点的位数</param>
        /// <returns></returns>
        public static string ToMoney(this Decimal s, int pointDigit)
        {
            return s.ToString(string.Format("f{0}", pointDigit));
        }

        #endregion

        #region float 扩展成2位小数

        /// <summary>
        /// 格式化金钱类型的字符串,保留两位小数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToMoney(this float s)
        {
            return s.ToString("f2");
        }

        #endregion

        #region String扩展


        /// <summary>
        /// 扩展string类型，判断string不为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s == null ? string.Empty : s.Trim());
        }
        /// <summary>
        /// 扩展string类型，判断string为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrEmpty(s == null ? string.Empty : s.Trim());
        }

        /// <summary>
        /// Format
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string FormatWith(this string s, params object[] arg)
        {
            return string.Format(s, arg);
        }

        /// <summary>
        /// 扩展string方法，增加将string转换为Guid类型的方法
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string s)
        {
            if (s.Length != 36)
            {
                return Guid.Empty;
            }

            try
            {
                return new Guid(s.Trim());
            }
            catch (System.Exception)
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 扩展string方法，转换为int类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            else
            {
                int result = 0;
                int.TryParse(s.Trim(), out result);
                return result;
            }
        }

        /// <summary>
        /// 扩展string方法，转换为int类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            else
            {
                Int64 result = 0;
                Int64.TryParse(s.Trim(), out result);
                return result;
            }
        }


        /// <summary>
        /// 扩展string方法，转换为float类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float ToFloat(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            else
            {
                float result = 0;
                float.TryParse(s.Trim(), out result);
                return result;
            }
        }


        /// <summary>
        /// 扩展string方法，转换为DateTime类型, 转换失败，返回今天的日期
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string s)
        {
            DateTime result;
            //DateTime.TryParse(s.Trim(), out result);

            if (!DateTime.TryParse(s.Trim(), out result))
            {
                //MsgHelper.ShowError(s + "日期格式转化错误，请确认!");

                return DateTime.Today;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 扩展string方法，转换为DateTime类型, 转换失败，返回空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeNull(this string s)
        {
            DateTime result;
            DateTime.TryParse(s.Trim(), out result);

            if (result == DateTime.MinValue)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 扩展string方法，转换为Decimal类型数据
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            else
            {
                decimal result = 0;
                decimal.TryParse(s, out result);
                return result;
            }
        }
        /// <summary>
        /// 扩展string方法,转换为Boolean类型  2011年1月5日 15:32:27  xyl add
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Boolean ToBoolean(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            else
            {
                Boolean result = false;
                Boolean.TryParse(s, out result);
                return result;
            }
        }

        /// <summary>
        /// 判断是否为字母
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsLetter(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            Regex reg = new Regex("^[A-Za-z]+$");

            return reg.IsMatch(s);
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumber(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            Regex reg = new Regex(@"^\d+$");

            return reg.IsMatch(s);
        }

        #endregion

        #region DataTable相关

        /// <summary>
        /// 获取DataTable的前多少列数据
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static DataTable GetTop(this DataTable s, int n)
        {
            DataTable dt = s.Clone();

            if (s.Rows.Count <= n)
            {
                return s;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    dt.ImportRow(s.Rows[i]);
                }
                return dt;
            }
        }

        public static void addNewColumn(this DataTable dt, DataTable srcDt, string colName)
        {
            dt.Columns.Add(srcDt.Columns[colName].Caption, srcDt.Columns[colName].DataType);
        }

        public static DataTable DeleteEmptyRow(this DataTable s, int n)
        {
            DataTable dt = s.Clone();
            //如果超限了 返回本身
            if (s.Columns.Count <= n)
            {
                return s;
            }
            else
            {
                for (int i = 0; i < s.Rows.Count; i++)
                {
                    //如果不为空,插入,为空删选出去
                    if (s.Rows[i][n].ToString() != "")
                    {
                        dt.ImportRow(s.Rows[i]);
                    }
                    else
                    {
                    }
                }
                return dt;
            }
        }

        public static DataTable ShowColumnAndRows(this DataTable s, string[] savecolumns, int liStartRow)
        {
            //数据 
            DataTable dtObject = s;
            //保留列 
            string[] saveColumns = savecolumns;
            //移除不需要的列 
            for (int i = dtObject.Columns.Count - 1; i >= 0; i--)//注意此处，一般习惯用i++则会引发OutOfIndex异常，由于部分列被移除，列索引减少，i++会超出不断减少的索引总数，注意。 
            {
                //移除指示器 
                bool remove = true;
                //是否在保留列中 
                for (int j = 0; j < saveColumns.Length; j++)
                {
                    if (dtObject.Columns[i].ColumnName == saveColumns[j])
                    {
                        //保留列不移除 
                        remove = false; break;
                    }
                }
                if (remove)
                {
                    //移除列 
                    dtObject.Columns.Remove(dtObject.Columns[i].ColumnName);
                }
            }

            //移除不需要的行
            for (int i = 0; i < liStartRow - 1; i++)
            {
                dtObject.Rows.Remove(dtObject.Rows[i]);
            }

            return dtObject;
        }

        #endregion

        #region int类型扩展

        /// <summary>
        /// 判断某个数字是否在指定的范围之内，包括等于
        /// </summary>
        /// <param name="s"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsInDefinedRange(this int s, int min, int max)
        {

            if (s >= min && s <= max)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 当为0的时候转化为String.empty
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string ToEmptyWhenZero(this int s)
        {
            if (s == 0)
            {
                return string.Empty;
            }
            else
            {
                return s.ToString();
            }
        }

        #endregion

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        //public static object GetValue(this object o, string key)
        //{
        //    if (o is List<object>)
        //    {
        //        foreach (var oo in (List<object>)o)
        //        {
        //            var result = GetValue(oo, key);//递归返回匹配的值
        //            if (result != null)
        //                return result;
        //        }
        //    }
        //    else if (o is object[])
        //    {
        //        foreach (var oo in (object[])o)
        //        {
        //            var result = GetValue(oo, key);//递归返回匹配的值
        //            if (result != null)
        //                return result;
        //        }
        //    }
        //    else if (o is IDictionary<string, object>)
        //    {
        //        foreach (var property in (IDictionary<String, Object>)o)
        //        {
        //            if (property.Key == key || property.Key == key.ToLower() || property.Key == key.ToUpper())
        //                return property.Value;
        //        }
        //        //如果上面的遍历没有结果，则该值可能嵌套在property.Value里面，需要递归解析
        //        foreach (var property in (IDictionary<String, Object>)o)
        //        {
        //            var result = GetValue(property.Value, key);//递归返回匹配的值
        //            if (result != null)
        //                return result;
        //        }
        //    }
        //    return null;//没有匹配值，返回null
        //}
    }

    public enum MessageType
    {
        订单生成通知 = 1,
        推荐成交通知 = 2,
        审核结果通知 = 3
    }
}
