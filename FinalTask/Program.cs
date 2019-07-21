using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FinalTask
{
    class Program
    {
        static void Main(string[] args)
        {

            //FileInfo
            string properFilesPath = ConfigurationManager.AppSettings["inputPath"];
            string[] files = Directory.GetFiles(properFilesPath, "*.sdlxliff", SearchOption.AllDirectories);
            XDocument xml;
            List<String> fileAttribute = new List<string>();
            string sourceLang;
            string targetLang;
            string targetFolderName;
            string fileOriginalPath;
            string jobID;
            string patternJobID= "\\[0-9]+\\";
            Regex rgxJobID = new Regex(patternJobID);

            foreach (var file in files)
            {
                xml = XDocument.Load(file);

                fileOriginalPath = ((System.Xml.Linq.XElement)xml.Root.LastNode).FirstAttribute.Value;
                sourceLang = ((System.Xml.Linq.XElement)xml.Root.LastNode).LastAttribute.PreviousAttribute.NextAttribute.PreviousAttribute.Value.ToUpper();
                targetLang = ((System.Xml.Linq.XElement)xml.Root.LastNode).LastAttribute.Value;
                targetFolderName = targetLang.Substring(targetLang.Length - 2);
                MatchCollection matches = rgxJobID.Matches(fileOriginalPath);
                jobID = matches[0].ToString();

                Console.WriteLine(jobID);
                Console.WriteLine(sourceLang);
                Console.WriteLine(targetFolderName);
            }
            
            Console.ReadLine();
        }

    }
}
