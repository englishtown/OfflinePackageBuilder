using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Biz.Models;

namespace Biz.Services
{
    public class LevelStructureContentResourceService : IContentResourceServcie
    {
        private const string levelLink = "/services/school/query?q=level!{0}.units.lessons.steps.activities,.units.unitImage,.units.lessons.lessonImage&c=siteversion={1}|cultureCode={2}|partnerCode={3}";

        private readonly Uri fullContentLink;
        private readonly IDownloadService downloadService;

        public int ModuleId { get; set; }
        public string Content { get; set; }

        public LevelStructureContentResourceService(IDownloadService downloadService, int levelId, IConstants constants)
        {
            this.downloadService = downloadService;

            this.ModuleId = levelId;

            // Get all course content.
            this.fullContentLink = new Uri(constants.ServicePrefix + string.Format(levelLink, this.ModuleId, constants.SiteVersion, constants.CultureCode, constants.PartnerCode));

            // Download activity content.
            this.Content = downloadService.DownloadFromPath(this.fullContentLink);

            // Covert to JSON

        }
    }
}
