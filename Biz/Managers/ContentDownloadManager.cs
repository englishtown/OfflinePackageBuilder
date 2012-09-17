using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz.Managers
{
    public class ContentDownloadManager : IContentDownloadManager
    {
        private const string courseLink = "/services/school/query?q=course!{0}.*&c=siteversion={1}|cultureCode={2}|partnerCode={3}";


        private readonly IDownloadService downloadService;

        public Course Course { get; set; }
        public string SiteVersion { get; set; }
        public string CultureCode { get; set; }
        public string PartnerCode { get; set; }

        // 
        public ContentDownloadManager(IDownloadService downloadService, Course course, string siteVersion, string cultureCode, string partnerCode)
        {
            this.downloadService = downloadService;

            this.Course = course;
            this.SiteVersion = siteVersion;
            this.CultureCode = cultureCode;
            this.PartnerCode = partnerCode;
        }

        public virtual void DownloadLevelContent()
        {
        }

        public virtual void DownloadUnitContent()
        {
        }

        // Download activity by level?
        public virtual void DownloadActivityContent(LevelType ltype)
        {
            switch (ltype)
            {
                case LevelType.Level:
                    DownloadActivityContentByLevel();
                    break;
                case LevelType.Unit:
                    //DownloadActivityContentByUnit();
                    break;
            }
        }



        // 
        private void DownloadActivityContentByLevel()
        {
            // Get all Activities under the level.
            foreach (Level l in this.Course.Levels)
            {
                foreach (Unit u in l.Units)
                {
                    DownloadActivityContentByUnit(u);
                }

                // package the content by level.
                //this.PackageContentFile(l);
            }


        }

        // 
        private void DownloadActivityContentByUnit(Unit unit)
        {
            // Get all activities under the unit.
            foreach (Lesson l in unit.Lessons)
            {
                DownloadActivityContentByLesson(l);
            }
        }

        // 
        private void DownloadActivityContentByLesson(Lesson lesson)
        {
            foreach (Step s in lesson.Steps)
            {
                foreach (Activity a in s.Activities)
                {
                    int levelId = a.ParentModule.ParentModule.ParentModule.ParentModule.Id;
                    int lessonId = a.ParentModule.ParentModule.Id;

                    ActivityContentService acs = new ActivityContentService(this.downloadService, a, this.SiteVersion, this.CultureCode, this.PartnerCode);
                    acs.DownloadTo(ConstantsDefault.LocalContentPath + "level_" + levelId + @"\" + this.CultureCode + @"\" + a.Id + ".json");

                    // Download the meida file.
                    foreach (var mediaPath in a.MediaResources)
                    {
                        IDownloadService dm = new DownloadService();
                        IContentServcie mediaService = new MediaResourceService(mediaPath, dm);
                        var path = ConstantsDefault.LocalMediaPath + "lesson_" + lessonId + @"\" + mediaPath;
                        mediaService.DownloadTo(path);
                    }
                }
            }

            //Package Media by lesson
            // this.PackageMeidaFile("lesson_" + lesson.Id);
        }

    }
}
