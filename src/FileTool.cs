using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Xmltool
{
    public static class FileTool
    {
        public static int FileDirectoryOrNothing(string fileOrPath)
        {
            int result = (int)filesystemObjectStatus.NonExistant;

            if (System.IO.Directory.Exists(fileOrPath))
            {
                result = (int)filesystemObjectStatus.Directory;
            }
            else if (System.IO.File.Exists(fileOrPath))
            {
                result = (int)filesystemObjectStatus.File;
            }

            return result;
        }

        private static string GetExtension(string fileOrPath)
        {
            int status = 0;
            string extension = null;

            status = FileDirectoryOrNothing(fileOrPath);

            switch (status)
            {
                case (int)filesystemObjectStatus.NonExistant:
                    break;
                case (int)filesystemObjectStatus.Directory:
                    extension = Path.GetExtension(GetRandomFile(fileOrPath));
                    break;
                case (int)filesystemObjectStatus.File:
                    extension = Path.GetExtension(fileOrPath);
                    break;
                default:
                    break;
            }

            // i am not going to check for an acceptable file extension here because excel has so many

            extension = extension.Substring(1);

            return extension;
        }

        private static string GetRandomFile(string fileOrPath)
        {
            var rand = new Random();
            var files = Directory.GetFiles(fileOrPath, @"*.*");
            return files[rand.Next(files.Length)];
        }

        public enum filesystemObjectStatus : int
        {
            NonExistant = 0,
            Directory = 1,
            File = 2
        }
    }
}
