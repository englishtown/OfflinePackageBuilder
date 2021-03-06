﻿using Biz;
using Biz.Models;
using Biz.Managers;
using Biz.Services;
using Newtonsoft.Json;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IDownloadService ds = new DownloadService();
            IResourcePackageManager rpm;

            DefaultConstants dc = new DefaultConstants();
            dc.CourseId = 201;
            dc.CultureCode = "zh-CN";
            dc.SiteVersion = "development";
            dc.PartnerCode = "none";
            dc.LocalContentPath = @"d:\offline\content\";
            dc.LocalMediaPath = @"d:\offline\media\";
            dc.LocalStructurePath = @"d:\offline\structure\";
            dc.ServicePrefix = "http://local.englishtown.com";
            dc.ResourcePrefix = "http://local.englishtown.com";
            dc.ContentGenerateBy = LevelType.Level;
            dc.MediaGenerateBy = LevelType.Lesson;

            IContentResourceServcie courseContentResourceService = new CourseContentResourceService(ds, dc);

            ICourseStructureManager cs = new CourseStructureManager(ds, courseContentResourceService, dc);
            Course course = cs.BuildCourseStructure();

            var json = JsonConvert.SerializeObject(course);

            // Get all Activities under the level.
            foreach (Level level in course.Levels)
            {
                IMapfileManager contentMapFileManager = new ContentMapfileManager(level, dc);

                foreach (Unit unit in level.Units)
                {
                    foreach (Lesson lesson in unit.Lessons)
                    {
                        IMapfileManager mediaMapFileManager = new MediaMapfileManager(lesson, dc);

                        foreach (Step step in lesson.Steps)
                        {
                            foreach (Activity activity in step.Activities)
                            {
                                IContentResourceServcie activityContentService = new ActivityContentResourceService(ds, activity.Id, dc);

                                IResourceDownloadManager activityContent = new ActivityContentResourceDownloadManager(ds, activity, activityContentService, dc);
                                activityContent.Download();
                                contentMapFileManager.Add(activityContent.ResourceList);

                                IResourceDownloadManager mediaResource = new MediaResourceDownloadManager(ds, activity, activityContentService, dc);
                                mediaResource.Download();
                                mediaMapFileManager.Add(mediaResource.ResourceList);

                                System.Console.WriteLine("Downloaded Content & Media -- Activity: " + activity.Id);
                            }
                        }

                        // Update Mapfile.
                        // TODO


                        if (mediaMapFileManager.CreateOrUpdated())
                        {
                            // Package by..
                            IResourcePackageManager mpm = new MediaResourcePackageManager(lesson, dc);
                            mpm.Package();

                            System.Console.WriteLine("Package -- Lesson: " + lesson.Id);
                        }
                    }

                    // Get Unit content structure
                    IContentResourceServcie ucs = new UnitContentResourceService(ds, unit.Id, dc);
                    IResourceDownloadManager unitContent = new UnitContentResourceDownloadManager(ds, unit, ucs, dc);
                    unitContent.Download();
                    contentMapFileManager.Add(unitContent.ResourceList);

                    System.Console.WriteLine("Downloaded -- Unit: " + unit.Id);
                }

                // Get level content structure
                IContentResourceServcie lcs = new LevelContentResourceService(ds, level.Id, dc);
                IResourceDownloadManager levelContent = new LevelContentResourceDownloadManager(ds, level, lcs, dc);
                levelContent.Download();

                // get level structure.
                IContentResourceServcie lscrs = new LevelStructureContentResourceService(ds, level.Id, dc);
                IResourceDownloadManager lsc = new LevelStructureContentResourceDeonloadManager(ds, level, lscrs, dc);
                lsc.Download();

                contentMapFileManager.Add(levelContent.ResourceList);

                if (contentMapFileManager.CreateOrUpdated())
                {
                    // if the local mapfile is different with new, just repackage and replace it.
                    IResourcePackageManager cpm = new ContentResourcePackageManager(level, dc);
                    cpm.Package();
                    System.Console.WriteLine("Packaged -- Level: " + level.Id);
                }
            }
        }
    }
}