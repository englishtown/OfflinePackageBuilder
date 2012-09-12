using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Biz.Extensions;

namespace Biz.Models
{
    public class Unit : IBaseModule
    {
        public int Id { get; set; }
        public IList<Lesson> Lessons { get; set; }

        // for unit
        public int unitNo { get; set; }
        public string unitName { get; set; }
        public string unitDescr { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }
        public IEnumerable<JToken> jModule { get; set; }

        public Unit(Dictionary<string, List<JToken>> csArray, int unitId, IBaseModule parentModule)
        {
            this.jModule =
                 from p in csArray["unit"].AsParallel()
                 where p["id"].ToString().Equals("unit!" + unitId)
                 select p;

            this.ParentModule = parentModule;

            //
            BuildModule();

            BuildSubmodule(csArray);
        }

        public void BuildModule()
        {
            this.Id = jModule.First()["id"].ToString().GetId();
            this.unitName = jModule.First()["unitName"].ToString();
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            this.Lessons = new List<Lesson>();

            foreach (var s in jModule.First()["lessons"].Children())
            {
                var unitId = s["id"].ToString().GetId();
                this.Lessons.Add(new Lesson(csArray, unitId, this));
            }
        }
    }
}
