using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Biz.Models;

namespace Biz.Services
{
    public interface IContentServcie
    {
        IBaseModule BaseModule { get; set; }
        string Content { get; set; }

        //void DownloadTo(string path);
    }
}
