//#region Using directives

//using System;
//using System.Collections;
//using TECHIS.Core.Serialization;


//#endregion

//namespace TECHIS.Core.Collections.Specialized
//{
//    /// <summary>
//    /// A fixed length Queue. When Max size is attained, and you add a new item, 
//    /// the oldest item in the queue is removed first
//    /// </summary>
//    [Serializable]
//    public class Pipe : Queue
//    {
//        #region Fields
//        private Guid _SafeKey = Guid.Empty;
//        private int _MaxSize;

//        public int MaxSize
//        {
//            get { return _MaxSize; }
//            set { _MaxSize = value; }
//        }
//        private bool _Notify = false;
//        #endregion

//        #region Properties
//        /// <summary>
//        /// Gets or Sets the Key that will be used to persist the Pipe
//        /// </summary>
//        public Guid SafeKey
//        {
//            get
//            {
//                return _SafeKey;
//            }
//            set
//            {
//                _SafeKey = value;
//            }
//        }
//        /// <summary>
//        /// Determines if the auto dequeue event will be fired
//        /// </summary>
//        public bool Notify
//        {
//            get
//            {
//                return _Notify;
//            }
//            set
//            {
//                _Notify = value;
//            }
//        }
//        #endregion

//        #region Events
//        public event PipeDequeueDelegate AtMaxCapacity;
//        #endregion

//        #region Constructors
//        public Pipe(int InitCapacity, int maxSize) : base(InitCapacity)
//        {
//            if (maxSize > 0)
//                _MaxSize = maxSize;
//            else
//                throw new InvalidOperationException("maxSize must be more than zero");
//        }

//        #endregion

//        #region Methods
//        /// <summary>
//        /// Enqueues the data
//        /// </summary>
//        /// <param name="data"></param>
//        public override void Enqueue(object data)
//        {
//            if (this.Count == _MaxSize)
//            {
//                object deqed = this.Dequeue();
//                base.Enqueue(data);

//                if (_Notify)
//                    OnAutoDequeue(deqed, data);
//            }
//            else
//            {
//                base.Enqueue(data);
//            }
//        }

//        private void OnAutoDequeue(object Deq, object Enq)
//        {
//            if (AtMaxCapacity != null)
//            {
//                PipeEventsArgs e = new PipeEventsArgs();

//                e.Deqeued = Deq;
//                e.Enqeued = Enq;
//                e.MaxSize = _MaxSize;
//                e.nextToDequeue = this.Peek();

//                AtMaxCapacity(this, e);
//            }
//        }

//        /// <summary>
//        /// The current instance of the pipe
//        /// </summary>
//        /// <param name="safe"></param>
//        /// <returns></returns>
//        public Guid Save(IBinarySafe safe)
//        {
//            byte[] data = BinarySerializer.Serialize(this);
//            if (_SafeKey == Guid.Empty)
//                _SafeKey = Guid.NewGuid();

//            safe.Add(_SafeKey, data);

//            return _SafeKey;
//        }

//        /// <summary>
//        /// Retrieves an instance of a Pipe from the passed-in Binary safe based on the key
//        /// </summary>
//        /// <param name="safe">The IBinarySafe instance that will be used to retrieve the persisted pipe</param>
//        /// <param name="key">A GUID that points to an instance of a Pipe</param>
//        /// <returns></returns>
//        public static T Get<T>(IBinarySafe safe, Guid key) where T:Pipe
//        {
//            byte[] data = safe.Get(key);
//            if (data != null && data.Length > 0)
//                return (T)(BinarySerializer.Deserialize(data));
//            else
//                return null;
//        }
//        #endregion
//    }
//}
