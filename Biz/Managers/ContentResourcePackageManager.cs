using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Services;
using Biz.Extensions;
using Biz.Helper;
using Newtonsoft.Json;

namespace Biz.Managers
{
    public class ContentResourcePackageManager : IResourcePackageManager
    {
        private readonly Level level;
        private readonly IConstants constants;

        private string levelName;

        public ContentResourcePackageManager(IBaseModule module, IConstants constants)
        {
            this.level = module as Level;
            this.constants = constants;

            this.BuildPackageVersionName();
        }

        public void Package()
        {
            var folderPath = this.constants.LocalContentPath + @"level_" + this.level.Id + @"\" + this.constants.CultureCode + @"\";

            var packagePath = this.constants.LocalContentPath + this.levelName + ".zip";

            long contentOriginalSize = 0;
            folderPath.GetDirSize(ref contentOriginalSize);

            //lesson = a;
            var contentZippedSize = PackageHelper.Package(folderPath, packagePath);

            // Set package size to lesson
            // TODO:: If the logic changed, don't download by lesson.
            level.ContentOriSize = contentOriginalSize;
            level.ContentZippedSize = contentZippedSize;

            // set remote path to lesson
            level.RemotePath = packagePath;

            this.BuildPackageInfoFile(contentOriginalSize, contentZippedSize);
        }

        // V1, V2 version support.
        // TODO
        private void BuildPackageVersionName()
        {
            this.levelName = "level_" + level.Id + "_" + this.constants.CultureCode;
        }

        // Generate package's info for build old course structure.
        private void BuildPackageInfoFile(long contentOriginalSize, long contentZippedSize)
        {
            PackageInfo pi = new PackageInfo();
            pi.PackagePath = this.levelName + ".zip";
            pi.Size = contentOriginalSize;
            pi.ZippedSize = contentZippedSize;

            var mapfileContent = JsonConvert.SerializeObject(pi);

            // for every package alwasy use the same package name. 
            // lesson_221.json
            var saveTo = this.constants.LocalContentPath + @"\level_" + this.level.Id + "_" + this.constants.CultureCode + ".pinfo";

            MapfileHelper.Save(mapfileContent, saveTo);
        }

    }
}
