using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.IO;
using System;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace MCComm
{
    public class JsonHelper
    {
        JToken jt;
        JObject jsonRoot;
        public JsonHelper()
        {
            jsonRoot = new JObject();
        }

        public JsonHelper(string sInput) : this()
        {
            jt = JToken.Parse(sInput);
        }


        public JToken GetValue(string itemname)
        {

            return jt.Value<JToken>(itemname);

        }
        public string GetStringValue(string itemname)
        {
            return jt.Value<string>(itemname);
        }

        public DateTime? GetDateTimeValue(string itemname)
        {
            return jt.Value<DateTime?>(itemname);
        }

        public int GetIntValue(string itemname)
        {
            return jt.Value<int>(itemname);
        }

        public JObject GetJObject(string itemname)
        {
            return jt.Value<JObject>(itemname);

        }

        public JArray GetJArray(string itemname)
        {
            return jt.Value<JArray>(itemname);

        }

        public void WriteValue(string name, JToken svalue)
        {
            jsonRoot.Add(name, svalue);
        }

        public override string ToString()
        {
            return jsonRoot.ToString();
        }

        #region 压缩解压数据集
        /// <summary>
        /// 压缩DataTableToString
        /// </summary>
        /// <param name="dtOriginal"></param>
        /// <returns></returns>

        public static string CompressionDataTableToString(DataTable dtOriginal)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            serializer.Serialize(writer, dtOriginal);
            writer.Close();
            return StringHelper.ZipString(sb.ToString());

        }
        //static int nNNNN = 0;
        public static DataTable DecompressionStringToDataTable(string strDt)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start(); //  开始监视代码运行时间
            //nNNNN++;
            StringReader strReader = new StringReader(StringHelper.UnZipString(strDt));
            XmlReader xmlReader = XmlReader.Create(strReader);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            DataTable dt = serializer.Deserialize(xmlReader) as DataTable;

            //stopwatch.Stop(); //  停止监视
            //TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            //System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("h:m:s fff") + "\t" + nNNNN + "\t:" + timespan.TotalMilliseconds);
            return dt;

        }


        /// <summary>
        /// 压缩DataSetToString
        /// </summary>
        /// <param name="dtOriginal"></param>
        /// <returns></returns>

        public static string CompressionDataSetToString(DataSet dtOriginal)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
            serializer.Serialize(writer, dtOriginal);
            writer.Close();
            return StringHelper.ZipString(sb.ToString());

        }

        public static DataSet DecompressionStringToDataSet(string strDt)
        {
            StringReader strReader = new StringReader(StringHelper.UnZipString(strDt));
            XmlReader xmlReader = XmlReader.Create(strReader);
            XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
            DataSet ds = serializer.Deserialize(xmlReader) as DataSet;
            return ds;
        }

        /// <summary>
        /// 压缩ListToString
        /// </summary>
        /// <param name="dtOriginal"></param>
        /// <returns></returns>

        public static string CompressionListToString(List<string> dtOriginal)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            serializer.Serialize(writer, dtOriginal);
            writer.Close();
            return StringHelper.ZipString(sb.ToString());

        }

        public static List<string> DecompressionStringToList(string strDt)
        {
            StringReader strReader = new StringReader(StringHelper.UnZipString(strDt));
            XmlReader xmlReader = XmlReader.Create(strReader);
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            List<string> list = serializer.Deserialize(xmlReader) as List<string>;
            return list;
        }
        #endregion

        public static string GetJsonString(object data)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = Int32.MaxValue;
            var jsonString = js.Serialize(data);

            //解码Unicode，也可以通过设置App.Config（Web.Config）设置来做，这里只是暂时弥补一下，用到的地方不多
            MatchEvaluator evaluator = new MatchEvaluator(DecodeUnicode);
            var json = Regex.Replace(jsonString, @"\\u[0123456789abcdef]{4}", evaluator);//或：[\\u007f-\\uffff]，\对应为\u000a，但一般情况下会保持\
            return json;
        }

        public static string DecodeUnicode(Match match)
        {
            if (!match.Success)
            {
                return null;
            }

            char outStr = (char)int.Parse(match.Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
            return new string(outStr, 1);
        }

        public static T GetObject<T>(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            T result = js.Deserialize<T>(json);
            return result;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            StringReader sr = new StringReader(json);
            Newtonsoft.Json.JsonTextReader jr = new Newtonsoft.Json.JsonTextReader(sr);
            object o = serializer.Deserialize(jr, typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }
    }
}
