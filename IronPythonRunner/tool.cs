using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ILR
{
    public class ToolFunc
    {
        #region TOOLAPI
        public void WriteAllText(string path, string contenst)
        {
            File.WriteAllText(path, contenst);
        }
        public void AppendAllText(string path, string contenst)
        {
            File.AppendAllText(path, contenst);
        }
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
        public string WorkingPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        public string ToMD5(string word)
        {
            string md5output = "";
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] date = Encoding.Default.GetBytes(word);
            byte[] date1 = md5.ComputeHash(date);
            md5.Clear();
            for (int i = 0; i < date1.Length - 1; i++)
            {
                md5output += date1[i].ToString("X");
            }
            return md5output;
        }
        public string HttpPost(string Url, string postDataStr)
        {
            var request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
            var response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        public string HttpGet(string Url, string postDataStr)
        {
            var request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            var response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        public void CreateDir(string path)
        {
            Directory.CreateDirectory(path);
        }
        public bool IfFile(string path)
        {
            return File.Exists(path);
        }
        public bool IfDir(string path)
        {
            return Directory.Exists(path);
        }
        public void ThrowException(string msg)
        {
            throw new ArgumentOutOfRangeException(msg);
        }
        #endregion
    }
}
