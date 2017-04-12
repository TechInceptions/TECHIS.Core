using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.IO
{
    /// <summary>
    /// Determines the outcome of processing two strings.
    /// </summary>
    /// <param name="oldText">The initial string</param>
    /// <param name="newText">The new string</param>
    /// <param name="output">the output of the combination, if any</param>
    /// <returns>if an outcome was arrived at</returns>
    public delegate bool AdditionalTextDelegate(string oldText, string newText, out string output);
}
