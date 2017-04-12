using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TECHIS.Core
{
    public static class Exceptions
    {
        [DebuggerStepThrough]
        public static void ThrowInvalidOps(Exception inner, params string[] messages)
        {
            StringBuilder msg = new StringBuilder();

            if (messages != null && messages.Length > 0)
            {
                foreach (string s in messages)
                {
                    msg.AppendLine(s);
                }
            }

            if (inner == null && msg.Length == 0)
                throw new InvalidOperationException();

            if (inner != null)
                throw new InvalidOperationException(msg.ToString(), inner);
            else
                throw new InvalidOperationException(msg.ToString());
        }

        [DebuggerStepThrough]
        public static void ThrowInvalidOps( params string[] messages )
        {
            ThrowInvalidOps(null, messages);
        }
    }
}
