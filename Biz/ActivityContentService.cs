using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Biz.Models
{
    public class ActivityContentService : IContentServcie
    {
        private const string courseLink = "/services/school/courseware/GetActivityXml.ashx?actvityId={0}&partnerCode={1}&cultureCode={2}&siteVersion={3}&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";
        private readonly Uri fullContentLink;

        private readonly IDownloadManager dm;

        public int Id { get; set; }
        public string Content { get; set; }

        public ActivityContentService(IDownloadManager dm, int id, string siteVersion, string cultureCode, string partnerCode)
        {
            this.dm = new DownloadManager();

            this.Id = id;

            // Get all course content.
            this.fullContentLink = new Uri(ConstantsDefault.SitePrefix + string.Format(courseLink, this.Id, partnerCode, cultureCode, siteVersion));
        }

        public void DownloadTo(string path)
        {
            dm.DownloadFromPathTo(this.fullContentLink, path);
        }


        public void Zip()
        {
        }
    }
}
