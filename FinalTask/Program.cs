using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace FinalTask
{
    class Program
    {
        static void Main(string[] args)
        {

            //FileInfo

            string correctedFilesPath = ConfigurationManager.AppSettings["correctedFiles"];
            string[] correctedFiles = Directory.GetFiles(correctedFilesPath, "*.sdlxliff", SearchOption.AllDirectories);
            string wrongFilesPath = ConfigurationManager.AppSettings["wrongFiles"];
            string[] wrongFiles = Directory.GetFiles(wrongFilesPath, "*.zip", SearchOption.AllDirectories);
            XDocument xml;
            string sourceLang;
            string targetLang;
            string targetFolderName;
            string fileOriginalPath;
            string jobID;
            string patternJobID = "\\\\[0-9]+\\\\";
            Regex rgxJobID = new Regex(patternJobID);
            string filename;


            foreach (var correctfile in correctedFiles)
            {
                xml = XDocument.Load(correctfile);

                fileOriginalPath = ((System.Xml.Linq.XElement)xml.Root.LastNode).FirstAttribute.Value;
                sourceLang = ((System.Xml.Linq.XElement)xml.Root.LastNode).LastAttribute.PreviousAttribute.NextAttribute.PreviousAttribute.Value.ToUpper();
                targetLang = ((System.Xml.Linq.XElement)xml.Root.LastNode).LastAttribute.Value;
                targetFolderName = targetLang.Substring(targetLang.Length - 2);
                MatchCollection matches = rgxJobID.Matches(fileOriginalPath);
                jobID = matches[0].ToString().Replace("\\","");
                filename = fileOriginalPath.Substring(fileOriginalPath.LastIndexOf("\\")+1);


                FileProperties fileProperties = new FileProperties(jobID,sourceLang,targetLang, filename);
                
                Console.WriteLine(jobID);
                Console.WriteLine(sourceLang);
                Console.WriteLine(targetFolderName);
                Console.WriteLine(filename);

                foreach (string wrongFile in wrongFiles)
                {
                    if (wrongFile.StartsWith(jobID))
                    {
                        //https://stackoverflow.com/questions/12553809/how-to-check-whether-file-exists-in-zip-file-using-dotnetzip
                    }
                }

            }
            
            Console.ReadLine();
        }

    }
}
