using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Biz
{
    public class PackageManager
    {
        // Zip a path to a zip file.
        public long Package(string folderPath, string packagePath)
        {
            if (!Directory.Exists(folderPath))
            {
                Logger.Write(folderPath + "NOT EXIST!!!!");
                return 0;
            }

            var fileName = GetFolderName(folderPath);

            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(folderPath);
                zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                zip.Save(packagePath);
            }

            return new FileInfo(packagePath).Length / 1024;
        }

        private string GetFolderName(string path)
        {
            Regex r = new Regex(@"\\\w+");
            MatchCollection mc = r.Matches(path);

            return mc[mc.Count - 1].Value;
        }
    }
}
