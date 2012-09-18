using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;

namespace Biz.Managers
{
    public class LevelContentResourceDownloadManager : IResourceDownloadManager
    {
        private readonly IDownloadService downloadService;
        private readonly IResourceServcie levelContentResourceService;

        private readonly IConstants constants;

        public Level Level { get; set; }

        // 
        public LevelContentResourceDownloadManager(IDownloadService downloadService, IBaseModule module, IResourceServcie resourceService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.levelContentResourceService = resourceService;
            this.constants = constants;

            this.Level = module as Level;
        }


        public virtual void Download()
        {
            var path = this.constants.LocalContentPath + "level_" + this.Level.Id + @"\" + this.constants.CultureCode + @"\Level_" + this.Level.Id + ".json";
            downloadService.SaveTo(this.levelContentResourceService.Content, path);
        }
    }
}