using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;
using Biz.Manager;

namespace Biz.Managers
{
    public class UnitContentDownloadManager : IContentDownloadManager
    {
        private readonly IDownloadManager downloadService;
        private readonly IConstants constants;

        public Unit Unit { get; set; }

        // 
        public UnitContentDownloadManager(IDownloadManager downloadService, IBaseModule unit, IConstants constants)
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
