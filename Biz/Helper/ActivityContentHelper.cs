using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Biz.Helper
{
    public class ActivityContentHelper
    {
        // Repalce live path to local.
        public static void ReplaceUrlToLocalResourcePath(ref string content)
        {
            string localResourcePathPattern = "\"http://([0-9.]+|[a-z0-9\\-._~%]+.englishtown.com)";
            //for the activity like activityId=29652
            string inlineLocalResourcePathPattern = "http://([0-9.]+|[a-z0-9\\-._~%]+.englishtown.com)";

            ChangeContent(ref content, localResourcePathPattern,
                match => "localResourcePath+\""
            );

            ChangeContent(ref content, inlineLocalResourcePathPattern,
                match => "localResourcePath"
            );
        }

        // Replace some swf to mp4.
        public static void ChangeContent(ref string content, string pattern, MatchEvaluator evaluator)
        {
            Regex grx = new Regex(pattern, RegexOptions.IgnoreCase);
            if (grx.IsMatch(content))
            {
                content = grx.Replace(content, evaluator);
            }
        }

        /// <summary>
        /// Change "http://local-ak.englishtown.com/Juno/school/videos/0a.2%20Scene%201.f4v" to localResourcePath+"Juno/school/videos/0a.2 Scene 1.mp4
        /// Change "http://local.englishtown.com/Juno/school/videos/123.swf" to localResourcePath+"Juno/school/imgs_epaper/123.jpg
        /// </summary>
        /// <returns></returns>
        public static void ReplaceUrlFileFormat(ref string content)
        {
            string f4vPattern = "/Juno/school/videos/(?<fileName>.+?).f4v";
            string swfPattern = "/Juno/school/videos/(?<fileName>.+?).swf";

            //step2: replace /Juno/school/videos/1.f4v to /Juno/school/videos/1.mp4
            ChangeContent(ref content, f4vPattern,
                match => string.Format("/Juno/school/videos/{0}.mp4", Uri.UnescapeDataString(match.Groups["fileName"].ToString()))
            );

            //step3: replace "/Juno/school/videos/1.swf" to "/Juno/school/imgs_epaper/1.jpg"
            ChangeContent(ref content, swfPattern,
                match => string.Format("/Juno/school/imgs_epaper/{0}.jpg", Uri.UnescapeDataString(match.Groups["fileName"].ToString()))
            );
        }

        /// Get the list of media resource path in the activity.</returns>
        public static IList<string> GetMediaResources(ref string activityContent)
        {
            //Replace flv to mp4
            ReplaceUrlFileFormat(ref activityContent);

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
    }
}
