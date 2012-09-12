using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biz;
using Biz.Models;
using Moq;
using System.IO;

namespace Tests
{
    [TestClass]
    public class CourseStructureTests
    {
        [TestMethod]
        public void CanDownloadCourseStructureSuccessful()
        {

            string content;

            using (StreamReader reader = new StreamReader(@"CourseStructure.json"))
            {
                content = reader.ReadToEnd(); 
            }

            IDownloadManager a = new DownloadManager();


            var mock = new Mock<IDownloadManager>();
            mock.Setup(foo => foo.DownloadFromPath(It.IsAny<Uri>())).Returns(content);

            //dm.Expect(ctx => ctx.DownloadFromPath(It.IsAny<Uri>())).Returns(list.ToString());

            //dm.Setup(f => f.DownloadFromPath(It.IsAny<Uri>())).Returns(list.ToString());

            ICourseStructureManager cs = new CourseStructureManager(mock.Object, 201, "development", "en", "none");

            Assert.IsNotNull(cs.Course);
        }
    }
}
