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
    public class Level : IBaseModule
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string levelCode { get; set; }

        [JsonProperty]
        public int levelNo { get; set; }

        [JsonProperty]
        public string levelName { get; set; }

        // Content Info
        [JsonProperty]
        public string RemotePath { get; set; }

        [JsonProperty]
        public long ContentOriSize { get; set; }

        [JsonProperty]
        public long ContentZippedSize { get; set; }

        [JsonProperty]
        public IList<Unit> Units { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }

        public JToken jToken { get; set; }

        public Level(Dictionary<string, List<JToken>> csArray, int levelId, IBaseModule parentModule)
        {
            var jModule =
                 from p in csArray["level"].AsParallel()
                 where p["id"].ToString().Equals("level!" + levelId)
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
            this.levelCode = jToken["levelCode"].ToString();
            this.levelNo = int.Parse(jToken["levelNo"].ToString());
            this.levelName = jToken["levelName"].ToString();
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            this.Units = new List<Unit>();

            foreach (var s in jToken["units"].Children())
            {
                var unitId = s["id"].ToString().GetId();
                this.Units.Add(new Unit(csArray, unitId, this));
            }
        }
    }
}
