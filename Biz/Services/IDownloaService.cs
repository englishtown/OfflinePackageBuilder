using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz.Services
{
    public interface IDownloadService
    {
        string DownloadFromPath(Uri url);

        string DownloadFromPath(Uri url, string path);

        void MediaDownload(Uri url, string path);

        void SaveTo(string content, string path);
    }
}
