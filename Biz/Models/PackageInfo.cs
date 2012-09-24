using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Models
{
    public class PackageInfo
    {
        public string PackagePath { get; set; }

        public long Size { get; set; }

        public long ZippedSize { get; set; }
    }
}
