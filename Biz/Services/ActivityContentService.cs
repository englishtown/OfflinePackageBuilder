using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Biz.Models;

namespace Biz.Services
{
    public class ActivityContentService : IContentServcie
    {
        private const string courseLink = "/services/school/courseware/GetActivityXml.ashx?actvityId={0}&partnerCode={1}&cultureCode={2}&siteVersion={3}&showBlurbs=0&consistentCacheSvr=true&jsoncallback=_jsonp_";
        private readonly Uri fullContentLink;

        private readonly IDownloadService downloadService;

        public IBaseModule BaseModule { get; set; }
        public string Content { get; set; }

        public ActivityContentService(IDownloadService downloadService, Activity activity, IConstants constants)
        {
            // TODO:: How to test?
            this.downloadService = downloadService;

            this.BaseModule = activity as Activity;

            // Get all course content.
            this.fullContentLink = new Uri(constants.ServicePrefix + string.Format(courseLink, this.BaseModule.Id, constants.PartnerCode, constants.PartnerCode, constants.SiteVersion));

            // Download activity content.
            this.Content = downloadService.DownloadFromPath(this.fullContentLink);

            // Replace swf to jpg, flv to mp4
            ReplaceUrlFileFormat();

            // Parse the content to get the media resource.
            (this.BaseModule as Activity).MediaResources = GetMediaResources(Content);

            ReplaceUrlToLocalResourcePath();
        }

        /// Get the list of media resource path in the activity.</returns>
        public IList<string> GetMediaResources(string activityContent)
        {
            IList<string> list = new List<string>();

            Regex r = new Regex(@"(?<=http://\w+.englishtown.com)/Juno/[\s\S]*?(\.mp3|\.jpg|\.png|\.gif|\.bmp|\.mp4|\.f4v|\.m3u8|\.swf)", RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(activityContent);

            for (int j = 0; j < m.Count; j++)
            {
                var a = m[j].Value.ToLower();
                list.Add(a);
            }

            return list;
        }

        // Repalce live path to local.
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

        // Replace some swf to mp4.
        protected void ChangeContent(string pattern, MatchEvaluator evaluator)
        {
            Regex grx = new Regex(pattern, RegexOptions.IgnoreCase);
            if (grx.IsMatch(this.Content))
            {
                this.Content = grx.Replace(this.Content, evaluator);
            }
        }

        /// <summary>
        /// Change "http://local-ak.englishtown.com/Juno/school/videos/0a.2%20Scene%201.f4v" to localResourcePath+"Juno/school/videos/0a.2 Scene 1.mp4
        /// Change "http://local.englishtown.com/Juno/school/videos/123.swf" to localResourcePath+"Juno/school/imgs_epaper/123.jpg
        /// </summary>
        /// <returns></returns>
        public void ReplaceUrlFileFormat()
        {
            string f4vPattern = "/Juno/school/videos/(?<fileName>.+?).f4v";
            string swfPattern = "/Juno/school/videos/(?<fileName>.+?).swf";

            //step2: replace /Juno/school/videos/1.f4v to /Juno/school/videos/1.mp4
            ChangeContent(f4vPattern,
                match => string.Format("/Juno/school/videos/{0}.mp4", Uri.UnescapeDataString(match.Groups["fileName"].ToString()))
            );

            //step3: replace "/Juno/school/videos/1.swf" to "/Juno/school/imgs_epaper/1.jpg"
            ChangeContent(swfPattern,
                match => string.Format("/Juno/school/imgs_epaper/{0}.jpg", Uri.UnescapeDataString(match.Groups["fileName"].ToString()))
            );
        }
    }
}
