using System;
using System.Reflection;
using System.Xml.Serialization;



namespace TECHIS.Core
{


    /// <summary>
    /// The Notification system's ObjectFactory.
    /// </summary>
    public abstract class ObjectFactory
    {
        #region Types 
        /// <summary>
        /// Represents the data required to instantiate an object 
        /// </summary>
        [Serializable]
        public class InstantiationInfo
        {
            #region Fields 
            private string _ConnectionString;
            private bool _IsRemote;
            private string _TypeName;
            private bool _Enabled;
            private NameValue[] _Attributes;
            private Server _Service;


            #endregion

            #region Properties 
            [XmlElement("Service")]
            public Server Service
            {
                get { return _Service; }
                set { _Service = value; }
            }
            [XmlElement]
            public bool Enabled
            {
                get
                {
                    return _Enabled;
                }
                set
                {
                    _Enabled = value;
                }
            }
            [XmlElement]
            public string ConnectionString
            {
                get
                {
                    return _ConnectionString;
                }
                set
                {
                    _ConnectionString = value;
                }
            }
            [XmlElement]
            public bool IsRemote
            {
                get
                {
                    return _IsRemote;
                }
                set
                {
                    _IsRemote = value;
                }
            }
            [XmlElement]
            public string TypeName
            {
                get
                {
                    return _TypeName;
                }
                set
                {
                    _TypeName = value;
                }
            }
            [XmlArray("Attributes")]
            [XmlArrayItem("Attribute", typeof(InstantiationInfo.NameValue))]
            public NameValue[] Attributes
            {
                get
                {
                    return _Attributes;
                }
                set
                {
                    _Attributes = value;
                }
            }
            #endregion

            #region Types 
            /// <summary>
            /// Represents server configuration settings
            /// </summary>
            [Serializable]
            public class Server
            {
                #region Fields
                private string _ObjectURI;
                private string _ApplicationName;
                #endregion

                #region Properties
                /// <summary>
                /// The uir that clients will use to connection to this service
                /// </summary>
                public string ObjectURI
                {
                    get { return _ObjectURI; }
                    set { _ObjectURI = value; }
                }

                /// <summary>
                /// The name of the application of the service
                /// </summary>
                public string ApplicationName
                {
                    get { return _ApplicationName; }
                    set { _ApplicationName = value; }
                }
                #endregion
            }
            /// <summary>
            /// A name value pair that includes type information
            /// </summary>
            [Serializable]
            public class NameValue
            {
                #region Fields

                private string _Name;

                private string _Value;

                private string _TypeName;

                private string _ParentName;

                #endregion

                #region Properties 
                
                [XmlAttribute]
                public string Name
                {
                    get { return _Name; }
                    set { _Name = value; }
                }
                
                [XmlAttribute]
                public string Value
                {
                    get { return _Value; }
                    set { _Value = value; }
                }
                
                [XmlAttribute]
                public string TypeName
                {
                    get { return _TypeName; }
                    set { _TypeName = value; }
                }
                
                [XmlIgnore]
                public object TypedValue
                {
                    get
                    {
                        return GetTypedValue();
                    }
                }

                [XmlAttribute]
                public string ParentName
                {
                    get { return _ParentName; }
                    set { _ParentName = value; }
                }
                #endregion

                private object GetTypedValue()
                {
                    object obj = _Value;

                    if (!(string.IsNullOrEmpty(_TypeName)))
                    {
                        switch (_TypeName.ToLower())
                        {
                            case "int":
                                obj = int.Parse(_Value);
                                break;
                        }
                    }

                    return obj;
                }
            }
            #endregion

            public override string ToString()
            {
                string retval;

                try
                {
                    retval = TECHIS.Core.Serialization.XmlSerializer.SerializeToUTF8(this);
                }
                catch
                {
                    retval = base.ToString();

                }

                return retval;

            }
        }
        #endregion

        #region Private methods 

        protected abstract object GetRemoteProxy(Type t, string connectionstring, InstantiationInfo.NameValue[] attributes);
        protected virtual object GetInstance(Type t, InstantiationInfo.NameValue[] attributes)
        {
            object[] args = null;
            object obj = Activator.CreateInstance(t, args);
            return obj;
        }
        #endregion

        #region Public Methods 
        public object Create( InstantiationInfo iInfo )
        {
            if (iInfo == null)
                throw new ArgumentNullException("iInfo");

            object o = null;

            Type t = Type.GetType(iInfo.TypeName, true);

            if (iInfo.IsRemote)
                o = GetRemoteProxy(t, iInfo.ConnectionString, iInfo.Attributes);
            else
                o = Activator.CreateInstance(t); //t.GetTypeInfo().Assembly.CreateInstance(t.FullName);

            return o;

        }
        #endregion


    }
}
