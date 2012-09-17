using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz.Managers
{
    public interface ICourseStructureManager
    {
        Course Course { get; set; }

        Course BuildCourseStructure();
    }
}
