using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Services;
using Biz.Models;
using System.Text.RegularExpressions;
using Biz.Helper;
using System.IO;

namespace Biz.Managers
{
    public class MediaResourceDownloadManager : IResourceDownloadManager
    {
        private string savePath;
        private readonly IDownloadService downloadService;
        private readonly IContentResourceServcie activityContentResourceService;

        private readonly IConstants constants;
        private readonly IList<string> mediaList;
        public IList<FileCheckInfo> ResourceList { get; set; }

        //activity
        private readonly IBaseModule baseModule;

        public MediaResourceDownloadManager(IDownloadService downloadService, IBaseModule module, IContentResourceServcie activityContentResourceService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.baseModule = module;
            this.constants = constants;
            this.activityContentResourceService = activityContentResourceService;

            string oriContent = this.activityContentResourceService.Content;
            mediaList = ActivityContentHelper.GetMediaResources(ref oriContent);

            this.ResourceList = new List<FileCheckInfo>();

            // Build ResourceList for Mapfile.
            foreach (var m in mediaList)
            {
                FileCheckInfo f = new FileCheckInfo();
                f.FileName = m;
                ResourceList.Add(f);
            }
        }

        public virtual void Download()
        {
            foreach (var fileName in mediaList)
            {
                string path = this.constants.LocalMediaPath + "lesson_" + baseModule.ParentModule.ParentModule.Id + fileName;

                if (FileExist(path))
                    return;

                var Url = new Uri(constants.ResourcePrefix + fileName);
                this.downloadService.MediaDownload(Url, path);
            }
        }

        // Check is the media file exist on disk.
        // 
        public bool FileExist(string path)
        {
            return File.Exists(path);
        }

    }
}
