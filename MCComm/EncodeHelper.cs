using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MCComm
{
    public class EncodeHelper
    {
        #region MD5加密
        /// <summary>
        /// MD5方式加密字符串
        /// </summary>
        /// <param name="Str">要加密的字符串</param>
        /// <returns>加密过的字符串</returns>
        public static string MD5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider hashmd5;
            hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return BitConverter.ToString(hashmd5.ComputeHash(System.Text.Encoding.Default.GetBytes(str))).Replace("-", "");
        }


        public static string FileMd5(string file)
        {
            Stream inputStream = null;
            string str2;
            try
            {
                MD5 md = new MD5CryptoServiceProvider();
                inputStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = md.ComputeHash(inputStream);
                md.Clear();
                string str = "";
                for (int i = 0; i < buffer.Length; i++)
                {
                    str = str + buffer[i].ToString("x").PadLeft(2, '0');
                }
                md = null;
                str2 = str;
            }
            catch
            {
                str2 = null;
            }
            finally
            {
                if (inputStream != null)
                {
                    inputStream.Close();
                }
            }
            return str2;
        }

        #endregion
    }
}
