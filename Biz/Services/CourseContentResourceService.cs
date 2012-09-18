using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz.Services
{
    public class CourseContentResourceService : IContentResourceServcie
    {
        private const string courseLink = "/services/school/query?q=course!{0}.*&c=siteversion={1}|cultureCode={2}|partnerCode={3}";

        // not used in here.
        public int ModuleId { get; set; }

        private readonly IDownloadService downloadService;
        private readonly IConstants constants; 

        public string Content { get; set; }

        //private readonly Uri fullContentLink;

        public CourseContentResourceService(IDownloadService downloadService, IConstants constants)
        {
            this.ModuleId = constants.CourseId;
            this.downloadService = downloadService;
            this.constants = constants;

            // Get all course content.
            var fullContentLink = new Uri(this.constants.ServicePrefix + string.Format(courseLink, this.ModuleId, this.constants.SiteVersion, this.constants.CultureCode, this.constants.PartnerCode));

            // Download activity content.
            this.Content = downloadService.DownloadFromPath(fullContentLink);
        }
    }
}