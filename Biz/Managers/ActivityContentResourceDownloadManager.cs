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
        private readonly IDownloadService downloadService;
        private readonly IConstants constants;
        private readonly IContentResourceServcie activityContentResourceService;

        public Activity Activity { get; set; }

        // Download by level, unit or lesson, use this id as folder name.
        public int baseModelId;
        public string updatedContent;

        // 
        public ActivityContentResourceDownloadManager(IDownloadService downloadService, IBaseModule activity, IContentResourceServcie resourceService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.constants = constants;
            this.activityContentResourceService = resourceService;

            this.Activity = activity as Activity;

            string oriContent = activityContentResourceService.Content;

            // Replace swf to jpg, flv to mp4
            ActivityContentHelper.ReplaceUrlFileFormat(ref oriContent);

            ActivityContentHelper.ReplaceUrlToLocalResourcePath(ref oriContent);

            this.updatedContent = oriContent;
        }

        // Download activity by level?
        public virtual void Download()
        {
            switch (this.constants.ContentGenerateBy)
            {
                case LevelType.Level:
                    // activity, step, lesson, unit, level.
                    this.baseModelId = this.Activity.ParentModule.ParentModule.ParentModule.ParentModule.Id;
                    DownloadActivityContentByLevel();
                    break;
                case LevelType.Unit:
                    this.baseModelId = this.Activity.ParentModule.ParentModule.ParentModule.ParentModule.Id;
                    DownloadActivityContentByUnit();
                    break;
                case LevelType.Lesson:
                    this.baseModelId = this.Activity.ParentModule.ParentModule.ParentModule.Id;
                    DownloadActivityContentByLesson();
                    break;
            }
        }

        // 
        private void DownloadActivityContentByLevel()
        {
            var path = this.constants.LocalContentPath + "level_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\Activity_" + this.Activity.Id + ".json";
            downloadService.SaveTo(this.updatedContent, path);
        }

        // 
        private void DownloadActivityContentByUnit()
        {
            var path = this.constants.LocalContentPath + "unit_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\Activity_" + this.Activity.Id + ".json";
            downloadService.SaveTo(this.updatedContent, path);
        }

        // 
        private void DownloadActivityContentByLesson()
        {
            var path = this.constants.LocalContentPath + "lesson_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\Activity_" + this.Activity.Id + ".json";
            downloadService.SaveTo(this.updatedContent, path);
        }
    }
}
