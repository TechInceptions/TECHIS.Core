#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;

#endregion

namespace TECHIS.Core.Collections.Specialized
{
    /// <summary>
    /// A UniquePipe only tries to ensure that items in the pipe will be unique.
    /// For something more robust, use a concurrent dictionary.
    /// </summary>
    [Serializable]
    public class UniquePipe<T> : ConcurrentPipe<T>
    {
        public UniquePipe(int maxSize) : base(maxSize)
        {

        }

        public new void Enqueue(T data)
        {
            if (!(this.Contains(data)))
                base.Enqueue(data);
        }

    }
}
