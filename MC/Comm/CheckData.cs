using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace MC.Common
{
    public class CheckData
    {
        static DateTime BaseDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
        public static string HtmlEncode(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&#39;").Replace("<", "&lt;").Replace(">", "&gt;");
        }
        public static string HtmlDecode(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&#39;", "'").Replace("&quot;", "\"").Replace("&amp;", "&");
        }
        public static JObject XML2JSON(string xml)
        {
            JObject ret = null;
            try
            {
                if (string.IsNullOrEmpty(xml)) return ret;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                ret = JsonConvert.DeserializeObject(JsonConvert.SerializeXmlNode(doc)) as JObject;
            }
            catch
            {
            }
            return ret;
        }
        public static bool ProcessSqlStr(string inputString)
        {
            //string testString = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
            string SqlStr = @"and |or |exec |execute |insert |select |delete |update |alter |create |drop |chr |char |mid |master |truncate |declare |xp_cmdshell|restore |backup |net +user|net +localgroup +administrators";
            try
            {
                if ((inputString != null) && (inputString != String.Empty))
                {
                    string str_Regex = @"\b(" + SqlStr + @")\b";
                    Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                    if (true == Regex.IsMatch(inputString))
                        return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool CheckQuote(string str)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(str))
                return ret;
            List<string> items = new List<string>();
            items.AddRange(new string[] { "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "{", "}", "[", "]", "(", ")" });
            items.AddRange(new string[] { ":", ";", "'", "|", "\\", "<", ">", "?", "/", "<<", ">>", "||", "//", ".", ",", "?" });
            items.AddRange(new string[] { "select", "delete", "update", "insert", "create", "drop", "alter", "trancate", "master" });
            for (int i = 0; i < items.Count; i++)
            {
                if (str.IndexOf(items[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return ret;
        }
        public static object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
        public static bool Check_Bool(object _o)
        {
            try
            {
                if (_o == null || string.IsNullOrEmpty(_o.ToString()))
                    return false;
                if (_o.GetType() == typeof(bool))
                    return (bool)_o;
                if (string.Compare(_o.ToString(), "1") == 0) return true;
                if (string.Compare(_o.ToString(), "0") == 0) return false;
                bool n;
                if (bool.TryParse(_o.ToString(), out n))
                {
                    return n;
                }
                return false;
            }
            catch (System.Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("Check_Bool:" + e1.ToString() + "," + _o.GetType().ToString() + "," + _o.ToString());
                return false;
            }
        }
        public static long Check_Long(object _o)
        {
            try
            {
                if (_o == null || string.IsNullOrEmpty(_o.ToString()))
                    return 0;
                if (_o.GetType() == typeof(long))
                    return (long)_o;
                long n;
                if (long.TryParse(_o.ToString(), out n))
                {
                    return n;
                }
                return 0;
            }
            catch (System.Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("Check_Long:" + e1.ToString() + "," + _o.GetType().ToString() + "," + _o.ToString());
                return 0;
            }
        }
        public static int Check_Int(object _o)
        {
            if (_o == null)
                return 0;
            if (_o.GetType() == typeof(int))
                return (int)_o;
            string s = _o.ToString();
            if (string.IsNullOrEmpty(s))
                return 0;
            int n;
            if (int.TryParse(s, out n))
            {
                return n;
            }
            return 0;
        }
        public static string Check_String(object _o)
        {
            try
            {
                if (_o == null) return "";
                if (string.IsNullOrEmpty(_o.ToString())) return "";
                return _o.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Log("CheckData-Check_String:" + ex.ToString());
                return "";
            }
        }
        public static double Check_Double(object _o)
        {
            if (_o == null || string.IsNullOrEmpty(_o.ToString()))
                return 0;
            if (_o.GetType() == typeof(double))
                return (double)_o;
            double a;
            double.TryParse(_o.ToString(), out a);
            return a;
        }
        public static decimal Check_Decimal(object _o)
        {
            try
            {
                if (_o == null || string.IsNullOrEmpty(_o.ToString()))
                    return 0;
                if (_o.GetType() == typeof(decimal))
                    return (decimal)_o;
                decimal a;
                if (decimal.TryParse(_o.ToString(), out a))
                {
                    return a;
                }
            }
            catch (System.Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("Check_Decimal:" + e1.ToString() + "," + _o.GetType().ToString() + "," + _o.ToString());
            }
            return 0;
        }
        public static DateTime Check_DateTime(object _o)
        {
            try
            {
                if (_o == null || string.IsNullOrEmpty(_o.ToString()))
                    return System.DateTime.MinValue;
                if (_o.GetType() == typeof(DateTime))
                    return (System.DateTime)_o;
                DateTime time0 = System.DateTime.ParseExact(_o.ToString(), new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy/MM/dd H:m:s", "yyyy/M/d H:m:s", "yyyy-M-d H:m:s", "MM-dd HH:mm:ss", "M-d H:m:s", "MM-dd HH:mm:ss", "yyyy-MM-dd", "yyyy-M-d", "HH:mm:ss", "H:m:s", "M/d/yyyy HH:mm:ss tt", "M/d/yyyy H:m:s tt", "yyyyMMddHHmmssffff" }, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None);
                if (time0 != System.DateTime.MinValue)
                {
                    return time0;
                }
                else
                {
                    return System.DateTime.Parse(_o.ToString(), System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None);
                }
            }
            catch (System.Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("Check_DateTime:" + e1.ToString() + "," + _o.GetType().ToString() + "," + _o.ToString());
                return System.DateTime.MinValue;
            }
        }
        public static DateTime TicksToDateTime(string ticks)
        {
            if (!string.IsNullOrEmpty(ticks))
            {
                long d;
                if (long.TryParse(ticks, out d))
                {
                    try
                    {
                        return new DateTime(d);
                    }
                    catch { }
                }
            }
            return System.DateTime.MinValue;
        }
        public static T ChangeType<T>(object oj)
        {
            T result = (T)Convert.ChangeType(oj, typeof(T));
            return result;
        }
        /// <summary>
        /// 合并到当前Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="obj"></param>
        public static void MergeModel<T>(T m, object obj) where T : new()
        {
            if (m == null || obj == null)
                return;
            PropertyInfo[] propertys = obj.GetType().GetProperties();
            Type self = typeof(T);
            PropertyInfo sp = null;
            foreach (PropertyInfo pi in propertys)
            {
                sp = self.GetProperty(pi.Name);
                if (sp == null)
                    continue;
                sp.SetValue(m, pi.GetValue(obj, null), null);
            }
        }
        /// <summary>
        /// 更新model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="dicData"></param>
        public static void UpdateModel<T>(T m, Dictionary<string, object> dicData) where T : new()
        {
            if (m == null || dicData == null || dicData.Count == 0)
                return;
            Type type = typeof(T);
            PropertyInfo[] propertys = typeof(T).GetProperties();
            object value = null;
            foreach (PropertyInfo pi in propertys)
            {
                if (dicData.ContainsKey(pi.Name))
                {
                    value = dicData[pi.Name];
                    if (pi.PropertyType == typeof(Int32))
                    {
                        pi.SetValue(m, CheckData.Check_Int(value), null);
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        pi.SetValue(m, CheckData.Check_DateTime(value), null);
                    }
                    else
                    {
                        pi.SetValue(m, value, null);
                    }
                }
            }
        }
        /// <summary>
        /// 判断值是否在枚举中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">值</param>
        /// <returns></returns>
        public static bool EnumIsDefined<T>(object o)
        {
            return Enum.IsDefined(typeof(T), o);
        }
        public static bool ContainProperty(object instance, string propertyName)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyInfo _findedPropertyInfo = instance.GetType().GetProperty(propertyName);
                return (_findedPropertyInfo != null);
            }
            return false;
        }
        public static bool ContainField(object instance, string fieldName)
        {
            if (instance != null && !string.IsNullOrEmpty(fieldName))
            {
                FieldInfo _findedInfo = instance.GetType().GetField(fieldName);
                return (_findedInfo != null);
            }
            return false;
        }
        public static System.Text.Encoding GetEncoding1(string ie)
        {
            if (string.IsNullOrEmpty(ie))
                return null;
            try
            {
                if (ie.StartsWith("utf", StringComparison.OrdinalIgnoreCase)) ie = "utf-8";
                return System.Text.Encoding.GetEncoding(ie);
            }
            catch { return null; }
        }
        public static string DecodeURL2(String uriString)
        {
            //正则1：^(?:[\x00-\x7f]|[\xe0-\xef][\x80-\xbf]{2})+$ 
            //正则2：^(?:[\x00-\x7f]|[\xfc-\xff][\x80-\xbf]{5}|[\xf8-\xfb][\x80-\xbf]{4}|[\xf0-\xf7][\x80-\xbf]{3}|[\xe0-\xef][\x80-\xbf]{2}|[\xc0-\xdf][\x80-\xbf])+$
            if (Regex.IsMatch(HttpUtility.UrlDecode(uriString, Encoding.GetEncoding("iso-8859-1")),
                @"^(?:[\x00-\x7f]|[\xe0-\xef][\x80-\xbf]{2})+$"
            ))
            {
                return HttpUtility.UrlDecode(uriString, Encoding.GetEncoding("UTF-8"));
            }
            else
            {
                return HttpUtility.UrlDecode(uriString, Encoding.GetEncoding("GB2312"));
            }
        }
        public static bool above65520(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int aa = (int)s[i];
                if (aa >= 0 && aa <= 127)
                    continue;
                if (aa >= 8192 && aa <= 8959)
                    continue;
                if (aa >= 12352 && aa <= 40959)
                    continue;
                return true;
            }
            return false;
        }
        private static System.Text.Encoding DetectEncoding(string encodingStr)
        {
            encodingStr = Regex.Replace(encodingStr, "\\s+", "");//去除空白符
            encodingStr = encodingStr.Replace("%20", "");////去除空格
            string tmp = "";
            tmp = System.Web.HttpUtility.UrlDecode(encodingStr, System.Text.Encoding.UTF8);
            if (string.Compare(System.Web.HttpUtility.UrlEncode(tmp, System.Text.Encoding.UTF8), encodingStr, true) == 0)
            {
                return System.Text.Encoding.GetEncoding("utf-8");
            }
            tmp = System.Web.HttpUtility.UrlDecode(encodingStr, System.Text.Encoding.GetEncoding("gb2312"));
            if (string.Compare(System.Web.HttpUtility.UrlEncode(tmp, System.Text.Encoding.GetEncoding("gb2312")), encodingStr, true) == 0)
            {
                return System.Text.Encoding.GetEncoding("gb2312");
            }
            return System.Text.Encoding.GetEncoding("utf-8");
        }
        /// <summary>
        /// 给定一个字符串，判断其是否只包含有汉字
        /// </summary>
        /// <param name="testStr"></param>
        /// <returns></returns>
        public static bool IsOnlyContainsChinese(string testStr)
        {
            char[] words = testStr.ToCharArray();
            foreach (char word in words)
            {
                if (IsGBCode(word.ToString()) || IsGBKCode(word.ToString())) // it is a GB2312 or GBK chinese word
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断一个word是否为GB2312编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(word);
            if (bytes.Length <= 1) // if there is only one byte, it is ASCII code or other code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)  //判断是否是GB2312
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBKCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(word.ToString());
            if (bytes.Length <= 1) // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)   //判断是否是GBK编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 判断一个word是否为Big5编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsBig5Code(string word)
        {
            byte[] bytes = Encoding.GetEncoding("Big5").GetBytes(word.ToString());
            if (bytes.Length <= 1) // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if ((byte1 >= 129 && byte1 <= 254) && ((byte2 >= 64 && byte2 <= 126) || (byte2 >= 161 && byte2 <= 254)))   //判断是否是Big5编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string GetSearchKey(string _url)
        {
            try
            {
                if (string.IsNullOrEmpty(_url)) return "";
                string searchkey;
                System.Text.RegularExpressions.Regex emailregex;
                System.Text.RegularExpressions.Match m;
                #region baidu soso haosou
                emailregex = new System.Text.RegularExpressions.Regex(@"\.baidu\..+?[&\?]word=(?<key>[^&]*)|\.baidu\..+?[&\?]wd=(?<key>[^&]*)|\.baidu\..+?[&\?]w=(?<key>[^&]*)|wap\.soso\.com.+?key=(?<key>[^&]*)|\.soso\.com.+?query=(?<key>[^&]*)|\.soso\.com.+?w=(?<key>[^&]*)|\.woso\.cn.+?wd=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    emailregex = new System.Text.RegularExpressions.Regex(@"[&\?]ie=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    m = emailregex.Match(_url);
                    if (m.Success)
                    {
                        System.Text.Encoding en = GetEncoding1(m.Groups["key"].Value);
                        if (en != null)
                            return System.Web.HttpUtility.UrlDecode(searchkey, en);
                    }
                    //  return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("gb2312"));
                    return DecodeURL2(searchkey);// System.Web.HttpUtility.UrlDecode(searchkey, DetectEncoding(searchkey));
                }
                #endregion
                #region google
                emailregex = new System.Text.RegularExpressions.Regex(@"\.google\..+?[&\?]q=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    emailregex = new System.Text.RegularExpressions.Regex(@"[&\?]ie=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    m = emailregex.Match(_url);
                    if (m.Success)
                    {
                        string enc = m.Groups["key"].Value;
                        if (enc.Equals("utf8", StringComparison.OrdinalIgnoreCase))
                            enc = "utf-8";

                        System.Text.Encoding en = GetEncoding1(enc);
                        if (en != null)
                            return System.Web.HttpUtility.UrlDecode(searchkey, en);
                    }
                    //2011-10-26 by yzt 兼容谷歌浏览器 http://www.google.com.hk/url? 搜索关键词多编码一次
                    if (_url.IndexOf("/url?", StringComparison.OrdinalIgnoreCase) > 0 && !string.IsNullOrEmpty(searchkey))
                    {
                        string[] ss = searchkey.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries); //2011-12-01 by yzt 谷歌编码次数恢复为一次，所以做此兼容
                        if (ss.Length > 0 && ss[0].Length == 4)//判断是否两次编码
                            searchkey = HttpUtility.UrlDecode(searchkey);
                    }
                    return System.Web.HttpUtility.UrlDecode(searchkey, DetectEncoding(searchkey));//DecodeURL2(searchkey);//
                }
                #endregion

                #region cn
                emailregex = new System.Text.RegularExpressions.Regex(@"\.sogou\.com.+?query=(?<key>[^&]*)|\.sogou\.com.+?keyword=(?<key>[^&]*)|\.zhongsou\.com.+?word=(?<key>[^&]*)|iask\.com.+?k=(?<key>[^&]*)|\.0063\..+?wd=(?<key>[^&]*)|\.youdao\.com.+?q=(?<key>[^&]*)|\.360.+?q=(?<key>[^&]*)|\.so\..+?q=(?<key>[^&]*)|\.haosou\.com.+?q=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    //判断是否两次编码
                    if (Regex.IsMatch(searchkey, "(%[a-zA-Z0-9]{4,})"))//                    if (Regex.IsMatch(searchkey, "^(%[a-zA-Z0-9]{4,})+$"))
                    {
                        searchkey = HttpUtility.UrlDecode(searchkey);
                    }
                    return DecodeURL2(searchkey);
                }
                emailregex = new System.Text.RegularExpressions.Regex(@"\.118114\.cn.+?kw=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    string searchkey1 = System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("utf-8"));
                    if (searchkey1.Length > 0 && above65520(searchkey1))
                    {
                        return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("gb2312"));
                    }
                    return searchkey1;
                }
                #endregion
                #region utf8+ie 日本搜索引擎
                emailregex = new System.Text.RegularExpressions.Regex(@"\.goo\.ne\..+?MT=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    emailregex = new System.Text.RegularExpressions.Regex(@"[&\?]ie=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    m = emailregex.Match(_url);
                    if (m.Success)
                    {
                        System.Text.Encoding en = GetEncoding1(m.Groups["key"].Value);
                        if (en != null)
                            return System.Web.HttpUtility.UrlDecode(searchkey, en);
                    }
                    return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("euc-jp"));
                }
                #endregion
                #region utf-8
                emailregex = new System.Text.RegularExpressions.Regex(@"\.sm\.cn.+?q=(?<key>[^&]*)|\.bing\.com.+?q=(?<key>[^&]*)|yandex\.ru/yandsearch.+?text=(?<key>[^&]*)|\.gougou\.com.+?search=(?<key>[^&]*)|wenwen\.soso\.com.+?sp=(?<key>[^&]*)|\.alltheweb\.com.+?q=(?<key>[^&]*)|\.altavista\.com.+?q=(?<key>[^&]*)|\.easou\.com.+?q=(?<key>[^&]*)|\.dmoz\.org.+?search=(?<key>[^&]*)|\.infoseek\.co\.jp.+?qt=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //gootrip.+?keyword=(?<key>[^&]*)|
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("utf-8"));
                }
                #endregion
                #region shift-jis  日本搜索引擎
                emailregex = new System.Text.RegularExpressions.Regex(@"\.excite\.co\.jp.+?search=(?<key>[^&]*)|\.fresheye\.com.+?kw=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("shift-jis"));
                }
                #endregion
                #region euc-jp  日本搜索引擎
                emailregex = new System.Text.RegularExpressions.Regex(@"\.livedoor\.com.+?q=(?<key>[^&]*)|\.biglobe\.ne\.jp.+?q=(?<key>[^&]*)|\.nifty\.com.+?Text=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("euc-jp"));
                }
                #endregion
                #region euc-kr  韩国搜索引擎
                emailregex = new System.Text.RegularExpressions.Regex(@"\.naver\.com.+?query=(?<key>[^&]*)|\.nate\.com.+?[&\?]q=(?<key>[^&]*)|\.daum\.net.+?[&\?]q=(?<key>[^&]*)|/kr\.search\.yahoo\.com.+?[&\?]p=(?<key>[^&]*)|/search\.whois\.co\.kr.+?bss_value=(?<key>[^&]*)|/search\.auction\.co\.kr.+?keyword=(?<key>[^&]*)|/search\.interpark\.com.+?tq=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("euc-kr"));
                }
                #endregion
                #region yahoo
                emailregex = new System.Text.RegularExpressions.Regex(@"hk\.search\.auctions\.yahoo\..+?[&\?]p=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding("big5"));
                }
                emailregex = new System.Text.RegularExpressions.Regex(@"\.yahoo\..+?[&\?]p=(?<key>[^&]*)|\.yahoo\..+?[&\?]q=(?<key>[^&]*)|\.yahoo\..+?[&\?]K=(?<key>[^&]*)|\.yahoo\..+?[&\?]key=(?<key>[^&]*)|\.yahoo\..+?[&\?]keyword=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                m = emailregex.Match(_url);
                if (m.Success)
                {
                    searchkey = m.Groups["key"].Value;
                    emailregex = new System.Text.RegularExpressions.Regex(@"[&\?]ei=(?<key>[^&]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    m = emailregex.Match(_url);
                    if (m.Success)
                    {
                        return System.Web.HttpUtility.UrlDecode(searchkey, System.Text.Encoding.GetEncoding(m.Groups["key"].Value));
                    }
                    return DecodeURL2(searchkey); //System.Web.HttpUtility.UrlDecode(searchkey, DetectEncoding(searchkey));
                }
                #endregion
            }
            catch
            {
            }
            return "";
        }
        /// <summary>
        /// 判断是否搜索引擎
        /// 返回标识
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string IsSearchEngine(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            url = url.ToLower();
            string[] engines = { "baidu", "so", "sogou", "soso", "youdao", "bing", "haosou", "easou", "woso", "sm", "google" };
            foreach (string e in engines)
            {
                if (url.IndexOf(e) > -1)
                {
                    return e;
                }
            }
            return string.Empty;
        }


        public static long IPToLong(string ip)
        {
            if (!string.IsNullOrEmpty(ip))
            {
                string[] strArray = ip.Split(new char[] { '.' });
                if (strArray.Length == 4)
                {
                    return ((((CheckData.Check_Long(strArray[0]) * 0x1000000L) + (CheckData.Check_Long(strArray[1]) * 0x10000L)) + (CheckData.Check_Long(strArray[2]) * 0x100L)) + CheckData.Check_Long(strArray[3]));
                }
            }
            return 0L;
        }
        /// <summary>
        /// Unicode编码且替换空格字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeUnicode(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return "";
                str = HttpUtility.UrlEncode(str);
                str = EncodeReplaceSpaces(str);
                return str;
            }
            catch (Exception ex)
            {
                LogHelper.Log("CheckData-EncodeUnicode:" + ex.ToString());
                return "";
            }
        }
        /// <summary>
        /// 替换编码字符串中的空格字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeReplaceSpaces(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return "";
                str = str.Replace("+", "%20");
                return str;
            }
            catch (Exception ex)
            {
                LogHelper.Log("CheckData-ReplaceEncodeSpaces:" + ex.ToString());
                return "";
            }
        }
        /// <summary>
        /// 获取网址域名 包含端口号
        /// </summary>
        /// <param name="_url"></param>
        /// <returns></returns>
        public static string GetDomain(string _url)
        {
            if (string.IsNullOrEmpty(_url)) return "";
            if (_url.StartsWith("file:", StringComparison.OrdinalIgnoreCase) || _url.StartsWith("ftp:", StringComparison.OrdinalIgnoreCase)) return "";
            _url = _url.ToLower().Trim();
            if (_url.StartsWith("http"))
            {
                _url = _url.Replace("http://", "").Replace("https://", "");
            }
            int n = _url.IndexOf("/", 0);
            if (n == -1)
                return _url;
            else
                return _url.Substring(0, n);
        }
        /// <summary>
        /// 获取网址的域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrlDomain(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            string p = @"(http|https|file)://(?<domain>[^(:|?|/]*)";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            Match m = reg.Match(url);
            if (m.Success)
                return m.Groups["domain"].Value;
            else
                return string.Empty;
        }
        public static string Check_Size(long length)
        {
            try
            {
                long FL = length;
                if (FL > 1024 * 1024 * 1024)
                {
                    //   KB      MB    GB   TB
                    return System.Convert.ToString(Math.Round((FL + 0.00) / (1024 * 1024 * 1024), 2)) + " GB";
                }
                else if (FL > 1024 * 1024)
                {
                    return System.Convert.ToString(Math.Round((FL + 0.00) / (1024 * 1024), 2)) + " MB";
                }
                else
                {
                    return System.Convert.ToString(Math.Round((FL + 0.00) / 1024, 2)) + " KB";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("CheckData-Check_Size:" + ex.ToString());
                return "";
            }
        }
        public static string CreateVisitorID()
        {
            string ret = "";
            ret = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000, 9999);
            return ret;
        }
        public static bool CheckVisitorID(string v)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(v)) return ret;
            if (v.Length != 22) return ret;
            try
            {
                DateTime t = Check_DateTime(v.Substring(0, 18));
                if (t > DateTime.Now.AddMinutes(1) || t < new DateTime(2012, 1, 1)) return ret;
            }
            catch
            {
                return ret;
            }
            int e = CheckData.Check_Int(v.Substring(18, 4));
            if (e < 1000 || e > 9999) return ret;
            ret = true;
            return ret;
        }
        public static bool CheckLoginName(string t)
        {
            if (string.IsNullOrEmpty(t)) return false;
            if (t.IndexOfAny(new char[] { '.' }) > -1) return false;
            if (t.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > -1) return false;
            return true;
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signature">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [Obsolete]
        public static bool CheckSignature(string signature, string timestamp, string nonce, string token)
        {
            string[] arr = new string[] { token, timestamp, nonce };
            Array.Sort<string>(arr);
            string tmpStr = string.Join("", arr);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return string.Compare(signature, tmpStr, true) == 0;
        }
        /// <summary>
        /// 获取域名根域名
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string Check_RootDomain(string d)
        {
            if (string.IsNullOrEmpty(d)) return "";
            try
            {
                d = d.ToLower();
                d = Regex.Replace(d, ":[0-9]{1,}", "");
                string[] dlist = new string[] { ".com.cn", ".net.cn", ".org.cn", ".cc", ".tv",
                                                           ".gov.cn", ".ac.cn", ".bj.cn", ".sh.cn",
                                                           ".tj.cn", ".cq.cn", ".he.cn", ".sx.cn", ".nm.cn",
                                                           ".ln.cn", ".jl.cn", ".hl.cn", ".js.cn", ".zj.cn", ".ah.cn",
                                                           ".fj.cn", ".jx.cn", ".sd.cn", ".ha.cn", ".hb.cn", ".hn.cn",
                                                           ".gd.cn", ".gx.cn", ".hi.cn", ".sc.cn", ".gz.cn", ".yn.cn",
                                                           ".xz.cn", ".sn.cn", ".gs.cn", ".qh.cn", ".nx.cn", ".xj.cn",
                                                           ".tw.cn", ".hk.cn", ".mo.cn",".com", ".net",".cn",".org", ".biz", ".info",".click" };
                for (int i = 0; i < dlist.Length; i++)
                {
                    if (d.Length > dlist[i].Length && d.Substring(d.Length - dlist[i].Length, dlist[i].Length) == dlist[i])
                    {
                        d = d.Substring(0, d.Length - dlist[i].Length);
                        if (d.LastIndexOf(".") > 0)
                        {
                            d = d.Substring(d.LastIndexOf("."), d.Length - d.LastIndexOf(".")) + dlist[i];
                            if (d.IndexOf(".") == 0)
                            {
                                d = d.Substring(1, d.Length - 1);
                            }
                        }
                        else
                        {
                            d += dlist[i];
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("CheckData-Check_RootDomain:" + ex.ToString());
            }
            return d;
        }
        /// <summary>
        /// 获取域名根域名
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string GetRootDomain(string domain)
        {
            if (string.IsNullOrEmpty(domain)) return "";
            try
            {
                string[] dlist = new string[] { ".com", ".co", ".cn", ".info", ".net", ".org", ".me", ".mobi", ".us", ".biz",".click",".xxx", ".ca", ".co.jp",
                                                          ".com.cn", ".net.cn", ".org.cn", ".gov.cn", ".ac.cn",".edu.cn",".sc.cn",".mx", ".tv", ".ws", ".ag", ".com.ag", ".net.ag",
                                                          ".org.ag", ".am", ".asia", ".at", ".be", ".com.br", ".net.br", ".bz", ".com.bz", ".net.bz",
                                                          ".cc", ".com.co", ".net.co", ".nom.co", ".de", ".es", ".com.es", ".nom.es", ".org.es", ".eu", ".fm", ".fr", ".gs",
                                                          ".in", ".co.in", ".firm.in", ".gen.in", ".ind.in", ".net.in", ".org.in", ".it", ".jobs", ".jp", ".ms", ".com.mx", ".nl", ".nu",
                                                          ".co.nz", ".net.nz", ".org.nz", ".se", ".tc", ".tk", ".tw", ".com.tw", ".com.hk", ".idv.tw", ".org.tw", ".hk", ".co.uk",
                                                          ".me.uk", ".org.uk", ".vg", ".name"};
                string ds = string.Join("|", dlist).Replace(".", "\\.");
                Regex regex = new Regex(@"\.?([^.]+(" + ds + "))$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Match m;
                m = regex.Match(domain);
                if (m.Success)
                {
                    domain = m.Groups[0].Value;
                    domain = domain.TrimStart(new char[] { '.' });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("CheckData-GetRootDomain:" + ex.ToString());
            }
            return domain;
        }
        /// <summary>
        /// 判定字符串是否为整数 >0  切首位非0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInt(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return Regex.IsMatch(s, @"^[1-9][0-9]*$");
        }
        /// <summary>
        /// 服务器时间转JS毫秒数
        /// </summary>
        /// <param name="time">c# datetime</param>
        /// <returns></returns>
        public static long ConvertToJSMilliseconds(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (time.Ticks - startTime.Ticks) / 10000;//除10000调整为13位
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime time)
        {
            return (long)(time.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds;
        }
        /// <summary>
        /// JS毫秒数转为服务器时间
        /// </summary>
        /// <param name="milliTime">js new Date().getTime();</param>
        /// <returns></returns>
        public static DateTime ConvertToTime(long milliTime)
        {
            long timeTricks = new DateTime(1970, 1, 1).Ticks + milliTime * 10000 + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours * 3600 * (long)10000000;
            return new DateTime(timeTricks);
        }
        static DateTime _Time = DateTime.MinValue;
        static long _TimeTicks = 0;
        static long _BaseTicks = 0;
        public static long ConvertToTicks(DateTime time)
        {
            if (_Time == DateTime.MinValue)
            {
                _Time = DateTime.Now.AddYears(-1);
                _TimeTicks = _Time.Ticks;
                _BaseTicks = time.Ticks - _TimeTicks;
            }
            return (time.Ticks - _TimeTicks);
        }
        public static bool IsValidTicks(long ticks)
        {
            return ((ticks >= _BaseTicks) && (ticks <= DateTime.Now.Ticks - _TimeTicks));
        }
        /// <summary>
        /// 检查是否包含手机号码
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool ContainPhoneNumber(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return false;
            try
            {
                //return Regex.Match(txt, @"(?is)1([0-9]{10,})", RegexOptions.IgnoreCase).Success;
                Regex reg = new Regex(@"(?is)1([0-9]{10,})");
                MatchCollection mc = reg.Matches(txt);
                foreach (Match m in mc)
                {
                    if (Regex.IsMatch(m.Value, @"^(1(3[0-9]|4[57]|5[0-9]|8[0-9]|7[0-9])\d{8})$"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static T ConvertToModel<T>(Hashtable ht) where T : new()
        {
            T m = new T();
            if (ht == null) return m;
            Type type = typeof(T);
            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                object value = ht[pi.Name];
                if (value != DBNull.Value)
                {
                    if (pi.PropertyType == typeof(Int32))
                    {
                        pi.SetValue(m, CheckData.Check_Int(value), null);
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        pi.SetValue(m, CheckData.Check_DateTime(value), null);
                    }
                    else
                    {
                        pi.SetValue(m, value, null);
                    }
                }
            }
            return m;
        }
        /// <summary>
        /// 获取模型对应表字段名
        /// </summary>
        public static string GetCollectionsColumnName<T>() where T : new()
        {
            PropertyInfo[] propertys = typeof(T).GetProperties();

            string ColumnNames = "";

            foreach (PropertyInfo p in propertys)
            {
                ColumnNames += ",[" + p.Name + "]";
            }

            if (ColumnNames.Length > 0)
            {
                ColumnNames = ColumnNames.Remove(0, 1);
            }

            return ColumnNames;
        }
        /// <summary>
        /// DataTable 转成 T 类型的 model List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>如果 dt 无row 则返回null</returns>
        public static List<T> ConvertToModelList<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            // 定义集合
            List<T> ts = new List<T>();
            // 获得此模型的类型
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                PropertyInfo[] propertys = typeof(T).GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        //if (!pi.SetValue) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (pi.PropertyType.ToString() == "System.String")
                            {
                                pi.SetValue(t, System.Web.HttpUtility.HtmlDecode(value.ToString()), null);
                            }
                            else
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        /// <summary>
        /// DataTable 转成 T 类型的 model
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>如果列表空，返回model默认值（null,0）。否则，返回列表中第一行记录转换的model</returns>
        public static T ConvertToModel<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count < 1)
            {
                return default(T);
            }
            List<T> list = ConvertToModelList<T>(dt);
            if (list.Count <= 0)
            {
                return default(T);
            }
            return list[0];
        }
        /// <summary>
        /// 实体转数据行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static DataRow ModelToRow<T>(T entity) where T : new()
        {
            if (entity == null) return null;
            Type entityType = entity.GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            DataTable dt = new DataTable("dt");
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name);
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
        /// 实体转字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ModelToDic<T>(T entity) where T : new()
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
        /// 数据行转集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="drs"></param>
        /// <returns></returns>
        public static List<T> RowsToList<T>(DataRow[] rows) where T : new()
        {
            List<T> listRet = new List<T>();
            if (rows == null || rows.Length == 0)
                return listRet;
            T item;
            foreach (DataRow dr in rows)
            {
                item = new T();
                PropertyInfo[] propertys = typeof(T).GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    try
                    {
                        if (!dr.Table.Columns.Contains(pi.Name))
                            continue;
                        object value = dr[pi.Name];
                        if (value != null && value != DBNull.Value)
                        {
                            if (pi.PropertyType.ToString() == "System.String")
                            {
                                pi.SetValue(item, value.ToString(), null);
                            }
                            else
                            {
                                pi.SetValue(item, value, null);
                            }
                        }
                    }
                    catch { }
                }
                listRet.Add(item);
            }
            return listRet;
        }
        /// <summary>
        /// 多行里面取一行的一条数据转为model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static T RowsToModel<T>(DataRow[] rows) where T : new()
        {
            if (rows == null || rows.Length < 1) return default(T);

            T item = RowToModel<T>(rows[0]);
            return item;
        }
        /// <summary>
        /// 数据行转实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T RowToModel<T>(DataRow row) where T : new()
        {
            if (row == null) return default(T);
            T item = new T();
            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                try
                {
                    if (row.Table.Columns.Contains(pi.Name) && row[pi.Name] != DBNull.Value)
                    {
                        if (pi.PropertyType.ToString() == "System.String")
                        {
                            pi.SetValue(item, Check_String(row[pi.Name]), null);
                        }
                        else
                        {
                            pi.SetValue(item, row[pi.Name], null);
                        }
                    }
                }
                catch
                {
                }
            }
            return item;
        }
        /// <summary>
        /// 数据表转数据实体模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T TableToModel<T>(DataTable dt) where T : new()
        {
            List<T> list = TableToList<T>(dt);
            if (list.Count <= 0)
            {
                return default(T);
            }
            return list[0];
        }
        /// <summary>
        /// 表转集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> TableToList<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0) return null;
            List<T> listRet = new List<T>();

            // 获得此模型的公共属性
            PropertyInfo[] propertys = typeof(T).GetProperties();
            T item;
            foreach (DataRow dr in dt.Rows)
            {
                item = new T();
                foreach (PropertyInfo pi in propertys)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        object value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            try
                            {
                                if (pi.PropertyType.ToString() == "System.String")
                                {
                                    pi.SetValue(item, HttpUtility.HtmlDecode(value.ToString()), null);
                                }
                                else
                                {
                                    pi.SetValue(item, value, null);
                                }
                            }
                            catch { }
                        }
                    }
                }
                listRet.Add(item);
            }
            return listRet;
        }
        /// <summary>
        /// 修改缓存某一行数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="row">需修改的行</param>
        /// <param name="item">最新数据</param>
        /// <param name="item">仅要修改的列名(多个|分割)</param>
        public static void ModifyModel<T>(DataRow row, T item, String Fields = "") where T : new()
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
        /// <summary>
        /// 设置对象属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pname"></param>
        /// <param name="pvalue"></param>
        public static void SetProperty<T>(T item, string pname, object pvalue) where T : new()
        {
            PropertyInfo property = typeof(T).GetProperty(pname, BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                if (typeof(Int32) == property.PropertyType)
                {
                    property.SetValue(item, CheckData.Check_Int(pvalue), null);
                }
                else if (typeof(long) == property.PropertyType)
                {
                    property.SetValue(item, CheckData.Check_Long(pvalue), null);
                }
                else if (typeof(bool) == property.PropertyType)
                {
                    property.SetValue(item, CheckData.Check_Bool(pvalue), null);
                }
                else if (typeof(DateTime) == property.PropertyType)
                {
                    if (pvalue != null && (pvalue.ToString().IndexOf(":") > -1 || pvalue.ToString().IndexOf("-") > -1))
                    {
                        property.SetValue(item, CheckData.Check_DateTime(pvalue), null);
                    }
                    else
                    {
                        property.SetValue(item, new DateTime(CheckData.Check_Long(pvalue)), null);
                    }
                }
                else if (typeof(string) == property.PropertyType)
                {
                    property.SetValue(item, CheckData.Check_String(pvalue), null);
                }
            }
        }
        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pname"></param>
        /// <param name="pvalue"></param>
        public static void SetField<T>(T item, string pname, object pvalue) where T : new()
        {
            FieldInfo fieldinfo = typeof(T).GetField(pname, BindingFlags.Public | BindingFlags.Instance);
            if (fieldinfo != null)
            {
                if (typeof(Int32) == fieldinfo.FieldType)
                {
                    fieldinfo.SetValue(item, CheckData.Check_Int(pvalue));
                }
                else if (typeof(long) == fieldinfo.FieldType)
                {
                    fieldinfo.SetValue(item, CheckData.Check_Long(pvalue));
                }
                else if (typeof(bool) == fieldinfo.FieldType)
                {
                    fieldinfo.SetValue(item, CheckData.Check_Bool(pvalue));
                }
                else if (typeof(DateTime) == fieldinfo.FieldType)
                {
                    if (pvalue != null && (pvalue.ToString().IndexOf(":") > -1 || pvalue.ToString().IndexOf("-") > -1))
                    {
                        fieldinfo.SetValue(item, CheckData.Check_DateTime(pvalue));
                    }
                    else
                    {
                        fieldinfo.SetValue(item, new DateTime(CheckData.Check_Long(pvalue)));
                    }
                }
                else if (typeof(string) == fieldinfo.FieldType)
                {
                    fieldinfo.SetValue(item, CheckData.Check_String(pvalue));
                }
            }
        }
        /// <summary>
        /// 将Dictionary转换成为Dynamic对象
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(IDictionary<string, object> dict)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            foreach (var entry in dict)
            {
                (result as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>(entry.Key, entry.Value));
            }
            return result;
        }
        public static Dictionary<string, object> FormToDic<T>(HttpRequest request) where T : new()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            PropertyInfo[] propertys = typeof(T).GetProperties();
            object pvalue = null;
            foreach (PropertyInfo property in propertys)
            {
                try
                {
                    if ((pvalue = request.Form[property.Name]) != null)
                    {
                        if (typeof(Int32) == property.PropertyType)
                        {
                            dic.Add(property.Name, CheckData.Check_Int(pvalue));
                        }
                        else if (typeof(long) == property.PropertyType)
                        {
                            dic.Add(property.Name, CheckData.Check_Long(pvalue));
                        }
                        else if (typeof(bool) == property.PropertyType)
                        {
                            dic.Add(property.Name, CheckData.Check_Bool(pvalue));
                        }
                        else if (typeof(DateTime) == property.PropertyType)
                        {
                            dic.Add(property.Name, new DateTime(CheckData.Check_Long(pvalue)));
                        }
                        else if (typeof(string) == property.PropertyType)
                        {
                            dic.Add(property.Name, CheckData.Check_String(pvalue));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dic;
        }
        public static bool IsPicContent(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return false;
            Regex b = new Regex(@"\[图片\][a-zA-Z0-9]{32}(\.gif|\.jpg|\.png)", RegexOptions.IgnoreCase);
            return b.IsMatch(txt);
        }
        /// <summary>
        /// 判断是否网址
        /// </summary>
        /// <param name="str_url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            return Regex.IsMatch(url, @"^(https?|ftp|file|ws)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }
        public static bool IsImage(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return false;
            Regex b = new Regex(@"\.(jpg|gif|jpeg|png)", RegexOptions.IgnoreCase);
            return b.IsMatch(ext);
        }
        /// <summary>
        /// 浏览器打开
        /// </summary>
        /// <param name="url"></param>
        public static void OpenBrowser(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch
            {
                try
                {
                    System.Diagnostics.Process.Start("IExplore.exe", url);
                }
                catch { }
            }
        }
        /// <summary>
        /// 检查浏览器是否存在 
        /// </summary>
        /// <param name="softName">chrome-----chrome.exe   firebox----firebox.exe</param>
        /// <returns></returns>
        public static bool CheckBrowserExist(string softName)
        {
            bool ret = false;
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + softName.ToString() + ".exe", false))
                {
                    if (key != null)
                    {
                        string strKeyName = string.Empty;//空字符串获取默认值
                        object objResult = key.GetValue(strKeyName);
                        RegistryValueKind regValueKind = key.GetValueKind(strKeyName);
                        if (regValueKind == RegistryValueKind.String)
                        {
                            ret = true;
                        }
                    }
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 验证密码强度
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string pwd)
        {
            var regex = new Regex(@"
                                (?=.*[0-9])                     #必须包含数字
                                (?=.*[a-zA-Z])                  #必须包含小写或大写字母
                                (?=.*[^a-zA-Z0-9])  #必须包含特殊符号        (?=([\x21-\x7e]+)[^a-zA-Z0-9]) 
                                .{8,16}                         #至少8个字符，最多16个字符
                                ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            return regex.IsMatch(pwd);
        }
        /// <summary>
        /// html转文本，替换网页标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlToText(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            html = Regex.Replace(html, "<(br|p|li)(.*?)>", " ", RegexOptions.IgnoreCase);//\r\n
            html = Regex.Replace(html, "<(select|option|script|style|title)(.*?)>((.|\n)*?)</(select|option|script|style|title)>", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(nbsp|quot|copy);", "");
            //html = Regex.Replace(html, "<[^>]*>", "");
            html = Regex.Replace(html, "<([\\s\\S])+?>", " ", RegexOptions.IgnoreCase).Replace("  ", "").Replace("\r\n ", "\r\n");
            // html = Regex.Replace(html, "\\s[^\\S]*", " ");
            if (html.StartsWith("\r\n")) html = html.TrimStart(new char[] { '\r', '\n' });
            return html;
        }
        public static string HtmlToText1(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            html = Regex.Replace(html, "<(p|li)(.*?)>", " ", RegexOptions.IgnoreCase);//\r\n
            html = Regex.Replace(html, "<(br)(.*?)>", "\n", RegexOptions.IgnoreCase);//\r\n
            html = Regex.Replace(html, "<(select|option|script|style|title)(.*?)>((.|\n)*?)</(select|option|script|style|title)>", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(nbsp|quot|copy);", "");
            //html = Regex.Replace(html, "<[^>]*>", "");
            html = Regex.Replace(html, "<([\\s\\S])+?>", " ", RegexOptions.IgnoreCase).Replace("  ", "");
            return html;
        }
        /// <summary>
        /// html内容转为推送到公众号的消息，只保留米多客表情
        /// </summary>
        /// <param name="html"></param>
        ///  <param name="onlyface">是否只保留图片表情 ，否则 保留全部图片</param>
        /// <returns></returns>
        public static string ConvertToWXText(string html, bool onlyface = false)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            html = Regex.Replace(html, "<img", "[img", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<(br)(.*?)>", "\n", RegexOptions.IgnoreCase);
            string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            //@"([\r\n])[\s]+",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);",
            @"&(nbsp|#160);",
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                html = regex.Replace(html, string.Empty);
            }
            html = html.Replace("[img", "<img");

            html = Regex.Replace(html, "\r\n", "\n", RegexOptions.IgnoreCase);

            //兼容内容中特定的a标签
            Match ubb = Regex.Match(html, @"\[url\=(.+?)\](.+?)\[\/url\]", RegexOptions.IgnoreCase);
            string af = "<a href=\"{0}\">{1}</a>";
            while (ubb.Success)
            {
                html = html.Replace(ubb.Groups[0].Value, string.Format(af, ubb.Groups[1].Value, ubb.Groups[2].Value));
                ubb = ubb.NextMatch();
            }

            if (onlyface)
            {
                Regex reg = new Regex("<img.+?>", RegexOptions.IgnoreCase);
                MatchCollection mc = reg.Matches(html);
                string src = string.Empty;
                foreach (Match m in mc)
                {
                    src = m.Value;
                    if (src.IndexOf("/Web/faces/qq/", StringComparison.OrdinalIgnoreCase) == -1 && src.IndexOf("/Web/faces/emoji/", StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        html = html.Replace(src, "");
                    }
                }
            }
            return html;
        }
        /// <summary>
        /// 转换为发送给公众号消息
        /// </summary>
        /// <param name="html"></param>
        /// <param name="onlyface">图片只保留表情</param>
        /// <returns></returns>
        public static string ConvertToText(string html, bool onlyface = false)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            //html = Regex.Replace(html, @"[\t\n\r]", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<(br)(.*?)>", "\n", RegexOptions.IgnoreCase);
            string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);",
            @"&(nbsp|#160);",
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                html = regex.Replace(html, string.Empty);
            }

            //移除html标签，保留a、img 标签
            html = Regex.Replace(html, @"</?(?(?=a|img)notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>", "");

            //兼容内容中特定的a标签
            Match ubb = Regex.Match(html, @"\[url\=(.+?)\](.+?)\[\/url\]", RegexOptions.IgnoreCase);
            string af = "<a href=\"{0}\">{1}</a>";
            while (ubb.Success)
            {
                html = html.Replace(ubb.Groups[0].Value, string.Format(af, ubb.Groups[1].Value, ubb.Groups[2].Value));
                ubb = ubb.NextMatch();
            }

            if (onlyface)
            {
                Regex reg = new Regex("<img.+?>", RegexOptions.IgnoreCase);
                MatchCollection mc = reg.Matches(html);
                string src = string.Empty;
                foreach (Match m in mc)
                {
                    src = m.Value;
                    if (src.IndexOf("/Web/faces/qq/", StringComparison.OrdinalIgnoreCase) == -1 && src.IndexOf("/Web/faces/emoji/", StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        if (src.IndexOf("../faces/qq/", StringComparison.OrdinalIgnoreCase) == -1 && src.IndexOf("../faces/emoji/", StringComparison.OrdinalIgnoreCase) == -1)
                        {
                            html = html.Replace(src, "");
                        }
                    }
                }
            }
            return html;
        }
        public static string ClearHtmlExceptA(string html)
        {
            string acceptable = "a|img";
            string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
            html = Regex.Replace(html, stringPattern, "");
            html = Regex.Replace(html, @"[\t\n]", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"[\r]", "", RegexOptions.IgnoreCase);
            return html;
        }
        /// <summary>
        /// 过滤标签 保留指定标签
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FilterHtmlTag(string s)
        {
            //<...>标记正则表达式
            return Regex.Replace(s, @"<[^>]*>", delegate (Match match)
            {
                string v = match.ToString();
                //图片,<p>,<br>正则表达式
                Regex rx = new Regex(@"^<(p|br|img.*)>$",
                 RegexOptions.Compiled | RegexOptions.IgnoreCase); //
                if (rx.IsMatch(v))
                {
                    return v; //保留图片,<p>,<br>
                }
                else
                {
                    return ""; //过滤掉
                }
            });
        }
        /// <summary>
        /// 分析url链接，返回参数集合
        /// </summary>
        /// <param name="url">url链接</param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static NameValueCollection ParseUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            NameValueCollection nvc = new NameValueCollection();
            try
            {
                // 开始分析参数对   
                System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", System.Text.RegularExpressions.RegexOptions.Compiled);
                System.Text.RegularExpressions.MatchCollection mc = re.Matches(url);
                foreach (System.Text.RegularExpressions.Match m in mc)
                {
                    nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Log("CheckData-ParseUrl:" + ex.ToString());
            }
            return nvc;
        }
        public static long GetHeadImgUrlSeq(string headimgurl)
        {
            long ret = 0;
            if (string.IsNullOrEmpty(headimgurl))
                return ret;
            NameValueCollection nvc = ParseUrl(headimgurl.ToLower());
            if (nvc == null)
                return ret;
            string[] ss = nvc.GetValues("seq");
            if (ss == null || ss.Length == 0)
                return ret;
            ret = CheckData.Check_Long(ss[0]);
            return ret;
        }

        /// <summary>
        /// 将对象序列化到文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="obj"></param>
        public static void WriteObjectToDisk(string file, object obj)
        {
            try
            {
                using (Stream stream = File.Create(file))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 读取序列话的文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static object ReadObjectFromDisk(string file)
        {
            try
            {
                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream);
                }
            }
            catch
            {
                return null;
            }
        }
  
        /// <summary>   
        /// 取得HTML中所有图片的 URL。   
        /// </summary>   
        /// <param name="sHtmlText">HTML代码</param>   
        /// <returns>图片的URL列表</returns>   
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            string[] sUrlList = null;
            // 定义正则表达式用来匹配 img 标签   
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(sHtmlText);
            if (matches.Count == 0)
                return sUrlList;
            sUrlList = new string[matches.Count];
            int i = 0;
            // 取得匹配项列表   
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }
        /// <summary>
        /// 清除html标签 保留img
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ClearHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            html = Regex.Replace(html, "<img", "[img", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<(br)(.*?)>", "\n", RegexOptions.IgnoreCase);
            string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            //@"([\r\n])[\s]+",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);",
            @"&(nbsp|#160);",
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };

            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                html = regex.Replace(html, string.Empty);
            }
            html = html.Replace("[img", "<img");
            html = Regex.Replace(html, "\r\n", "\n", RegexOptions.IgnoreCase);
            return html;
        }
        public static string[] GetHtmlImageTags(string sHtmlText)
        {
            string[] sUrlList = null;
            // 定义正则表达式用来匹配 img 标签     new Regex(@"^<(p|br|img.*)>$",  
            Regex regImg = new Regex(@"^<(img.*)>$", RegexOptions.IgnoreCase);
            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(sHtmlText);
            if (matches.Count == 0)
                return sUrlList;
            sUrlList = new string[matches.Count];
            int i = 0;
            // 取得匹配项列表   
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }

        /// <summary>
        /// 将秒转成时间，如果失败返回1970年基准时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime ConvertDateTimeFromSeconds(long seconds)
        {
            try
            {
                DateTime date = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return date.AddSeconds(seconds);
            }
            catch (Exception ex)
            {
                LogHelper.Log("CheckData-ReplaceEncodeSpaces:" + ex.ToString());
            }
            return BaseDateTime;
        }

        /// <summary>
        /// 检查IP是否合法
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIP(string ipAddress)
        {
            try
            {
                Regex rx = new Regex(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))");
                if (rx.IsMatch(ipAddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("CheckData-IsIP:" + ex.ToString());
            }
            return false;
        }
    }
}
