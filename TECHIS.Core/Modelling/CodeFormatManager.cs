using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public static class CodeFormatManager
    {
        public static void WriteOpenBraces(StringBuilder target, string spacing)
        {
            target.AppendLine().Append(spacing).Append('{');
        }

        public static void WriteCloseBraces(StringBuilder target, string spacing)
        {
            target.AppendLine().Append(spacing).AppendLine("}");
        }
    }
}
