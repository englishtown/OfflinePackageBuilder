using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Managers
{
    public interface IResourcePackageManager
    {
        // if Package exist, verson +1
        void Package();
    }
}
