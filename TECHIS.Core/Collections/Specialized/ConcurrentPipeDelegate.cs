using System;

namespace TECHIS.Core.Collections
{
	/// <summary>
	/// Summary description for UniqueQueueEvents.
	/// </summary>
	public delegate void PipeDequeueDelegate<T>(object source, PipeEventsArgs<T> e);

	public class PipeEventsArgs<T> : EventArgs
	{
		public T Enqeued;
		public T Deqeued;
		public T nextToDequeue;
		public int MaxSize;
	}

	
}
