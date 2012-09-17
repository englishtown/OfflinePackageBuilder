using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz
{
    public class DefaultConstants : IConstants
    {

        public DefaultConstants()
        {
        }

        public virtual string SiteVersion { get; set; }
        public virtual string CultureCode { get; set; }
        public virtual string PartnerCode { get; set; }

        // Generate by?

        public virtual LevelType ContentGenerateBy { get; set; }

        // Default by Lesson.
        public virtual LevelType MediaGenerateBy { get; set; }

        // = @"d:\offline\content\";
        public virtual string LocalContentPath { get; set; }

        //= @"d:\offline\media\";
        public virtual string LocalMediaPath { get; set; }

        //  = "http://mobiledev.englishtown.com";
        public virtual string ServicePrefix { get; set; }

        //= "http://local.englishtown.com";
        public virtual string ResourcePrefix { get; set; }
    }
}
