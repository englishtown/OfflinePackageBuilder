using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz.Extensions
{
    public static class DirectoryExtension
    {
        // Get the folder size.
        public static void GetDirSize(this string dir, ref long size)
        {
            try
            {
                string[] fileList = Directory.GetFileSystemEntries(dir);

                foreach (string file in fileList)
                {
                    if (Directory.Exists(file))
                    {
                        GetDirSize(file, ref size);
                    }
                    else
                    {
                        FileInfo fiArr = new FileInfo(file);
                        size += fiArr.Length / 1024;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
        }
    }
}
