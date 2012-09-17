using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;

namespace Biz.Managers
{
    public class ActivityContentDownloadManager : IContentDownloadManager
    {
        // private const string courseLink = "/services/school/query?q=course!{0}.*&c=siteversion={1}|cultureCode={2}|partnerCode={3}";


        private readonly IDownloadService downloadService;
        private readonly IConstants constants;

        public Activity Activity { get; set; }

        // Download by level, unit or lesson, use this id as folder name.
        public int baseModelId;

        private readonly ActivityContentService acs;

        // 
        public ActivityContentDownloadManager(IDownloadService downloadService, IBaseModule activity, IContentServcie contentService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.constants = constants;
            this.acs = contentService as ActivityContentService;

            this.Activity = activity as Activity;
        }

        // Download activity by level?
        public virtual void Download()
        {
            // TODO
            switch (LevelType.Level)
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
            var path = this.constants.LocalContentPath + "level_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\" + this.Activity.Id + ".json";
            downloadService.SaveTo(acs.Content, path);
        }

        // 
        private void DownloadActivityContentByUnit()
        {
            var path = this.constants.LocalContentPath + "unit_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\" + this.Activity.Id + ".json";
            downloadService.SaveTo(acs.Content, path);
        }

        // 
        private void DownloadActivityContentByLesson()
        {
            var path = this.constants.LocalContentPath + "lesson_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\" + this.Activity.Id + ".json";
            downloadService.SaveTo(acs.Content, path);
        }
    }
}
