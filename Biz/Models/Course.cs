using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using Biz.Extensions;

namespace Biz.Models
{
    public class Course : IBaseModule
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseTypeCode { get; set; }

        public IEnumerable<JToken> jModule { get; set; }

        public IBaseModule ParentModule { get; set; }

        public IList Levels { get; set; }

        public IList<IBaseModule> SubModules
        {
            get { return this.Levels as IList<IBaseModule>; }
            set { value = this.Levels as IList<IBaseModule>; }
        }

        public Course(int moduleId, Dictionary<string, List<JToken>> csArray)
        {
            this.jModule =
               from p in csArray["course"].AsParallel()
               where p["id"].ToString().Equals("course!" + moduleId)
               select p;

            //Build
            BuildModule();

            BuildSubmodule(csArray);
        }

        public void BuildModule()
        {
            this.Id = jModule.First()["id"].ToString().GetId();
            this.CourseName = jModule.First()["courseName"].ToString();
            this.CourseTypeCode = jModule.First()["courseTypeCode"].ToString();
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            this.Levels = new List<Level>();

            foreach (var s in jModule.First()["levels"].Children())
            {
                var levelId = s["id"].ToString().GetId();
                this.Levels.Add(new Level(csArray, levelId, this));
            }
        }
    }
}
