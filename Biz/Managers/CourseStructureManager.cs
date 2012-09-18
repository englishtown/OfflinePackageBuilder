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

namespace Biz.Managers
{
    public class CourseStructureManager : ICourseStructureManager
    {
        private readonly IDownloadService downloadService;
        private readonly IConstants constants;
        private readonly IContentResourceServcie courseContentResourceService;

        // Course ID
        public int Id { get; set; }
        public Course Course { get; set; }

        public CourseStructureManager(IDownloadService ds, IContentResourceServcie resource, IConstants constants)
        {
            this.downloadService = ds;
            this.constants = constants;
            this.courseContentResourceService = resource;
        }

        // Build Course structure in memory.
        public Course BuildCourseStructure()
        {
            var courseStructureContent = this.courseContentResourceService.Content;

            var courseArray = BuildCourseArray(courseStructureContent);

            this.Course = new Course(this.constants.CourseId, courseArray);

            return this.Course;
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
