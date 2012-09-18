using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;

namespace Biz.Managers
{
    public class UnitContentResourceDownloadManager : IResourceDownloadManager
    {
        private readonly IDownloadService downloadService;
        private readonly IConstants constants;
        private readonly IResourceServcie unitContentService;

        // Download by level, unit or lesson, use this id as folder name.
        public int baseModelId;

        public Unit Unit { get; set; }

        // 
        public UnitContentResourceDownloadManager(IDownloadService downloadService, IBaseModule unit, IResourceServcie unitContentService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.constants = constants;
            this.unitContentService = unitContentService;

            this.Unit = unit as Unit;
        }

        // Download activity by level?
        public virtual void Download()
        {
            // TODO
            switch (this.constants.ContentGenerateBy)
            {
                case LevelType.Level:
                    // activity, step, lesson, unit, level.
                    this.baseModelId = this.Unit.ParentModule.Id;
                    DownloadActivityContentByLevel();
                    break;
            }
        }

        // 
        private void DownloadActivityContentByLevel()
        {
            var path = this.constants.LocalContentPath + "level_" + this.baseModelId + @"\" + this.constants.CultureCode + @"\Unit_" + this.Unit.Id + ".json";
            downloadService.SaveTo(this.unitContentService.Content, path);
        }

    }
}
