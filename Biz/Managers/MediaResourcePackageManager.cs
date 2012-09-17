using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz.Managers
{
    public class MediaResourcePackageManager : IResourcePackageManager
    {
        public void Package()
        { }

        public void Package(IBaseModule m)
        {
            Level l = m as Level;
            //var levelName = "level_" + l.Id + "_" + this.CultureCode;

            //var folderPath = ConstantsDefault.LocalContentPath + "level_" + l.Id + @"\" + this.CultureCode + @"\";
            //var packagePath = ConstantsDefault.LocalContentPath + levelName + ".zip";
            //long a = 0;
            //folderPath.GetDirSize(ref a);

            //l.contentSize = a;
            //ps.Package(folderPath, packagePath);
        }
    }
}
