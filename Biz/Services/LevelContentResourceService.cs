using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Biz.Models;

namespace Biz.Services
{
    public class LevelContentResourceService : IResourceServcie
    {
        private const string levelLink = "/services/school/courseware/GetActivityXml.ashx?actvityId={0}&cultureCode={1}&siteVersion={2}&partnerCode={3}&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";
        
        private readonly Uri fullContentLink;
        private readonly IDownloadService downloadService;

        public IBaseModule BaseModule { get; set; }
        public string Content { get; set; }

        public LevelContentResourceService(IDownloadService downloadService, IBaseModule module, IConstants constants)
        {
            this.downloadService = downloadService;

            this.BaseModule = module as Level;

            // Get all course content.
            this.fullContentLink = new Uri(constants.ServicePrefix + string.Format(levelLink, this.BaseModule.Id, constants.SiteVersion, constants.CultureCode, constants.PartnerCode));

            // Download activity content.
            this.Content = downloadService.DownloadFromPath(this.fullContentLink);
        }
    }
}
