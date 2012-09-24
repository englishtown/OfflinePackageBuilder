using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Biz.Extensions;
using Biz.Services;
using System.IO;
using Ionic.Zip;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Biz.Helper;
using Newtonsoft.Json;

namespace Biz.Managers
{
    public class MediaResourcePackageManager : IResourcePackageManager
    {
        private readonly Lesson lesson;
        private readonly IConstants constants;

        private string lessonName;

        public MediaResourcePackageManager(IBaseModule module, IConstants constants)
        {
            this.lesson = module as Lesson;
            this.constants = constants;

            this.BuildPackageVersionName();
        }

        public void Package()
        {
            // Todo if file exist version +1
            var folderPath = this.constants.LocalMediaPath + this.lessonName + @"\";
            
            // TODO:: Check the file changed
            // TODO:: Version support;
            var packagePath = this.constants.LocalMediaPath + this.lessonName + ".zip";

            long mediaOriginalSize = 0;
            folderPath.GetDirSize(ref mediaOriginalSize);

            //lesson = a;
            var mediaZippedSize = PackageHelper.Package(folderPath, packagePath);

            // Set package size to lesson
            // TODO:: If the logic changed, don't download by lesson.
            lesson.MeidaOriSize = mediaOriginalSize;
            lesson.MeidaZipedSize = mediaZippedSize;

            // TODO:: set remote path to lesson
            lesson.RemotePath = packagePath;

            this.BuildPackageInfoFile(mediaOriginalSize, mediaZippedSize);
        }

        // V1, V2 version support.
        // TODO
        private void BuildPackageVersionName()
        {
            this.lessonName = "lesson_" + this.lesson.Id;
        }

        // Generate package's info for build old course structure.
        private void BuildPackageInfoFile(long mediaOriginalSize, long mediaZippedSize)
        {
            PackageInfo pi = new PackageInfo();
            pi.PackagePath = this.lessonName + ".zip";
            pi.Size = mediaOriginalSize;
            pi.ZippedSize = mediaZippedSize;

            var mapfileContent = JsonConvert.SerializeObject(pi);

            // for every package alwasy use the same package name. 
            // lesson_221.json
            var saveTo = this.constants.LocalMediaPath + @"\lesson_" + lesson.Id + ".pinfo";

            MapfileHelper.Save(mapfileContent, saveTo);
        }
    }
}
