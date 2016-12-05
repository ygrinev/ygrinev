using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ConsoleApplication1
{
    public static class FileNameParser
    {
        public static List<string> ReadMsiFileNames(string rootFolder)
        {
            List<string> msiFileNameList = new List<string>();
            string[] msiFiles = Directory.GetFiles(rootFolder, "*.msi", SearchOption.AllDirectories)
            .Select(path => Path.GetFullPath(path))
            .ToArray();

            foreach (string msiFile in msiFiles)
            {
                if(msiFile.ToUpper().Contains(("\\RELEASE\\")))
                {
                    msiFileNameList.Add(msiFile);
                }
            }

            return msiFileNameList;
        }
    }
}
