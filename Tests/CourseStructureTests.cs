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
            dc.LocalContentPath = @"d:\offline\content\";
            dc.LocalMediaPath = @"d:\offline\media\";
            dc.ServicePrefix = "http://mobiledev.englishtown.com";
            dc.ResourcePrefix = "http://local.englishtown.com";

            var mock = new Mock<IDownloadService>();
            mock.Setup(foo => foo.DownloadFromPath(It.IsAny<Uri>())).Returns(content);

            ICourseStructureManager cs = new CourseStructureManager(mock.Object, 201, dc);
            Course course = cs.BuildCourseStructure();

            // Get all Activities under the level.
            foreach (Level level in course.Levels)
            {
                foreach (Unit unit in level.Units)
                {
                    foreach (Lesson lesson in unit.Lessons)
                    {
                        foreach (Step step in lesson.Steps)
                        {
                            foreach (Activity activity in step.Activities)
                            {
                                IContentServcie activityContentService = new ActivityContentService(mock.Object, activity, dc);

                                IContentDownloadManager activityContent = new ActivityContentDownloadManager(mock.Object, activity, activityContentService, dc);
                                activityContent.Download();
                            }
                        }

                        IResourcePackageManager mpm = new MediaResourcePackageManager(lesson, dc);
                        mpm.Package();
                    }

                    // Get Unit content structure
                    IContentDownloadManager unitContent = new UnitContentDownloadManager(mock.Object, unit, dc);
                    unitContent.Download();
                }

                // Get level content structure
                IContentDownloadManager levelContent = new UnitContentDownloadManager(mock.Object, level, dc);
                levelContent.Download();

                IResourcePackageManager cpm = new ContentResourcePackageManager(level, dc);
                cpm.Package();
            }

            Assert.IsNotNull(cs.Course);
        }
    }
}
