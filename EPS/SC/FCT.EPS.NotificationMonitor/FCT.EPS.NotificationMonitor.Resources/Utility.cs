using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.NotificationMonitor.Resources
{
    class Utility
    {
        public static string GetRelativeSearchPath(string passedChildPath, string passedParentPath, string passedPathSeparator)
        {
            string returnValue = "";
            DirectoryInfo childPath = new DirectoryInfo(passedChildPath);
            DirectoryInfo parentPath = new DirectoryInfo(passedParentPath);

            while (NormalizePath(childPath.FullName) != NormalizePath(parentPath.FullName))
            {
                returnValue = returnValue + passedPathSeparator + RemoveParentDir(MakeRelativePath(NormalizePath(parentPath.FullName), NormalizePath(childPath.FullName)));
                childPath = childPath.Parent;
            }
            return returnValue;
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }

        private static string RemoveParentDir(string passedPath)
        {
            string[] myValues = passedPath.Split(Path.DirectorySeparatorChar);
            string[] returnValue = new string[myValues.GetUpperBound(0)];
            for (int Counter = 1; Counter <= myValues.GetUpperBound(0); Counter++)
            {
                returnValue[Counter - 1] = myValues[Counter];
            }
            return Path.Combine(returnValue);
        }

        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.ToUpperInvariant() == "FILE")
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }
}
