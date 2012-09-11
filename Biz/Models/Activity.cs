using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Biz.Models
{
    public class Activity : IBaseModule
    {
        public int Id { get; set; }

        // for Activity
        public int ActivityNo { get; set; }
        public int GradeMode_id { get; set; }
        public string RemotePath { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }
        public IEnumerable<JToken> jModule { get; set; }

        public Activity(JArray courseStructure, int activityId, IBaseModule parentModule)
        {
            this.jModule =
                 from p in courseStructure.Children()
                 where p["id"].ToString().Equals("activity!" + activityId)
                 select p;

            this.ParentModule = parentModule;
            this.SubModules = null;

            //
            BuildModule();

            BuildSubmodule(courseStructure);
        }

        public void BuildModule()
        {
            this.ActivityNo = int.Parse(jModule.First()["activityNo"].ToString());
            this.GradeMode_id = int.Parse(jModule.First()["gradeMode_id"].ToString());
        }

        public void BuildSubmodule(JArray courseStructure)
        {
            return;
        }
    }
}
