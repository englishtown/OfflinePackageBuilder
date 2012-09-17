using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;
using Biz.Extensions;
using Biz.Helper;

namespace Biz.Managers
{
    public class ContentResourcePackageManager : IResourcePackageManager
    {
         private readonly IBaseModule module;
        private readonly IConstants constants;

        public ContentResourcePackageManager(IBaseModule module, IConstants constants)
        {
            this.module = module;
            this.constants = constants;
        }

        public void Package()
        {
            Level level = module as Level;
            var levelName = "level_" + level.Id + "_" + this.constants.CultureCode;

            var folderPath = this.constants.LocalMediaPath + levelName;

            // TODO:: Check the file changed
            // TODO:: Version support;
            var packagePath = this.constants.LocalContentPath + levelName + ".zip";

            long meidaOriSize = 0;
            folderPath.GetDirSize(ref meidaOriSize);

            //lesson = a;
            var meidaZipedSize = PackageHelper.Package(folderPath, packagePath);

            // Set package size to lesson
            // TODO:: If the logic changed, don't download by lesson.
            level.ContentOriSize = meidaOriSize;
            level.ContentZippedSize = meidaZipedSize;

            // TODO:: set remote path to lesson
            level.RemotePath = packagePath;
        }
    }
}
