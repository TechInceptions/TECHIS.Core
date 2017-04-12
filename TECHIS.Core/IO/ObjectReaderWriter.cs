using System;
using System.Collections.Generic;
using System.Text;
using TECHIS.Core.Modelling;
using System.IO;

namespace TECHIS.Core.IO
{
    public class ObjectReaderWriter
    {
        private const string _XMLExtension = ".xml";

        #region Fields 
        private DirectoryReaderWriter _ReaderWriter;
        #endregion

        #region Constructors 
        public ObjectReaderWriter(string folderPath)
        {
            InputValidator.ArgumentNullOrEmptyCheck(folderPath, "folderPath");

            _ReaderWriter = new DirectoryReaderWriter(folderPath);
        }
        #endregion

        #region Methods 

        public static string DeriveShortFullyQualifiedTypeName(string fullyQualifiedAssemblyName)
        {
            //System.String[], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
            //TECHIS.Core.Modelling.Group`1[[TECHIS.Core.Modelling.Attribute, OxyData, Version=3.0.20.0, Culture=neutral, PublicKeyToken=3ce59740ac1f38fb]], OxyData, Version=3.0.20.0, Culture=neutral, PublicKeyToken=3ce59740ac1f38fb

            InputValidator.ArgumentNullOrEmptyCheck(fullyQualifiedAssemblyName, "fullyQualifiedAssemblyName");

            fullyQualifiedAssemblyName = TECHIS.Core.Text.Strings.RemoveWhitespaces(fullyQualifiedAssemblyName);

            bool search = true;
            string tmpOutput = null;
            while (search)
            {
                int startIdx = 0, endIdx = -1, endIdx2 = -1, endIdx3 = -1;

                tmpOutput = RemoveTypeInfoSegment(fullyQualifiedAssemblyName, "Version=", startIdx, out endIdx);
                if (!string.IsNullOrEmpty(tmpOutput))
                {
                    fullyQualifiedAssemblyName = tmpOutput;
                }
                tmpOutput = RemoveTypeInfoSegment(fullyQualifiedAssemblyName, "Culture=", startIdx, out endIdx2);
                if (!string.IsNullOrEmpty(tmpOutput))
                {
                    fullyQualifiedAssemblyName = tmpOutput;
                }
                tmpOutput = RemoveTypeInfoSegment(fullyQualifiedAssemblyName, "PublicKeyToken=", startIdx, out endIdx3);
                if (!string.IsNullOrEmpty(tmpOutput))
                {
                    fullyQualifiedAssemblyName = tmpOutput;
                }

                search = !(endIdx == -1 && endIdx2 == -1 && endIdx3 == -1);
            }


            return fullyQualifiedAssemblyName.Replace(",",", ") ;

        }

        private static string RemoveTypeInfoSegment(string typeInfo, string stringToFind, int startIdx, out int endIdx)
        {//
            string val = null;
            bool endIsTerminal = false;
            //determine where to stop looking
            int cnt = typeInfo.Length-startIdx;

            int tmpIdx = typeInfo.IndexOf(stringToFind, startIdx, cnt, StringComparison.OrdinalIgnoreCase);
            endIdx = -1;
            if (tmpIdx!=-1)
            {
                for (int i = tmpIdx; i < typeInfo.Length; i++)
                {
                    char c = typeInfo[i];
                    if (c.Equals(',') || c.Equals(']') || char.IsWhiteSpace(c) || c.Equals('['))
                    {
                        endIdx = i - 1;
                        break;
                    }
                }

                //the segment ends the string
                if (endIdx==-1)
                {
                    endIdx = typeInfo.Length - 1;
                    endIsTerminal = true;
                }

                //take one of the comma's
                if (endIsTerminal)
                {
                    //The comma must be at the left
                    tmpIdx=tmpIdx - 1;
                }
                else
                {
                    if (typeInfo[tmpIdx-1]==',')
                    {
                        //the comma is at the right
                        tmpIdx = tmpIdx - 1;
                    }
                    else
                    {
                        //the comma is at the left
                        endIdx = endIdx + 1;
                    }
                }

                //remove the segment
                val = typeInfo.Remove(tmpIdx, (endIdx - tmpIdx) + 1); 
            }

            return val;
        }

