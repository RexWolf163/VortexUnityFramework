using System;
using System.Security.Cryptography;
using System.Text;
using NotImplementedException = System.NotImplementedException;

namespace Vortex.Core.Extensions.LogicExtensions
{
    public static class Crypto
    {
        private static long lastTime;

        private static int counter;

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

        public static string GetNewGuid()
        {
            var temp = DateTime.UtcNow.ToFileTimeUtc();
            if (temp == lastTime)
                counter++;
            else
            {
                lastTime = temp;
                counter = 0;
            }

            return GetHashSha256($"{temp}-{counter}");
        }
    }
}