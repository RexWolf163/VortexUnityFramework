using System;
using System.Security.Cryptography;
using System.Text;

namespace Vortex.Core.Extensions.LogicExtensions
{
    public static class Crypto
    {
        private static long _lastTime;

        private static int _counter;

        private static Random _random;
        private static Random Random => _random ??= new Random(DateTime.Now.Millisecond);


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
            if (temp == _lastTime)
                _counter++;
            else
            {
                _lastTime = temp;
                _counter = 0;
            }

            return GetHashSha256($"{temp}-{_counter}-{Random.NextDouble()}-{Random.NextDouble()}");
        }
    }
}