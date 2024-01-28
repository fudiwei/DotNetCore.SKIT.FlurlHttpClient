using System;
using System.IO;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    public static class DirectoryInfoFilesExtensions
    {
        /// <summary>
        /// 获取当前目录（含子目录）下与给定的搜索模型匹配的文件列表。
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public static FileInfo[] GetAllFiles(this DirectoryInfo directoryInfo, string searchPattern = "*")
        {
            if (!directoryInfo.Exists)
            {
                return Array.Empty<FileInfo>();
            }

            return directoryInfo.GetFiles(searchPattern, SearchOption.AllDirectories);
        }
    }
}
