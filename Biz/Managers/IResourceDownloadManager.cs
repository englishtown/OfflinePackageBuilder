using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz.Managers
{
    public interface IResourceDownloadManager
    {
        IList<MapfileItem> ResourceList { get; set; }

        void Download();
    }
}
