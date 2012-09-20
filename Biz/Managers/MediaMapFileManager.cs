using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biz.Models;
using Newtonsoft.Json;
using System.IO;
using Biz.Helper;

namespace Biz.Managers
{
    public class MediaMapFileManager : IMapFileManager
    {
        public IList<FileCheckInfo> Files { get; set; }

        private IBaseModule baseModule;
        private IConstants constants;

        private string filePath;

        // Support Lesson media package now.
        public MediaMapFileManager(IBaseModule baseModule, IConstants constants)
        {
            this.baseModule = baseModule;
            this.constants = constants;

            this.Files = new List<FileCheckInfo>();

            this.filePath = this.constants.LocalMediaPath + string.Format(@"lesson_{0}.json", this.baseModule.Id);

        }

        public void Add(IList<FileCheckInfo> fileInfo)
        {
            foreach (var fi in fileInfo)
            {
                this.Files.Add(fi);
            }
        }

        // Is the file changed.
        public bool CreateOrUpdated()
        {
            var mapfileContent = JsonConvert.SerializeObject(this.Files);

            // If the file exist, read it and check and update.
            if (!File.Exists(this.filePath))
            {
                // if not exist create the file.
                // Update mapfile.
                MapfileHelper.Save(mapfileContent, this.filePath);
                return true;
            }

            if (MapfileHelper.Read(this.filePath) != mapfileContent) //updated
            {
                MapfileHelper.Save(mapfileContent, this.filePath);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
