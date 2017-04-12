///******************************************************************************
//Module:  ThreadUtility.cs
//Notices: Copyright (c) 2006-2007 by Jeffrey Richter and Wintellect
//******************************************************************************/


//using System;
//using System.Text;
//using System.Threading;
//using System.Diagnostics;
//using System.ComponentModel;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using Microsoft.Win32.SafeHandles;
//////using Wintellect;
//using HWND = System.IntPtr;


/////////////////////////////////////////////////////////////////////////////////


//namespace TECHIS.Core.Threading
//{
//    public static class ThreadUtility
//    {
//        public static readonly Boolean IsSingleCpuMachine =
//           (Environment.ProcessorCount == 1);

//        public static void Spin(Int32 milliseconds)
//        {
//            for (Int32 stop = Environment.TickCount + milliseconds; Environment.TickCount < stop; ) ;
//        }

//        #region Simple Win32 Thread Wrappers
//        public static Int32 GetWindowThreadId(HWND hwnd)
//        {
//            Int32 processId;
//            return GetWindowThreadProcessId(hwnd, out processId);
//        }

//        public static Int32 GetWindowProcessId(HWND hwnd)
//        {
//            Int32 processId;
//            GetWindowThreadProcessId(hwnd, out processId);
//            return processId;
//        }

//        [DllImport("User32")]
//        private static extern Int32 GetWindowThreadProcessId(HWND hwnd, out Int32 pdwProcessId);

//        public static SafeWaitHandle OpenThread(ThreadRights rights, Boolean inheritHandle, Int32 threadId)
//        {
//            SafeWaitHandle thread = nativeOpenThread(rights, inheritHandle, threadId);
//            if (thread.IsInvalid) throw new WaitHandleCannotBeOpenedException();
//            return thread;
//        }

//        [Flags]
//        public enum ThreadRights : int
//        {
//            Terminate = 0x0001,
//            SuspendResume = 0x0002,
//            GetContext = 0x0008,
//            SetContext = 0x0010,
//            SetInformation = 0x0020,
//            QueryInformation = 0x0040,
//            SetThreadToken = 0x0080,
//            Impersonate = 0x0100,
//            DirectImpersonation = 0x0200,
//            Delete = 0x00010000,
//            ReadPermissions = 0x20000,
//            ChangePermissions = 0x40000,
//            TakeOwnership = 0x80000,
//            Synchronize = 0x100000,
//            StandardRightsRequired = 0x000F0000,
//            FullControl = StandardRightsRequired | Synchronize | 0x3FF,
//        }

//        [DllImport("Kernel32", SetLastError = true, EntryPoint = "OpenThread")]
//        private static extern SafeWaitHandle nativeOpenThread(ThreadRights dwDesiredAccess,
//           [MarshalAs(UnmanagedType.Bool)] Boolean bInheritHandle, Int32 threadId);

//        [DllImport("Kernel32", ExactSpelling = true)]
//        public static extern Int32 GetCurrentProcessorNumber();

//        [DllImport("Kernel32", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
//        public static extern Int32 GetCurrentWin32ThreadId();

//        [DllImport("Kernel32", EntryPoint = "GetCurrentThread", ExactSpelling = true)]
//        public static extern SafeWaitHandle GetCurrentWin32ThreadHandle();

//        [DllImport("Kernel32", EntryPoint = "GetCurrentProcess", ExactSpelling = true)]
//        public static extern SafeWaitHandle GetCurrentWin32ProcessHandle();

//        [DllImport("Kernel32", ExactSpelling = true)]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        public static extern Boolean SwitchToThread();

//        public static void StallThread()
//        {
//            if (IsSingleCpuMachine)
//            {
//                // On single-CPU system, spinning does no good
//                SwitchToThread();
//            }
//            else
//            {
//                // The multi-CPU system might be hyper-threaded, let the other thread run
//                Thread.SpinWait(1);
//            }
//        }
//        #endregion

//        #region Cancel Synchronous I/O

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="thread"></param>
//        /// <returns>true if an operation is cancelled; false if the thread was not waiting for I/O</returns>
//        public static Boolean CancelSynchronousIO(SafeWaitHandle thread)
//        {
//            if (nativeCancelSynchronousIO(thread)) return true;
//            Int32 error = Marshal.GetLastWin32Error();

//            const Int32 ErrorNotFound = 1168;
//            if (error == ErrorNotFound) return false; // failed to cancel because thread was not waiting

//            throw new Win32Exception(error);
//        }

//        // http://msdn.microsoft.com/windowsvista/default.aspx?pull=/library/en-us/dnlong/html/win32iocancellationapisv2.asp
//        [DllImport("Kernel32", SetLastError = true, EntryPoint = "CancelSynchronousIo")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        private static extern Boolean nativeCancelSynchronousIO(SafeWaitHandle hThread);
//        #endregion

//        #region I/O Background Processing Mode


//        public static void EndBackgroundProcessingMode()
//        {
//            if (SetThreadPriority(GetCurrentWin32ThreadHandle(), BackgroundProcessingMode.End))
//                return;
//            throw new Win32Exception();
//        }

//        private enum BackgroundProcessingMode
//        {
//            Start = 0x10000,
//            End = 0x20000
//        }

//        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        private static extern Boolean SetThreadPriority(SafeWaitHandle hthread, BackgroundProcessingMode mode);
//        #endregion
//    }
//}


////////////////////////////////// End of File //////////////////////////////////
