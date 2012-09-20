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
    public class Lesson : IBaseModule
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string LessonNo { get; set; }

        [JsonProperty]
        public string LessonTopic { get; set; }

        [JsonProperty]
        public string LessonTypeName { get; set; }

        [JsonProperty]
        public string LessonDescr { get; set; }

        [JsonProperty]
        public int Lessontype_id { get; set; }

        // For lesson Meida Package
        [JsonProperty]
        public long MeidaOriSize { get; set; }

        [JsonProperty]
        public long MeidaZipedSize { get; set; }

        [JsonProperty]
        public string RemotePath { get; set; }

        [JsonProperty]
        public IList<Step> Steps { get; set; }

        public IBaseModule ParentModule { get; set; }
        public IList<IBaseModule> SubModules { get; set; }
        public JToken jToken { get; set; }

        public Lesson(Dictionary<string, List<JToken>> csArray, int lessonId, IBaseModule parentModule)
        {
            var jModule =
                from p in csArray["lesson"].AsParallel()
                where p["id"].ToString().Equals("lesson!" + lessonId)
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
            this.LessonNo = jToken["lessonNo"].ToString();
            this.LessonTopic = jToken["lessonTopic"].ToString();
            this.LessonTypeName = jToken["lessonTypeName"].ToString();
            this.LessonDescr = jToken["lessonDescr"].ToString();
            this.Lessontype_id = int.Parse(jToken["lessonType_id"].ToString());
        }

        public void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        {
            this.Steps = new List<Step>();

            foreach (var s in jToken["steps"].Children())
            {
                var stepId = s["id"].ToString().GetId();
                this.Steps.Add(new Step(csArray, stepId, this));
            }
        }
    }
}
