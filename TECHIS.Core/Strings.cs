using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Text
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class Strings
    {
        #region Fields
        private const int UNIQUE_IDENTIFIER_MAX_LENGTH = 9;
        private string _Source;
        public const string SPACING_TAB = "   ";
        private static string _PaddedNewline;

        /// <summary>
        /// The newline string plus tab spacing
        /// </summary>
        public static string PaddedNewline
        {
            get { return _PaddedNewline; }
        }
        #endregion Fields

        #region Constructors 
        static Strings()
        {
            _PaddedNewline = Environment.NewLine + SPACING_TAB;
        }
        /// <summary>
        /// Creates an instance of OxyGen.UI.Util.Text.StringHelper
        /// </summary>
        /// <param name="source">The underlying string</param>
        public Strings(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            _Source = source;
        }
        #endregion Constructors

        #region Public Properties
        /// <summary>
        /// Gets the source string
        /// </summary>
        public string Source
        {
            get
            {
                return _Source;
            }
        }
        #endregion Public Properties

        #region public Methods 
        
        public static string GetNewName(char extensionChar = '.', bool excludeExtension = false, bool mergeNameParts = false)
        {
            string tmp = Path.GetRandomFileName();
            if (extensionChar != '.')
            {
                tmp = tmp.Replace('.', extensionChar);
            }
            if (mergeNameParts)
            {
                StringBuilder sb = new StringBuilder(tmp);
                int idx = tmp.IndexOf(extensionChar);
                if (idx != -1)
                {
                    sb.Remove(idx, 1);
                    tmp = sb.ToString();
                }
            }
            else if (excludeExtension)
            {
                if (tmp.IndexOf(extensionChar) != -1)
                {
                    tmp = tmp.Split(extensionChar)[0];
                }
            }
            return tmp;
        }
        public static string RemoveWhitespaces(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            List<char> allChar = new List<char>(text);

            for (int i = allChar.Count - 1; i >= 0; i--)
            {
                if (char.IsWhiteSpace(allChar[i]))
                {
                    allChar.RemoveAt(i);
                }
            }

            return new string(allChar.ToArray());
        } 
        /// <summary>
        /// Determines whether the underlying string contains punctuation characters
        /// </summary>
        /// <returns>true if the underlying string contains punctuation characters, otherwise false</returns>
        public bool ContainsPunctuation()
        {
            bool containsPunctuation = false;
            foreach (char dataChar in _Source)
            {
                if (char.IsPunctuation(dataChar) && (dataChar != '_'))
                {
                    containsPunctuation = true;
                    break;
                }
            }
            return containsPunctuation;
        }

        /// <summary>
        /// Determines whether the underlying string contains separator characters
        /// </summary>
        /// <returns>true if the underlying string contains punctuation characters, otherwise false</returns>
        public bool ContainsSeparator()
        {
            bool containsPunctuation = false;
            foreach (char dataChar in _Source)
            {
                if (char.IsSeparator(dataChar))
                {
                    containsPunctuation = true;
                    break;
                }
            }
            return containsPunctuation;
        }

        /// <summary>
        /// Determines whether the underlying string starts with one character from the specified array
        /// </summary>
        /// <param name="values">The array of characters to check against</param>
        /// <returns>true if the underlying string starts with one of the specified characters, otherwise false</returns>
        public bool StartsWithOneChar(char[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            bool startWithChar = false;
            foreach (char value in values)
            {
                if (_Source.StartsWith(value.ToString()))
                {
                    startWithChar = true;
                    break;
                }
            }
            return startWithChar;
        }

        /// <summary>
        /// Gets the result of trimming the underlying string with one of the specified characters.
        /// </summary>
        /// <param name="values">The array of characters to trim against</param>
        /// <returns>The result of trimming the underlying string with one of the specified characters.</returns>
        public string TrimStartOneChar(char[] values)
        {
            string result = _Source;
            foreach (char value in values)
            {
                if (_Source.StartsWith(value.ToString()))
                {
                    result = result.Substring(1);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the string as the result of trimming the underlying source up to the specified maximum length
        /// that guarantees the uniqueness of 2 strings that have the same number of semnificative chars
        /// </summary>
        /// <param name="maxLength">The maximum length of the result</param>
        /// <returns>The trimmed string up to the maximum specified length</returns>
        public string TrimEndUniqueResult(int maxLength)
        {
            return TrimEndUniqueResult(maxLength, false);
        }

        /// <summary>
        /// Gets the string as the result of trimming the underlying source up to the specified maximum length
        /// that guarantees the uniqueness of 2 strings that have the same number of semnificative chars
        /// </summary>
        /// <param name="maxLength">The maximum length of the result</param>
        /// <param name="includeDelimiter">Include a delimiter between the remaining string and its identifier</param>
        /// <returns>The trimmed string up to the maximum specified length</returns>
        public string TrimEndUniqueResult(int maxLength, bool includeDelimiter)
        {
            if (maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException("maxLength", maxLength, "Maximum length is less or equal to 0.");
            }

            if (_Source.Length <= maxLength)
            {
                return _Source;
            }
            else
            {
                int sourceMinLength = includeDelimiter ? UNIQUE_IDENTIFIER_MAX_LENGTH + 1 : UNIQUE_IDENTIFIER_MAX_LENGTH;
                if (_Source.Length < sourceMinLength)
                {
                    throw new InvalidOperationException(String.Format("Source minimum length should be {0}.", sourceMinLength));
                }
            }
            string sourceHashCode = String.Format("{0:X9}", _Source.GetHashCode());
            bool isNegative = sourceHashCode.StartsWith("-");
            if (isNegative)
            {
                sourceHashCode = "F" + sourceHashCode.Substring(1);
            }

            if (includeDelimiter)
            {
                return _Source.Substring(0, maxLength - UNIQUE_IDENTIFIER_MAX_LENGTH - 1) + "_" + sourceHashCode;
            }
            else
            {
                return _Source.Substring(0, maxLength - UNIQUE_IDENTIFIER_MAX_LENGTH) + sourceHashCode;
            }

        }

        public StringBuilder XmlEncode()
        {
            StringBuilder val = new StringBuilder(_Source);
            val.Replace("&", "&amp;");
            val.Replace("<", "&lt;");
            val.Replace(">", "&gt;");
            val.Replace(@"""", "&quot;");
            val.Replace(@"'", "&apos;");

            return val;
        }
        /// <summary>
        /// Pads all new lines in the target with the specified string (value)
        /// </summary>
        /// <param name="value">what to pad with. if value is null, pads with the default tab string. If empty string, no padding occurs</param>
        /// <returns>returns the padded target</returns>
        public static StringBuilder PadLines(StringBuilder target, string value)
        {
            target.Insert(0, Environment.NewLine);
            if (value == null)
            {
                target.Replace(Environment.NewLine, _PaddedNewline);
            }
            else
            {
                if(! (value.Length==0) )
                    target.Replace(Environment.NewLine, Environment.NewLine + value);
            }
            target.Remove(0, Environment.NewLine.Length);
            return target;
        }

        protected static StringBuilder StatementWriter(StringBuilder statementToAdd, StringBuilder Container, string PlaceHolderConstant)
        {
            return Container.Replace(PlaceHolderConstant, statementToAdd.Append(System.Environment.NewLine).Append(PlaceHolderConstant).ToString());
            
        }

        public static int GetHashCodeFromCollection(IList<string> parts)
        {
            int retval;
            if (parts == null || parts.Count == 0)
            {
                retval = 0;
            }
            else
            {
                StringBuilder builder = new StringBuilder(parts.Count * 16);
                foreach (string s in parts)
                {
                    builder.Append('$').Append(s);
                }
                retval = builder.ToString().GetHashCode();
            }
            return retval;
        }
        #endregion
    }
}
