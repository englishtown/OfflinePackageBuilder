using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Biz.Models;

using Biz.Extensions;

namespace Biz
{
    public class CourseStructure
    {
        private const string courseLink = "http://mobiledev.englishtown.com/services/school/query?q=course!{0}.*&c=siteversion={1}|cultureCode={2}|partnerCode={3}";

        // Course structure in JSON format.
        private readonly JArray courseStructureArray = null;
        public Course course { get; private set; }

        public CourseStructure(int courseId, string siteVersion, string cultureCode, string partnerCode)
        {
            // Get all course content.
            var fullCourseLink = string.Format(courseLink, courseId, siteVersion, cultureCode, partnerCode);

            WebClient c = new WebClient();
            c.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var courseContent = c.DownloadString(fullCourseLink);

            this.courseStructureArray = JArray.Parse(courseContent);

            var abc = this.courseStructureArray.Children();

            this.course = BuildCourse(courseId);

            var bc = "ddd";
        }

        public void a()
        {
            var levels =
                from p in courseStructureArray.Children()
                where p["id"].ToString().StartsWith("level")
                select p;

            foreach (var cd in levels)
            {
                string a = cd["id"].ToString();
            }
            //this.courseStructure = JsonConvert.DeserializeObject(courseContent);
        }

        //
        public Course BuildCourse(int courseId)
        {
            return new Course(courseStructureArray, courseId);
        }

        public void BuildLevels()
        {
            var levels = new List<Level>();

            var lls =
                from p in courseStructureArray.Children()
                where p["id"].ToString().Equals("course!" + course.Id)
                select p["levels"];

            foreach (var l in lls)
            {
                var lvs =
                    from p in courseStructureArray.Children()
                    where p["id"].ToString().Equals("level!" + l["id"].ToString())
                    select p;

            }

            this.course.Levels = levels;
        }
    }
}
