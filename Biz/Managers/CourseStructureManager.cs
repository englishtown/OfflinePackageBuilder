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
using Biz.Services;
using Biz.Manager;

namespace Biz.Managers
{
    public class CourseStructureManager : ICourseStructureManager
    {
        private const string courseLink = "/services/school/query?q=course!{0}.*&c=siteversion={1}|cultureCode={2}|partnerCode={3}";

        private readonly IDownloadManager downloadService;
        private readonly IConstants constants;

        // Course ID
        public int Id { get; set; }
        public Course Course { get; set; }

        public CourseStructureManager(IDownloadManager ds, int courseId, IConstants constants)
        {
            this.downloadService = ds;
            this.constants = constants;

            this.Id = courseId;
        }

        // Build Course structure in memory.
        public Course BuildCourseStructure()
        {
            var courseStructureContent = DownloadCourseStructure();

            var courseArray = BuildCourseArray(courseStructureContent);

            this.Course = new Course(this.Id, courseArray);

            return this.Course;
        }

        // Download the course from new api, maybe need download by level.
        private string DownloadCourseStructure()
        {
            // Get all course content.
            var fullContentLink = new Uri(this.constants.ServicePrefix + string.Format(courseLink, this.Id, this.constants.SiteVersion, this.constants.CultureCode, this.constants.PartnerCode));
            return this.downloadService.DownloadFromPath(fullContentLink);
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
    }
}
