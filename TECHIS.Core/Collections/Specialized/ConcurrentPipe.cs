#region Using directives

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using TECHIS.Serialization.Absractions;
//using OxyData;
//using OxyData.Serialization;


#endregion

namespace TECHIS.Core.Collections
{
    /// <summary>
    /// A fixed length Queue. When Max size is attained, and you add a new item, 
    /// the oldest item in the queue is removed first
    /// </summary>
    [Serializable]
    public class ConcurrentPipe<T> : ConcurrentQueue<T>
    {
        #region Fields
        private Guid _SafeKey = Guid.Empty;
        private int _MaxSize;

        public int MaxSize
        {
            get { return _MaxSize; }
            set { _MaxSize = value; }
        }
        private bool _Notify = false;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or Sets the Key that will be used to persist the Pipe
        /// </summary>
        public Guid SafeKey
        {
            get
            {
                return _SafeKey;
            }
            set
            {
                _SafeKey = value;
            }
        }
        /// <summary>
        /// Determines if the auto dequeue event will be fired
        /// </summary>
        public bool Notify
        {
            get
            {
                return _Notify;
            }
            set
            {
                _Notify = value;
            }
        }
        #endregion

        #region Events
        public event PipeDequeueDelegate<T> AtMaxCapacity;
        #endregion

        #region Constructors
        public ConcurrentPipe( int maxSize) : base()
        {
            if (maxSize > 0)
                _MaxSize = maxSize;
            else
                throw new InvalidOperationException("maxSize must be more than zero");
        }

        #endregion

        #region Methods
        /// <summary>
        /// Enqueues the data
        /// </summary>
        /// <param name="data"></param>
        public void Add(T data)
        {
            if (this.Count == _MaxSize)
            {
                T deqed;
                TryDequeue(out deqed);
                
                Enqueue(data);

                if (_Notify)
                    OnAutoDequeue(deqed, data);
            }
            else
            {
                Enqueue(data);
            }
        }

        private void OnAutoDequeue(T Deq, T Enq)
        {
            if (AtMaxCapacity != null)
            {
                PipeEventsArgs<T> e = new PipeEventsArgs<T>();

                e.Deqeued = Deq;
                e.Enqeued = Enq;
                e.MaxSize = _MaxSize;

                T tp;
                TryPeek(out tp);
                e.nextToDequeue = tp;

                AtMaxCapacity(this, e);
            }
        }

        /// <summary>
        /// The current instance of the pipe
        /// </summary>
        /// <param name="safe"></param>
        /// <returns></returns>
        public Guid Save(IBinarySafe safe, ISerializer serializer)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                 serializer.Serialize(this, ms);
                byte[] data = ms.ToArray();

                if (_SafeKey == Guid.Empty)
                    _SafeKey = Guid.NewGuid();

                safe.Add(_SafeKey, data);

                return _SafeKey;
            }
        }

        /// <summary>
        /// Retrieves an instance of a Pipe from the passed-in Binary safe based on the key
        /// </summary>
        /// <param name="safe">The IBinarySafe instance that will be used to retrieve the persisted pipe</param>
        /// <param name="key">A GUID that points to an instance of a Pipe</param>
        /// <returns></returns>
        public static TPipe Get<TPipe>(IBinarySafe safe, ISerializer serializer, Guid key) where TPipe: ConcurrentPipe<T>
        {
            byte[] data = safe.Get(key);
            if (data != null && data.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    return serializer.Deserialize<TPipe>(ms);
                }
            }
            else
                return null;
        }
        #endregion
    }
}
