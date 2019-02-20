using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;

namespace MCComm
{
    public class StringHelper
    {
        public static string GetDecimalStr(string moneyStr, string FormatStr)
        {
            decimal oldDecimal = moneyStr.ToDecimal();
            if (FormatStr.IsNotEmpty())
                return oldDecimal.ToString(FormatStr);
            else
                return moneyStr;
        }

        public static string GetDateTimeStr(string timeStr, string FormatStr)
        {
            if (timeStr.IsNotEmpty())
                return timeStr.ToDateTime().ToString(FormatStr);
            else
                return "";
        }


        #region 压缩解压自符串,Base64加密

        /// <summary>
        /// 压缩自符串
        /// </summary>
        /// <param name="unCompressedString"></param>
        /// <returns></returns>
        public static string ZipString(string unCompressedString)
        {
            byte[] bytData = System.Text.Encoding.GetEncoding("gb2312").GetBytes(unCompressedString);

            MemoryStream oStream = new MemoryStream();
            DeflateStream zipStream = new DeflateStream(oStream, CompressionMode.Compress);
            zipStream.Write(bytData, 0, bytData.Length);
            zipStream.Flush();
            zipStream.Close();
            byte[] byteResult = oStream.ToArray();

            return System.Convert.ToBase64String(byteResult, 0, byteResult.Length);
        }

        /// <summary>
        /// 解压自符串
        /// </summary>
        /// <param name="unCompressedString"></param>
        /// <returns></returns>
        public static string UnZipString(string unCompressedString)
        {

            using (MemoryStream mStream = new MemoryStream(System.Convert.FromBase64String(unCompressedString)))
            {
                DeflateStream unZipStream = new DeflateStream(mStream, CompressionMode.Decompress, true);
                StreamReader streamR = new StreamReader(unZipStream, System.Text.Encoding.GetEncoding("gb2312"));
                return streamR.ReadToEnd();
            }

        }
        #endregion

        #region 序列化集合对象
        /// <summary>  
        /// 序列化集合对象  
        /// </summary>  
        public static string JsonSerializerByArrayData<T>(T[] tArray)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T[]));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, tArray);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            string p = @"\\/Date(\d+)\+\d+\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }

        /// <summary>   
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串   
        /// </summary>   
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
        #endregion

        #region 反序列化集合对象
        /// <summary>   
        /// 反序列化集合对象  
        /// </summary>   
        public static T[] JsonDeserializeByArrayData<T>(string jsonString)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式    
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T[]));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T[] arrayObj = (T[])ser.ReadObject(ms);
            return arrayObj;
        }

        /// <summary>    
        /// 将时间字符串转为Json时间   
        /// </summary>   
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }


        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = timeStamp * 10000000;
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        #endregion

        public static byte[] GetBytes(string str)
        {
            return System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(str);
        }

        public static string GetString(byte[] bytes)
        {
            //return new  String(bytes, "ISO-8859-1"); 

            return System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(bytes);
        }

        public static byte[] GetBytesByBase64(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(UnZipString(str));
        }

        public static string GetStringBase64(byte[] butes)
        {
            return ZipString(System.Text.Encoding.UTF8.GetString(butes));
        }

        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getMd5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
