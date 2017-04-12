//using System;
//using System.Threading;

//namespace TECHIS.Core.Threading
//{
//    //Avoid boxing this or you will lose thread safety
//    public class SlimOneManySpinLock
//    {
//        #region Constants 
//        private const Int32 ISFREE = 0;
//        private const Int32 ISLOCKED = 1;
//        #endregion

//        #region Fields 
//        private Int32 _LockState;	// Defaults to 0=ISFREE
//        private Int32 _ReaderCount;
//        private bool _NoReadWait;
//        #endregion

//        #region Properties 
//        public bool IsLocked
//        {
//            get { return ( Thread.VolatileRead(ref _LockState) != ISFREE); }
//        }
//        #endregion

//        #region Constructors
//        /// <summary>
//        /// Initializes an instance of the lock. Tp protect a resource, all access should pass through the same lock instance
//        /// </summary>
//        /// <param name="noReadWait">if 'true', this lock will not block access to the resource for a read operation even if the resource is locked</param>
//        public SlimOneManySpinLock(bool noReadWait)
//        {
//            _NoReadWait = noReadWait;
//        }
//        /// <summary>
//        /// Initializes an instance of the lock. Tp protect a resource, all access should pass through the same lock instance
//        /// </summary>
//        public SlimOneManySpinLock()
//        {
//            _NoReadWait = false;
//        }
//        #endregion

//        #region Public Methods
//        public bool Enter(bool IsWrite)
//        {
//            bool success = true;
//            while (true)
//            {
//                if (IsWrite)
//                {
//                    if (Interlocked.Exchange(ref _LockState, ISLOCKED) == ISFREE)
//                    {
//                        if (Thread.VolatileRead(ref _ReaderCount) == 0)
//                        {
//                            return success;
//                        }
//                    }
//                }
//                else
//                {
//                    if (Thread.VolatileRead(ref _LockState) == ISFREE)
//                    {
//                        Interlocked.Increment(ref _ReaderCount);
//                        return success;
//                    }
//                    else
//                    {
//                        if (_NoReadWait)
//                        {
//                            return false;
//                        }
//                    }
//                }

//                while (Thread.VolatileRead(ref _LockState) == ISLOCKED)
//                {
//                    TECHIS.Core.Threading.ThreadUtility.StallThread();
//                }
//            }

//            //return success;
//        }

//        public void Exit(bool IsWrite)
//        {
//            if (IsWrite)
//            {
//                Interlocked.Exchange(ref _LockState, ISFREE);
//            }
//            else
//            {
//                Interlocked.Decrement(ref _ReaderCount);
//            }
//        }
//        #endregion
//    }
//}
