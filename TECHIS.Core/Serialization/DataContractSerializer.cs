using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;

namespace TECHIS.Core.Serialization
{
    /// <summary>
    /// Performs serialization to and from xml, using the DataContractSerializer
    /// </summary>
    public static class DataContractXmlSerializer
    {
        /// <summary>
        /// Uses DataContractSerializer.WriteObject to write object to file.
        /// </summary>
        public static void SerializeToFile(XmlWriter tw, object data)
        {
            DataContractSerializer ser = new DataContractSerializer(data.GetType());
            
                ser.WriteObject(tw, data);
            
        }

        /// <summary>
        /// Serialize an object to a string using the specified encoding. If encoding is null, the default encoding is used.
        /// </summary>
        public static string Serialize(object data, Encoding encoding)
        {
            InputValidator.ArgumentNullCheck(data, "data");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            if (encoding!=null)
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
        public static string Serialize(object data)
        {
            return Serialize(data, null);
        }

        /// <summary>
        /// Deserializes the string to the generic type
        /// </summary>
        public static T Deserialize<T>(string data)
        {
            Type objectType = typeof(T);
            T obj = (T)(Deserialize(data, objectType));

            return obj;
        }

        /// <summary>
        /// Deserializes the string to the type of the passed-in objectType
        /// </summary>
        public static object Deserialize(string data, Type objectType)
        {
            object obj;
            using (StringReader sr = new StringReader(data))
            {
                obj = Deserialize(sr, objectType);
            }
            return obj;
        }

        /// <summary>
        /// Deserializes the text from the reader to the type of the passed-in objectType
        /// </summary>
        public static object Deserialize(TextReader data, Type objectType)
        {
            object obj = null;
            DataContractSerializer ser = new DataContractSerializer(objectType);

            using (XmlReader xr = XmlReader.Create(data))
            {
                obj = ser.ReadObject(xr, true);
            }

            return obj;
        }


        /// <summary>
        /// Use this method to deserialize large files. Deserializes the contents of a text file to the type of the passed-in objectType. 
        /// The contents of the file is streamed from the file into the object.
        /// </summary>
        public static object DeserializeFileStream(string fullyQualifiedFileName, Type objectType)
        {
            object obj;
            using (System.IO.StreamReader sr = (new System.IO.FileInfo(fullyQualifiedFileName)).OpenText())
            {
                obj = Deserialize(sr, objectType);
            }

            return obj;
        }

        /// <summary>
        /// Deserializes the contents of a text file to the type of the passed-in objectType. 
        /// The contents are initially read into a string object. The UTF8 encoding is specified.
        /// </summary>
        public static object DeserializeFile(string fullyQualifiedFileName, Type objectType)
        {
            string sXml = TECHIS.Core.IO.DirectoryReaderWriter.ReadText(fullyQualifiedFileName);
            return Deserialize(sXml, objectType);
        }


        /// <summary>
        /// Serializes the object to the specified file. A string is created then saved to the specified file
        /// </summary>
        public static void SerializeToFile(object o, string FQFilename)
        {
            InputValidator.ArgumentNullCheck(o, "o");
            InputValidator.ArgumentNullOrEmptyCheck(FQFilename, "FQFilename");

            string sXml = Serialize(o);
            TECHIS.Core.IO.DirectoryReaderWriter.WriteString(sXml, FQFilename);

        }

        /// <summary>
        /// Serializes the object to the specified file. The object is serialized directly to the file. Use for large objects.
        /// </summary>
        public static void SerializeLargeObjectToFile(object o, string FQFilename, Encoding encoding)
        {
            InputValidator.ArgumentNullCheck(o, "o");
            InputValidator.ArgumentNullOrEmptyCheck(FQFilename, "FQFilename");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            if (encoding != null)
            {
                settings.Encoding = encoding;
            }

            using (FileStream fs = new FileStream(FQFilename, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.AutoFlush = true;
                    using (XmlWriter tw = XmlWriter.Create(sw, settings))
                    {
                        SerializeToFile(tw, o);
                    }
                }
            }
        }
    }
}
