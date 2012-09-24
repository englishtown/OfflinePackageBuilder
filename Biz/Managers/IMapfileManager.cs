using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz.Managers
{
    public interface IMapfileManager
    {
        Mapfile Mapfile { get; set; }


        void Add(IList<MapfileItem> mapfiles);

        // Is the file changed.
        bool CreateOrUpdated();
    }
}