        /// <summary>
        /// Serializes an object if 'input' is not a primitive type.
        /// If 'input' is a primitive type, the 'ToString' method is called on the instance
        /// </summary>
        /// <returns>The object as a string</returns>
        public static string SerializeObject(object input)
        {
            return SerializeObject(input, null);
        }

        /// <summary>
        /// Serializes an object if 'input' is not a primitive type.
        /// If 'input' is a primitive type, the 'ToString' method is called on the instance
        /// </summary>
        /// <param name="t">The Type to serialize the object as. This may be used to serialize a complex object as a primitive type.</param>
        /// <returns>The object as a string</returns>
        public static string SerializeObject(object input, Type t)
        {
            string typeName;
            return SerializeObject(input, t, out typeName);
        }

        /// <summary>
        /// Serializes an object if 'input' is not a primitive type.
        /// If 'input' is a primitive type, the 'ToString' method is called on the instance
        /// </summary>
        /// <param name="t">The Type to serialize the object as. This may be used to serialize a complex object as a primitive type.</param>
        /// <returns>The object as a string</returns>
        public static string SerializeObject(object input, Type t, out string typeName)
        {
            string value;
            

            InputValidator.ArgumentNullCheck(input, "input");

            if (t == null)
            {
                t = input.GetType();
            }

            TypeCode typecode = Type.GetTypeCode(t);
            typeName = typecode.ToString();

            switch (typecode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeExtensions.TypeCodeDbNull:
                case TypeCode.DateTime:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    value = input.ToString();
                    break;
                default:
                    value = Serialization.XmlSerializer.SerializeToUTF8(input);
                    typeName = DeriveShortFullyQualifiedTypeName( t.AssemblyQualifiedName);
                    break;
            }
            return value;
        }

        /// <summary>
        /// Provides a consist way to name types that is compatible with the ObjectReaderWriter.
        /// </summary>
        public static string GetTypeName(Type t)
        {
            InputValidator.ArgumentNullCheck(t, "t");

            TypeCode typecode = Type.GetTypeCode(t);
            string   typeName = typecode.ToString();

            switch (typecode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeExtensions.TypeCodeDbNull:
                case TypeCode.DateTime:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:

                    break;
                default:
                    typeName = DeriveShortFullyQualifiedTypeName(t.AssemblyQualifiedName);
                    break;
            }

            return typeName;
        }

        /// <summary>
        /// Parses or deserializes an object.
        /// if 't' is a primitive type, the 'Parse' method is called get an instance of the primitive type from the string.
        /// if 't' is a complex type, the string 'input' is deserialized.
        /// </summary>
        /// <param name="input">The string to Parse or deserialize</param>
        /// <param name="t">The type of the object to be derived from the string</param>
        public static object ParseObject(string input, Type t)
        {
            object value;
            TypeCode typecode = Type.GetTypeCode(t);

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
                case TypeExtensions.TypeCodeDbNull:
                    value = TypeExtensions.DbNullValue;
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
                    value = Serialization.XmlSerializer.Deserialize(input, t);
                    break;
            }
            return value;
        }

        public string Write<T>(Object objectToWrite, string objectName)
        {
            InputValidator.ArgumentNullCheck(objectToWrite, "objectToWrite");
            return WriteDataContract(objectToWrite, objectName);
        }
        public string Write<T>(T objectToWrite) where T : TECHIS.Core.Modelling.IAttribute
        {
            InputValidator.ArgumentNullCheck(objectToWrite, "objectToWrite");
            return WriteDataContract(objectToWrite, objectToWrite.Name);
        }
        public string Write<T>(Group<List<T>, T> objectToWrite) where T: Modelling.Attribute
        {
            InputValidator.ArgumentNullCheck(objectToWrite, "objectToWrite");
            return WriteDataContract(objectToWrite, objectToWrite.Name);
        }

        public string Write(Groups<List<Modelling.Attribute>, Modelling.Attribute> objectToWrite)
        {
            InputValidator.ArgumentNullCheck(objectToWrite, "objectToWrite");
            return Write(objectToWrite, objectToWrite.Name);
        }

