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
        private string savePath;

        private readonly IDownloadService downloadService;
        private readonly IContentResourceServcie levelContentResourceService;

        private readonly IConstants constants;

        public IList<FileCheckInfo> ResourceList { get; set; }
        public Level Level { get; set; }

        // 
        public LevelContentResourceDownloadManager(IDownloadService downloadService, IBaseModule module, IContentResourceServcie resourceService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.levelContentResourceService = resourceService;
            this.constants = constants;

            this.Level = module as Level;

            var filePath = string.Format(@"level_{0}\{1}\Level_{2}.json", this.Level.Id, this.constants.CultureCode, this.Level.Id);
            this.savePath = this.constants.LocalContentPath + filePath;

            this.ResourceList = new List<FileCheckInfo>();

            FileCheckInfo f = new FileCheckInfo();
            f.FileName = filePath;

            this.ResourceList.Add(f);
        }


        public virtual void Download()
        {
            downloadService.SaveTo(this.levelContentResourceService.Content, savePath);
        }
    }
}