using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Splite the id from 'course!201' like string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetId(this string s)
        {
            return int.Parse(s.Split('!')[1]);
        }

        public static string GetETType(this string s)
        {
            return s.Split('!')[0];
        }
    }
}
