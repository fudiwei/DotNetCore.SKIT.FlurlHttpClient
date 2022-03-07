using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Helpers
{
    internal static class FileHelper
    {
        public static async Task WriteTextAsync(string filePath, string fileName, string text, CancellationToken cancellationToken)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c.ToString(), "");

            try
            {
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }
            catch (NotSupportedException) { }

            await File.WriteAllTextAsync(path: Path.Combine(filePath, fileName), contents: text, cancellationToken);
        }
    }
}
