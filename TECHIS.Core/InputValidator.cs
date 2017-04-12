//----------------------------------------------------------OCG Version: 3.6.12.0
// <copyright company="TECH-IS INC.">
//     Copyright (c) TECH-IS INC.  All rights reserved.
//     Please read license file: http://www.OxyGenCode.com/licence/TECH-IS_INC_PUBLIC_LICENCE.rtf
// </copyright>
//----------------------------------------------------------OCG Version: 3.6.12.0
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TECHIS.Core
{
    public static class InputValidator
    {
        /// <summary>
        /// Throws an ArgumentException if the list 'value' is null or empty
        /// </summary>
        /// <param name="argumentName">The name of the argument that was null or empty</param>
        /// <exception cref="ArgumentException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNullOrEmptyCheck<T>(IList<T> value, string argumentName)
        {
            if (value==null || value.Count==0)
            {
                if (!string.IsNullOrEmpty(argumentName))
                    throw new ArgumentException(string.Format("{0}, a required argument, is null or empty.", argumentName));

                throw new ArgumentException("A required argument is null or empty");
            }
        }

        /// <summary>
        /// Throws an ArgumentException if the string 'value' is null or empty
        /// </summary>
        /// <param name="argumentName">The name of the argument that was null or empty</param>
        /// <exception cref="ArgumentException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNullOrEmptyCheck(string value, string argumentName)
        {
            if (string.IsNullOrEmpty(value))
            {
                if(! string.IsNullOrEmpty( argumentName))
                    throw new ArgumentException(string.Format("{0}, a required argument, is null or an empty string.", argumentName));

                throw new ArgumentException("A required argument is null or an empty string");
            }
        }

        /// <summary>
        /// Throws an ArgumentException if the string 'value' is null or empty
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNullOrEmptyCheck(string value)
        {
            ArgumentNullOrEmptyCheck(value, null);
        }

        /// <summary>
        /// Throws an ArgumentNullException if the object 'value' is null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="argumentName">The name of the argument that was null or empty</param>
        [DebuggerStepThrough]
        public static void ArgumentNullCheck(object value, string argumentName)
        {
            if (value==null)
            {
                if (!string.IsNullOrEmpty(argumentName))
                    throw new ArgumentNullException(string.Format("{0}, a required argument, is null", argumentName));

                throw new ArgumentNullException("A required argument is null");
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException if the object 'value' is null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNullCheck(object value)
        {
            ArgumentNullCheck(value, null);
        }

        /// <summary>
        /// Throws an invalid operation exception if value is null
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="value">object to check</param>
        /// <param name="message">Exception message</param>
        [DebuggerStepThrough]
        public static void InvalidOperationIfNullCheck(object value, string message)
        {
            if (value == null)
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Throws an invalid operation exception if value is null or empty
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="value">object to check</param>
        /// <param name="message">Exception message</param>
        [DebuggerStepThrough]
        public static void InvalidOperationIfNullOrEmptyCheck(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException(message);
            }
        }


        /// <summary>
        /// Throws an invalid operation exception if the list 'value' is null or empty
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="value">object to check</param>
        /// <param name="messages">Exception messages</param>
        [DebuggerStepThrough]
        public static void InvalidOperationIfNullOrEmptyCheck<T>(IList<T> value, params string[] messages)
        {
            if (value == null || value.Count == 0)
            {
                string msg = string.Empty;
                if (messages!=null && messages.Length>0)
                {
                    msg = string.Join(System.Environment.NewLine, messages);
                }

                throw new InvalidOperationException(msg);
            }
        }
    }
}
