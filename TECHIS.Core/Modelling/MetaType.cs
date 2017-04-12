using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TECHIS.Core.Modelling
{
    [Serializable]
    public class MetaType : Attribute
    {
        public enum TypeCodes
        {
            Unknown,

            ByteArray,

            Int64,

            Datetime,

            Decimal,

            Double,

            Single,

            Int16,

            Object,

            Byte,

            Guid,

            XmlDocument,

            XmlReader,

            String,

            Boolean,

            Int32,

            DatetimeOffset,

            TimeZone,

            TimeZoneInfo,

            Enum,

            LookUpKey,

            TimeSpan,

            Stream


        }

        #region Constants 
        public const string ARRAY_SIGNIFIER = "[]";
        public const string TYPE_UNDEFINED = "3c9e245737324c649df253660f15a62c";
        public const string BYTE = "Byte";
        public const string BYTE_ARRAY = BYTE + ARRAY_SIGNIFIER;
        public const string INT64 = "Int64";
        public const string STRING = "String";
        public const string DATETIME = "DateTime";
        public const string DECIMAL = "Decimal";
        public const string DOUBLE = "Double";
        public const string INT32 = "Int32";
        public const string SINGLE = "Single";
        public const string INT16 = "Int16";
        public const string OBJECT = "Object";
        public const string GUID = "Guid";
        public const string BOOLEAN = "Boolean";
        public const string XMLDOCUMENT = "XmlDocument";
        public const string XMLREADER = "XmlReader";
        public const string STREAM = "Stream";
        public const string DATETIMEOFFSET = "DateTimeOffset";
        public const string TIMESPAN = "TimeSpan";

        public const string ALIAS_BYTE_ARRAY = "byte[]";
        public const string ALIAS_INT64 = "long";
        public const string ALIAS_STRING = "string";
        public const string ALIAS_DATETIME = DATETIME;
        public const string ALIAS_DECIMAL = "decimal";
        public const string ALIAS_DOUBLE = "double";
        public const string ALIAS_INT32 = "int";
        public const string ALIAS_SINGLE = "float";
        public const string ALIAS_INT16 = "short";
        public const string ALIAS_OBJECT = "object";
        public const string ALIAS_BYTE = "byte";
        public const string ALIAS_GUID = GUID;
        public const string ALIAS_BOOLEAN = "bool";


        public const string NAMESPACE_MICROSOFT = "Microsoft";
        public const string NAMESPACE_SYSTEM = "System";
        public const string NAMESPACE_SYSTEM_COLLECTIONS = "System.Collections";
        public const string NAMESPACE_SYSTEM_COLLECTIONS_GENERIC = "System.Collections.Generic";
        public const string NAMESPACE_SYSTEM_DATA_SQLTYPES = "System.Data.SqlTypes";
        public const string NAMESPACE_OXYDATA_DATA = "TECHIS.Core.Data";
        public const string NAMESPACE_OXYDATA_BUSINESS = "TECHIS.Core.Business";
        public const string NAMESPACE_OXYDATA_WEBSERVICES = "TECHIS.Core.WS";
        public const string NAMESPACE_OXYDATA_UI = "TECHIS.Core.UI";
        public const string NAMESPACE_SYSTEM_XML = "System.Xml";
        public const string NAMESPACE_SYSTEM_IO = "System.IO";

        public const string FULLNAME_DATETIMEOFFSET = NAMESPACE_SYSTEM + "." + DATETIMEOFFSET;
        public const string FULLNAME_TIMESPAN = NAMESPACE_SYSTEM + "." + TIMESPAN;
        public const string FULLNAME_DATETIME = NAMESPACE_SYSTEM + "." + DATETIME;
        public const string FULLNAME_BYTE_ARRAY = NAMESPACE_SYSTEM + "." + BYTE_ARRAY;
        public const string FULLNAME_INT64 = NAMESPACE_SYSTEM + "." + INT64;
        public const string FULLNAME_STRING = NAMESPACE_SYSTEM + "." + STRING;
        public const string FULLNAME_DECIMAL = NAMESPACE_SYSTEM + "." + DECIMAL;
        public const string FULLNAME_DOUBLE = NAMESPACE_SYSTEM + "." + DOUBLE;
        public const string FULLNAME_INT32 = NAMESPACE_SYSTEM + "." + INT32;
        public const string FULLNAME_SINGLE = NAMESPACE_SYSTEM + "." + SINGLE;
        public const string FULLNAME_INT16 = NAMESPACE_SYSTEM + "." + INT16;
        public const string FULLNAME_OBJECT = NAMESPACE_SYSTEM + "." + OBJECT;
        public const string FULLNAME_BYTE = NAMESPACE_SYSTEM + "." + BYTE;
        public const string FULLNAME_GUID = NAMESPACE_SYSTEM + "." + GUID;
        public const string FULLNAME_BOOLEAN = NAMESPACE_SYSTEM + "." + BOOLEAN;
        public const string FULLNAME_XMLDOCUMENT = NAMESPACE_SYSTEM_XML + "." + XMLDOCUMENT;
        public const string FULLNAME_XMLREADER = NAMESPACE_SYSTEM_XML + "." + XMLREADER;
        public const string FULLNAME_STREAM = NAMESPACE_SYSTEM_IO + "." + STREAM;
        #endregion

        #region Static Fields 
        private static MetaType _DatetimeOffset;

        private static MetaType _Timespan;

        private static MetaType _Datetime;

        private static MetaType _ByteArray;

        private static MetaType _Int64;

        private static MetaType _Decimal;

        private static MetaType _Double;

        private static MetaType _Single;

        private static MetaType _Int16;

        private static MetaType _Object;

        private static MetaType _Byte;

        private static MetaType _Guid;

        private static MetaType _XmlDocument;

        private static MetaType _XmlReader;

        private static MetaType _String;

        private static MetaType _Boolean;

        private static MetaType _Int32;

        private static MetaType _Stream;
        #endregion

        #region Fields 
        private string _Namespace;
        private string _TypeName;
        private string _ItemNamespace;
        private string _ItemTypeName;
        private bool _IsImmutable = false;
        private TypeCodes _TypeCode;
        private bool? _IsValueType;
        private StringBuilder _FullNameBuilder = new StringBuilder(20);
        private Type _SystemType;
        #endregion

        #region Properties 
        [XmlElement("IVT")]
        public bool? IsValueType
        {
            get { return _IsValueType; }
            set { _IsValueType = value; }
        }
        [XmlAttribute("TC")]
        public TypeCodes TypeCode
        {
            get { return _TypeCode; }
            set { _TypeCode = value; }
        }

        /// <summary>
        /// The namespace of the type
        /// </summary>
        [XmlAttribute]
        public string Namespace
        {
            get { return _Namespace; }
            set
            {
                if (_IsImmutable)
                    Exceptions.ThrowInvalidOps("This instance is immutable");

                _Namespace = value; 
            }
        }

        /// <summary>
        /// The name of the type
        /// </summary>
        [XmlAttribute]
        public string TypeName
        {
            get { return _TypeName; }
            set
            {
                if (_IsImmutable)
                    Exceptions.ThrowInvalidOps("This instance is immutable");

                _TypeName = value; 
            }
        }

        /// <summary>
        /// If the type is a collection, the namespace of the items in the collection
        /// </summary>
        [XmlAttribute]
        public string ItemNamespace
        {
            get { return _ItemNamespace; }
            set
            {
                if (_IsImmutable)
                    Exceptions.ThrowInvalidOps("This instance is immutable");

                _ItemNamespace = value; 
            }
        }

        /// <summary>
        /// if the type is a collection, this will represent the items in the collection
        /// </summary>
        [XmlAttribute]
        public string ItemTypeName
        {
            get
            {
                return _ItemTypeName;
            }
            set
            {
                if (_IsImmutable)
                    Exceptions.ThrowInvalidOps("This instance is immutable");

                _ItemTypeName = value;
            }
        }
        #endregion

        #region Constructors 
        static MetaType()
        {
            try
            {
                _DatetimeOffset = new MetaType(DATETIMEOFFSET, NAMESPACE_SYSTEM, DATETIMEOFFSET, NAMESPACE_SYSTEM);
                _DatetimeOffset._IsImmutable = true;
                _DatetimeOffset.TypeCode = TypeCodes.DatetimeOffset;
                _DatetimeOffset._IsValueType = true;
                _DatetimeOffset._SystemType = typeof(DateTimeOffset);

                _Timespan = new MetaType(TIMESPAN, NAMESPACE_SYSTEM, TIMESPAN, NAMESPACE_SYSTEM);
                _Timespan._IsImmutable = true;
                _Timespan.TypeCode = TypeCodes.TimeSpan;
                _Timespan._IsValueType = true;
                _Timespan._SystemType = typeof(TimeSpan);

                _Datetime = new MetaType(DATETIME, NAMESPACE_SYSTEM, DATETIME, NAMESPACE_SYSTEM);
                _Datetime._IsImmutable = true;
                _Datetime.TypeCode = TypeCodes.Datetime;
                _Datetime._IsValueType = true;
                _Datetime._SystemType = typeof(DateTime);

                _ByteArray = new MetaType(BYTE_ARRAY, NAMESPACE_SYSTEM, BYTE_ARRAY, NAMESPACE_SYSTEM);
                _ByteArray._IsImmutable = true;
                _ByteArray.TypeCode = TypeCodes.ByteArray;
                _ByteArray._SystemType = typeof(byte[]);

                _Int64 = new MetaType(INT64, NAMESPACE_SYSTEM, INT64, NAMESPACE_SYSTEM);
                _Int64._IsImmutable = true;
                _Int64.TypeCode = TypeCodes.Int64;
                _Int64._IsValueType = true;
                _Int64._SystemType = typeof(Int64);

                _Decimal = new MetaType(DECIMAL, NAMESPACE_SYSTEM, DECIMAL, NAMESPACE_SYSTEM);
                _Decimal._IsImmutable = true;
                _Decimal.TypeCode = TypeCodes.Decimal;
                _Decimal._IsValueType = true;
                _Decimal._SystemType = typeof(decimal);

                _Double = new MetaType(DOUBLE, NAMESPACE_SYSTEM, DOUBLE, NAMESPACE_SYSTEM);
                _Double._IsImmutable = true;
                _Double.TypeCode = TypeCodes.Double;
                _Double._IsValueType = true;
                _Double._SystemType = typeof(double);

                _Single = new MetaType(SINGLE, NAMESPACE_SYSTEM, SINGLE, NAMESPACE_SYSTEM);
                _Single._IsImmutable = true;
                _Single.TypeCode = TypeCodes.Single;
                _Single._IsValueType = true;
                _Single._SystemType = typeof(Single);

                _Int16 = new MetaType(INT16, NAMESPACE_SYSTEM, INT16, NAMESPACE_SYSTEM);
                _Int16._IsImmutable = true;
                _Int16.TypeCode = TypeCodes.Int16;
                _Int16._IsValueType = true;
                _Int16._SystemType = typeof(Int16);

                _Object = new MetaType(OBJECT, NAMESPACE_SYSTEM, OBJECT, NAMESPACE_SYSTEM);
                _Object._IsImmutable = true;
                _Object.TypeCode = TypeCodes.Object;
                _Object._SystemType = typeof(object);

                _Byte = new MetaType(BYTE, NAMESPACE_SYSTEM, BYTE, NAMESPACE_SYSTEM);
                _Byte._IsImmutable = true;
                _Byte.TypeCode = TypeCodes.Byte;
                _Byte._IsValueType = true;
                _Byte._SystemType = typeof(byte);

                _Guid = new MetaType(GUID, NAMESPACE_SYSTEM, GUID, NAMESPACE_SYSTEM);
                _Guid._IsImmutable = true;
                _Guid.TypeCode = TypeCodes.Guid;
                _Guid._IsValueType = true;
                _Guid._SystemType = typeof(Guid);

                _XmlDocument = new MetaType(XMLDOCUMENT, NAMESPACE_SYSTEM_XML, XMLDOCUMENT, NAMESPACE_SYSTEM_XML);
                _XmlDocument._IsImmutable = true;
                _XmlDocument.TypeCode = TypeCodes.XmlDocument;
                _XmlDocument._SystemType = typeof(System.Xml.XmlDocument);

                _XmlReader = new MetaType(XMLREADER, NAMESPACE_SYSTEM_XML, XMLREADER, NAMESPACE_SYSTEM_XML);
                _XmlReader._IsImmutable = true;
                _XmlReader.TypeCode = TypeCodes.XmlReader;
                _XmlReader._SystemType = typeof(System.Xml.XmlReader);

                _Stream = new MetaType(STREAM, NAMESPACE_SYSTEM_IO, STREAM, NAMESPACE_SYSTEM_IO);
                _Stream._IsImmutable = true;
                _Stream.TypeCode = TypeCodes.Stream;
                _Stream._SystemType = typeof(System.IO.Stream);

                _String = new MetaType(STRING, NAMESPACE_SYSTEM, STRING, NAMESPACE_SYSTEM);
                _String._IsImmutable = true;
                _String.TypeCode = TypeCodes.String;
                _String._SystemType = typeof(string);

                _Boolean = new MetaType(BOOLEAN, NAMESPACE_SYSTEM, BOOLEAN, NAMESPACE_SYSTEM);
                _Boolean._IsImmutable = true;
                _Boolean.TypeCode = TypeCodes.Boolean;
                _Boolean._IsValueType = true;
                _Boolean._SystemType = typeof(Boolean);

                _Int32 = new MetaType(INT32, NAMESPACE_SYSTEM, INT32, NAMESPACE_SYSTEM);
                _Int32._IsImmutable = true;
                _Int32.TypeCode = TypeCodes.Int32;
                _Int32._IsValueType = true;
                _Int32._SystemType = typeof(Int32);
            }
            catch(Exception ex) 
            {
                throw new TypeInitializationException("TECHIS.Core.Modelling.MetaType", ex);
            }
        }
        public MetaType() { }
        public MetaType(string typeName, string namespaceName)
            : this(typeName, namespaceName, typeName, namespaceName)
        {
        }

        public MetaType(string typeName, string namespaceName, string itemType, string itemNamespace)
        {
            _TypeName = typeName;
            Name = typeName;
            _Namespace = namespaceName;
            _ItemTypeName = itemType;
            _ItemNamespace = itemNamespace;

        }

        #endregion

        #region Public Methods 
        
        public bool IsInSystemNamespace()
        {
            return IsInSystemNamespace(this.Namespace);
        }

        public bool IsInMicrosoftNamespace()
        {
            return IsInMicrosoftNamespace(this.Namespace);
        }
        
        public Type GetSystemType()
        {
            if (_SystemType == null)
            {
                try
                {
                    _SystemType = Type.GetType(GetFullTypeName());
                }
                catch
                {
                    _SystemType = typeof(object);
                }
            }

            return _SystemType;
        }

        protected void MakeImmutable()
        {
            _IsImmutable = true;
        }

        public StringBuilder GetDeclaration(string instanceName, bool useNamespace)
        {
            StringBuilder retval = new StringBuilder(100);

            if (useNamespace)
            {
                if (!string.IsNullOrEmpty(_Namespace))
                    retval.Append(Namespace).Append('.');
            }

            if ((!(string.IsNullOrEmpty(_Namespace))) && (!(string.IsNullOrEmpty(_TypeName))))
                retval.Append(TypeName).Append(' ').Append(instanceName);


            return retval;
        }

        public StringBuilder GetDeclarationAndDefaultInitialization(string instanceName, bool useNamespace)
        {
            StringBuilder sb = GetDeclaration(instanceName, useNamespace);
            if (sb.Length > 0)
            {
                sb.Append(" = ").Append(GetDefaultAsString());
            }

            return sb;
        }

        public static string GetAlias(string primitiveType)
        {
            string val = primitiveType;
            switch (primitiveType)
            {
                case "Byte[]":
                    val = ALIAS_BYTE_ARRAY;
                    break;
                case "Int64":
                    val = ALIAS_INT64;
                    break;
                case "String":
                    val = ALIAS_STRING;
                    break;
                case "DateTime":
                    val = ALIAS_DATETIME;
                    break;
                case "Decimal":
                    val = ALIAS_DECIMAL;
                    break;
                case "Double":
                    val = ALIAS_DOUBLE;
                    break;
                case "Int32":
                    val = ALIAS_INT32;
                    break;
                case "Single":
                    val = ALIAS_SINGLE;
                    break;
                case "Int16":
                    val = ALIAS_INT16;
                    break;
                case "Object":
                    val = ALIAS_OBJECT;
                    break;
                case "Byte":
                    val = ALIAS_BYTE;
                    break;
                case "Guid":
                    val = ALIAS_GUID;
                    break;
                case "Boolean":
                    val = ALIAS_BOOLEAN;
                    break;
            }

            return val;
        }

        public string GetFullTypeName()
        {
            if (string.IsNullOrEmpty(_Namespace))
            {
                return _TypeName;
            }

            string val = _FullNameBuilder.Append(_Namespace).Append('.').Append(_TypeName).ToString();
            _FullNameBuilder.Length = 0;

            return val;
        }

        public override string ToString()
        {
            return _TypeName;
        }

        public string GetDefaultAsString()
        {
            return GetDefaultAsString(_TypeName,false,_IsValueType);
        }

        public string GetDefaultAsString(bool isNullable)
        {
            return GetDefaultAsString(_TypeName, isNullable, _IsValueType);
        }

        public StringBuilder GetFullItemTypeName()
        {
            if (string.IsNullOrEmpty(_ItemNamespace))
                return (new StringBuilder(_ItemTypeName));

            return (new StringBuilder(20)).Append(_ItemNamespace).Append('.').Append(_ItemTypeName);
        }

        public string GetNullableType()
        {
            string retval;

            switch (_TypeName)
            {

                case ALIAS_INT64:
                case ALIAS_DECIMAL:
                case ALIAS_DOUBLE:
                case ALIAS_INT32:
                case ALIAS_SINGLE:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                case ALIAS_BOOLEAN:
                case INT64:
                case DATETIME:
                case DECIMAL:
                case DOUBLE:
                case INT32:
                case SINGLE:
                case INT16:
                case BYTE:
                case GUID:
                case BOOLEAN:
                case DATETIMEOFFSET:
                case TIMESPAN:
                    retval = _TypeName + "?";
                    break;
                default:
                    retval = _TypeName;
                    break;
            }
            return retval;

        }

        public bool IsBooleanType()
        {
            if (_TypeCode == TypeCodes.Boolean)
                return true;

            if (string.IsNullOrEmpty(_TypeName))
            {
                throw new ArgumentException("typeName");
            }

            string tName = _TypeName.ToLower();

            if (tName == "bool")
                return true;
            if (tName == "boolean")
                return true;

            return false;
        }

        public bool IsInt32()
        {
            if (_TypeCode == TypeCodes.Int32)
                return true;

            if (string.IsNullOrEmpty(_TypeName))
            {
                throw new ArgumentException("typeName");
            }

            string tName = _TypeName.ToLower();

            if (tName.Equals("int", StringComparison.Ordinal))
                return true;

            if (tName.Equals("int32", StringComparison.Ordinal))
                return true;

            return false;
        }

        public bool IsPrimitiveValueType()
        {
            bool retval;

            switch (_TypeName)
            {
                case INT64:
                case DATETIME:
                case DECIMAL:
                case DOUBLE:
                case INT32:
                case SINGLE:
                case INT16:
                case BYTE:
                case GUID:
                case BOOLEAN:
                case DATETIMEOFFSET:
                case TIMESPAN:
                case ALIAS_INT64:
                case ALIAS_DECIMAL:
                case ALIAS_DOUBLE:
                case ALIAS_INT32:
                case ALIAS_SINGLE:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                case ALIAS_BOOLEAN:
                    retval = true;
                    break;
                default:
                    retval = false;
                    break;
            }
            return retval;
        }

        public bool IsNumericType()
        {
            bool retval;

            switch (_TypeName)
            {
                case DATETIMEOFFSET:
                case TIMESPAN:
                case BYTE_ARRAY:
                case STRING:
                case OBJECT:
                case ALIAS_BYTE_ARRAY:
                case ALIAS_STRING:
                case ALIAS_OBJECT:
                case DATETIME:
                case GUID:
                case BOOLEAN:
                case ALIAS_BOOLEAN:
                    retval = false;
                    break;

                case INT64:
                case DECIMAL:
                case DOUBLE:
                case INT32:
                case SINGLE:
                case INT16:
                case BYTE:
                case ALIAS_INT64:
                case ALIAS_DECIMAL:
                case ALIAS_DOUBLE:
                case ALIAS_INT32:
                case ALIAS_SINGLE:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                    retval = true;
                    break;

                default:
                    retval = false;
                    break;

            }
            return retval;
        }

        public bool IsIntegerType()
        {
            bool retval;

            switch (_TypeName)
            {
                case DATETIMEOFFSET:
                case TIMESPAN:
                case BYTE_ARRAY:
                case STRING:
                case OBJECT:
                case ALIAS_BYTE_ARRAY:
                case ALIAS_STRING:
                case ALIAS_OBJECT:
                case DATETIME:
                case GUID:
                case BOOLEAN:
                case ALIAS_BOOLEAN:
                case DECIMAL:
                case DOUBLE:
                case SINGLE:
                case INT64:
                case ALIAS_INT64:
                case ALIAS_DECIMAL:
                case ALIAS_DOUBLE:
                case ALIAS_SINGLE:
                    retval = false;
                    break;

                case INT32:
                case INT16:
                case BYTE:
                case ALIAS_INT32:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                    retval = true;
                    break;

                default:
                    retval = false;
                    break;

            }
            return retval;
        }

        public bool IsString()
        {
            if (_TypeCode == TypeCodes.String)
                return true;

            if (_TypeName.Equals(STRING, StringComparison.Ordinal))
                return true;

            if (_TypeName.Equals(ALIAS_STRING, StringComparison.Ordinal))
                return true;

            return false;
        }

        public bool IsObject()
        {
            if (_TypeCode == TypeCodes.Object)
                return true;

            if (_TypeName.Equals(OBJECT, StringComparison.Ordinal))
                return true;

            if (_TypeName.Equals(ALIAS_OBJECT, StringComparison.Ordinal))
                return true;

            return false;
        }

        public bool IsInSystem()
        {
            bool retval;

            switch (_TypeName)
            {
                case DATETIMEOFFSET:
                case TIMESPAN:
                case BYTE_ARRAY:
                case STRING:
                case OBJECT:
                case INT64:
                case DATETIME:
                case DECIMAL:
                case DOUBLE:
                case INT32:
                case SINGLE:
                case INT16:
                case BYTE:
                case GUID:
                case BOOLEAN:
                case ALIAS_BYTE_ARRAY:
                case ALIAS_STRING:
                case ALIAS_OBJECT:
                case ALIAS_INT64:
                case ALIAS_DECIMAL:
                case ALIAS_DOUBLE:
                case ALIAS_INT32:
                case ALIAS_SINGLE:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                case ALIAS_BOOLEAN:
                    retval = true;
                    break;
                default:
                    retval = false;
                    break;
            }
            return retval;
        }

        public bool IsInSystemXml()
        {
            bool retval;

            switch (_TypeName)
            {
                case XMLDOCUMENT:
                case XMLREADER:
                    retval = true;
                    break;
                default:
                    retval = false;
                    break;
            }
            return retval;
        }

        public bool IsInSystemIO()
        {
            bool retval;

            switch (_TypeName)
            {
                case STREAM:
                    retval = true;
                    break;
                default:
                    retval = false;
                    break;
            }
            return retval;
        }

        public bool IsByteArray()
        {
            if (_TypeCode == TypeCodes.ByteArray)
                return true;

            if (string.IsNullOrEmpty(_TypeName))
            {
                throw new ArgumentException("typeName");
            }

            string tName = _TypeName.ToLower();

            if (tName == "byte[]")
                return true;

            return false;
        }

        public bool IsArray()
        {
            if (_TypeCode == TypeCodes.ByteArray)
                return true;

            if (string.IsNullOrEmpty(_TypeName))
            {
                throw new ArgumentException("typeName");
            }

            if (_TypeName.EndsWith("[]"))
                return true;

            return false;
        }

        public bool IsNullableValueType()
        {
            return (_TypeName[_TypeName.Length - 1] == '?');
        }

        public bool IsDateTime()
        {
            if (_TypeCode == TypeCodes.Datetime)
                return true;

            if (_TypeName.Equals(DATETIME, StringComparison.Ordinal))
                return true;

            if (_TypeName.Equals(ALIAS_DATETIME, StringComparison.Ordinal))
                return true;

            return false;
        }

        public bool IsTimespan()
        {
            if (_TypeCode == TypeCodes.TimeSpan)
                return true;

            if (_TypeName.Equals(TIMESPAN, StringComparison.Ordinal))
                return true;

            return false;
        }

        public bool IsDateTimeOffset()
        {
            if (_TypeCode == TypeCodes.DatetimeOffset)
                return true;

            if (_TypeName.Equals(DATETIMEOFFSET, StringComparison.Ordinal))
                return true;

            return false;
        }
        
        public bool IsXmlReader()
        {
            if (_TypeCode == TypeCodes.XmlReader)
                return true;

            if (_TypeName.Equals(XMLREADER, StringComparison.Ordinal))
                return true;

            return false;
        }

        public bool IsXmlDocument()
        {
            if (_TypeCode == TypeCodes.XmlDocument)
                return true;

            if (_TypeName.Equals(XMLDOCUMENT, StringComparison.Ordinal))
                return true;

            return false;
        }

        public bool IsStream()
        {
            if (_TypeCode == TypeCodes.Stream)
                return true;

            if (_TypeName.Equals(STREAM, StringComparison.Ordinal))
                return true;

            return false;
        }


        #endregion

        #region Static Methods 
        
        public static MetaType ConstructGenericType(string genericTypeName, string genericTypeNamespace, string parameterTypeName, string parameterTypeNamespace, bool includeParameterTypeNamespace)
        {
            string paramTypeFullName;
            if (includeParameterTypeNamespace)
            {
                includeParameterTypeNamespace = !string.IsNullOrEmpty(parameterTypeNamespace);
            }
            paramTypeFullName = (includeParameterTypeNamespace ? String.Format(@"{0}.{1}", parameterTypeNamespace, parameterTypeName) : parameterTypeName);
            string typeName = string.Format("{0}<{1}>", genericTypeName, paramTypeFullName);

            MetaType mt = new MetaType(typeName, genericTypeNamespace, parameterTypeName, parameterTypeNamespace);

            return mt;
        }
        public static string GetDefaultAsString( string typeName )
        {
            return GetDefaultAsString(typeName, false,null);
        }

        public static string GetDefaultAsString(string typeName, bool isNullable)
        {
            return GetDefaultAsString(typeName, isNullable, null);
        }

        //public static string GetDefaultAsString(string typeName, bool isNullable, bool? isValueType)
        //{
        //    string retval = null;
        //    if (isNullable)
        //    {
        //        retval = "null";
        //    }
        //    else
        //    {
        //        switch (typeName)
        //        {
        //            case TIMESPAN:
        //                retval = "new TimeSpan()";
        //                break;
        //            case DATETIMEOFFSET:
        //                retval = "new DateTimeOffset()";
        //                break;
        //            case DATETIME:
        //                retval = "new DateTime()";
        //                break;
        //            case BOOLEAN:
        //                retval = "false";
        //                break;
        //            case INT16:
        //                retval = "(Int16)0";
        //                break;
        //            case BYTE:
        //                retval = "(Byte)0";
        //                break;
        //            case INT32:
        //            case INT64:
        //                retval = "0";
        //                break;
        //            case SINGLE:
        //                retval = "(Single)0";
        //                break;
        //            case DECIMAL:
        //                retval = "Decimal.Zero";
        //                break;
        //            case DOUBLE:
        //                retval = "(double)0";
        //                break;
        //            case GUID:
        //                retval = "Guid.Empty";
        //                break;
        //            case BYTE_ARRAY:
        //            case OBJECT:
        //            case STRING:
        //            case XMLDOCUMENT:
        //            case XMLREADER:
        //            case STREAM:
        //                retval = "null";
        //                break;
        //            default:
        //                if (isValueType.HasValue && isValueType.Value)
        //                {
        //                    retval = "new " + typeName + "()";
        //                }
        //                else
        //                {
        //                    retval = "null";
        //                }

        //                break;
        //        }
        //    }


        //    return retval;
        //}

        public static string GetDefaultAsString(string typeName, bool isNullable, bool? isValueType)
        {
            string retval = null;
            if (isNullable)
            {
                retval = "null";
            }
            else
            {
                switch (typeName)
                {
                    case BOOLEAN:
                        retval = "false";
                        break;
                    case TIMESPAN:
                    case DATETIMEOFFSET:
                    case DATETIME:
                        retval = string.Format( @"new {0}()",typeName);
                        break;
                    case INT16:
                    case BYTE:
                    case SINGLE:
                    case DOUBLE:
                        retval = string.Format( @"({0})0",typeName);
                        break;
                    case INT32:
                    case INT64:
                        retval = "0";
                        break;
                    case DECIMAL:
                        retval = "Decimal.Zero";
                        break;
                    case GUID:
                        retval = "Guid.Empty";
                        break;
                    case BYTE_ARRAY:
                    case OBJECT:
                    case STRING:
                    case XMLDOCUMENT:
                    case XMLREADER:
                    case STREAM:
                        retval = "null";
                        break;
                    default:
                        if (isValueType.HasValue && isValueType.Value)
                        {
                            retval = string.Format(@"({0})0", typeName);
                        }
                        else
                        {
                            retval = "null";
                        }

                        break;
                }
            }


            return retval;
        }

        public static object GetDefaultAsObject(string typeName)
        {
            return GetDefaultAsObject(typeName, false);
        }

        public static object GetDefaultAsObject(string typeName, bool isNullable)
        {
            object retval = null;

            if (isNullable)
            {
                retval = null;
            }
            else
            {
                switch (typeName)
                {
                    case TIMESPAN:
                        retval = new TimeSpan();
                        break;
                    case DATETIMEOFFSET:
                        retval = new DateTimeOffset();
                        break;
                    case DATETIME:
                        retval = new DateTime();
                        break;
                    case BOOLEAN:
                        retval = false;
                        break;
                    case INT16:
                    case INT32:
                    case INT64:
                    case BYTE:
                        retval = 0;
                        break;
                    case SINGLE:
                        retval = (Single)0;
                        break;
                    case DECIMAL:
                        retval = Decimal.Zero;
                        break;
                    case DOUBLE:
                        retval = (double)0;
                        break;
                    case GUID:
                        retval = Guid.Empty;
                        break;
                    case BYTE_ARRAY:
                    case OBJECT:
                    case STRING:
                    case XMLDOCUMENT:
                    case XMLREADER:
                    default:
                        retval = null;
                        break;
                }
            }

            return retval;
        }

        public static MetaType GetStream()
        {
            return _Stream;
        }

        public static MetaType GetDatetimeOffset()
        {
            return _DatetimeOffset;
        }

        public static MetaType GetTimespan()
        {
            return _Timespan;
        }

        public static MetaType GetByteArray()
        {
            return _ByteArray;
        }

        public static MetaType GetInt64()
        {
            return _Int64;
        }

        public static MetaType GetDatetime()
        {
            return _Datetime;
        }

        public static MetaType GetDecimal()
        {
            return _Decimal;
        }

        public static MetaType GetDouble()
        {
            return _Double;
        }

        public static MetaType GetSingle()
        {
            return _Single;
        }

        public static MetaType GetInt16()
        {
            return _Int16;
        }

        public static MetaType GetObject()
        {
            return _Object;
        }

        public static MetaType GetByte()
        {
            return _Byte;
        }

        public static MetaType GetGuid()
        {
            return _Guid;
        }

        public static MetaType GetXmlDocument()
        {
            return _XmlDocument;
        }

        public static MetaType GetXmlReader()
        {
            return _XmlReader;
        }

        public static MetaType GetString()
        {
            return _String;
        }

        public static MetaType GetBoolean()
        {
            return _Boolean;
        }

        public static MetaType GetInt32()
        {
            return _Int32 ;
        }

        public static string GetNullableType(string typeName)
        {
            string retval;
            switch (typeName)
            {
                case INT64:
                case DATETIME:
                case DECIMAL:
                case DOUBLE:
                case INT32:
                case SINGLE:
                case INT16:
                case BYTE:
                case GUID:
                case BOOLEAN:
                case DATETIMEOFFSET:
                case TIMESPAN:
                    retval = typeName + "?";
                    break;
                default:
                    retval = typeName;
                    break;
            }
            return retval;

        }

        public static bool IsBooleanType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("typeName");
            }

            string tName = typeName.ToLower();

            if (tName == "bool" || tName == "boolean")
                return true;

            return false;
        }

        public static bool IsInt32(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("typeName");
            }

            string tName = typeName.ToLower();

            if (tName.Equals("int", StringComparison.Ordinal)||tName.Equals("int32", StringComparison.Ordinal))
                return true;

            return false;
        }

        public static bool IsPrimitiveValueType(string typeName)
        {
            bool retval;

            switch (typeName)
            {
                case INT64:
                case DATETIME:
                case DECIMAL:
                case DOUBLE:
                case INT32:
                case SINGLE:
                case INT16:
                case BYTE:
                case GUID:
                case BOOLEAN:
                case TIMESPAN:
                case DATETIMEOFFSET:
                case ALIAS_INT64:
                case ALIAS_DECIMAL:
                case ALIAS_DOUBLE:
                case ALIAS_INT32:
                case ALIAS_SINGLE:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                case ALIAS_BOOLEAN:
                    retval = true;
                    break;
                default:
                    retval = false;
                    break;
            }
            return retval;
        }

        public static bool IsNumericType(string typeName)
        {
            bool retval;

            switch (typeName)
            {

                case INT64:
                case DECIMAL:
                case DOUBLE:
                case INT32:
                case SINGLE:
                case INT16:
                case BYTE:
                case ALIAS_INT64:
                case ALIAS_DECIMAL:
                case ALIAS_DOUBLE:
                case ALIAS_INT32:
                case ALIAS_SINGLE:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                    retval = true;
                    break;

                default:
                    retval = false;
                    break;

            }
            return retval;
        }

        public static bool IsIntegerType(string typeName)
        {
            bool retval;

            switch (typeName)
            {
                case INT32:
                case INT16:
                case BYTE:
                case ALIAS_INT32:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                    retval = true;
                    break;

                default:
                    retval = false;
                    break;

            }
            return retval;
        }

        public static bool IsString(string typeName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(typeName);

            if (typeName.Equals(STRING, StringComparison.Ordinal))
                return true;

            if (typeName.Equals(ALIAS_STRING, StringComparison.Ordinal))
                return true;

            return false;
        }

        public static bool IsObject(string typeName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(typeName);

            if (typeName.Equals(OBJECT, StringComparison.Ordinal))
                return true;

            if (typeName.Equals(ALIAS_OBJECT, StringComparison.Ordinal))
                return true;

            return false;
        }


        public static bool IsInSystemNamespace(string namespaceName)
        {
            bool retval;

            if (string.IsNullOrEmpty(namespaceName))
            {
                retval = false;
            }
            else
            {
                if (namespaceName.Equals(NAMESPACE_SYSTEM, StringComparison.Ordinal) || namespaceName.StartsWith(NAMESPACE_SYSTEM, StringComparison.Ordinal))
                {
                    retval = true;
                }
                else
                {
                    retval = false;
                }
            }
            return retval;
        }

        public static bool IsInMicrosoftNamespace(string namespaceName)
        {
            bool retval;

            if (string.IsNullOrEmpty(namespaceName))
            {
                retval = false;
            }
            else
            {
                if (namespaceName.Equals(NAMESPACE_MICROSOFT, StringComparison.Ordinal) || namespaceName.StartsWith(NAMESPACE_MICROSOFT, StringComparison.Ordinal))
                {
                    retval = true;
                }
                else
                {
                    retval = false;
                }
            }
            return retval;
        }

        public static bool IsInSystem(string typeName)
        {
            bool retval;

            switch (typeName)
            {
                case DATETIMEOFFSET:
                case TIMESPAN:
                case BYTE_ARRAY:
                case STRING:
                case OBJECT:
                case INT64:
                case DATETIME:
                case DECIMAL:
                case DOUBLE:
                case INT32:
                case SINGLE:
                case INT16:
                case BYTE:
                case GUID:
                case BOOLEAN:
                case ALIAS_BYTE_ARRAY:
                case ALIAS_STRING:
                case ALIAS_OBJECT:
                case ALIAS_INT64:
                case ALIAS_DECIMAL:
                case ALIAS_DOUBLE:
                case ALIAS_INT32:
                case ALIAS_SINGLE:
                case ALIAS_INT16:
                case ALIAS_BYTE:
                case ALIAS_BOOLEAN:
                    retval = true;
                    break;
                default:
                    retval = false;
                    break;
            }
            return retval;
        }
        
        /// <summary>
        /// changes an alias to a CTS name.
        /// Example: an input of 'bool' will return 'Boolean'.
        /// If a matching CTS name is not found, the input itself will be returned.
        /// </summary>
        public static string GetCTSName(string alias)
        {
            string retval;

            switch (alias)
            {
                case ALIAS_BYTE_ARRAY:
                    retval = BYTE_ARRAY;
                    break;
                case ALIAS_STRING:
                    retval = STRING;
                    break;
                case ALIAS_OBJECT:
                    retval = OBJECT;
                    break;
                case ALIAS_INT64:
                    retval = INT64;
                    break;
                case ALIAS_DECIMAL:
                    retval = DECIMAL;
                    break;
                case ALIAS_DOUBLE:
                    retval = DOUBLE;
                    break;
                case ALIAS_INT32:
                    retval = INT32;
                    break;
                case ALIAS_SINGLE:
                    retval = SINGLE;
                    break;
                case ALIAS_INT16:
                    retval = INT16;
                    break;
                case ALIAS_BYTE:
                    retval = BYTE;
                    break;
                case ALIAS_BOOLEAN:
                    retval = BOOLEAN;
                    break;
                default:
                    retval = alias;
                    break;
            }

            return retval;
        }
        
        public static bool IsByteArray(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("typeName");
            }

            string tName = typeName.ToLower();

            if (tName == "byte[]")
                return true;

            return false;
        }

        public static bool IsNullableValueType(string typeName)
        {
            return (typeName[typeName.Length - 1] == '?');
        }

        public static bool IsDateTime(string typeName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(typeName);

            if (typeName.Equals(DATETIME, StringComparison.Ordinal))
                return true;

            return false;
        }

        public static bool IsTimespan(string typeName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(typeName);

            if (typeName.Equals(TIMESPAN, StringComparison.Ordinal))
                return true;

            return false;
        }
        
        public static bool IsDateTimeOffset(string typeName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(typeName);

            if (typeName.Equals(DATETIMEOFFSET, StringComparison.Ordinal))
                return true;

            return false;
        }
        
        public static bool IsXmlReader(string typeName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(typeName);

            if (typeName.Equals(XMLREADER, StringComparison.Ordinal))
                return true;

            return false;
        }

        public static bool IsXmlDocument(string typeName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(typeName);

            if (typeName.Equals(XMLDOCUMENT, StringComparison.Ordinal))
                return true;

            return false;
        }
        public static bool IsStream(string typeName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(typeName);

            if (typeName.Equals(STREAM, StringComparison.Ordinal))
                return true;

            return false;
        }


        /// <summary>
        /// Takes the type name and determines if it is a known 
        /// MetaType. For better clarity, you may use the fullname. If it is, the MetaType version of the name is returned. 
        /// If the type is not known, the MetaType of object
        /// is returned
        /// </summary>
        /// <param name="fullTypeName">The type to create (it must include the namespace).</param>
        /// <returns>The MetaType</returns>
        public static MetaType GetMetaType(string fullTypeName)
        {
            MetaType val = null;

            switch (fullTypeName)
            {
                case FULLNAME_DATETIMEOFFSET:
                case DATETIMEOFFSET:
                    val = GetDatetimeOffset();
                    break;
                case FULLNAME_TIMESPAN:
                case TIMESPAN:
                    val = GetTimespan();
                    break;
                case FULLNAME_DATETIME:
                case DATETIME: 
                    val = GetDatetime();
                    break;
                case FULLNAME_BYTE_ARRAY:
                case BYTE_ARRAY:
                    val = GetByte();
                    break;
                case FULLNAME_INT64:
                case INT64:
                    val = GetInt64();
                    break;
                case FULLNAME_STRING:
                case STRING:
                    val = GetString();
                    break;
                case FULLNAME_DECIMAL:
                case DECIMAL:
                    val = GetDecimal();
                    break;
                case FULLNAME_DOUBLE:
                case DOUBLE:
                    val = GetDouble();
                    break;
                case FULLNAME_INT32:
                case INT32:
                    val = GetInt32();
                    break;
                case FULLNAME_SINGLE:
                case SINGLE:
                    val = GetSingle();
                    break;
                case FULLNAME_INT16:
                case INT16:
                    val = GetInt16();
                    break;
                case FULLNAME_OBJECT:
                case OBJECT:
                    val = GetObject();
                    break;
                case FULLNAME_BYTE:
                case BYTE:
                    val = GetByte();
                    break;
                case FULLNAME_GUID:
                case GUID:
                    val = GetGuid();
                    break;
                case FULLNAME_BOOLEAN:
                case BOOLEAN:
                    val = GetBoolean();
                    break;
                case FULLNAME_XMLDOCUMENT:
                case XMLDOCUMENT:
                    val = GetXmlDocument();
                    break;
                case FULLNAME_XMLREADER:
                case XMLREADER:
                    val = GetXmlReader();
                    break;
                case FULLNAME_STREAM:
                case STREAM:
                    val = GetStream();
                    break;
                default:
                    val = GetObject();
                    break;
            }

            return val;
        }

        ///// <summary>
        ///// Takes the type name (it must include the namespace) and determines if it is a known 
        ///// MetaType. If it is, the MetaType version of the name is returned. If the type is not known, the MetaType of object
        ///// is returned
        ///// </summary>
        ///// <param name="fullTypeName">The type to confirm (it must include the namespace).</param>
        ///// <returns>The MetaType version of the name</returns>
        //public static string ConfirmMetaType(string fullTypeName)
        //{
        //    string val = null;

        //    switch (fullTypeName)
        //    {
        //        case FULLNAME_BYTE_ARRAY:
        //            val = BYTE_ARRAY;
        //            break;
        //        case FULLNAME_INT64:
        //            val = INT64;
        //            break;
        //        case FULLNAME_STRING:
        //            val = STRING;
        //            break;
        //        case FULLNAME_DATETIME:
        //            val = DATETIME;
        //            break;
        //        case FULLNAME_DECIMAL:
        //            val = DECIMAL;
        //            break;
        //        case FULLNAME_DOUBLE:
        //            val = DOUBLE;
        //            break;
        //        case FULLNAME_INT32:
        //            val = INT32;
        //            break;
        //        case FULLNAME_SINGLE:
        //            val = SINGLE;
        //            break;
        //        case FULLNAME_INT16:
        //            val = INT16;
        //            break;
        //        case FULLNAME_OBJECT:
        //            val = OBJECT;
        //            break;
        //        case FULLNAME_BYTE:
        //            val = BYTE;
        //            break;
        //        case FULLNAME_GUID:
        //            val = GUID;
        //            break;
        //        case FULLNAME_BOOLEAN:
        //            val = BOOLEAN;
        //            break;
        //        case FULLNAME_XMLDOCUMENT:
        //            val = XMLDOCUMENT;
        //            break;
        //        case FULLNAME_XMLREADER:
        //            val = XMLREADER;
        //            break;
        //        default:
        //            val = OBJECT;
        //            break;
        //    }

        //    return val;
        //}


        #endregion
    }
}
