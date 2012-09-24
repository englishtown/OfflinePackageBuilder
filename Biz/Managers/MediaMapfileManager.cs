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
    public class MediaMapfileManager : IMapfileManager
    {
        public Mapfile Mapfile { get; set; }

        private IBaseModule baseModule;
        private IConstants constants;

        private string filePath;

        // Support Lesson media package now.
        public MediaMapfileManager(IBaseModule baseModule, IConstants constants)
        {
            this.baseModule = baseModule;
            this.constants = constants;

            this.Mapfile = new Mapfile();

            this.filePath = this.constants.LocalMediaPath + string.Format(@"lesson_{0}.json", this.baseModule.Id);

        }

        public void Add(IList<MapfileItem> mapfiles)
        {
            this.Mapfile.AddFiles(mapfiles);
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

            var oldMapfile = MapfileHelper.Read(this.filePath);
            if (oldMapfile != mapfileContent) //updated
            {
                DeleteUnusedFiles(oldMapfile, this.Mapfile);

                MapfileHelper.Save(mapfileContent, this.filePath);
                return true;
            }

            return false;
        }

        // Delete unused files.
        private void DeleteUnusedFiles(string oldMapfile, Mapfile newMapfile)
        {
            // Need Remove unused files.
            Mapfile mapfile = JsonConvert.DeserializeObject<Mapfile>(oldMapfile);

            foreach (var a in mapfile.Files)
            {
                int s = newMapfile.Files.Where(f => f.FileName == a.FileName).Count();
                if (s == 0)
                {
                    var filepath = this.constants.LocalMediaPath + string.Format(@"lesson_{0}\{1}", this.baseModule.Id, a.FileName);
                    File.Delete(filepath);
                }
            }
        }
    }
}
