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

namespace Biz.Managers
{
    public class MediaResourcePackageManager : IResourcePackageManager
    {
        private readonly IBaseModule module;
        private readonly IConstants constants;

        public MediaResourcePackageManager(IBaseModule module, IConstants constants)
        {
            this.module = module;
            this.constants = constants;
        }

        public void Package()
        {
            // Todo
            Lesson lesson = module as Lesson;
            var lessonName = "lesson_" + lesson.Id;

            var folderPath = this.constants.LocalContentPath + "lesson_" + lesson.Id + @"\";
            
            // TODO:: Check the file changed
            // TODO:: Version support;
            var packagePath = this.constants.LocalContentPath + lessonName + ".zip";

            long meidaOriSize = 0;
            folderPath.GetDirSize(ref meidaOriSize);

            //lesson = a;
            var meidaZipedSize = PackageHelper.Package(folderPath, packagePath);

            // Set package size to lesson
            // TODO:: If the logic changed, don't download by lesson.
            lesson.MeidaOriSize = meidaOriSize;
            lesson.MeidaZipedSize = meidaZipedSize;

            // TODO:: set remote path to lesson
            lesson.RemotePath = packagePath;
        }        
    }
}
