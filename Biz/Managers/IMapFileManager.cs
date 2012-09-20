using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz.Managers
{
    public interface IMapFileManager
    {
        IList<FileCheckInfo> Files { get; set; }


        void Add(IList<FileCheckInfo> fileInfo);

        // Is the file changed.
        bool CreateOrUpdated();
    }
}
