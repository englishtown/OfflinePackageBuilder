using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;

namespace Biz.Managers
{
    public class ActivityContentResourceDownloadManager : IResourceDownloadManager
    {
        private readonly IDownloadService downloadService;
        private readonly IConstants constants;
        private readonly IResourceServcie activityContentResourceService;

        public Activity Activity { get; set; }

        // Download by level, unit or lesson, use this id as folder name.
        public int baseModelId;

        // 
        public ActivityContentResourceDownloadManager(IDownloadService downloadService, IBaseModule activity, IResourceServcie resourceService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.constants = constants;
            this.activityContentResourceService = resourceService;

            this.Activity = activity as Activity;
        }

        // Download activity by level?
        public virtual void Download()
        {
            // TODO
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
            downloadService.SaveTo(activityContentResourceService.Content, path);
        }

        // 
        private void DownloadActivityContentByUnit()
        {
            var path = this.constants.LocalContentPath + "unit_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\Activity_" + this.Activity.Id + ".json";
            downloadService.SaveTo(activityContentResourceService.Content, path);
        }

        // 
        private void DownloadActivityContentByLesson()
        {
            var path = this.constants.LocalContentPath + "lesson_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\Activity_" + this.Activity.Id + ".json";
            downloadService.SaveTo(activityContentResourceService.Content, path);
        }
    }
}
