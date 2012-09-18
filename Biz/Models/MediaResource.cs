using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Biz.Models
{
    public class MediaResource
    {
        int Id { get; set; }

        IEnumerable<JToken> jModule { get; set; }

        IList<IBaseModule> SubModules { get; set; }

        IBaseModule ParentModule { get; set; }

        void BuildModule()
        { 
        }

        void BuildSubmodule(Dictionary<string, List<JToken>> csArray)
        { 
        }
    }
}
