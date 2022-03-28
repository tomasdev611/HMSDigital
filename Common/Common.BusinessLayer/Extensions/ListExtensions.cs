using System.Collections.Generic;
using System.Linq;

namespace HMSDigital.Common.BusinessLayer.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> collection, int size)
        {
            var chunks = new List<List<T>>();
            var chunkCount = collection.Count() / size;

            if (collection.Count() % size > 0)
            {
                chunkCount++;
            }

            for (var i = 0; i < chunkCount; i++)
            {
                chunks.Add(collection.Skip(i * size).Take(size).ToList());
            }

            return chunks;
        }
    }
}
