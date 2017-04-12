using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TECHIS.Core.Collections
{
    public class CollectionUtil
    {
        /// <summary>
        /// Isolates instances in 'subjectCollection' that are not in 'compareToCollection'
        /// <param name="nonUnique">Instances that are not unique</param>
        /// <returns>unique instances</returns>
        /// </summary>
        public static List<T> IsolateUnique<T>(List<T> subjectCollection, List<T> compareToCollection, EqualityComparerDelegate<T> comparer, out List<T> nonUnique)
        {
            List<T> unique = new List<T>();
            nonUnique = new List<T>();

            foreach (T compareTo in compareToCollection)
            {
                bool found=false;
                foreach (T subject in subjectCollection)
                {
                    if (comparer(subject,compareTo) )
                    {
                        found = true;
                        break;
                    }
                }

                if (! found)
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

        /// <summary>
        /// Converts a collection of 'T' to an array of 'T'
        /// </summary>
        /// <typeparam name="T">The type of the instances in the collection</typeparam>
        public static T[] ConvertToArray<T>(System.Collections.ICollection source)
        {
            return source.Cast<T>().ToArray();
        }
    }
}
