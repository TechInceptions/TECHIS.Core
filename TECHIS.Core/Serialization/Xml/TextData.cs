using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TECHIS.Core.Serialization.XML
{
    [Serializable]
    public class TextData : IXmlSerializable
    {
        #region Fields 
        private string _DataType;
        private string _Value;
        #endregion

        #region Properties 
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

        [XmlText]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        #endregion

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {

            _DataType = reader.GetAttribute("DataType", null);
            _Value = reader.ReadElementContentAsString();
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("DataType", _DataType);
            writer.WriteCData(_Value);
        }

        #endregion

        public TextData() { }
        public TextData(string value)
        {
            _Value = value;
        }


    }
}
