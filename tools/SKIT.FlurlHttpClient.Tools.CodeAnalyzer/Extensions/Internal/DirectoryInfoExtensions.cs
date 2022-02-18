using System;
using System.IO;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    internal static class DirectoryInfoExtensions
    {
        public static FileInfo[] GetAllFiles(this DirectoryInfo directory, string searchPattern = "*")
        {
            if (directory is null || !directory.Exists)
                return Array.Empty<FileInfo>();

            try
            {
                return directory.GetFiles(searchPattern, SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException)
            {
                return Array.Empty<FileInfo>();
            }
            catch (PathTooLongException)
            {
                return Array.Empty<FileInfo>();
            }
        }
    }
}
