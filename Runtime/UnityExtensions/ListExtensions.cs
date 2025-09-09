using System.Collections.Generic;
using System.Linq;

namespace CustomTools.UnityExtensions
{
    public static class ListExtensions
    {
        public static string JoinToString<T>(this List<T> list, string separator = ",")
        {
            return string.Join(separator, list.Select((x => x.ToString())));
        }

        public static int LastIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }
        
        public static List<T> Shuffle<T>(this List<T> list, System.Random rng, int shuffleAmt = 1)
        {
            var count = list.Count;
            var last = count - 1;

            for (var a = 0; a < shuffleAmt; a++)
            {
                for (var i = 0; i < last; ++i)
                {
                    var r = rng.Next(i, count);
                    (list[i], list[r]) = (list[r], list[i]);
                }
            }

            return list;
        }

        public static T Dequeue<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default;
            
            T returnElement = list[0];
            list.RemoveAt(0);
            return returnElement;
        }
    }
}