using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Biz.Models;

using Biz.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz
{
    public class CourseStructureManager : ICourseStructureManager
    {
        private const string courseLink = "/services/school/query?q=course!{0}.*&c=siteversion={1}|cultureCode={2}|partnerCode={3}";

        private readonly IDownloadManager dm;

        public int Id { get; set; }
        public Course Course { get; set; }
        public string SiteVersion { get; set; }
        public string CultureCode { get; set; }
        public string PartnerCode { get; set; }

        public LogEntry Logger { get; set; }

        public CourseStructureManager(IDownloadManager dm, int courseId, string siteVersion, string cultureCode, string partnerCode)
        {
            this.dm = dm;

            this.SiteVersion = siteVersion;
            this.CultureCode = cultureCode;
            this.PartnerCode = partnerCode;

            var a = DownloadCourseStructure(courseId);
            var b = BuildCourseArray(a);
            this.Course = BuildCourse(courseId, b);

            DownloadActivityContentByLevel();
        }

        // Download the course from new api, maybe need download by level.
        public string DownloadCourseStructure(int courseId)
        {
            // Get all course content.
            var fullContentLink = new Uri(ConstantsDefault.ServicePrefix + string.Format(courseLink, courseId, this.SiteVersion, this.CultureCode, this.PartnerCode));

            return dm.DownloadFromPath(fullContentLink);
        }

        // Splite the all course to different list.
        // For speed.
        private Dictionary<string, List<JToken>> BuildCourseArray(string courseContent)
        {
            // Course structure in JSON format.
            var ccArray = JArray.Parse(courseContent).Children();

            List<JToken> courseArray = new List<JToken>();
            List<JToken> levelArray = new List<JToken>();
            List<JToken> unitArray = new List<JToken>();
            List<JToken> lessonArray = new List<JToken>();
            List<JToken> stepArray = new List<JToken>();
            List<JToken> activityArray = new List<JToken>();

            Dictionary<string, List<JToken>> csArray = new Dictionary<string, List<JToken>>();

            csArray.Add("course", courseArray);
            csArray.Add("level", levelArray);
            csArray.Add("unit", unitArray);
            csArray.Add("lesson", lessonArray);
            csArray.Add("step", stepArray);
            csArray.Add("activity", activityArray);

            foreach (var item in ccArray)
            {
                switch (item["id"].ToString().GetETType())
                {
                    case "course":
                        courseArray.Add(item);
                        break;
                    case "level":
                        levelArray.Add(item);
                        break;
                    case "unit":
                        unitArray.Add(item);
                        break;
                    case "lesson":
                        lessonArray.Add(item);
                        break;
                    case "step":
                        stepArray.Add(item);
                        break;
                    case "activity":
                        activityArray.Add(item);
                        break;
                }
            }

            return csArray;
        }

        // Generate the content to a tree.
        private Course BuildCourse(int courseId, Dictionary<string, List<JToken>> csArray)
        {
            return new Course(courseId, csArray);
        }

        public void DownloadActivityContentByLevel()
        {
            // Get all Activities under the level.
            foreach (Level l in this.Course.Levels)
            {
                foreach (Unit u in l.Units)
                {
                    DownloadActivityContentByUnit(u);
                }
            }
        }

        private void DownloadActivityContentByUnit(Unit unit)
        {
            // Get all activities under the unit.
            foreach (Lesson l in unit.Lessons)
            {
                DownloadActivityContentByLesson(l);
            }
        }

        private void DownloadActivityContentByLesson(Lesson lesson)
        {
            foreach (Step s in lesson.Steps)
            {
                foreach (Activity a in s.Activities)
                {
                    int unitId = a.ParentModule.ParentModule.ParentModule.Id;
                    int lessonId = a.ParentModule.Id;

                    ActivityContentService acs = new ActivityContentService(this.dm, a, this.SiteVersion, this.CultureCode, this.PartnerCode);
                    acs.DownloadTo(@"d:\offline\activities\" + "unit" + unitId + @"\" + this.CultureCode + @"\" + a.Id + ".json");

                    foreach (var mediaPath in a.MediaResources)
                    {
                        IDownloadManager d = new DownloadManager();
                        IContentServcie mediaService = new MediaResourceService(mediaPath, d);
                        var path = @"d:\offline\meida\" + "lesson" + lessonId + @"\" + this.CultureCode + mediaPath;
                        mediaService.DownloadTo(path);
                    }
                }
            }
        }
    }
}
