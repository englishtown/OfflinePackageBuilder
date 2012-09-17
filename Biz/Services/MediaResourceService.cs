using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.IO;

namespace Biz.Services
{
    public class MediaResourceService : IContentServcie
    {
        public Uri Url { get; set; }

        public int Id { get; set; }
        public string Content { get; set; }

        public LogEntry Logger { get; set; }

        private readonly IDownloadService downloadManager;
        private readonly IConstants constants;

        public MediaResourceService(string url, IDownloadService dm, IConstants constants)
        {
            this.downloadManager = dm;
            this.constants = constants;

            this.Url = new Uri(constants.ResourcePrefix + url);
        }

        // Check is the media file exist on disk.
        // 
        public bool FileExist(string path)
        {
            return File.Exists(path);
        }

        public void DownloadTo(string path)
        {
            if (FileExist(path))
                return;

            this.downloadManager.MediaDownload(this.Url, path);
        }
    }
}