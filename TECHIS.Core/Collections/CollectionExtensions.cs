using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TECHIS.Core.Collections.Extensions
{
    /// <summary>
    /// Encapsulates extension methods that are designed to work against collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Converts the objects in an ICollection to an array of T
        /// </summary>
        /// <typeparam name="T">The Type to Convert the objects in the ICollection to</typeparam>
        /// <param name="source">The ICollection</param>
        public static T[] ConvertToArray<T>(this System.Collections.ICollection source)
        {
            //object[] tmp = new object[source.Count];
            //source.CopyTo(tmp, 0);
            //return Array.ConvertAll<object, T>(tmp, delegate(object o) { return (T)o; });

            return CollectionUtil.ConvertToArray<T>(source);
        }

        /// <summary>
        /// Isolates instances in 'subjectCollection' that are not in 'compareToCollection'
        /// <param name="nonUnique">Instances that are not unique</param>
        /// <returns>unique instances</returns>
        /// </summary>
        public static List<T> IsolateUnique<T>(this List<T> subjectCollection, List<T> compareToCollection, EqualityComparerDelegate<T> comparer, out List<T> nonUnique)
        {
            List<T> unique = new List<T>();
            nonUnique = new List<T>();

            foreach (T compareTo in compareToCollection)
            {
                bool found = false;
                foreach (T subject in subjectCollection)
                {
                    if (comparer(subject, compareTo))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    unique.Add(compareTo);
                }
                else
                {
                    nonUnique.Add(compareTo);
                }
            }

            return unique;
        }
    }
}
