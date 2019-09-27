using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace MC.Comm
{
    public class GZipUtil
    {
        /// <summary>
        /// 字节数组压缩  
        /// </summary>  
        /// <param name ="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, data.Length);
                zip.Close();
                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>  
        /// 字节数组解压缩  
        /// </summary>  
        /// <param name="data"></param>  
        /// <returns></returns>  
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 字符串压缩
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="str">String.</param>
        public static string CompressString(string str)
        {
            byte[] compressBeforeByte = Encoding.UTF8.GetBytes(str);
            byte[] compressAfterByte = Compress(compressBeforeByte);
            return Convert.ToBase64String(compressAfterByte);
        }
        /// <summary>
        /// 字符串解压缩
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="str">String.</param>
        public static string DecompressString(string str)
        {
            byte[] compressBeforeByte = Convert.FromBase64String(str);
            byte[] compressAfterByte = Decompress(compressBeforeByte);
            return Encoding.UTF8.GetString(compressAfterByte);
        }
    }
}
