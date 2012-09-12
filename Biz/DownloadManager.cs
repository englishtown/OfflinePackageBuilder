using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Biz
{
    public class DownloadManager : IDownloadManager
    {
        public string DownloadFromPath(Uri path)
        {
            WebClient c = new WebClient();
            c.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var courseContent = c.DownloadString(path);

            return courseContent;
        }


        public void DownloadFromPathTo(Uri url, string path)
        {
            WebClient c = new WebClient();
            c.Headers.Add("Content-Type", "application/json; charset=utf-8");

            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            try
            {
                // Save the content to some path use Async
                c.DownloadFile(url, path);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
