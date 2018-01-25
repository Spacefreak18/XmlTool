using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmltool
{
    public struct Arguments
    {
        public bool Quiet;
        public bool PrettyPrint;

        public bool Overwrite;
        public bool LogIt;
        public bool Xslt;

        public string Input;
        public string XsltFile;
        public string Log;
        public string Output;
        public string Ext;

        public string BasePath;
        public string Glob;

        public void Set(Dictionary<string, string> Arguments)
        {
            Quiet = false;
            PrettyPrint = false;

            Xslt = false;
            Overwrite = true;
            LogIt = false;

            XsltFile = null;
            Log = null;
            Output = null;
            Ext = @"xml";

            Glob = @"*." + @Ext;

            if (Arguments.ContainsKey(@"LOG"))
            {
                Log = Arguments[@"LOG"];

                string path = System.IO.Path.GetDirectoryName(Log);
                string file = System.IO.Path.GetFileName(Log);

                if (string.IsNullOrEmpty(file))
                    file = @"Log.txt";

                Log = @path + @"\" + @file;

                Quiet = true;
                LogIt = true;
            }

            if (Arguments.ContainsKey(@"QUIET"))
            {
                if (Arguments[@"QUIET"] == @"TRUE")
                    Quiet = true;
            }

            if (Arguments.ContainsKey(@"PRETTY"))
            {
                if (Arguments[@"PRETTY"] == @"TRUE")
                    PrettyPrint = true;
            }         

            if (Arguments.ContainsKey(@"XSLT"))
            {
                Xslt = true;
                XsltFile = Arguments[@"XSLT"];
            }       

            if (Arguments.ContainsKey(@"OUTPUT"))
            {
                Output = Arguments[@"OUTPUT"];
                Overwrite = false;
            }

            // trying to setup directory and filename with globbing for getfiles()
            // regardless of if you input a file or a directory or globbing
            // and work regardless of how the input was sent in
            Input = Arguments[@"INPUT"];
            Input = Input.Trim();
            BasePath = Input;

            SetBaseAndGlob();

            if (!BasePath.EndsWith(@"\"))
                BasePath = BasePath + @"\";

            if (!Glob.Contains(@"*"))
                Glob = @"*" + @Glob;

            if (!Glob.EndsWith(@Ext))
                Glob = Glob + "." + @Ext;
        }

        private void SetBaseAndGlob()
        {
            BasePath = String.Empty;
            int index = Input.LastIndexOf(@"\");
            string[] seqNum;

            if (index > 0)
            {
                seqNum = new string[2] { Input.Substring(0, index), Input.Substring(index + 1) };
                BasePath = seqNum[0];
                Glob = seqNum[1];
            }   
        }

    }
}
