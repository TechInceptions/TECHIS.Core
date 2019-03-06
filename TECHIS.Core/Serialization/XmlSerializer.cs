using System;
using System.IO;
using System.Xml;

namespace TECHIS.Core.Serialization
{
	/// <summary>
	/// Summary description for XmlSerializer.
	/// </summary>
	public class XmlSerializer
	{
		public XmlSerializer()
		{

		}
		/// <summary>
		/// Serialize an object to a string using the default encoding
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static byte[] Serialize(object o)
		{

            System.Xml.Serialization.XmlSerializer XS = new System.Xml.Serialization.XmlSerializer(o.GetType());

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            XS.Serialize(ms, o);

            return ms.ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static string SerializeToUTF8(object o)
		{
            byte[] data = Serialize(o);
            return System.Text.UTF8Encoding.UTF8.GetString(data);
        }

        public static string DecodeToUTF8(byte[] data)
        {
            return System.Text.UTF8Encoding.UTF8.GetString(data);
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
        /// Deserializes the string to the type of the passed-in objectType. An XmlTextReader is used to better preserve the format of internal strings
        /// </summary>
        public static object Deserialize2(string data, Type objectType)
        {

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(objectType);
            object obj;
            //System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(data));
            using (StringReader sr = new StringReader(data))
            {
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    obj = xs.Deserialize(xr);
                }
            }

            return  obj ;

        }

        /// <summary>
        /// Deserializes the string to the type of the passed-in objectType
        /// </summary>
        public static object Deserialize(TextReader data, Type objectType)
        {

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(objectType);

            return xs.Deserialize(data);

        }

        public static object Deserialize2(string data, string objectType)
        {

            System.Type t = Type.GetType(objectType, true);

            return Deserialize2(data, t);

        }
        public static object Deserialize(string data, string objectType)
        {

            System.Type t = Type.GetType(objectType, true);

            return Deserialize(data, t);

        }

        /// <summary>
        /// Deserializes the contents of a text file to the type of the passed-in objectType. 
        /// The contents are initially read into a string object. The UTF8 encoding is specified.
        /// </summary>
        public static object DeserializeFile(string fullyqualifiedFileName, Type objectType)
        {
            using (var fs = File.Open(fullyqualifiedFileName, FileMode.Open))
            {
                string sXml;
                using (System.IO.StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    sXml = sr.ReadToEnd();
                }

                return Deserialize(sXml, objectType);
            }
        }

        /// <summary>
        /// Use this method to deserialize large files. Deserializes the contents of a text file to the type of the passed-in objectType. 
        /// The contents of the file is streamed from the file into the object.
        /// </summary>
        public static object DeserializeFileStream(string FQFileName, Type objectType)
        {
            object obj;
            using (System.IO.StreamReader sr = (new System.IO.FileInfo(FQFileName)).OpenText())
            {
                obj = Deserialize(sr, objectType);
            }

            return obj;
        }

        /// <summary>
        /// Serializes the object to the specified file. The object is serialized directly to the file. Use for large objects.
        /// </summary>
        public static string SerializeToFile(object o, DirectoryInfo folder, string FilenameOnly)
        {
            InputValidator.ArgumentNullCheck(folder, "folder");
            InputValidator.ArgumentNullCheck(FilenameOnly, "Filename");

            string fPath = Path.Combine( folder.FullName, FilenameOnly);

            SerializeToFile(o, fPath);

            return fPath;
        }

        /// <summary>
        /// Reads the file into a string object, then deserializes the string. no encoding is specified.
        /// An XmlTextReader is used to better preserve the format of internal strings.
        /// </summary>
        public static object DeserializeFile2(string FQFileName, Type objectType)
        {
            string sXml = TECHIS.Core.IO.DirectoryReaderWriter.ReadText(FQFileName);

            return Deserialize2(sXml, objectType);
        }

        /// <summary>
        /// Serializes the object to the specified file. The object is serialized directly to the file. Use for large objects.
        /// </summary>
        public static void SerializeToFile(object o, string FQFilename)
        {
            InputValidator.ArgumentNullCheck(o, "o");
            InputValidator.ArgumentNullOrEmptyCheck(FQFilename, "FQFilename");
            
            System.Xml.Serialization.XmlSerializer XS = new System.Xml.Serialization.XmlSerializer(o.GetType());

            using (FileStream fs = new FileStream(FQFilename, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.AutoFlush = true;
                    XS.Serialize(sw, o);
                }
            }
        }

        /// <summary>
        /// Serializes the object to the specified file. The object is serialized directly to the file. Use for large objects.
        /// </summary>
        public static void Serialize(object o, TextWriter writer)
        {
            InputValidator.ArgumentNullCheck(o, "o");

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(o.GetType());

            xs.Serialize(writer, o);

        }
        /// <summary>
        /// Serializes the object to the specified file. A string is created then saved to the specified file
        /// </summary>
        public static void SerializeToFile2(object o, string FQFilename)
        {
            InputValidator.ArgumentNullCheck(o, "o");
            InputValidator.ArgumentNullOrEmptyCheck(FQFilename, "FQFilename");

            string sXml = XmlSerializer.SerializeToUTF8(o);
            TECHIS.Core.IO.DirectoryReaderWriter.WriteString(sXml, FQFilename);

        }
    }
}
