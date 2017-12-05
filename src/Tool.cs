using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Linq;

namespace Xmltool
{
    public static class Tool
    {

        public static string run(Arguments args)
        {
            string ErrorMessage = string.Empty;

            System.IO.DirectoryInfo directory;
            System.IO.FileInfo[] filesList;
            StreamWriter outputfile = null;

            directory = new System.IO.DirectoryInfo(args.BasePath);
            filesList = directory.GetFiles(args.Glob);

            if (filesList.Length <= 0)
                return @"No files found by given input criteria.";

            if (args.LogIt == true)
                outputfile = new StreamWriter(args.Log, true);

            int FilesWorkedOn = 0;
            foreach (FileInfo file in filesList)
            {
                string newxml = null;
                XmlDocument x = new XmlDocument();

                x.Load(file.FullName);
                newxml = x.OuterXml;

                if (args.Xslt == true)
                {
                    XslCompiledTransform myXslTrans = new XslCompiledTransform();
                    myXslTrans.Load(args.XsltFile);

                    StringWriter stringwriter = new StringWriter();
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    settings.OmitXmlDeclaration = true;

                    XmlWriter myWriter = XmlWriter.Create(stringwriter, settings);
                    myXslTrans.Transform(x, null, myWriter);

                    newxml = stringwriter.ToString();
                }
                
                if (args.PrettyPrint == true)
                {
                    newxml = PrettyXml(newxml);
                }

                if (args.Overwrite == true)
                {
                    x.LoadXml(newxml);
                    x.Save(file.FullName);
                }
                else
                {
                    int status = 0;
                    status = FileTool.FileDirectoryOrNothing(args.Output);

                    switch (status)
                    {
                        case (int)FileTool.filesystemObjectStatus.Directory:
                            x.Save(args.Output + @"\" + @file.Name);
                            break;
                        case (int)FileTool.filesystemObjectStatus.File:
                            x.Save(args.Output);

                            if (filesList.Length > 1 && FilesWorkedOn == 0)
                            {
                                log(args, outputfile, @"Warning: Multiple input files found yet output is a single file.");
                            }

                            break;
                        default:
                            break;
                    }
                }

                FilesWorkedOn++;

                log(args, outputfile, @"Processed Xml " + file.FullName);
            }

            if (outputfile != null)
            {
                outputfile.Flush();
                outputfile.Close();
            }
            

            return ErrorMessage;
        }

        static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        static void log(Arguments args, StreamWriter outputfile, string Message)
        {
            if (args.Quiet == false)
                Console.WriteLine(@Message);
            if (args.LogIt == true)
                outputfile.WriteLine(@Message);
        }
    }

    
}