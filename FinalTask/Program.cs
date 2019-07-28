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
using Sdl.Sdlx.ManagedFramework;

namespace FinalTask
{
    class Program
    {
        static void Main(string[] args)
        {
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
            string xliffName;


            foreach (var correctfile in correctedFiles)
            {
                xml = XDocument.Load(correctfile);

                fileOriginalPath = ((System.Xml.Linq.XElement)xml.Root.LastNode).FirstAttribute.Value;
                sourceLang = ((System.Xml.Linq.XElement)xml.Root.LastNode).LastAttribute.PreviousAttribute.NextAttribute.PreviousAttribute.Value.ToUpper();
                targetLang = ((System.Xml.Linq.XElement)xml.Root.LastNode).LastAttribute.Value;
                targetFolderName = targetLang.Substring(targetLang.Length - 2);
                MatchCollection matches = rgxJobID.Matches(fileOriginalPath);
                jobID = matches[0].ToString().Replace("\\","");
                xliffName = correctfile.Substring(correctfile.LastIndexOf("\\")+1);

                FileProperties fileProperties = new FileProperties(jobID, sourceLang, targetLang, xliffName);
                
                Console.WriteLine(jobID);
                Console.WriteLine(sourceLang);
                Console.WriteLine(targetFolderName);
                Console.WriteLine(xliffName);

                string archivePatternJobID = "[0-9]+_";
                string archiveJobID;
                //ZipArchive archive;

                foreach (string wrongFile in wrongFiles)
                {
                    Regex rgxArchiveJobID = new Regex(archivePatternJobID);
                    MatchCollection archiveMatches = rgxArchiveJobID.Matches(wrongFile);
                    archiveJobID = archiveMatches[0].ToString().Replace("_","");
                    if (wrongFile.Contains(jobID))
                    {
                        using (FileStream zipToOpen = new FileStream(wrongFile, FileMode.Open))
                        {
                            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                            {
                                foreach (var entry in archive.Entries)
                                {
                                    if (entry.FullName.Contains($"TGT/{sourceLang}_{targetFolderName}/{xliffName}"))
                                    {
                                        Console.WriteLine("Uwaga, działam!");
                                        entry.Delete();
                                        archive.CreateEntryFromFile(correctfile, $"TGT/{sourceLang}_{targetFolderName}/{xliffName}");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
