using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Extensions;
using Biz.Services;

namespace Biz.Managers
{
    public class MediaResourcePackageManager : IResourcePackageManager
    {
        private readonly IBaseModule module;
        private readonly IConstants constants;
        private readonly IPackageService packageService;

        public MediaResourcePackageManager(IBaseModule module, IConstants constants)
        {
            this.module = module;
            this.constants = constants;
        }

        public void Package()
        {
            Level l = module as Level;
            var levelName = "level_" + l.Id + "_" + this.constants.CultureCode;

            var folderPath = this.constants.LocalContentPath + "level_" + l.Id + @"\" + this.constants.CultureCode + @"\";
            var packagePath = this.constants.LocalContentPath + levelName + ".zip";

            long a = 0;
            folderPath.GetDirSize(ref a);

            l.contentSize = a;
            //ps.Package(folderPath, packagePath);
        }
    }
}
