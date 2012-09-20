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
    public class Unit : IBaseModule
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public int unitNo { get; set; }

        [JsonProperty]
        public string unitName { get; set; }

        [JsonProperty]
        public string unitDescr { get; set; }

        [JsonProperty]
        public IList<Lesson> Lessons { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }
        public JToken jToken { get; set; }

        public Unit(Dictionary<string, List<JToken>> csArray, int unitId, IBaseModule parentModule)
        {
            var jModule =
                 from p in csArray["unit"].AsParallel()
                 where p["id"].ToString().Equals("unit!" + unitId)
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
            this.unitName = jToken["unitName"].ToString();
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            this.Lessons = new List<Lesson>();

            foreach (var s in jToken["lessons"].Children())
            {
                var unitId = s["id"].ToString().GetId();
                this.Lessons.Add(new Lesson(csArray, unitId, this));
            }
        }
    }
}
