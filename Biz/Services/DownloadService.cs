using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.IO.IsolatedStorage;

namespace Biz.Services
{
    public class DownloadService : IDownloadService
    {
        public string DownloadFromPath(Uri url)
        {
            string courseContent = string.Empty;
            WebClient c = new WebClient();
            c.Headers.Add("Content-Type", "application/json; charset=utf-8");

            try
            {
                courseContent = c.DownloadString(url);
            }
            catch (WebException ex)
            {
                Logger.Write(url.ToString() + "\r\n" + ex);
            }

            return courseContent;
        }

        public string DownloadFromPath(Uri url, string path)
        {
            var data = DownloadFromPath(url);
            SaveTo(data, path);
            return data;
        }


        public void MediaDownload(Uri url, string path)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                using (var responseForMedia = request.GetResponse())
                {
                    using (Stream st = responseForMedia.GetResponseStream())
                    {
                        WriteToFile(st, path);
                    }
                }
            }
            catch (WebException ex)
            {
                Logger.Write(path.ToString() + "\r\n" + ex);
            }           
        }

        private void WriteToFile(Stream st, string path)
        {
            try
            {
                CreateFoler(path);

                using (Stream fs = new System.IO.FileStream(path, System.IO.FileMode.CreateNew))
                {
                    long totalDownloadedByte = 0;
                    byte[] buffer = new byte[1024];
                    int osize = st.Read(buffer, 0, (int)buffer.Length);
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;

                        fs.Write(buffer, 0, osize);

                        osize = st.Read(buffer, 0, (int)buffer.Length);
                    }
                }
            }
            catch (WebException ex)
            {
                Logger.Write(path.ToString() + "\r\n" + ex);
            }
        }



        public void SaveTo(string content, string path)
        {
            try
            {
                // Save the content to some path use Async
                //c.DownloadFile(url, path);
                CreateFoler(path);
                File.WriteAllText(path, content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateFoler(string path)
        {
            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

        }
    }
}
