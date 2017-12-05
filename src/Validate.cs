using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Xmltool
{
    public static class Validator
    {
        static string[] ActionParameters = { @"PRETTY", @"XSLT" };
        static string[] PathParameters = { @"OUTPUT"};
        static List<string> FileSystemParameters = new List<string>{ @"OUTPUT", @"XSLT"};

        public static void AdjustParameters(Dictionary<string, string> Arguments)
        {
            

            foreach (string element in PathParameters)
            {
                if (Arguments.ContainsKey(element))
                {

                    int status = 0;
                    status = FileTool.FileDirectoryOrNothing(Arguments[element]);

                    if (status == (int)FileTool.filesystemObjectStatus.Directory)
                    {
                        Arguments[element] = Arguments[element] + @"\";
                    }
                    
                }

            }

            if (Arguments.ContainsKey(@"LOG"))
            {
                if (File.Exists(Arguments[@"LOG"]))
                {
                    File.Delete(Arguments[@"LOG"]);
                }
                Arguments[@"LOG"] = Arguments[@"LOG"].Trim();
            }
        }
        public static string ValidateParameters(Dictionary<string, string> Arguments)
        {
            string ErrorMessage = string.Empty;


            if (string.IsNullOrEmpty(ErrorMessage) && !Arguments.ContainsKey(@"INPUT"))
                ErrorMessage = @"No input specified.";

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                int i = 1;
                int actions = 0;
                while (i <= ActionParameters.Length)
                {
                    if (Arguments.ContainsKey(ActionParameters[i - 1]))
                        actions++;
                    i++;
                }

                if (actions == 0)
                    ErrorMessage = @"Nothing for Xmltool to do.";
            }
            

            // special validation of input
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                if (!Arguments[@"INPUT"].Contains(@"*"))
                    FileSystemParameters.Add(@"INPUT");
            }

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                foreach (string element in FileSystemParameters)
                {
                    if (Arguments.ContainsKey(element))
                    {
                        int status = 0;
                        status = FileTool.FileDirectoryOrNothing(Arguments[element]);

                        switch (status)
                        {
                            case (int)FileTool.filesystemObjectStatus.NonExistant:
                                ErrorMessage = @Arguments[element] + @" is not a file or directory.";
                                break;
                            case (int)FileTool.filesystemObjectStatus.Directory:
                                break;
                            case (int)FileTool.filesystemObjectStatus.File:
                                break;
                            default:
                                // shouldn't happen
                                ErrorMessage = @"I am not sure what " + @element + @" is.";
                                break;
                        }
                    }
                }
            }

            return ErrorMessage;
        }
        

    }

}
