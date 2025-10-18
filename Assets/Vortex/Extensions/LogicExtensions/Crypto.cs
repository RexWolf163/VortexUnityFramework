using System.Security.Cryptography;
using System.Text;

namespace Vortex.Extensions
{
    public static class Crypto
    {
        public static string GetHashSha256(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var hashManager = new SHA256Managed();
            var hash = hashManager.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var x in hash)
                sb.Append($"{x:x2}");
            return sb.ToString();
        }
    }
}