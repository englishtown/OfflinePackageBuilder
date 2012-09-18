using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Biz.Models;

namespace Biz.Services
{
    public class UnitContentResourceService : IContentResourceServcie
    {
        private const string unitLink = "/services/school/courseware/GetUnitStructure.ashx?unit_id={0}&cultureCode={1}&siteVersion={2}&from=tablet&UiType=HTML5&partnerCode={3}&areaCode=&marketCode=us&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";
        
        private readonly Uri fullContentLink;
        private readonly IDownloadService downloadService;

        public int ModuleId { get; set; }
        public string Content { get; set; }

        private readonly IConstants constants;

        public UnitContentResourceService(IDownloadService downloadService, int unitId, IConstants constants)
        {
            this.downloadService = downloadService;
            this.ModuleId = unitId;
            this.constants = constants;

            // Get all course content.
            this.fullContentLink = new Uri(constants.ServicePrefix + string.Format(unitLink, this.ModuleId, this.constants.CultureCode, this.constants.SiteVersion, this.constants.PartnerCode));

            // Download unit content.
            this.Content = downloadService.DownloadFromPath(this.fullContentLink);
        }
    }
}
