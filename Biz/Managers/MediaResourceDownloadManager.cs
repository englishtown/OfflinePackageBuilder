using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Services;
using Biz.Models;

namespace Biz.Managers
{
    public class MediaResourceDownloadManager : IResourceDownloadManager
    {
        private readonly IDownloadService downloadService;
        private readonly IResourceServcie meidaResourceService;

        private readonly IConstants constants;

        //activity
        private readonly Activity activtiy;

        public MediaResourceDownloadManager(IDownloadService downloadService, IBaseModule module, IResourceServcie resourceService, IConstants constants)
        {
            this.downloadService = downloadService;
            this.meidaResourceService = resourceService as MediaResourceService;
            this.constants = constants;

            this.activtiy = module as Activity;
        }

        public virtual void Download()
        { 
        }
    }
}
