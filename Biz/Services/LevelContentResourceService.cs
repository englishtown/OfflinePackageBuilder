using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Biz.Models;

namespace Biz.Services
{
    public class LevelContentResourceService : IContentResourceServcie
    {
        private const string levelLink = "/services/school/courseware/CourseStructure/GetLevelStructure?level_id={0}&cultureCode={1}&siteVersion={2}&partnerCode={3}&jsoncallback=_jsonp_";
        
        private readonly Uri fullContentLink;
        private readonly IDownloadService downloadService;

        public int ModuleId { get; set; }
        public string Content { get; set; }

        public LevelContentResourceService(IDownloadService downloadService, int levelId, IConstants constants)
        {
            this.downloadService = downloadService;

            this.ModuleId = levelId;

            // Get all course content.
            this.fullContentLink = new Uri(constants.ServicePrefix + string.Format(levelLink, this.ModuleId, constants.CultureCode, constants.SiteVersion, constants.PartnerCode));

            // Download activity content.
            this.Content = downloadService.DownloadFromPath(this.fullContentLink);
        }
    }
}
