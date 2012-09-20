using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Biz.Extensions;
using Newtonsoft.Json;

namespace Biz.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Step : IBaseModule
    {
        [JsonProperty]
        public int Id { get; set; }

        // for unit
        [JsonProperty]
        public int StepNo { get; set; }

        [JsonProperty]
        public int StepType_id { get; set; }

        [JsonProperty]
        public string StepTitle { get; set; }

        [JsonProperty]
        public string StepTypeName { get; set; }

        [JsonProperty]
        public IList<Activity> Activities { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }
        public JToken jToken { get; set; }

        public Step(Dictionary<string, List<JToken>> csArray, int lessonId, IBaseModule parentModule)
        {
            var jModule =
                from p in csArray["step"].AsParallel()
                where p["id"].ToString().Equals("step!" + lessonId)
                select p;

            this.jToken = jModule.First();

            this.ParentModule = parentModule;

            //
            BuildModule();

            BuildSubmodule(csArray);
        }

        public void BuildModule()
        {
            this.Id = jToken["id"].ToString().GetId();
            this.StepNo = int.Parse(jToken["stepNo"].ToString());
            this.StepType_id = int.Parse(jToken["stepType_id"].ToString());
            this.StepTitle = jToken["stepTitle"].ToString();
            this.StepTypeName = jToken["stepTypeName"].ToString();
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            this.Activities = new List<Activity>();

            foreach (var s in jToken["activities"].Children())
            {
                var activityId = s["id"].ToString().GetId();
                this.Activities.Add(new Activity(csArray, activityId, this));
            }
        }
    }
}
