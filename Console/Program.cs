using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.IO;
using Biz;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IDownloadManager dm = new DownloadManager();

            ICourseStructureManager cs = new CourseStructureManager(dm, 201, "development", "en", "none");
        }
    }
}
