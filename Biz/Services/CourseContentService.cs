using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz.Services
{
    public class CourseContentService : IContentServcie
    {
        public IBaseModule BaseModule { get; set; }
        public string Content { get; set; }

        public LogEntry Logger { get; set; }

        public CourseContentService(IBaseModule module)
        {
            this.BaseModule = module as Course;
            this.Content = BaseModule.ToString();
        }

        // This will generate tree structure of course.
        public void DownloadTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}
