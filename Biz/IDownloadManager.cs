using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz
{
    public interface IDownloadManager
    {
        string DownloadFromPath(Uri url);

        void DownloadFromPathTo(Uri url, string path);
    }
}
