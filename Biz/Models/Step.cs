using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Biz.Extensions;

namespace Biz.Models
{
    public class Step : IBaseModule
    {
        public int Id { get; set; }
        public IList<Activity> Activities { get; set; }

        // for unit
        public int StepNo { get; set; }
        public int StepType_id { get; set; }
        public string StepTitle { get; set; }
        public string StepTypeName { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }
        public IEnumerable<JToken> jModule { get; set; }

        public Step(JArray courseStructure, int lessonId, IBaseModule parentModule)
        {
            this.jModule =
                 from p in courseStructure.Children()
                 where p["id"].ToString().Equals("step!" + lessonId)
                 select p;

            this.ParentModule = parentModule;

            //
            BuildModule();

            BuildSubmodule(courseStructure);
        }

        public void BuildModule()
        {
            this.StepNo = int.Parse(jModule.First()["stepNo"].ToString());
            this.StepType_id = int.Parse(jModule.First()["stepType_id"].ToString());
            this.StepTitle = jModule.First()["stepTitle"].ToString();
            this.StepTypeName = jModule.First()["stepTypeName"].ToString();
        }

        public void BuildSubmodule(JArray courseStructure)
        {
            this.Activities = new List<Activity>();

            foreach (var s in jModule.First()["activities"].Children())
            {
                var activityId = s["id"].ToString().GetId();
                this.Activities.Add(new Activity(courseStructure, activityId, this));
            }
        }
    }
}
