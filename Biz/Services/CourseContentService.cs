using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz
{
    public class CourseContentService : IContentServcie
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public LogEntry Logger { get; set; }

        public CourseContentService(Course course)
        {
            this.Content = course.ToString();
        }

        // This will generate tree structure of course.
        public void DownloadTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}
