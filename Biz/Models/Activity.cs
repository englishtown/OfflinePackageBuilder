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
    public class Activity : IBaseModule
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public int ActivityNo { get; set; }

        [JsonProperty]
        public int GradeMode_id { get; set; }

        // Add media download path at here.
        public IList<string> MediaResources { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }
        public JToken jToken { get; set; }

        public Activity(Dictionary<string, List<JToken>> csArray, int activityId, IBaseModule parentModule)
        {
            var jModule =
                 from p in csArray["activity"]
                 where p["id"].ToString().Equals("activity!" + activityId)
                 select p;

            this.jToken = jModule.First();

            this.ParentModule = parentModule;
            this.SubModules = null;

            //
            BuildModule();

            BuildSubmodule(csArray);
        }

        public void BuildModule()
        {
            this.Id = jToken["id"].ToString().GetId();
            this.ActivityNo = int.Parse(jToken["activityNo"].ToString());
            this.GradeMode_id = int.Parse(jToken["gradeMode_id"].ToString());
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            return;
        }
    }
}
