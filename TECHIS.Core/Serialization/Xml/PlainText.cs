

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace TECHIS.AppModel
{
    [DataContract]
    [Serializable]
    public class PlainText : IXmlSerializable
    {
        #region Fields
        private string _Value;
        #endregion

        #region Properties
        [DataMember(EmitDefaultValue = false)]
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

            _Value = reader.ReadElementString();
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteCData(_Value);
        }

        #endregion

        public PlainText() { }
        public PlainText(string value)
        {
            _Value = value;
        }

        public override string ToString()
        {
            return _Value;
        }
    }
}
