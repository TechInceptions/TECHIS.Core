using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

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
        public static void Write(TextWriter tw, Exception xc)
        {
            var initialException = xc;
            if (initialException != null)
            {
                int depth = 0;
                while (xc != null)
                {
                    //Write message
                    tw.WriteLine(xc.Message);

                    //stack trace
                    tw.WriteLine($"START: Stack Trace: {depth}");
                    tw.WriteLine(xc.StackTrace);
                    tw.WriteLine($"END: Stack Trace: {depth}");

                    xc = xc.InnerException;
                    depth++;
                }

                //stack trace
                tw.WriteLine("START: Stack Trace: initialException");
                tw.WriteLine(initialException.StackTrace);
                tw.WriteLine("END: Stack Trace: initialException");
            }

        }
        public static StringBuilder GetMessages(Exception xc)
        {
            StringBuilder sb = new StringBuilder(100);
            using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
            {
                Write(sw, xc);
            }
            return sb;
        }
        public static StringBuilder GetMessages(Exception xc, params string[] additionalMessages)
        {
            StringBuilder sb = GetMessages(xc);
            if (additionalMessages != null && additionalMessages.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Additional messages: ");
                foreach (string s in additionalMessages)
                {
                    sb.AppendLine(s);
                }
            }

            return sb;
        }
    }
}
