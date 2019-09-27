using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MC.Comm
{
    /// <summary>
    /// 与文件有关的操作类
    /// </summary>
    public class FileOperate
    {
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="FileFullPath">要删除的文件全路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string FileFullPath)
        {
            if (File.Exists(FileFullPath) == true)
            {
                File.SetAttributes(FileFullPath, FileAttributes.Normal); //设置文件为正常状态，防止只读等文件发生异常
                File.Delete(FileFullPath);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 返回文件名，包括文件的扩展名
        /// </summary>
        /// <param name="FileFullPath">文件的全路径</param>
        /// <returns></returns>
        public static string GetFileName(string FileFullPath)
        {
            if (File.Exists(FileFullPath) == true)
            {
                FileInfo F = new FileInfo(FileFullPath);
                return F.Name;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="FileFullPath">文件的全路径</param>
        /// <param name="IncludeExtension">是否包含文件的扩展名</param>
        /// <returns></returns>
        public static string GetFileName(string FileFullPath, bool IncludeExtension)
        {
            if (File.Exists(FileFullPath) == true)
            {
                FileInfo F = new FileInfo(FileFullPath);
                if (IncludeExtension == true)
                {
                    return F.Name;
                }
                else
                {
                    return F.Name.Replace(F.Extension, "");
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="FileFullPath">文件的全路径</param>
        /// <returns></returns>
        public static string GetFileExtension(string FileFullPath)
        {
            if (File.Exists(FileFullPath) == true)
            {
                FileInfo F = new FileInfo(FileFullPath);
                return F.Extension;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="FileFullPath">文件的全路径</param>
        /// <returns></returns>
        public static bool OpenFile(string FileFullPath)
        {
            if (File.Exists(FileFullPath) == true)
            {
                System.Diagnostics.Process.Start(FileFullPath);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="FileFullPath">文件的全路径</param>
        /// <returns></returns>
        public static string GetFileSize(string FileFullPath)
        {
            if (File.Exists(FileFullPath) == true)
            {
                FileInfo F = new FileInfo(FileFullPath);
                long FL = F.Length;
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
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将文件读取到数据流中
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="dstStream"></param>
        /// <returns></returns>
        public static bool ReadFileToStream(string srcFile, Stream dstStream)
        {
            bool ret = true;
            FileStream stream = null;
            try
            {
                stream = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
                int num = 0;
                if (stream.Position != 0)
                {
                    stream.Position = 0;
                }
                byte[] buffer = new byte[32768];
                while ((num = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    dstStream.Write(buffer, 0, num);
                }
                dstStream.Flush();
            }
            catch
            {
                ret = false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return ret;
        }

        /// <summary>
        /// 文件转为字节数组
        /// </summary>
        /// <param name="FileFullPath">文件的全路径</param>
        /// <returns></returns>
        public static byte[] FileToStreamByte(string FileFullPath)
        {
            byte[] fileData = null;
            if (File.Exists(FileFullPath) == true)
            {
                FileStream FS = new FileStream(FileFullPath, System.IO.FileMode.Open);
                fileData = new byte[FS.Length];
                FS.Read(fileData, 0, fileData.Length);
                FS.Close();
                return fileData;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 字节数组转换为文件.
        /// </summary>
        /// <param name="CreateFileFullPath">创建文件的全路径</param>
        /// <param name="StreamByte">二进制字节数组</param>
        /// <returns></returns>
        public static bool ByteStreamToFile(string CreateFileFullPath, byte[] StreamByte)
        {
            try
            {
                if (File.Exists(CreateFileFullPath) == true)
                {
                    DeleteFile(CreateFileFullPath);
                }
                FileStream FS;
                FS = File.Create(CreateFileFullPath);
                FS.Write(StreamByte, 0, StreamByte.Length);
                FS.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 序列化XML文件
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <returns></returns>
        public static bool SerializeXmlFile(string FileFullPath)
        {
            try
            {
                System.Data.DataSet DS = new System.Data.DataSet();
                DS.ReadXml(FileFullPath);
                FileStream FS = new FileStream(FileFullPath + ".tmp", FileMode.OpenOrCreate);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter FT = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                FT.Serialize(FS, DS);
                FS.Close();
                DeleteFile(FileFullPath);
                File.Move(FileFullPath + ".tmp", FileFullPath);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 反序列化XML文件
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <returns></returns>
        public static bool DeserializeXmlFile(string FileFullPath)
        {
            try
            {
                System.Data.DataSet DS = new System.Data.DataSet();
                FileStream FS = new FileStream(FileFullPath, FileMode.Open);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter FT = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                ((System.Data.DataSet)FT.Deserialize(FS)).WriteXml(FileFullPath + ".tmp");
                FS.Close();
                DeleteFile(FileFullPath);
                File.Move(FileFullPath + ".tmp", FileFullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 与文件夹有关的操作类
    /// </summary>
    public class DirOperate
    {
        /// <summary>
        /// 操作选项
        /// </summary>
        public enum OperateOption
        {
            /// <summary>
            /// 存在删除再创建
            /// </summary>
            ExistDelete,
            /// <summary>
            /// 存在直接返回
            /// </summary>
            ExistReturn
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <param name="DirOperateOption">操作选项</param>
        /// <returns></returns>
        public bool CreateDir(string DirFullPath, OperateOption DirOperateOption)
        {
            try
            {
                if (Directory.Exists(DirFullPath) == false)
                {
                    Directory.CreateDirectory(DirFullPath);
                }
                else if (DirOperateOption == OperateOption.ExistDelete)
                {
                    Directory.Delete(DirFullPath, true);//包括目录子目录
                    Directory.CreateDirectory(DirFullPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <returns></returns>
        public bool DeleteDir(string DirFullPath)
        {
            if (Directory.Exists(DirFullPath) == true)
            {
                Directory.Delete(DirFullPath, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取目录文件列表，仅搜索当前路径
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <returns></returns>
        public string[] GetDirFiles(string DirFullPath)
        {
            string[] FileList = null;
            if (Directory.Exists(DirFullPath) == true)
            {
                FileList = Directory.GetFiles(DirFullPath, "*.*", SearchOption.TopDirectoryOnly);//仅搜索当前路径

            }
            return FileList;
        }

        /// <summary>
        /// 获取目录文件列表
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <param name="SO">查找方式</param>
        /// <returns></returns>
        public string[] GetDirFiles(string DirFullPath, SearchOption SO)
        {
            string[] FileList = null;
            if (Directory.Exists(DirFullPath) == true)
            {
                FileList = Directory.GetFiles(DirFullPath, "*.*", SO);
            }
            return FileList;
        }
        /// <summary>
        /// 获取目录文件列表，搜索指定文件
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <param name="SearchPattern">搜索扩展名</param>
        /// <returns></returns>
        public string[] GetDirFiles(string DirFullPath, string SearchPattern)
        {
            string[] FileList = null;
            if (Directory.Exists(DirFullPath) == true)
            {
                FileList = Directory.GetFiles(DirFullPath, SearchPattern);
            }
            return FileList;
        }

        /// <summary>
        /// 获取目录文件列表
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <param name="SearchPattern">搜索扩展名</param>
        /// <param name="SO">查找方式</param>
        /// <returns></returns>
        public string[] GetDirFiles(string DirFullPath, string SearchPattern, SearchOption SO)
        {
            string[] FileList = null;
            if (Directory.Exists(DirFullPath) == true)
            {
                FileList = Directory.GetFiles(DirFullPath, SearchPattern, SO);
            }
            return FileList;
        }
        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="DirFullPath"></param>
        /// <returns></returns>
        public static bool OpenDir(string DirFullPath)
        {
            if (Directory.Exists(DirFullPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", DirFullPath);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}