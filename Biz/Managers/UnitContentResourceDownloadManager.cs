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
        private string savePath;

        private readonly IDownloadService downloadService;
        private readonly IConstants constants;
        private readonly IContentResourceServcie unitContentService;

        // Download by level, unit or lesson, use this id as folder name.
        public int baseModelId;

        public Unit Unit { get; set; }
        public IList<MapfileItem> ResourceList { get; set; }

        // 
        public UnitContentResourceDownloadManager(IDownloadService downloadService, IBaseModule unit, IContentResourceServcie unitContentService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.constants = constants;
            this.unitContentService = unitContentService;

            this.Unit = unit as Unit;

            this.ResourceList = new List<MapfileItem>();
            this.BuildDownloadResource();
        }

        private void BuildDownloadResource()
        {
            this.savePath = this.constants.LocalContentPath + @"{0}_{1}" + @"\" + this.constants.CultureCode + @"\Unit_" + this.Unit.Id + ".json";

            switch (this.constants.ContentGenerateBy)
            {
                case LevelType.Level:
                    // activity, step, lesson, unit, level.
                    this.baseModelId = this.Unit.ParentModule.Id;
                    this.savePath = string.Format(savePath, "level", this.baseModelId);
                    break;
            }

            // Add download path to
            MapfileItem f = new MapfileItem();
            f.FileName = this.savePath;
            // TODO:: To get SHA like value.
            f.SHA = "2";
            ResourceList.Add(f);
        }


        // Download activity by level?
        public virtual void Download()
        {
            downloadService.SaveTo(this.unitContentService.Content, this.savePath);
        }


    }
}
