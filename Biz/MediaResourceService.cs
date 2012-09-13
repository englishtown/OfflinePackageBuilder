﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz
{
    public class MediaResourceService : IContentServcie
    {
        public Uri Url { get; set; }

        public int Id { get; set; }
        public string Content { get; set; }

        public LogEntry Logger { get; set; }

        public IDownloadManager DownloadManager { get; set; }

        public MediaResourceService(string url, IDownloadManager dm)
        {
            this.Url = new Uri(ConstantsDefault.ResourcePrefix + url);
            this.DownloadManager = dm;
        }

        public void DownloadTo(string path)
        {
            this.DownloadManager.DownloadFromPath(this.Url, path);
        }
    }
}