using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz.Models
{
    public class ActivityContentService : IContentServcie
    {
        private const string courseLink = "/services/school/courseware/GetActivityXml.ashx?actvityId={0}&partnerCode={1}&cultureCode={2}&siteVersion={3}&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";
        private readonly Uri fullContentLink;

        private readonly IDownloadManager dm;

        public int Id { get; set; }
        public Activity Activity { get; set; }
        public string Content { get; set; }

        public LogEntry Logger { get; set; }

        public ActivityContentService(IDownloadManager dm, Activity activity, string siteVersion, string cultureCode, string partnerCode)
        {
            // TODO:: How to test?
            this.dm = new DownloadManager();

            this.Activity = activity;
            this.Id = activity.Id;

            // Get all course content.
            this.fullContentLink = new Uri(ConstantsDefault.ServicePrefix + string.Format(courseLink, this.Id, partnerCode, cultureCode, siteVersion));
        }

        public void DownloadTo(string path)
        {
            this.Content = dm.DownloadFromPath(this.fullContentLink);
            this.Activity.MediaResources = GetMediaResources(Content);

            ReplaceUrlToLocalResourcePath();

            // Save localed path to disk.
            dm.SaveTo(this.Content, path);
        }

        /// Get the list of media resource path in the activity.</returns>
        public IList<string> GetMediaResources(string activityContent)
        {
            IList<string> list = new List<string>();

            Regex r = new Regex(@"(?<=http://\w+.englishtown.com)/(Juno|juno)/[\s\S]*?(.mp3|.jpg|.png|.mp4|.f4v|.swf)", RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(activityContent);

            for (int j = 0; j < m.Count; j++)
            {
                var a = m[j].Value.ToLower();
                list.Add(a);
            }

            return list;
        }


        protected void ReplaceUrlToLocalResourcePath()
        {
            string localResourcePathPattern = "\"http://([0-9.]+|[a-z0-9\\-._~%]+.englishtown.com)";
            //for the activity like activityId=29652
            string inlineLocalResourcePathPattern = "http://([0-9.]+|[a-z0-9\\-._~%]+.englishtown.com)";

            ChangeContent(localResourcePathPattern,
                match => "localResourcePath+\""
            );

            ChangeContent(inlineLocalResourcePathPattern,
                match => "localResourcePath"
            );
        }

        protected void ChangeContent(string pattern, MatchEvaluator evaluator)
        {
            Regex grx = new Regex(pattern, RegexOptions.IgnoreCase);
            if (grx.IsMatch(this.Content))
            {
                this.Content = grx.Replace(this.Content, evaluator);
            }
        }

        public void Zip()
        {
        }
    }
}
