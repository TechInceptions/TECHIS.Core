using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECHIS.Core
{
    public static class ArrayUtil
    {
        public static T Get<T>(this T[] array, int index = 0)
        {
            T val;
            if (array?.Length > 0 && array.Length > index)
            {
                val = array[index];
            }
            else
            {
                val = default(T);
            }

            return val;
        }
        public static TOutput Get<TOutput>(this ICollection<string> array, int index = 0)
        {
            string arg;
            if (array?.Count > 0 && array.Count > index)
            {
                arg = array.ElementAt(index);
            }
            else
            {
                arg = null;
            }

            TOutput val;
            if ((!string.IsNullOrWhiteSpace(arg)) && ObjectReader.TryParseObject(arg, typeof(TOutput), out object o))
            {
                val = (TOutput)o;
            }
            else
            {
                val = default(TOutput);
            }

            return val;
        }
    }
}
