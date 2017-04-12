using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace TECHIS.Core.Serialization.XML
{
    /// <summary>
    /// This class encapsulates a serialized object. The type that the object can be deserialized into is 
    /// represented by the DataType property. The deserialized object can be retrieved via the Instance property
    /// The value property is an XMLElement object that represents the object
    /// </summary>
    [Serializable]
    public class Data : System.Runtime.Serialization.ISerializable, IXmlSerializable
    {
        #region Constants 
        private const string NAME_ELEMENT_SERIALIZATION = "E55B600A6527444aA90A2991ABE0E346";
        #endregion

        #region Static Fields 
        private static int _SerializationCount = 0;

        private static object _XDocSyncLock = new object();
        private static System.Xml.XmlDocument _XDoc = new System.Xml.XmlDocument();
        
        private static object _XDoc1SyncLock = new object();
        private static System.Xml.XmlDocument _XDoc1 = new System.Xml.XmlDocument();

        private static object _XDoc2SyncLock = new object();
        private static System.Xml.XmlDocument _XDoc2 = new System.Xml.XmlDocument();

        private static object _XDoc3SyncLock = new object();
        private static System.Xml.XmlDocument _XDoc3 = new System.Xml.XmlDocument();
       
        #endregion

        #region Fields

        private string                  _DataType;
        private XmlElement _Value;
        private object                  _Instance;
        private Exception _LastSerializationException;

        #endregion

        #region Properties 
        [XmlIgnore]
        public Exception LastSerializationException
        {
            get 
            {
                SetObject();
                return _LastSerializationException; 
            }
        }
        /// <summary>
        /// Returns the deserialized object contained in InfoData.Value
        /// </summary>
        [XmlIgnore]
        public object Instance
        {
            get
            {
                SetObject();
                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }

        /// <summary>
        /// The value property is an XMLElement object that represents the object
        /// </summary>
        [XmlAnyElement]
        public XmlElement Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        /// <summary>
        /// The type that the object can be deserialized into is 
        /// represented by the DataType property
        /// </summary>
        [XmlAttribute]
        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }
        #endregion

        #region Constructors 

        public Data() { }

        public Data(string xmlData, string dataType)
        {
            InputValidator.ArgumentNullOrEmptyCheck(xmlData, "xmlData");
            InputValidator.ArgumentNullOrEmptyCheck(dataType, "dataType");

            CreateInnerXml(xmlData);
        }
        public Data(object instance)
        {
            InputValidator.ArgumentNullCheck(instance);
            _Instance = instance;
            _DataType = _Instance.GetType().AssemblyQualifiedName;
        }


        #endregion
        
        #region private methods 

        private static XmlElement CreateXMLElement(string XMLData)
        {
            _SerializationCount++;
            lock (_XDocSyncLock)
            {
                _XDoc.InnerXml = XMLData;
                return (XmlElement)_XDoc.RemoveChild(_XDoc.DocumentElement);
            }
        }
        private static XmlElement CreateXMLElement1(string XMLData)
        {
            _SerializationCount++;
            lock (_XDoc1SyncLock)
            {
                _XDoc1.InnerXml = XMLData;
                return (XmlElement)_XDoc1.RemoveChild(_XDoc1.DocumentElement);
            }
        }
        private static XmlElement CreateXMLElement2(string XMLData)
        {
            _SerializationCount++;
            lock (_XDoc2SyncLock)
            {
                _XDoc2.InnerXml = XMLData;
                return (XmlElement)_XDoc2.RemoveChild(_XDoc2.DocumentElement);
            }
        }
        private static XmlElement CreateXMLElement3(string XMLData)
        {
            _SerializationCount++;
            lock (_XDoc3SyncLock)
            {
                _XDoc3.InnerXml = XMLData;
                return (XmlElement)_XDoc3.RemoveChild(_XDoc3.DocumentElement);
            }
        }

        private void CreateInnerXml(string xmlData)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.InnerXml = xmlData;
            _Value = (XmlElement)xDoc.RemoveChild(xDoc.DocumentElement);
        }

        private void SetObject()
        {

            if (_Instance == null)
            {
                if (_Value != null && _DataType != null && _DataType != string.Empty)
                {

                    try
                    {
                        _Instance = TECHIS.Core.Serialization.XmlSerializer.Deserialize2(_Value.OuterXml, _DataType);
                    }
                    catch (Exception ex)
                    {
                        _LastSerializationException = ex;
                    }
                }
            }

        }

        private void SerializeInstance()
        {
            if (_Value == null)
            {
                _DataType = Instance.GetType().AssemblyQualifiedName;
                
                if (Instance != null)
                    CreateInnerXml(TECHIS.Core.Serialization.XmlSerializer.SerializeToUTF8(_Instance));
            }
        }

        #endregion

        #region Public Methods 

        #endregion

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            string inxml = null;
            try
            {

                _DataType = reader.GetAttribute("DataType", null);

                try
                {
                    inxml = reader.ReadInnerXml();
                }
                catch (Exception XC)
                {
                    throw new Exception("Error in reader.ReadInnerXml()", XC);
                }

                if (! string.IsNullOrEmpty(inxml) )
                {
                    CreateXMLElementSync(inxml);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(
                    (new StringBuilder())
                    .Append( "Error in 'IXmlSerializable.ReadXml(System.Xml.XmlReader reader)': XML Value: ")
                    .Append(reader.Value)
                    .AppendLine(". ReadState:")
                    .Append(reader.ReadState)
                    .AppendLine(". ReadOuterXml():")
                    .Append(reader.ReadOuterXml())
                    .AppendLine(". ReadString():")
                    .Append(reader.ReadElementContentAsString() )
                    .AppendLine(". ReadInnerXml():")
                    .Append(reader.ReadInnerXml())
                    .AppendLine(". DataType")
                    .Append(_DataType)
                    .AppendLine(". InnerXml:")
                    .Append(inxml)
                    .ToString() ,Ex);

            }
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            SerializeInstance();

            writer.WriteAttributeString("DataType",_DataType);
            writer.WriteRaw(_Value.OuterXml);
        }

        #endregion

        #region ISerializable Members 

        void System.Runtime.Serialization.ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            SerializeInstance();

            info.AddValue("DataType",_DataType);

            if(_Value!=null)
                info.AddValue(NAME_ELEMENT_SERIALIZATION, _Value.OuterXml);
        }

        public Data(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            _DataType = (string)info.GetValue("DataType", typeof(string));

            string xmlData = info.GetString(NAME_ELEMENT_SERIALIZATION);

            CreateXMLElementSync(xmlData);
        }

        private void CreateXMLElementSync(string xmlData)
        {
            try
            {
                switch (_SerializationCount)
                {
                    case 0:
                        _Value = CreateXMLElement(xmlData);
                        break;
                    case 1:
                        _Value = CreateXMLElement1(xmlData);
                        break;
                    case 2:
                        _Value = CreateXMLElement2(xmlData);
                        break;
                    case 3:
                        _Value = CreateXMLElement3(xmlData);
                        break;
                    default:
                        _Value = CreateXMLElement2(xmlData);
                        break;
                }

                if (_SerializationCount > 3 || _SerializationCount < 0)
                {
                    _SerializationCount = 0;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception( (new StringBuilder(100)).Append( "Error in CreateXMLElementSync(string xmlData): XML:" ).Append( xmlData ).ToString(),Ex);
            }
        }

        #endregion
    }
}
