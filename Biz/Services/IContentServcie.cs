using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz
{
    public interface IContentServcie
    {
        int Id { get; set; }
        string Content { get; set; }

        void DownloadTo(string path);
    }
}
