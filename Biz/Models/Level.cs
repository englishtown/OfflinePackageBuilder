using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using Biz.Extensions;

namespace Biz.Models
{
    public class Level : IBaseModule
    {
        public int Id { get; set; }
        public IList<Unit> Units { get; set; }

        public string levelCode { get; set; }
        public int levelNo { get; set; }
        public string levelName { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }

        public IEnumerable<JToken> jModule { get; set; }

        public Level(JArray courseStructure, int levelId, IBaseModule parentModule)
        {
            this.jModule =
                 from p in courseStructure.Children()
                 where p["id"].ToString().Equals("level!" + levelId)
                 select p;

            this.ParentModule = parentModule;

            //
            BuildModule();

            BuildSubmodule(courseStructure);
        }

        public void BuildModule()
        {
            this.levelCode = jModule.First()["levelCode"].ToString();
            this.levelNo = int.Parse(jModule.First()["levelNo"].ToString());
            this.levelName = jModule.First()["levelName"].ToString();
        }

        public void BuildSubmodule(JArray courseStructure)
        {
            this.Units = new List<Unit>();

            foreach (var s in jModule.First()["units"].Children())
            {
                var unitId = s["id"].ToString().GetId();
                this.Units.Add(new Unit(courseStructure, unitId, this));
            }
        }
    }
}
