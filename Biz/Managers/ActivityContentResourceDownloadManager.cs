using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;
using Biz.Helper;

namespace Biz.Managers
{
    public class ActivityContentResourceDownloadManager : IResourceDownloadManager
    {
        // Save to level_112\en\activity_4317.json
        private string SavePath;

        private readonly IDownloadService downloadService;
        private readonly IConstants constants;
        private readonly IContentResourceServcie activityContentResourceService;

        public Activity Activity { get; set; }

        public IList<MapfileItem> ResourceList { get; set; }

        // Download by level, unit or lesson, use this id as folder name.
        public int baseModelId;
        public string updatedContent;

        // 
        public ActivityContentResourceDownloadManager(IDownloadService downloadService, IBaseModule activity, IContentResourceServcie resourceService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.constants = constants;
            this.activityContentResourceService = resourceService;

            ResourceList = new List<MapfileItem>();

            this.Activity = activity as Activity;

            string oriContent = activityContentResourceService.Content;

            // Replace swf to jpg, flv to mp4
            ContentHelper.ReplaceUrlFileFormat(ref oriContent);

            ContentHelper.ReplaceUrlToLocalResourcePath(ref oriContent);

            this.updatedContent = oriContent;

            BuildDownloadResource();
        }

        private void BuildDownloadResource()
        {
            var filePath =  @"{0}_{1}" + @"\" + this.constants.CultureCode + @"\Activity_" + this.Activity.Id + ".json";
            SavePath = this.constants.LocalContentPath + filePath;

            switch (this.constants.ContentGenerateBy)
            {
                case LevelType.Level:
                    // activity, step, lesson, unit, level.
                    this.baseModelId = this.Activity.ParentModule.ParentModule.ParentModule.ParentModule.Id;
                    this.SavePath = string.Format(SavePath, "level", this.baseModelId);
                    break;
                case LevelType.Unit:
                    this.baseModelId = this.Activity.ParentModule.ParentModule.ParentModule.ParentModule.Id;
                    this.SavePath = string.Format(SavePath, "unit", this.baseModelId);
                    break;
                case LevelType.Lesson:
                    this.baseModelId = this.Activity.ParentModule.ParentModule.ParentModule.Id;
                    this.SavePath = string.Format(SavePath, "lesson", this.baseModelId);
                    break;
            }

            // Add download path to
            MapfileItem f = new MapfileItem();
            f.FileName = filePath;
            // TODO:: To get SHA like value.
            f.SHA = "1";
            ResourceList.Add(f);
        }

        // Download activity by level?
        public virtual void Download()
        {
            downloadService.SaveTo(this.updatedContent, SavePath);
        }
    }
}
