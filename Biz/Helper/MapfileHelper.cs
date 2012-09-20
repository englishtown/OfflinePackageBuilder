using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Biz.Helper
{
    public class MapfileHelper
    {
        public static string Read(string path)
        {
            string mediaFileContent;

            // Get current file content.
            using (StreamReader reader = new StreamReader(path))
            {
                mediaFileContent = reader.ReadToEnd();
            }

            return mediaFileContent;
        }

        public static void Save(string content, string path)
        {
            // Just replace 
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter(path);

            //Write a line of text
            sw.Write(content);

            //Close the file
            sw.Close();
        }
    }
}
