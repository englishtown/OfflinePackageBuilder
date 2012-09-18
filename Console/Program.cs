using Biz;
using Biz.Models;
using Biz.Managers;
using Biz.Services;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IDownloadService ds = new DownloadService();
            IResourcePackageManager rpm;

            DefaultConstants dc = new DefaultConstants();
            dc.CultureCode = "en";
            dc.SiteVersion = "development";
            dc.PartnerCode = "none";
            dc.LocalContentPath = @"d:\offline\content\";
            dc.LocalMediaPath = @"d:\offline\media\";
            dc.ServicePrefix = "http://mobiledev.englishtown.com";
            dc.ResourcePrefix = "http://local.englishtown.com";
            dc.ContentGenerateBy = LevelType.Level;
            dc.MediaGenerateBy = LevelType.Lesson;

            ICourseStructureManager cs = new CourseStructureManager(ds, 201, dc);
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
                                IResourceServcie activityContentService = new ActivityContentResourceService(ds, activity, dc);

                                IResourceDownloadManager activityContent = new ActivityContentResourceDownloadManager(ds, activity, activityContentService, dc);
                                activityContent.Download();
                            }
                        }

                        IResourcePackageManager mpm = new MediaResourcePackageManager(lesson, dc);
                        mpm.Package();
                    }

                    // Get Unit content structure
                    IResourceServcie ucs = new UnitContentResourceService(new DownloadService(), unit, dc);
                    IResourceDownloadManager unitContent = new UnitContentResourceDownloadManager(ds, unit, ucs, dc);
                    unitContent.Download();
                }

                // Get level content structure
                IResourceServcie lcs = new LevelContentResourceService(new DownloadService(), level, dc);
                IResourceDownloadManager levelContent = new UnitContentResourceDownloadManager(ds, level, lcs, dc);
                levelContent.Download();

                IResourcePackageManager cpm = new ContentResourcePackageManager(level, dc);
                cpm.Package();
            }

        }
    }
}
