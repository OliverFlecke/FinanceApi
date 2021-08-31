using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceApi.Test.Utils
{
    public class DataGenerator
    {
        public DataGenerator() => Random = new();

        public Random Random { get; init; }

        public static DataGenerator CreateWithSeed(int seed) => new()
        {
            Random = new Random(seed),
        };

        public IEnumerable<int> Numbers =>
            Enumerable.Range(0, int.MaxValue).Select(_ => Random.Next(int.MinValue, int.MaxValue));

        public IEnumerable<byte> Bytes => Numbers.Select(x => (byte)x);

        public DateTime DateTime => new(Random.Next());

        public DateTime DateTimeAfter(DateTime date) => new(date.Ticks + Random.Next());

        public string String(int length = 32) => new(
            Enumerable
                .Range(0, length)
                .Select(_ => (char)Random.Next('A', 'Z' + 1))
                .ToArray());

        /// <summary>
        /// In general <c>Guid.NewGuid()</c> should be preferred,
        /// but this can be used to generate `random` GUIDs
        /// from a seed
        /// </summary>
        public Guid Guid() => new(Bytes.Take(16).ToArray());

        public string Email() => $"{String()}@{String()}.com";

        public string Url() => $"http://{String()}.com";

        public string Base64String(int length)
        {
            var bytes = new byte[length];
            Random.NextBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        public ulong Ulong() => BitConverter.ToUInt64(Bytes.Take(8).ToArray());
    }
}
