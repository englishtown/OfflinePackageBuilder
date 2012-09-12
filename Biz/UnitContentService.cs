using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Biz
{
    public class UnitContentService
    {
        private const string unitLink = "/services/school/courseware/GetActivityXml.ashx?actvityId={0}&cultureCode={1}&siteVersion={2}&partnerCode={3}&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";
        private readonly Uri fullContentLink;

        public int Id { get; set; }
        public string Content { get; set; }

        public UnitContentService(int id, string siteVersion, string cultureCode, string partnerCode)
        {
            this.Id = id;

            // Get all course content.
            this.fullContentLink = new Uri(ConstantsDefault.SitePrefix + string.Format(unitLink, this.Id, siteVersion, cultureCode, partnerCode));
        }

        public void DownloadTo(string path)
        {
            WebClient c = new WebClient();
            c.Headers.Add("Content-Type", "application/json; charset=utf-8");

            // Save the content to some path use Async
            c.DownloadFileAsync(this.fullContentLink, path);
        }
    }
}
