using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biz;
using Biz.Models;

namespace Tests
{
    [TestClass]
    public class CourseStructureTests
    {
        [TestMethod]
        public void CanDownloadCourseStructureSuccessful()
        {
            CourseStructure cs = new CourseStructure(201, "development", "en", "none");
            Assert.IsNotNull(cs);
        }
    }
}
