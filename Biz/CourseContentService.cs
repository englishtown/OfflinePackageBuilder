using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;

namespace Biz
{
    public class CourseContentService : IContentServcie
    {
        public int Id { get; set; }
        public string Content { get; set; }

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
