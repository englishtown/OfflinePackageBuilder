using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using Biz.Extensions;
using Newtonsoft.Json;

namespace Biz.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Course : IBaseModule
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string CourseName { get; set; }

        [JsonProperty]
        public string CourseTypeCode { get; set; }

        [JsonProperty]
        public int MaxAge { get; set; }

        [JsonProperty]
        public IList Levels { get; set; }

        public JToken jToken { get; set; }
        public IBaseModule ParentModule { get; set; }

        public IList<IBaseModule> SubModules
        {
            get { return this.Levels as IList<IBaseModule>; }
            set { value = this.Levels as IList<IBaseModule>; }
        }

        public Course(int moduleId, Dictionary<string, List<JToken>> csArray)
        {
            var jModule =
               from p in csArray["course"].AsParallel()
               where p["id"].ToString().Equals("course!" + moduleId)
               select p;

            this.jToken = jModule.First();

            //Build
            BuildModule();

            BuildSubmodule(csArray);
        }

        public void BuildModule()
        {
            this.Id = jToken["id"].ToString().GetId();
            this.CourseName = jToken["courseName"].ToString();
            this.CourseTypeCode = jToken["courseTypeCode"].ToString();
            this.MaxAge = int.Parse(jToken["maxAge"].ToString());
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            this.Levels = new List<Level>();

            foreach (var s in this.jToken["levels"].Children())
            {
                var levelId = s["id"].ToString().GetId();
                this.Levels.Add(new Level(csArray, levelId, this));
            }
        }
    }
}
