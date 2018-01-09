using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmltool
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<String, String> Parameters = new Dictionary<string, string>();

            int iteration = 0;
            int skipnextiteration = 0;
            string ErrorMessage = string.Empty;
            foreach (string element in args)
            {
                if (skipnextiteration == 0 && string.IsNullOrEmpty(ErrorMessage))
                {
                    if (((element.StartsWith(@"/")) || (element.StartsWith(@"-")) || (element.StartsWith(@"--"))))
                    {

                        string parameter = element;
                        parameter = parameter.TrimStart('/');
                        parameter = parameter.TrimStart('-');
                        string rawArgument = parameter;

                        int hasequals = HasEquals(parameter);
                        if (hasequals > 0)
                        {
                            rawArgument = parameter.Substring(0, parameter.IndexOf('='));
                        }
                        else
                        {
                            rawArgument = parameter;
                        }

                        switch (rawArgument.ToUpper())
                        {
                            case @"I":
                            case @"INPUT":
                                skipnextiteration = EvaluateSetting(parameter, @"INPUT", args[iteration + 1], hasequals, Parameters);
                                break;
                            case @"H":
                            case @"HELP":
                                skipnextiteration = 0;
                                EvaluateSetting(parameter, @"HELP", @"TRUE", hasequals, Parameters);
                                break;
                            case @"P":
                            case @"PRETTY":
                            case @"PRETTY-PRINT":
                            case @"PRETTIFY":
                                skipnextiteration = 0;
                                EvaluateSetting(parameter, @"PRETTY", @"TRUE", hasequals, Parameters);
                                break;
                            case @"Q":
                            case @"QUIET":
                                skipnextiteration = 0;
                                EvaluateSetting(parameter, @"QUIET", @"TRUE", hasequals, Parameters);
                                break;
                            case @"X":
                            case @"XSL":
                            case @"XSLT":
                            case @"XSLFILE":
                            case @"XSLTFILE":
                                skipnextiteration = EvaluateSetting(parameter, @"XSLT", args[iteration + 1], hasequals, Parameters);
                                break;
                            case @"O":
                            case @"OUTFOLDER":
                            case @"OUTFILE":
                                skipnextiteration = EvaluateSetting(parameter, @"OUTPUT", args[iteration + 1], hasequals, Parameters);
                                break;
                            case @"E":
                            case @"EXT":
                                skipnextiteration = EvaluateSetting(parameter, @"EXT", args[iteration + 1], hasequals, Parameters);
                                break;
                            case @"L":
                            case @"LOG":
                                skipnextiteration = EvaluateSetting(parameter, @"LOG", args[iteration + 1], hasequals, Parameters);
                                break;
                            
                            default:
                                ErrorMessage = @parameter + @" is not a valid parameter.";
                                break;
                        }
                    }
                    else
                    {
                        ErrorMessage = @"Unrecognized Parameter " + @element;
                    }
                }
                else
                {
                    skipnextiteration = 0;
                }
                iteration++;
            }

            if (Parameters.ContainsKey("HELP"))
                ErrorMessage = "Xmltool -I [InputFile] [Options]";

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                if (Parameters.ContainsKey("INPUT"))
                {
                    ErrorMessage = RunInput(ErrorMessage, Parameters);
                }
                else
                {
                    Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput(8192))); // This will allow input >256 chars
                    while (Console.In.Peek() != -1)
                    {
                        string input = Console.In.ReadLine();
                        if (!string.IsNullOrEmpty(input))
                        {
                            Parameters["INPUT"] = input;
                            ErrorMessage = RunInput(ErrorMessage, Parameters);
                        }
                    }
                }
            }
            


            if (!string.IsNullOrEmpty(ErrorMessage))
                Console.WriteLine(ErrorMessage);
        }

        static string RunInput(string ErrorMessage, Dictionary<string, string> Parameters)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                Validator.AdjustParameters(Parameters);

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = Validator.ValidateParameters(Parameters);
            }

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                try
                {
                    Arguments a = new Arguments();
                    a.Set(Parameters);
                    ErrorMessage = Tool.run(a);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.ToString();
                }
            }

            return ErrorMessage;
        }

        static int EvaluateSetting(string setting, string parameter, string nextArgument, int hasEquals, Dictionary<string, string> Arguments)
        {

            if (hasEquals > 0)
            {
                string output = setting.Substring(setting.IndexOf('=') + 1);
                output = output.Trim();
                // do not skip next iteration
                Arguments.Add(parameter, output);
                return 0;
            }
            else
            {
                Arguments.Add(parameter, nextArgument);
                // skip the next iteration
                return 1;
            }


        }

        static int HasEquals(string argument)
        {
            if (argument.IndexOf('=') > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
