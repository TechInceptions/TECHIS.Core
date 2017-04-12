using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public static class AccessibilityLevels
    {
        public const string PRIVATE = "private";
        public const string PUBLIC = "public";
        public const string INTERNAL = "internal";
        public const string PROTECTED = "protected";

        public static bool IsAccessibilityLevel(string value)
        {
            if (PRIVATE.Equals(value, StringComparison.Ordinal))
                return true;

            if (PUBLIC.Equals(value, StringComparison.Ordinal))
                return true;

            if (INTERNAL.Equals(value, StringComparison.Ordinal))
                return true;

            if (PROTECTED.Equals(value, StringComparison.Ordinal))
                return true;

            return false;
        }
    }
}
