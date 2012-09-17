using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;
using Biz.Manager;

namespace Biz.Managers
{
    public class LevelContentDownloadManager : IContentDownloadManager
    {
        private readonly IDownloadManager downloadService;

        private readonly IContentServcie contentService;

        public Level Level { get; set; }

        // 
        public LevelContentDownloadManager(IDownloadManager downloadService, IBaseModule level, IConstants constants)
        {
            this.downloadService = downloadService;
            this.contentService = new LevelContentService(level.Id, constants);

            this.Level = level as Level;
        }


        public virtual void Download()
        {
            contentService.DownloadTo("");
        }
    }
}
