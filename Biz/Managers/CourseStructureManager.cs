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

namespace Biz.Managers
{
    public class CourseStructureManager : ICourseStructureManager
    {
        private const string courseLink = "/services/school/query?q=course!{0}.*&c=siteversion={1}|cultureCode={2}|partnerCode={3}";

        private readonly IDownloadService downloadService;
        private readonly PackageService ps;

        // Course ID
        public int Id { get; set; }
        public Course Course { get; set; }
        public string SiteVersion { get; set; }
        public string CultureCode { get; set; }
        public string PartnerCode { get; set; }

        public CourseStructureManager(IDownloadService ds, int courseId, string siteVersion, string cultureCode, string partnerCode)
        {
            this.downloadService = ds;
            this.ps = new PackageService();

            this.Id = courseId;
            this.SiteVersion = siteVersion;
            this.CultureCode = cultureCode;
            this.PartnerCode = partnerCode;
        }

        // Build Course structure in memory.
        public Course BuildCourseStructure()
        {
            var content = DownloadCourseStructure();
            BuildCourseStructure(content);

            return this.Course;
        }

        // Download the course from new api, maybe need download by level.
        private string DownloadCourseStructure()
        {
            // Get all course content.
            var fullContentLink = new Uri(ConstantsDefault.ServicePrefix + string.Format(courseLink, this.Id, this.SiteVersion, this.CultureCode, this.PartnerCode));

            return this.downloadService.DownloadFromPath(fullContentLink);
        }

        // Build Course structure in memory.
        private void BuildCourseStructure(string courseStructureContent)
        {
            var b = BuildCourseArray(courseStructureContent);
            this.Course = BuildCourse(this.Id, b);
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
    }
}
