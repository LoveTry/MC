using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace MC.Comm
{
    public class CImageLibrary
    {
        public enum ValidateImageResult { OK, InvalidFileSize, InvalidImageSize }

        //检查图片大小 
        public static ValidateImageResult ValidateImage(string file, int MAX_FILE_SIZE, int MAX_WIDTH, int MAX_HEIGHT)
        {
            byte[] bs = File.ReadAllBytes(file);

            double size = (bs.Length / 1024);
            //大于50KB 
            if (size > MAX_FILE_SIZE) return ValidateImageResult.InvalidFileSize;
            Image img = Image.FromFile(file);
            if (img.Width > MAX_WIDTH || img.Height > MAX_HEIGHT) return ValidateImageResult.InvalidImageSize;
            return ValidateImageResult.OK;
        }

        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放          
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量        
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量    
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }

        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="destHeight"></param>
        /// <param name="destWidth"></param>
        /// <returns></returns>
        public static bool GetThumbnail(string source, string target, int destWidth = 1024, int destHeight = 800)
        {
            try
            {
                if (!File.Exists(source))
                    return false;
                System.Drawing.Image imgSource = Image.FromFile(source);
                System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
                int sW = 0, sH = 0;
                // 按比例缩放          
                int sWidth = imgSource.Width;
                int sHeight = imgSource.Height;
                if (sHeight > destHeight || sWidth > destWidth)
                {
                    if ((sWidth * destHeight) > (sHeight * destWidth))
                    {
                        sW = destWidth;
                        sH = (destWidth * sHeight) / sWidth;
                    }
                    else
                    {
                        sH = destHeight;
                        sW = (sWidth * destHeight) / sHeight;
                    }
                }
                else
                {
                    sW = sWidth;
                    sH = sHeight;
                }
                Bitmap outBmp = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage(outBmp);
                g.Clear(Color.Transparent);
                // 设置画布的描绘质量        
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
                g.Dispose();
                // 以下代码为保存图片时，设置压缩质量    
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo(FileContentType.GetMimeMapping(source));
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                imgSource.Dispose();
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = encoderParam;
                outBmp.Save(target, myImageCodecInfo, myEncoderParameters);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool GetThumbnail(Image imgSource, string target, int destWidth = 1024, int destHeight = 800)
        {
            try
            {
                if (imgSource == null)
                    return false;
                System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
                int sW = 0, sH = 0;
                // 按比例缩放          
                int sWidth = imgSource.Width;
                int sHeight = imgSource.Height;
                if (sHeight > destHeight || sWidth > destWidth)
                {
                    if ((sWidth * destHeight) > (sHeight * destWidth))
                    {
                        sW = destWidth;
                        sH = (destWidth * sHeight) / sWidth;
                    }
                    else
                    {
                        sH = destHeight;
                        sW = (sWidth * destHeight) / sHeight;
                    }
                }
                else
                {
                    sW = sWidth;
                    sH = sHeight;
                }
                Bitmap outBmp = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage(outBmp);
                g.Clear(Color.Transparent);
                // 设置画布的描绘质量        
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
                g.Dispose();
                // 以下代码为保存图片时，设置压缩质量    
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo(FileContentType.GetMimeMapping(target));
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                imgSource.Dispose();
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = encoderParam;
                outBmp.Save(target, myImageCodecInfo, myEncoderParameters);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool GetThumbnail(byte[] srcBytes, string target, int destWidth = 1024, int destHeight = 800)
        {
            try
            {
                if (srcBytes == null)
                    return false;
                Image imgSource = Image.FromStream(new MemoryStream(srcBytes));
                System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
                int sW = 0, sH = 0;
                // 按比例缩放          
                int sWidth = imgSource.Width;
                int sHeight = imgSource.Height;
                if (sHeight > destHeight || sWidth > destWidth)
                {
                    if ((sWidth * destHeight) > (sHeight * destWidth))
                    {
                        sW = destWidth;
                        sH = (destWidth * sHeight) / sWidth;
                    }
                    else
                    {
                        sH = destHeight;
                        sW = (sWidth * destHeight) / sHeight;
                    }
                }
                else
                {
                    sW = sWidth;
                    sH = sHeight;
                }
                Bitmap outBmp = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage(outBmp);
                g.Clear(Color.Transparent);
                // 设置画布的描绘质量        
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
                g.Dispose();
                // 以下代码为保存图片时，设置压缩质量    
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo(FileContentType.GetMimeMapping(target));
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                imgSource.Dispose();
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = encoderParam;
                outBmp.Save(target, myImageCodecInfo, myEncoderParameters);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>  
        /// 获取图片编码信息  
        /// </summary>  
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        //按宽度比例缩小图片 
        public static Image GetOutputSizeImage(Image imgSource, int MAX_WIDTH)
        {
            Image imgOutput = imgSource;

            Size size = new Size(imgSource.Width, imgSource.Height);
            if (imgSource.Width <= 3 || imgSource.Height <= 3) return imgSource; //3X3大小的图片不转换 

            if (imgSource.Width > MAX_WIDTH || imgSource.Height > MAX_WIDTH)
            {
                double rate = MAX_WIDTH / (double)imgSource.Width;

                if (imgSource.Height * rate > MAX_WIDTH)
                    rate = MAX_WIDTH / (double)imgSource.Height;

                size.Width = Convert.ToInt32(imgSource.Width * rate);
                size.Height = Convert.ToInt32(imgSource.Height * rate);

                imgOutput = imgSource.GetThumbnailImage(size.Width, size.Height, null, IntPtr.Zero);
            }
            return imgOutput;
        }

        //按比例缩小图片 
        public static Image GetOutputSizeImage(Image imgSource, Size outSize)
        {
            Image imgOutput = imgSource.GetThumbnailImage(outSize.Width, outSize.Height, null, IntPtr.Zero);
            return imgOutput;
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitmap">传入的Bitmap对象</param>
        /// <param name="destStream">压缩后的Stream对象</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitmap, Stream destStream, long level, string mimeType = "image/jpeg")
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo(mimeType);
            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);
            // Save the bitmap as a JPEG file with 给定的 quality level
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitMap">传入的Bitmap对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitMap, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(srcBitMap, s, level);
            s.Close();
        }
        /// <summary>
        /// 图片压缩到字节数组
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="level">0-100</param>
        /// <returns></returns>
        public static byte[] CompressToBytes(string srcFile, long level)
        {
            byte[] bs = null;
            Bitmap srcBitMap = new Bitmap(srcFile);
            System.IO.MemoryStream ms = new MemoryStream();
            Compress(srcBitMap, ms, level, FileContentType.GetMimeMapping(srcFile));
            bs = ms.ToArray();
            ms.Close();
            return bs;
        }
        /// <summary>
        /// 将图片字节数组保存到文件
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <param name="destFile"></param>
        /// <param name="level"></param>
        public static void Compress(byte[] srcBytes, string destFile, long level)
        {
            if (srcBytes == null)
                return;
            Bitmap srcBitMap = (Bitmap)Image.FromStream(new MemoryStream(srcBytes));
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(srcBitMap, s, level);
            s.Close();
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <param name="level"></param>
        public static void Compress(string srcFile, string destFile, long level)
        {
            Bitmap srcBitMap = new Bitmap(srcFile);
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(srcBitMap, s, level, FileContentType.GetMimeMapping(srcFile));
            s.Close();
        }

        /// <summary>
        /// 获取图片尺寸
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Size GetImageSize(string file)
        {
            if (!File.Exists(file))
                return new Size(0, 0);
            Image img = Image.FromFile(file);
            return img.Size;
        }

        /// <summary> 
        /// 由图片文件转字节 
        /// </summary> 
        public static byte[] GetImageBytes(string imageFileName)
        {
            Image img = Image.FromFile(imageFileName);
            return GetImageBytes(img);
        }

        /// <summary> 
        /// 图片转字节 
        /// </summary> 
        public static byte[] GetImageBytes(Image img)
        {
            if (img == null) return null;
            try
            {
                System.IO.MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Jpeg);
                byte[] bs = ms.ToArray();
                ms.Close();
                return bs;
            }
            catch { return null; }
        }

        /// <summary>
        /// base64转图片
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static Image FromBase64(string base64)
        {
            byte[] buffer = Convert.FromBase64String(base64.Substring(base64.IndexOf(',') + 1));
            return FromBytes(buffer);
        }
        /// <summary> 
        /// 字节转图片 
        /// </summary> 
        public static Image FromBytes(byte[] bs)
        {
            if (bs == null) return null;
            try
            {
                MemoryStream ms = new MemoryStream(bs);
                Image returnImage = Image.FromStream(ms);
                ms.Close();
                return returnImage;
            }
            catch { return null; }
        }
        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="imgfile"></param>
        /// <returns></returns>
        public static string ToBase64(string imgfile)
        {
            string ret = string.Empty;
            if (!File.Exists(imgfile))
            {
                return ret;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                Image img = Image.FromFile(imgfile);
                img.Save(ms, ImageFormat.Png);
                ret = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
            return ret;
        }
        /// <summary>
        /// img转base64
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string ToBase64(Image img)
        {
            string ret = string.Empty;
            if (img == null)
            {
                return ret;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ret = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
            return ret;
        }
        /// <summary> 
        /// 将其它格式的图片转为JPG文件 
        /// </summary> 
        public static Image ToJPG(Image source)
        {
            //注意，先定义Bitmap类，否则会报A generic error occurred in GDI+ 
            Bitmap bmp = new Bitmap(source);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            return Bitmap.FromStream(ms);
        }
        public static Image ToJPG(string source)
        {
            //注意，先定义Bitmap类，否则会报A generic error occurred in GDI+ 
            Bitmap bmp = new Bitmap(source);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            return Bitmap.FromStream(ms);
        }
        /// <summary> 
        /// 将其它格式的图片转为PNG文件 
        /// </summary> 
        public static Image ToPNG(Image source)
        {
            //注意，先定义Bitmap类，否则会报A generic error occurred in GDI+ 
            Bitmap bmp = new Bitmap(source);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return FromBytes(ms.ToArray());
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="source"></param>

        public static void SaveFile(string fileName, Image source)
        {
            source.Save(fileName, source.RawFormat);
        }
        /// <summary>
        /// 将字节数组保存为图片文件
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <param name="destFile"></param>
        /// <param name="format"></param>
        public static void SaveFile(byte[] srcBytes, string destFile, ImageFormat format)
        {
            Image img = Image.FromStream(new MemoryStream(srcBytes));
            img.Save(destFile, format);
        }
        /// <summary>
        /// 字节数组直接保存到文件
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <param name="destFile"></param>
        public static void SaveFile(byte[] srcBytes, string destFile)
        {
            FileStream fs = new FileStream(destFile, FileMode.Create);
            MemoryStream ms = new MemoryStream(srcBytes);
            ms.WriteTo(fs);
            ms.Close();
            fs.Close();
        }
        public static Bitmap GetPart(string pPath, int pPartStartPointX, int pPartStartPointY, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
        {
            Image originalImg = Image.FromFile(pPath);
            Bitmap partImg = new Bitmap(pPartWidth, pPartHeight);
            Graphics graphics = Graphics.FromImage(partImg);
            Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//目标位置
            Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
            graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
            return partImg;
        }

        public static Bitmap GetPart(Image originalImg, int pPartStartPointX, int pPartStartPointY, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
        {
            Bitmap partImg = new Bitmap(pPartWidth, pPartHeight);
            Graphics graphics = Graphics.FromImage(partImg);
            Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//目标位置
            Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
            graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
            return partImg;
        }

    }
}
