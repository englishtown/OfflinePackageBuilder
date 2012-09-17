using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Biz.Extensions;

namespace Biz.Models
{
    public class Lesson : IBaseModule
    {
        public int Id { get; set; }
        public IList<Step> Steps { get; set; }

        // for unit
        public string LessonNo { get; set; }
        public string LessonTopic { get; set; }
        public string LessonTypeName { get; set; }
        public string LessonDescr { get; set; }
        public int Lessontype_id { get; set; }

        // For lesson Meida Package
        public long MeidaOriSize { get; set; }
        public long MeidaZipedSize { get; set; }
        public string RemotePath { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }
        public IEnumerable<JToken> jModule { get; set; }

        public Lesson(Dictionary<string, List<JToken>> csArray, int lessonId, IBaseModule parentModule)
        {
            this.jModule =
                 from p in csArray["lesson"].AsParallel()
                 where p["id"].ToString().Equals("lesson!" + lessonId)
                 select p;

            this.ParentModule = parentModule;

            //
            BuildModule();

            BuildSubmodule(csArray);
        }

        public void BuildModule()
        {
            this.Id = jModule.First()["id"].ToString().GetId();
            this.LessonNo = jModule.First()["lessonNo"].ToString();
            this.LessonTopic = jModule.First()["lessonTopic"].ToString();
            this.LessonTypeName = jModule.First()["lessonTypeName"].ToString();
            this.LessonDescr = jModule.First()["lessonDescr"].ToString();
            this.Lessontype_id = int.Parse(jModule.First()["lessonType_id"].ToString());
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            this.Steps = new List<Step>();

            foreach (var s in jModule.First()["steps"].Children())
            {
                var stepId = s["id"].ToString().GetId();
                this.Steps.Add(new Step(csArray, stepId, this));
            }
        }
    }
}
