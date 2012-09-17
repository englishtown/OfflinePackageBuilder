using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;

namespace Biz.Managers
{
    public class LevelContentDownloadManager : IContentDownloadManager
    {
        private readonly IDownloadService downloadService;

        private readonly IContentServcie contentService;

        public Level Level { get; set; }

        // 
        public LevelContentDownloadManager(IDownloadService downloadService, IBaseModule level, IConstants constants)
        {
            this.downloadService = downloadService;
            this.contentService = new LevelContentService(level, constants);

            this.Level = level as Level;
        }


        public virtual void Download()
        {
            
        }
    }
}
