using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Models
{
    public class Mapfile
    {
        public IList<MapfileItem> Files { get; set; }
        //public bool IsChanged { get; set; }

        public Mapfile()
        {
            this.Files = new List<MapfileItem>();
        }

        public void AddFile(MapfileItem file)
        {
            this.Files.Add(file);
        }

        public void AddFiles(IList<MapfileItem> files)
        {
            foreach (var f in files)
            {
                AddFile(f);
            }
        }
    }
}
