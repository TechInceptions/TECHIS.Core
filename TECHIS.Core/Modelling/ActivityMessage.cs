using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TECHIS.Core.Modelling
{

    public class ActivityMessage<T>
    {
        private string                  _Data;
        private ActivityMessageTypes    _MessageType;
        private int                     _Classification;
        private DateTime _EntryTime;
        private T _Source;
        private string _Reason;

        [XmlAttribute]
        public string Reason
        {
            get { return _Reason; }
            set { _Reason = value; }
        }

        public T Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        [XmlAttribute]
        public DateTime EntryTime
        {
            get { return _EntryTime; }
            set { _EntryTime = value; }
        }

        [XmlAttribute]
        public int Classification
        {
            get { return _Classification; }
            set { _Classification = value; }
        }

        [XmlAttribute]
        public ActivityMessageTypes MessageType
        {
            get { return _MessageType; }
            set { _MessageType = value; }
        }

        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        public ActivityMessage(ActivityMessageTypes type, string data, int classification, string reason, T source)
        {
            _MessageType    = type;
            _Classification = classification;
            _Data           = data;
            _EntryTime = DateTime.UtcNow;
            _Source = source;
            _Reason = reason;
        }
        public ActivityMessage()
        {
            _EntryTime = DateTime.UtcNow;
        }
    }
}
