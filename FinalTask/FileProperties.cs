using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask
{
    class FileProperties
    {
        public string JobID { get; set; }
        public string SourceLang { get; set; }
        public string TargetLang { get; set; }
        public string Filename { get; set; }

        public FileProperties(string jobID, string sourceLang, string targetLang, string filename)
        {
            this.JobID = jobID;
            this.SourceLang = sourceLang;
            this.TargetLang = targetLang;
            this.Filename = filename;

        }

    }
}
