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
    public class ContentMapfileManager : IMapfileManager
    {
        public Mapfile Mapfile { get; set; }

        private IBaseModule baseModule;
        private IConstants constants;

        private string filePath;

        // Support Lesson media package now.
        public ContentMapfileManager(IBaseModule baseModule, IConstants constants)
        {
            this.baseModule = baseModule;
            this.constants = constants;

            this.Mapfile = new Mapfile();

            this.filePath = this.constants.LocalContentPath + string.Format(@"level_{0}_{1}.json", this.baseModule.Id, this.constants.CultureCode);

        }

        public void Add(IList<MapfileItem> fileInfo)
        {
            this.Mapfile.AddFiles(fileInfo);
        }

        // Is the file changed.
        public bool CreateOrUpdated()
        {
            var mapfileContent = JsonConvert.SerializeObject(this.Mapfile);

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
