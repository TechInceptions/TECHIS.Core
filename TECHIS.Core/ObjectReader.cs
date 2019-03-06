//----------------------------------------------------------OCG Version: 3.6.12.0
// <copyright company="TECH-IS INC.">
//     Copyright (c) TECH-IS INC.  All rights reserved.
//     Please read license file: http://www.OxyGenCode.com/licence/TECH-IS_INC_PUBLIC_LICENCE.rtf
// </copyright>
//----------------------------------------------------------OCG Version: 3.6.12.0

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace TECHIS.Core
{
    /// <summary>
    /// Parses strings to objects
    /// </summary>
    public static class ObjectReader
    {
        private static int _SizeOfChar = sizeof(char);
        private const char SEPERATOR_ASSEMBLYNAME = '.';
        private const string PREFIX_DEFAULT_ASSEMBLYNAME = "System";

        public static Dictionary<string, Type> _TypeHistory = new Dictionary<string, Type>();

        /// <summary>
        /// Serialize an object to a string using the specified encoding. If encoding is null, the default encoding is used.
        /// </summary>
        public static string SerializeWithDataContract(object data, Encoding encoding)
        {
            InputValidator.ArgumentNullCheck(data, "data");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            if (encoding != null)
            {
                settings.Encoding = encoding;
            }

            StringBuilder target = new StringBuilder();
            DataContractSerializer ser = new DataContractSerializer(data.GetType());
            using (XmlWriter tw = XmlWriter.Create(target, settings))
            {
                ser.WriteObject(tw, data);
            }


            return target.ToString();
        }

        /// <summary>
        /// Serialize an object to a string using the default encoding
        /// </summary>
        public static string SerializeWithDataContract(object data)
        {
            return SerializeWithDataContract(data, null);
        }

        /// <summary>
        /// Deserializes the string to the type of the passed-in objectType
        /// </summary>
        public static object Deserialize(string data, Type objectType)
        {

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(objectType);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(data)))
            {
                return xs.Deserialize(ms);
            }
        }

        /// <summary>
        /// Parses or deserializes an object.
        /// if 't' is a primitive type, the 'Parse' method is called to get an instance of the primitive type from the string.
        /// if 't' is a complex type, the string 'input' is deserialized.
        /// </summary>
        /// <param name="input">The string to Parse or deserialize</param>
        /// <param name="t">The type of the object to be derived from the string</param>
        public static object ParseObject(string input, Type t)
        {
            object value;
            TypeCode typecode = t.GetTypeCode();

            switch (typecode)
            {
                case TypeCode.Boolean:
                    value = bool.Parse(input);
                    break;
                case TypeCode.Byte:
                    value = byte.Parse(input);
                    break;
                case TypeCode.Char:
                    value = char.Parse(input);
                    break;
#if NETSTANDARD1_5
                case TypeExtensions.TypeCodeDbNull:
                    value = TypeExtensions.DbNullValue;
#elif NETSTANDARD1_6
                    case TypeExtensions.TypeCodeDbNull:
                    value = TypeExtensions.DbNullValue;
#else
                    case TypeCode.DBNull:
                     value = DBNull.Value;
#endif
                    break;
                case TypeCode.DateTime:
                    value = DateTime.Parse(input);
                    break;
                case TypeCode.Decimal:
                    value = Decimal.Parse(input);
                    break;
                case TypeCode.Double:
                    value = Double.Parse(input);
                    break;
                case TypeCode.Int16:
                    value = Int16.Parse(input);
                    break;
                case TypeCode.Int32:
                    value = Int32.Parse(input);
                    break;
                case TypeCode.Int64:
                    value = Int64.Parse(input);
                    break;
                case TypeCode.SByte:
                    value = SByte.Parse(input);
                    break;
                case TypeCode.Single:
                    value = Single.Parse(input);
                    break;
                case TypeCode.String:
                    value = input;
                    break;
                case TypeCode.UInt16:
                    value = UInt16.Parse(input);
                    break;
                case TypeCode.UInt32:
                    value = UInt32.Parse(input);
                    break;
                case TypeCode.UInt64:
                    value = UInt64.Parse(input);
                    break;
                default:
                    value = Deserialize(input, t);
                    break;
            }
            return value;
        }

        public static object ParseObject(string input, string typeName)
        {
            object value = null;

            Type t = GetType(typeName);

            value = ParseObject(input, t);
            return value;
        }

        /// <summary>
        /// Parses or deserializes an object.
        /// if 't' is a primitive type, the 'TryParse' method is called to get an instance of the primitive type from the string.
        /// if 't' is a complex type, the string 'input' is deserialized.
        /// </summary>
        /// <param name="input">The string to Parse or deserialize</param>
        /// <param name="t">The type of the object to be derived from the string</param>
        public static bool TryParseObject(string input, Type t, out object value)
        {
            //
            
            bool success;
            value=null;
            TypeCode typecode = t.GetTypeCode();

            switch (typecode)
            {
                case TypeCode.Boolean:
                    {
                        bool val;
                        success = bool.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.Byte:
                    {
                        byte val;
                        success = byte.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.Char:
                    {
                        char val;
                        success = char.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
#if NETSTANDARD1_5
                    case TypeExtensions.TypeCodeDbNull:
                    value = TypeExtensions.DbNullValue;
#elif NETSTANDARD1_6
                    case TypeExtensions.TypeCodeDbNull:
                    value = TypeExtensions.DbNullValue;
#else
                    case TypeCode.DBNull:
                     value = DBNull.Value;
#endif
                    success = true;
                    break;
                case TypeCode.DateTime:
                    {
                        DateTime val;
                        success = DateTime.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.Decimal:
                    {
                        Decimal val;
                        success = Decimal.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.Double:
                    {
                        Double val;
                        success = Double.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.Int16:
                    {
                        Int16 val;
                        success = Int16.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.Int32:
                    {
                        Int32 val;
                        success = Int32.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.Int64:
                    {
                        Int64 val;
                        success = Int64.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.SByte:
                    {
                        SByte val;
                        success = SByte.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.Single:
                    {
                        Single val;
                        success = Single.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.String:
                    value = input;
                    success = true;
                    break;
                case TypeCode.UInt16:
                    {
                        UInt16 val;
                        success = UInt16.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.UInt32:
                    {
                        UInt32 val;
                        success = UInt32.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                case TypeCode.UInt64:
                    {
                        UInt64 val;
                        success = UInt64.TryParse(input, out val);
                        if (success)
                        {
                            value = val;
                        }
                    }
                    break;
                default:
                    try
                    {
                        value = Deserialize(input, t);
                        success =true;
                    }
                    catch {
                        success = false;
                        value = null;
                    }
                    break;
            }

            return success;
        }

        /// <summary>
        /// Parses or deserializes an object. The type of the object is derived from typeName
        /// if 't' is a primitive type, the 'TryParse' method is called to get an instance of the primitive type from the string.
        /// if 't' is a complex type, the string 'input' is deserialized.
        /// </summary>
        public static bool TryParseObject(string input, string typeName, out object value)
        {
            return TryParseObject(input, GetType(typeName), out value);
        }

        /// <summary>
        /// Derives a Type instance from a 'typeName'.
        /// The type history is checked.
        /// </summary>
        private static Type GetType(string typeName)
        {
            Type t = null;
            if (_TypeHistory.ContainsKey(typeName))
            {
                t = _TypeHistory[typeName];
            }
            else
            {

                string fullTypeName;
                bool needsAssemblyName = (typeName.IndexOf(SEPERATOR_ASSEMBLYNAME) == -1);
                if (needsAssemblyName)
                {
                    fullTypeName = string.Format("{0}{1}{2}", PREFIX_DEFAULT_ASSEMBLYNAME, SEPERATOR_ASSEMBLYNAME, typeName);
                    if (fullTypeName.EndsWith("?"))
                    {
                        //fullTypeName = string.Format(@"System.Nullable`1[{0}]", fullTypeName.Substring(0, fullTypeName.Length - 1));

                        //since a value is involved, the nullable type will not be null
                        fullTypeName = fullTypeName.Substring(0, fullTypeName.Length - 1);
                    }

                    t = Type.GetType(fullTypeName);

                    _TypeHistory[fullTypeName] = t;
                }
                if (t == null)
                {
                    t = Type.GetType(typeName);
                }
                _TypeHistory[typeName] = t;

            }
            return t;
        }


        /// <summary>
        /// Deserializes the string to the type of the passed-in objectType
        /// </summary>
        public static object XmlDeserialize(TextReader data, Type objectType)
        {

            System.Xml.Serialization.XmlSerializer XS = new System.Xml.Serialization.XmlSerializer(objectType);

            return XS.Deserialize(data);

        }

        /// <summary>
        /// Use this method to deserialize large files. Deserializes the contents of a text file to the type of the passed-in objectType. 
        /// The contents of the file is streamed from the file into the object.
        /// </summary>
        public static object DeserializeFileStream(string fullyQualifiedFileName, Type objectType, int numberOfTries = 3, int delayOnRetry = 500)
        {
            System.Xml.Serialization.XmlSerializer XS = new System.Xml.Serialization.XmlSerializer(objectType);
            object obj = null;
            for (int i = 1; i <= numberOfTries; ++i)
            {
                try
                {
                    using (System.IO.StreamReader sr = (new System.IO.FileInfo(fullyQualifiedFileName)).OpenText())
                    {
                        obj = XS.Deserialize(sr);
                    }
                }
                catch (IOException)
                {
                    if (i == numberOfTries)
                        throw;


#if NETSTANDARD1_5
                    
#elif NETSTANDARD1_6
                   
#else
                     System.Threading.Thread.Sleep(delayOnRetry);
#endif
                }
            }

            return obj;

        }
        public static object DeserializeFileStream(byte[] data, Type objectType)
        {
            object obj;
            if (data == null || data.Length == 0)
            {
                obj = null;
            }
            using (System.IO.StreamReader sr = new StreamReader(new MemoryStream(data)))
            {
                obj = XmlDeserialize(sr, objectType);
            }

            return obj;
        }

        /// <summary>
        /// Deserializes the string to the type of the passed-in objectType
        /// </summary>
        public static object XmlDeserialize(string data, Type objectType, Encoding encoding = null, bool byPassDefaultEncoding = false)
        {
            var xs = new System.Xml.Serialization.XmlSerializer(objectType);
            if (encoding == null && (!byPassDefaultEncoding))
            {
                encoding = System.Text.Encoding.UTF8;
            }
            using (MemoryStream ms = new MemoryStream(encoding == null ? GetBytes(data) : encoding.GetBytes(data)))
            {
                return xs.Deserialize(ms);
            }
        }
        public static object XmlDeserialize(byte[] data, Type objectType, System.Text.Encoding encoding = null)
        {
            var xs = new System.Xml.Serialization.XmlSerializer(objectType);

            using (MemoryStream ms = new MemoryStream(data))
            {
                return xs.Deserialize(ms);
            }
        }
        /// <summary>
        /// Parses or deserializes an object.
        /// if 't' is a primitive type, the 'Parse' method is called to get an instance of the primitive type from the string.
        /// if 't' is a complex type, the string 'input' is deserialized.
        /// </summary>
        /// <param name="input">The string to Parse or deserialize</param>
        /// <param name="t">The type of the object to be derived from the string</param>
        public static object ParseObject(string input, Type t, Encoding encoding = null, bool byPassDefaultEncoding = false)
        {
            object value;
            TypeCode typecode = Type.GetTypeCode(t);

            switch (typecode)
            {
                case TypeCode.Boolean:
                    {
                        if (input != null && input.Length == 1)
                        {
                            value = ParseForSingleCharBoolean(input);
                        }
                        else
                        {
                            value = bool.Parse(input);
                        }
                    }
                    break;
                case TypeCode.Byte:
                    value = byte.Parse(input);
                    break;
                case TypeCode.Char:
                    value = char.Parse(input);
                    break;
#if NETSTANDARD1_5
                case TypeExtensions.TypeCodeDbNull:
                    value = TypeExtensions.DbNullValue;
#elif NETSTANDARD1_6
                    case TypeExtensions.TypeCodeDbNull:
                    value = TypeExtensions.DbNullValue;
#else
                    case TypeCode.DBNull:
                     value = DBNull.Value;
#endif
                    break;
                case TypeCode.DateTime:
                    value = DateTime.Parse(input);
                    break;
                case TypeCode.Decimal:
                    value = Decimal.Parse(input);
                    break;
                case TypeCode.Double:
                    value = Double.Parse(input);
                    break;
                case TypeCode.Int16:
                    value = Int16.Parse(input);
                    break;
                case TypeCode.Int32:
                    value = Int32.Parse(input);
                    break;
                case TypeCode.Int64:
                    value = Int64.Parse(input);
                    break;
                case TypeCode.SByte:
                    value = SByte.Parse(input);
                    break;
                case TypeCode.Single:
                    value = Single.Parse(input);
                    break;
                case TypeCode.String:
                    value = input;
                    break;
                case TypeCode.UInt16:
                    value = UInt16.Parse(input);
                    break;
                case TypeCode.UInt32:
                    value = UInt32.Parse(input);
                    break;
                case TypeCode.UInt64:
                    value = UInt64.Parse(input);
                    break;
                default:
                    if (string.IsNullOrEmpty(input))
                    {
                        value = null;
                    }
                    else
                    {
                        value = XmlDeserialize(input, t, encoding, byPassDefaultEncoding);
                    }
                    break;
            }
            return value;
        }

        private static object ParseForSingleCharBoolean(string input)
        {
            object value;
            value = null;
            char c = input[0];
            if (c == '1')
            {
                value = true;
            }
            else if (c == '0')
            {
                value = false;
            }

            if (value == null)
            {
                c = char.ToLower(c);
                if (c == 'y')
                {
                    value = true;
                }
                else if (c == 'n')
                {
                    value = false;
                }
            }
            return value;
        }



        public static Dictionary<string, int> Deserialize(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int count = reader.ReadInt32();
                Dictionary<string, int> dictionary = new Dictionary<string, int>(count);
                for (int n = 0; n < count; n++)
                {
                    var key = reader.ReadString();
                    var value = reader.ReadInt32();
                    dictionary.Add(key, value);
                }
                return dictionary;
            }
        }


        public static Dictionary<string, Guid> Deserialize(byte[] data)
        {
            //Dictionary<string, Guid> vals = null;
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    Dictionary<string, Guid> vals = new Dictionary<string, Guid>(count);
                    for (int n = 0; n < count; n++)
                    {
                        var key = reader.ReadString();
                        var value = new Guid(reader.ReadBytes(16));
                        vals[key] = value;
                    }
                    return vals;
                }
            }
        }


        public static Dictionary<string, DateTime> DeserializeTimeDictionary(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int count = reader.ReadInt32();
                var dictionary = new Dictionary<string, DateTime>(count);
                for (int n = 0; n < count; n++)
                {
                    var key = reader.ReadString();
                    var value = DateTime.FromBinary(reader.ReadInt64());
                    dictionary.Add(key, value);
                }
                return dictionary;
            }
        }


        public static Dictionary<string, DateTime> DeserializeTimeDictionary(byte[] data)
        {
            //Dictionary<string, DateTime> vals = null;
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    Dictionary<string, DateTime> vals = new Dictionary<string, DateTime>(count);
                    for (int n = 0; n < count; n++)
                    {
                        var key = reader.ReadString();
                        var value = DateTime.FromBinary(reader.ReadInt64());
                        vals[key] = value;
                    }
                    return vals;
                }
            }
        }

        public static Dictionary<string, string> DeserializeStringDictionary(byte[] data)
        {
            //Dictionary<string, string> vals = null;
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    Dictionary<string, string> vals = new Dictionary<string, string>(count);
                    for (int n = 0; n < count; n++)
                    {
                        var key = reader.ReadString();
                        var value = reader.ReadString();
                        vals[key] = value;
                    }
                    return vals;
                }
            }
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * _SizeOfChar];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / _SizeOfChar];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
