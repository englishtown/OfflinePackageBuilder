using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz.Managers
{
    public interface IContentDownloadManager
    {
        void DownloadLevelContent();

        void DownloadUnitContent();

        void DownloadActivityContent(LevelType ltype);
    }
}