        public string Write(IAttribute objectToWrite)
        {
            InputValidator.ArgumentNullCheck(objectToWrite, "objectToWrite");
            return Write(objectToWrite, objectToWrite.Name);
        }

        public string WriteDataContract(Object objectToWrite, string objectName)
        {
            InputValidator.ArgumentNullCheck(objectToWrite, "objectToWrite");
            InputValidator.ArgumentNullOrEmptyCheck(objectName, "objectName");

            string fqFilename = Path.Combine(_ReaderWriter.InitialFolder.FullName, GetFileName(objectName));
            Serialization.DataContractXmlSerializer.SerializeToFile(objectToWrite, fqFilename);

            return fqFilename;
        }

        public string Write(Object objectToWrite, string objectName)
        {
            InputValidator.ArgumentNullCheck(objectToWrite, "objectToWrite");
            InputValidator.ArgumentNullOrEmptyCheck(objectName, "objectName");

            return Serialization.XmlSerializer.SerializeToFile(objectToWrite, _ReaderWriter.InitialFolder, GetFileName(objectName));
        }

        /// <summary>
        /// Deserializes a named object from the well known store.
        /// This method uses the DataContractXmlSerializer.
        /// </summary>
        public T Read<T>(string objectName)
        {
            return (T)(ReadDataContract(objectName, typeof(T)));
        }

        /// <summary>
        /// Deserializes a named object from the well known store.
        /// This method uses the XmlSerializer.
        /// </summary>
        public T ReadXml<T>(string objectName)
        {
            return (T)(Read(objectName, typeof(T)));
        }
        public T ReadNamedObject<T>(string objectName) where T: class, IAttribute
        {
            T obj = Read<T>(objectName);
            if (obj!=null)
            {
                obj.Name = objectName;
            }

            return obj;
        }

        public object Read(string objectName, Type type)
        {
            InputValidator.ArgumentNullCheck(objectName);

            string fileName = GetFullyQualifiedFileName(objectName);

            object val = null;
            
            if (File.Exists(fileName))
            {
                if (Type.GetTypeCode(type) == TypeCode.String)
                {
                    val = _ReaderWriter.Read(fileName);
                }
                else
                {
                    val = Serialization.XmlSerializer.DeserializeFileStream(fileName, type);
                }
            }

            return val;
        }


        public object ReadDataContract(string objectName, Type type)
        {
            InputValidator.ArgumentNullCheck(objectName);

            string fileName = GetFullyQualifiedFileName(objectName);

            object val = null;

            if (File.Exists(fileName))
            {
                if (Type.GetTypeCode(type) == TypeCode.String)
                {
                    val = _ReaderWriter.Read(fileName);
                }
                else
                {
                    val = Serialization.DataContractXmlSerializer.DeserializeFileStream(fileName, type);
                }
            }

            return val;
        }

        /// <summary>
        /// Reads all the file in the folder with the specified prefix or suffix.
        /// </summary>
        /// <returns>a KeyValue collection. The key is the filename without the extension</returns>
        public List<KeyValuePair<string,string>> ReadFiles(string part, bool isPrefix)
        {
            List<KeyValuePair<string, string>> val = new List<KeyValuePair<string,string>>();
            
            FileInfo[] files = _ReaderWriter.Files;
            foreach (FileInfo fi in files)
            {
                string fName = Path.GetFileNameWithoutExtension(fi.Name);
                bool isInGroup;
                if (isPrefix)
                {
                    isInGroup = fName.StartsWith(part);
                }
                else
                {
                    isInGroup = fName.EndsWith(part);
                }

                if (isInGroup)
                {
                    string tmp = _ReaderWriter.Read(fi.Name);
                    val.Add(new KeyValuePair<string, string>(fName, tmp));
                }
            }

            return val;

        }

        protected virtual string GetFileName(string objectName)
        {
            return objectName + _XMLExtension;
        }

        protected string GetFullyQualifiedFileName(string objectName)
        {
            return Path.Combine(_ReaderWriter.InitialFolder.FullName, GetFileName(objectName));
        }

        #endregion


    }
}
