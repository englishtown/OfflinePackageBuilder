using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Biz.Models;

namespace Biz.Services
{
    public class UnitContentResourceService : IResourceServcie
    {
        private const string unitLink = "/services/school/courseware/GetActivityXml.ashx?actvityId={0}&cultureCode={1}&siteVersion={2}&partnerCode={3}&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";
        
        private readonly Uri fullContentLink;
        private readonly IDownloadService downloadService;

        public IBaseModule BaseModule { get; set; }
        public string Content { get; set; }

        private readonly IConstants constants;

        public UnitContentResourceService(IDownloadService downloadService, IBaseModule module, IConstants constants)
        {
            this.BaseModule = module as Unit;
            this.constants = constants;

            // Get all course content.
            this.fullContentLink = new Uri(constants.ServicePrefix + string.Format(unitLink, this.BaseModule.Id, this.constants.SiteVersion, this.constants.CultureCode, this.constants.CultureCode));

            // Download unit content.
            this.Content = downloadService.DownloadFromPath(this.fullContentLink);
        }
    }
}
