using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz
{
    public interface IConstants
    {
        int CourseId { get; set; }

        string SiteVersion { get; set; }
        string CultureCode { get; set; }
        string PartnerCode { get; set; }

        LevelType ContentGenerateBy { get; set; }
        LevelType MediaGenerateBy { get; set; }

        string LocalContentPath { get; set; }
        string LocalMediaPath { get; set; }
        string LocalStructurePath { get; set; }

        string ServicePrefix { get; set; }
        string ResourcePrefix { get; set; }
    }
}
