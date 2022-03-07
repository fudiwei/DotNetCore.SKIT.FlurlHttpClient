using System;
using System.Security.Cryptography;
using System.Text;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Helpers
{
    internal static class SHA256Helper
    {
        public static string SHA1Encrypt(string text)
        {
            using SHA256 sha = SHA256.Create();
            byte[] bMsg = Encoding.UTF8.GetBytes(text);
            byte[] bHash = sha.ComputeHash(bMsg);
            return BitConverter.ToString(bHash).Replace("-", "").ToLower();
        }
    }
}
