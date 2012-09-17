using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.IO;
using Biz;
using Biz.Models;
using Biz.Managers;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IDownloadService ds = new DownloadService();

            ICourseStructureManager cs = new CourseStructureManager(ds, 201, "development", "en", "none");
            Course course = cs.BuildCourseStructure();

            IContentDownloadManager cdm = new ContentDownloadManager(ds, course, "development", "en", "none");
            cdm.DownloadActivityContent(LevelType.Level);

            IResourcePackageManager crpm = new ContentResourcePackageManager();
            crpm.Package();

            IResourcePackageManager mrpm = new MediaResourcePackageManager();
            mrpm.Package();
        }
    }
}
