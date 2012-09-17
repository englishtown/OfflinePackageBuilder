using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biz;
using Biz.Models;
using Moq;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Biz.Managers;
using Biz.Services;

namespace Tests
{
    [TestClass]
    public class CourseStructureTests
    {
        [TestMethod]
        public void CanDownloadCourseStructureSuccessful()
        {
            string content;

            LogEntry logEntry = new LogEntry();

            using (StreamReader reader = new StreamReader(@"CourseStructureLite.json"))
            {
                content = reader.ReadToEnd(); 
            }


            DefaultConstants dc = new DefaultConstants();
            dc.CultureCode = "en";
            dc.SiteVersion = "development";
            dc.PartnerCode = "none";
            dc.LocalContentPath = "";

            IDownloadService ds = new DownloadService();

            var mock = new Mock<IDownloadService>();
            mock.Setup(foo => foo.DownloadFromPath(It.IsAny<Uri>())).Returns(content);

            //dm.Expect(ctx => ctx.DownloadFromPath(It.IsAny<Uri>())).Returns(list.ToString());

            //dm.Setup(f => f.DownloadFromPath(It.IsAny<Uri>())).Returns(list.ToString());

            ICourseStructureManager css = new CourseStructureManager(ds, 201, dc);
            Course course = css.BuildCourseStructure();

            IContentDownloadManager cdm = new LevelContentDownloadManager(ds, course, dc);
            cdm.Download();
            cdm.Download();
            cdm.Download();

            //IResourcePackageManager crpm = new ContentResourcePackageManager();
            //crpm.Package();

            //IResourcePackageManager mrpm = new MediaResourcePackageManager();
            //mrpm.Package();

            Assert.IsNotNull(css.Course);
        }
    }
}
