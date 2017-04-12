#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace TECHIS.Core.Collections.Specialized
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class StringUniquePipe : UniquePipe<string>
    {

        public StringUniquePipe(int maxSize) : base(maxSize)
        {

        }
        /// <summary>
        /// Add a string to the queue 
        /// </summary>
        /// <param name="data"></param>
        public new void Enqueue(string data)
        {
            base.Enqueue(data);
        }

        /// <summary>
        /// Remove the string
        /// </summary>
        /// <returns></returns>
        public string Dequeue()
        {

            TryDequeue(out var result);

            return result;
        }

    }
}
