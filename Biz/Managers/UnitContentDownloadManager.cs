using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;

namespace Biz.Managers
{
    public class UnitContentDownloadManager : IContentDownloadManager
    {
        private readonly IDownloadService downloadService;
        private readonly IConstants constants;

        public Unit Unit { get; set; }

        // 
        public UnitContentDownloadManager(IDownloadService downloadService, IBaseModule unit, IConstants constants)
        {
            this.downloadService = downloadService;
            this.constants = constants;

            this.Unit = unit as Unit;
        }

        public virtual void Download()
        {
        }
    }
}
