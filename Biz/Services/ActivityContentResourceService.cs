using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Biz.Models;
using Biz.Helper;

namespace Biz.Services
{
    public class ActivityContentResourceService : IContentResourceServcie
    {
        private const string courseLink = "/services/school/courseware/GetActivityXml.ashx?actvityId={0}&partnerCode={1}&cultureCode={2}&siteVersion={3}&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";
        private readonly Uri fullContentLink;

        private readonly IDownloadService downloadService;

        public int ModuleId { get; set; }
        public string Content { get; set; }

        public ActivityContentResourceService(IDownloadService downloadService, int activityId, IConstants constants)
        {
            this.downloadService = downloadService;

            this.ModuleId = activityId;

            // Get all course content.
            this.fullContentLink = new Uri(constants.ServicePrefix + string.Format(courseLink, this.ModuleId, constants.PartnerCode, constants.CultureCode, constants.SiteVersion));

            // Download activity content.
            string oriContent = downloadService.DownloadFromPath(this.fullContentLink);

            this.Content = oriContent;
        }       
    }
}
