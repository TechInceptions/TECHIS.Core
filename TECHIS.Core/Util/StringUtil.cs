using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;


namespace TECHIS.Core.Text
{
    public static class StringUtil
    {

        public static string RemoveAccents(string text, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            try
            {
                text = text.Normalize(NormalizationForm.FormD);
            }
            catch (Exception ex) when (ex is ArgumentException) 
            {
                logger?.LogError(ex, "RemoveAccents Normalize(NormalizationForm.FormD) failed. Text was: '{text}'", text);
                return text;
            }

            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            char[] chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                                .ToArray();
            
            var tmp = new string(chars);

            try
            {
                tmp = tmp.Normalize(NormalizationForm.FormC);
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                logger?.LogError(ex, "RemoveAccents Normalize(NormalizationForm.FormC) failed. Text was: '{text}'", tmp);
                return tmp;
            }

            return tmp;
        }
        /// <summary>  
        /// Turn a string into a slug by removing all accents,   
        /// special characters, additional spaces, substituting   
        /// spaces with hyphens & making it lower-case.  
        /// </summary>  
        /// <param name="phrase">The string to turn into a slug.</param>  
        /// <returns></returns>  
        public static string Slugify(this string phrase, ILogger logger = null)
        {
            // Remove all accents and make the string lower case.  
            string output = RemoveAccents(phrase, logger).ToLower();
            if (string.IsNullOrWhiteSpace(output))
            {
                return string.Empty;
            }

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }
        public static string Slugify2(this string phrase, bool asLowerCase=false, bool removeAccents=false, string pattern= @"[^A-Za-z0-9\s-]", ILogger logger = null, bool removeAdditionalDashes=false)
        {
            string output = removeAccents ? RemoveAccents(phrase, logger) : phrase;
            if (string.IsNullOrWhiteSpace(output))
            {
                return string.Empty;
            }

            if (asLowerCase)
            {
                output = output.ToLower();
            }

            // Remove all special characters from the string.  
            output = Regex.Replace(output, pattern, "");

            if (string.IsNullOrWhiteSpace(output))
            {
                return string.Empty;
            }
            var spacePattern = removeAdditionalDashes ? @"[\s-]+" : @"\s+";
            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, spacePattern, " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }
        public static string SlugifyTitle(this string phrase, int approximateMaxLength = 50, ILogger logger = null)
        {
            if (string.IsNullOrWhiteSpace(phrase))
            {
                return "-";
            }
            var pattern = @"^(.{" + approximateMaxLength.ToString() + @"}[^_\s-]*).*";
            //max length 50; will be long if required to complete the last word
            var text = Regex.Replace(phrase, pattern, "$1"); 
            //var text = Regex.Match(phrase, pattern).Value;

            return Slugify2(text, true, true, removeAdditionalDashes: true);
        }

        /// <summary>  
        /// Turn a string into a slug by removing all accents,   
        /// special characters, additional spaces, substituting   
        /// spaces with hyphens & making it lower-case.  
        /// </summary>  
        /// <param name="phrase">The string to turn into a slug.</param>  
        /// <returns></returns>  
        public static string MakeHtmlSafe(this string phrase, ILogger logger)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                return phrase;
            }
            // Remove all accents 
            string output = RemoveAccents(phrase, logger);
            if (string.IsNullOrWhiteSpace(output))
            {
                return string.Empty;
            }

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s?#!,.@-]", "");

            if (string.IsNullOrWhiteSpace(output))
            {
                return string.Empty;
            }

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            return output;
        }

