using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz.Services
{
    public class CourseContentResourceService : IResourceServcie
    {
        private const string CourseContentLink = "/services/school/courseware/GetActivityXml.ashx?actvityId={0}&cultureCode={1}&siteVersion={2}&partnerCode={3}&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";

        public IBaseModule BaseModule { get; set; }
        private readonly IDownloadService downloadService;

        public string Content { get; set; }

        private readonly Uri fullContentLink;

        public CourseContentResourceService(IDownloadService downloadService, IBaseModule module, IConstants constants)
        {
            this.downloadService = downloadService;

            this.BaseModule = module;
            this.Content = BaseModule.ToString();

            // Get all course content.
            this.fullContentLink = new Uri(constants.ServicePrefix + string.Format(CourseContentLink, this.BaseModule.Id, constants.SiteVersion, constants.CultureCode, constants.PartnerCode));

            // Download activity content.
            this.Content = downloadService.DownloadFromPath(this.fullContentLink);
        }
    }
}
