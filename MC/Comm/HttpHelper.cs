using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MC.Comm
{
    public class HttpHelper
    {
        MemoryStream oPostStream;
        BinaryWriter oPostData;
        int nPostMode = 1;
        int nConnectTimeout = 20;
        string cUserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        string cUsername = "";
        string cPassword = "";
        Encoding encoding = Encoding.UTF8;
        bool bHandleCookies;
        string cErrorMsg = "";
        bool bError;
        HttpWebResponse oWebResponse;
        HttpWebRequest oWebRequest;
        CookieCollection oCookies;
        string cMultiPartBoundary = "---------------------------7d33a816d302b6";

        public HttpHelper()
        {
            if (nConnectTimeout < 20)
            {
                nConnectTimeout = 60;
            }
        }
        public int PostMode
        {
            get { return this.nPostMode; }
            set { this.nPostMode = value; }
        }
        private bool keeplive;
        public bool KeepAlive
        {
            get { return keeplive; }
            set { keeplive = value; }
        }
        private bool oldhttpversion;
        public bool OldHttpVersion
        {
            get { return oldhttpversion; }
            set { oldhttpversion = value; }
        }
        public string Username
        {
            get { return this.cUsername; }
            set { cUsername = value; }
        }
        public string Password
        {
            get { return this.cPassword; }
            set { this.cPassword = value; }
        }
        public int Timeout
        {
            get { return this.nConnectTimeout; }
            set { this.nConnectTimeout = value; }
        }
        public string ErrorMsg
        {
            get { return this.cErrorMsg; }
            set { this.cErrorMsg = value; }
        }
        public bool Error
        {
            get { return this.bError; }
            set { this.bError = value; }
        }
        public bool HandleCookies
        {
            get { return this.bHandleCookies; }
            set { this.bHandleCookies = value; }
        }
        public CookieCollection Cookies
        {
            get { return this.oCookies; }
            set { this.Cookies = value; }
        }
        public HttpWebResponse WebResponse
        {
            get { return this.oWebResponse; }
            set { this.oWebResponse = value; }
        }
        public HttpWebRequest WebRequest
        {
            get { return this.oWebRequest; }
            set { this.oWebRequest = value; }
        }

        public void AddPostKey(string Key, byte[] Value)
        {

            if (this.oPostData == null)
            {
                this.oPostStream = new MemoryStream();
                this.oPostData = new BinaryWriter(this.oPostStream);
            }

            if (Key == "RESET")
            {
                this.oPostStream = new MemoryStream();
                this.oPostData = new BinaryWriter(this.oPostStream);
            }

            switch (this.nPostMode)
            {
                case 1:
                    this.oPostData.Write(encoding.GetBytes(
                        Key + "=" + System.Web.HttpUtility.UrlEncode(Value) + "&"));
                    break;
                case 2:
                    this.oPostData.Write(encoding.GetBytes(
                        "--" + this.cMultiPartBoundary + "\r\n" +
                        "Content-Disposition: form-data; name=\"" + Key + "\"\r\n\r\n"));

                    this.oPostData.Write(Value);

                    this.oPostData.Write(encoding.GetBytes("\r\n"));
                    break;
                default:
                    this.oPostData.Write(Value);
                    break;
            }
        }

        public void AddPostKey(string Key, string Value)
        {
            this.AddPostKey(Key, encoding.GetBytes(Value));
        }

        public void AddPostKey(string FullPostBuffer)
        {
            this.oPostData.Write(encoding.GetBytes(FullPostBuffer));
        }

        public bool AddPostFile(string Key, string FileName)
        {
            if (this.oPostData == null)
            {
                this.oPostStream = new MemoryStream();
                this.oPostData = new BinaryWriter(this.oPostStream);
            }
            byte[] lcFile;

            if (this.nPostMode != 2)
            {
                this.cErrorMsg = "File upload allowed only with Multi-part forms";
                this.bError = true;
                return false;
            }

            try
            {
                FileStream loFile = new FileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                lcFile = new byte[loFile.Length];
                loFile.Read(lcFile, 0, (int)loFile.Length);
                loFile.Close();
            }
            catch (Exception e)
            {
                this.cErrorMsg = e.Message;
                this.bError = true;
                return false;
            }

            this.oPostData.Write(encoding.GetBytes(
                "--" + this.cMultiPartBoundary + "\r\n" +
                "Content-Disposition: form-data; name=\"" + Key + "\"; filename=\"" +
                new FileInfo(FileName).Name + "\"\r\n\r\n"));

            this.oPostData.Write(lcFile);

            this.oPostData.Write(encoding.GetBytes("\r\n"));

            return true;
        }

        public bool AddPostZipFile(string Key, Stream loFile)
        {
            if (this.oPostData == null)
            {
                this.oPostStream = new MemoryStream();
                this.oPostData = new BinaryWriter(this.oPostStream);
            }
            byte[] lcFile;
            if (this.nPostMode != 2)
            {
                this.cErrorMsg = "File upload allowed only with Multi-part forms";
                this.bError = true;
                return false;
            }
            if (loFile.Position != 0) loFile.Position = 0;
            lcFile = new byte[loFile.Length];
            loFile.Read(lcFile, 0, (int)loFile.Length);
            loFile.Close();


            this.oPostData.Write(encoding.GetBytes(
                "--" + this.cMultiPartBoundary + "\r\n" +
                "Content-Disposition: form-data; name=\"" + Key + "\"; filename=\"" +
                Key + ".xml" + "\"\r\n\r\n"));

            this.oPostData.Write(lcFile);

            this.oPostData.Write(encoding.GetBytes("\r\n"));

            return true;
        }

        public bool AddPostZipFile(string Key, byte[] lcFile)
        {
            if (this.oPostData == null)
            {
                this.oPostStream = new MemoryStream();
                this.oPostData = new BinaryWriter(this.oPostStream);
            }

            if (this.nPostMode != 2)
            {
                this.cErrorMsg = "File upload allowed only with Multi-part forms";
                this.bError = true;
                return false;
            }
            this.oPostData.Write(encoding.GetBytes(
                "--" + this.cMultiPartBoundary + "\r\n" +
                "Content-Disposition: form-data; name=\"" + Key + "\"; filename=\"" +
                Key + ".xml" + "\"\r\n\r\n"));

            this.oPostData.Write(lcFile);

            this.oPostData.Write(encoding.GetBytes("\r\n"));

            return true;
        }


        protected StreamReader GetUrlStream(string Url, HttpWebRequest Request)
        {
            try
            {
                this.bError = false;
                this.cErrorMsg = "";
                if (Request == null)
                {
                    Request = (HttpWebRequest)System.Net.WebRequest.Create(Url);
                    Request.KeepAlive = keeplive;
                    ServicePointManager.Expect100Continue = false;
                    if (this.oldhttpversion)
                    {
                        Request.KeepAlive = false;
                        Request.ProtocolVersion = HttpVersion.Version10;
                    }
                }

                //自动解压缩
                //Request.AutomaticDecompression = DecompressionMethods.GZip;
                Request.UserAgent = this.cUserAgent;
                Request.Timeout = this.nConnectTimeout * 1000;

                // *** Save for external access
                this.oWebRequest = Request;
                //设置请求服务点的最大连接数
                this.oWebRequest.ServicePoint.ConnectionLimit = 512;

                if (this.cUsername.Length > 0)
                {
                    if (this.cUsername == "AUTOLOGIN")
                        Request.Credentials = CredentialCache.DefaultCredentials;
                    else
                        Request.Credentials = new NetworkCredential(this.cUsername, this.cPassword);
                }

                if (this.bHandleCookies)
                {
                    Request.CookieContainer = new CookieContainer();
                    if (this.oCookies != null && this.oCookies.Count > 0)
                    {
                        Request.CookieContainer.Add(this.oCookies);
                    }
                }
                if (this.oPostData != null)
                {
                    Request.Method = "POST";
                    switch (this.nPostMode)
                    {
                        case 1:
                            Request.ContentType = "application/x-www-form-urlencoded";
                            break;
                        case 2:
                            Request.ContentType = "multipart/form-data; boundary=" + this.cMultiPartBoundary;
                            this.oPostData.Write(encoding.GetBytes("--" + this.cMultiPartBoundary + "--\r\n"));
                            break;
                        case 4:
                            Request.ContentType = "text/xml";
                            break;
                        default:
                            goto case 1;
                    }
                    Stream loPostData = Request.GetRequestStream();
                    this.oPostStream.WriteTo(loPostData);

                    this.oPostStream.Close();
                    this.oPostStream = null;

                    this.oPostData.Close();
                    this.oPostData = null;
                    loPostData.Close();
                }

                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

                if (this.bHandleCookies)
                {
                    if (Response.Cookies.Count > 0)
                    {
                        if (this.oCookies == null)
                        {
                            this.oCookies = Response.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in Response.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.oCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Name;
                                        bMatch = true;
                                        break;
                                    }
                                }
                                if (!bMatch)
                                    this.oCookies.Add(oRespCookie);
                            }
                        }
                    }
                }

                this.oWebResponse = Response;

                Encoding enc;
                try
                {
                    if (Response.ContentEncoding.Length > 0)
                        enc = Encoding.GetEncoding(Response.ContentEncoding);
                    else
                        enc = encoding;
                }
                catch
                {
                    enc = encoding;
                }

                StreamReader strResponse =
                    new StreamReader(Response.GetResponseStream(), enc);
                return strResponse;
            }
            catch (Exception e)
            {
                this.cErrorMsg = e.Message;
                this.bError = true;
                return null;
            }
        }

        public Stream GetResponseStream(string Url, HttpWebRequest Request)
        {
            try
            {
                this.bError = false;
                this.cErrorMsg = "";
                if (Request == null)
                {
                    Request = (HttpWebRequest)System.Net.WebRequest.Create(Url);
                    Request.KeepAlive = keeplive;
                    ServicePointManager.Expect100Continue = false;
                    if (this.oldhttpversion)
                    {
                        Request.KeepAlive = false;
                        Request.ProtocolVersion = HttpVersion.Version10;
                    }
                }
                //自动解压缩
                //Request.AutomaticDecompression = DecompressionMethods.GZip;
                Request.UserAgent = this.cUserAgent;
                Request.Timeout = this.nConnectTimeout * 1000;

                // *** Save for external access
                this.oWebRequest = Request;
                //设置请求服务点的最大连接数
                this.oWebRequest.ServicePoint.ConnectionLimit = 512;

                if (this.cUsername.Length > 0)
                {
                    if (this.cUsername == "AUTOLOGIN")
                        Request.Credentials = CredentialCache.DefaultCredentials;
                    else
                        Request.Credentials = new NetworkCredential(this.cUsername, this.cPassword);
                }
                if (this.bHandleCookies)
                {
                    Request.CookieContainer = new CookieContainer();
                    if (this.oCookies != null && this.oCookies.Count > 0)
                    {
                        Request.CookieContainer.Add(this.oCookies);
                    }
                }
                if (this.oPostData != null)
                {
                    Request.Method = "POST";
                    switch (this.nPostMode)
                    {
                        case 1:
                            Request.ContentType = "application/x-www-form-urlencoded";
                            break;
                        case 2:
                            Request.ContentType = "multipart/form-data; boundary=" + this.cMultiPartBoundary;
                            this.oPostData.Write(encoding.GetBytes("--" + this.cMultiPartBoundary + "--\r\n"));
                            break;
                        case 4:
                            Request.ContentType = "text/xml";
                            break;
                        default:
                            goto case 1;
                    }
                    Stream loPostData = Request.GetRequestStream();
                    this.oPostStream.WriteTo(loPostData);
                    this.oPostStream.Close();
                    this.oPostStream = null;
                    this.oPostData.Close();
                    this.oPostData = null;
                    loPostData.Close();
                }

                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

                if (this.bHandleCookies)
                {
                    if (Response.Cookies.Count > 0)
                    {
                        if (this.oCookies == null)
                        {
                            this.oCookies = Response.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in Response.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.oCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Name;
                                        bMatch = true;
                                        break;
                                    }
                                }
                                if (!bMatch)
                                    this.oCookies.Add(oRespCookie);
                            }
                        }
                    }
                }

                this.oWebResponse = Response;
                return Response.GetResponseStream();
            }
            catch (Exception e)
            {
                this.cErrorMsg = e.Message;
                this.bError = true;
                return null;
            }
        }

        public StreamReader GetUrlStream(string Url)
        {
            HttpWebRequest oHttpWebRequest = null;
            return this.GetUrlStream(Url, oHttpWebRequest);
        }

        public StreamReader GetUrlStream(HttpWebRequest Request)
        {
            return this.GetUrlStream(Request.RequestUri.AbsoluteUri, Request);
        }

        /// <summary>
        ///请求网址获取字符串结果
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url)
        {
            StreamReader oHttpResponse = this.GetUrlStream(Url);
            if (oHttpResponse == null)
                return "";

            string lcResult = oHttpResponse.ReadToEnd();
            oHttpResponse.Close(); oHttpResponse = null;

            return lcResult;
        }

        /// <summary>
        /// 请求网址获取字节数组
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public byte[] GetUrlBytes(string Url)
        {
            StreamReader oHttpResponse = this.GetUrlStream(Url);
            if (oHttpResponse == null)
            {
                return null;
            }
            string lcResult = oHttpResponse.ReadToEnd();
            oHttpResponse.Close(); oHttpResponse = null;
            return null;
        }

    }
}