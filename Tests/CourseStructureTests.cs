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

            var ds = new Mock<IDownloadService>();
            ds.Setup(foo => foo.DownloadFromPath(It.IsAny<Uri>())).Returns((string s) => s.ToLower());

            var crs = new Mock<IContentResourceServcie>();
            // auto-mocking hierarchies (a.k.a. recursive mocks)
            crs.Setup(foo => foo.Content).Returns(content);

            DefaultConstants dc = new DefaultConstants();
            dc.CourseId = 201;
            dc.CultureCode = "en";
            dc.SiteVersion = "development";
            dc.PartnerCode = "none";
            dc.LocalContentPath = @"d:\offline\content\";
            dc.LocalMediaPath = @"d:\offline\media\";
            dc.ServicePrefix = "http://mobiledev.englishtown.com";
            dc.ResourcePrefix = "http://local.englishtown.com";
            dc.ContentGenerateBy = LevelType.Level;
            dc.MediaGenerateBy = LevelType.Lesson;

            //IContentResourceServcie courseContentResourceService = new CourseContentResourceService(mock.Object, dc);

            ICourseStructureManager cs = new CourseStructureManager(ds.Object, crs.Object, dc);
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
                                IContentResourceServcie activityContentService = new ActivityContentResourceService(new DownloadService(), activity.Id, dc);

                                IResourceDownloadManager activityContent = new ActivityContentResourceDownloadManager(ds.Object, activity, activityContentService, dc);
                                activityContent.Download();


                                IResourceDownloadManager mediaResource = new MediaResourceDownloadManager(ds.Object, activity, activityContentService, dc);
                                mediaResource.Download();
                            }
                        }

                        IResourcePackageManager mpm = new MediaResourcePackageManager(lesson, dc);
                        mpm.Package();
                    }

                    IContentResourceServcie ucs = new UnitContentResourceService(new DownloadService(), unit.Id, dc);
                    // Get Unit content structure
                    IResourceDownloadManager unitContent = new UnitContentResourceDownloadManager(ds.Object, unit, ucs, dc);
                    unitContent.Download();
                }

                // Get level content structure
                IContentResourceServcie lcs = new LevelContentResourceService(new DownloadService(), level.Id, dc);
                IResourceDownloadManager levelContent = new LevelContentResourceDownloadManager(ds.Object, level, lcs, dc);
                levelContent.Download();

                IResourcePackageManager cpm = new ContentResourcePackageManager(level, dc);
                cpm.Package();
            }

            Assert.IsNotNull(cs.Course);
        }
    }
}