        public static string AsOnlyLetterOrDigit(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            char[] ncs = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (!(char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_'))
                {
                    c = ' ';
                }
                ncs[i] = c;
            }

            return new string(ncs);
        }
        public static StringBuilder Join(char seperator, params string[] parts)
        {
            StringBuilder val;
            if (parts==null)
            {
                val = new StringBuilder(0);
            }
            else
            {
                val = new StringBuilder();
                foreach (var part in parts)
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        val.Append(part).Append(seperator);
                    }
                }

                val.RemoveLast();
            }
            return val;

        }

        public static StringBuilder Join(char seperator,IEnumerable<string> parts)
        {
            return Join(seperator, parts.ToArray());
        }

        public static StringBuilder RemoveLast(this StringBuilder sb)
        {
            if (sb!=null && sb.Length>0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }
        /// <summary>
        /// Removes the last chars n from the stringbuilder
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="charCount">The number of chars to remove</param>
        /// <returns></returns>
        public static StringBuilder RemoveLast(this StringBuilder sb, int charCount)
        {
            if (sb != null && sb.Length > 0 && charCount > 0)
            {
                if (charCount >= sb.Length)
                {
                    sb.Clear();
                }
                else
                {
                    sb.Remove(sb.Length - charCount, charCount);
                }
            }
            return sb;
        }

        /// <summary>
        /// Replaces a segment of a string.
        /// The search uses 'IndexOf', so it starts at the beginning of the string.
        /// </summary>
        /// <param name="target">The container of the entire string</param>
        /// <param name="startMarker"> a string fragment that denotes the start of the segment to replace</param>
        /// <param name="endMarker">a string fragment that denotes the end of the segment to replace. If null or empty, the replacement is done to the end of the string</param>
        /// <param name="replacement">The replacement text. If null or empty, the segment of string demarcated by start and end is only removed</param>
        /// <param name="stringComparison"></param>
        /// <param name="replaceOnlyFirstOccurance">The default value is true. If true, only the first occurance is replacement. 
        /// Otherwise, all occurances are replaced. If the replacement string contains both the start and end markers, then an infinite loop may occur</param>
        /// <param name="loopBreak">The maximum number of times to loop through the string</param>
        /// <returns></returns>
        public static StringBuilder Replace(this StringBuilder target, string startMarker, string endMarker = null, string replacement = null, StringComparison stringComparison = StringComparison.Ordinal, bool replaceOnlyFirstOccurance=true, int loopBreak=100000)
        {
            if (target == null || target.Length == 0)
            {
                return target;
            }

            InputValidator.ArgumentNullOrEmptyCheck(startMarker, "startMarker");
            int sIdx = 0;
            int loopCount = 0;
            int beginIdx = 0;
            while (sIdx != -1 && beginIdx< target.Length)
            {
                loopCount++;
                if (loopCount>loopBreak)
                {
                    break;
                }
                string data = target.ToString();
                sIdx = data.IndexOf(startMarker, beginIdx, stringComparison);
                if (sIdx != -1)
                {
                    int eIdx = -1;
                    bool hasEnd = false;
                    if (!string.IsNullOrEmpty(endMarker))
                    {
                        eIdx = data.IndexOf(endMarker, sIdx + startMarker.Length);
                        hasEnd = true;
                        if (eIdx==-1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        eIdx = target.Length - 1;
                    }

                    if (eIdx != -1)
                    {
                        int startIndex = sIdx;
                        int endIndex = eIdx;
                        if (hasEnd)
                        {
                            endIndex = eIdx + endMarker.Length;
                        }

                        target.Remove(startIndex, ((endIndex - startIndex) + 1));
                        if (!string.IsNullOrEmpty(replacement))
                        {
                            target.Insert(startIndex, replacement);
                            beginIdx = startIndex + replacement.Length;
                        }
                        else
                        {
                            beginIdx = startIndex + 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                if (replaceOnlyFirstOccurance)
                {
                    break;
                }

            }
            return target;
        }

        public static String MakeClientSafeId(String s)
        {
            return MakeClientSafeId(new StringBuilder(s)).ToString();
        }
        public static StringBuilder MakeClientSafeId(StringBuilder sb)
        {
            sb  .Replace('.', '_')
                .Replace('#', '_')
                .Replace(':', '_')
                .Replace(';', '_')
                .Replace('/', '_')
                .Replace('*', '-')
                .Replace('|', '-')
                .Replace('~', '-');

            return sb;
        }

        public static string Truncate(this string sourceString, int maxLength)
        {
            if (string.IsNullOrEmpty(sourceString))
            {
                return sourceString;
            }
            else
            {
                return sourceString.Length <= maxLength ? sourceString : sourceString.Substring(0, maxLength);
            }
        }

        /// <summary>
        /// Conpare 2 strings to see if they start with the same characters
        /// </summary>
        /// <param name="first">The 1st string</param>
        /// <param name="second">The 2nd string</param>
        /// <param name="startIndex1st">start index</param>
        /// <param name="length">the length of the segment</param>
        /// <param name="startIndex2nd">start index</param>
        /// <param name="length2nd">the length of the segment</param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static bool StartsWith(this string first, string second, int startIndex1st, int length, int startIndex2nd, StringComparison stringComparison)
        {
            if (first is null || second is null)
            {
                return false;
            }

            return first.AsSpan().Slice(startIndex1st, length).Equals(second.AsSpan().Slice(startIndex2nd, length), stringComparison);
        }
        /// <summary>
        /// Conpare 2 strings to see if they start with the same characters
        /// </summary>
        /// <param name="first">The 1st string</param>
        /// <param name="second">The 2nd string</param>
        /// <param name="startIndex">start index</param>
        /// <param name="length">the length of the segment</param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static bool StartsWith(this string first, string second, int startIndex, int length, StringComparison stringComparison)
        {
            return first.StartsWith(second, startIndex, length, startIndex, stringComparison);
        }
        /// <summary>
        /// Conpare 2 strings to see if they start with the same characters
        /// </summary>
        /// <param name="first">The 1st string</param>
        /// <param name="second">The 2nd string</param>
        /// <param name="start">start index</param>
        /// <param name="length">the length of the segment</param>
        /// <returns></returns>
        public static bool StartsWith(this string first, string second, int startIndex, int length)
        {
            return first.StartsWith(second, startIndex, length, startIndex, StringComparison.Ordinal);
        }
        /// <summary>
        /// Conpare 2 strings to see if they start with the same characters
        /// </summary>
        /// <param name="first">The 1st string</param>
        /// <param name="second">The 2nd string</param>
        /// <param name="length">the length of the segment</param>
        /// <returns></returns>
        public static bool StartsWith(this string first, string second, int length)
        {
            return first.StartsWith(second, 0, length, 0, StringComparison.Ordinal);
        }
        /// <summary>
        /// Conpare 2 strings to see if they start with the same characters
        /// </summary>
        /// <param name="first">The 1st string</param>
        /// <param name="second">The 2nd string</param>
        /// <param name="length">the length of the segment</param>
        /// <returns></returns>
        public static bool StartsWith(this string first, string second, int length, StringComparison stringComparison)
        {
            return first.StartsWith(second, 0, length, 0, stringComparison);
        }
    }
}
