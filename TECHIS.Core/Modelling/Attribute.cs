using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace TECHIS.Core.Modelling
{
    [Serializable]
    [XmlRoot("Attribute")]
    [DataContract(Name="Attribute")]
    public class Attribute : ITextSource,IAttribute
    {
        #region Constants 
        public const string PROPERTYKEY_NAME = "Name";
        public const string PROPERTYKEY_ID = "Id";
        public const string PROPERTYKEY_DESCRIPTION = "Description";
        public const string PROPERTYKEY_VALUE = "Value";
        public const string PROPERTYKEY_USERDEFINEDTYPE = "UserDefinedType";
        #endregion

        #region Fields
        private string _Name;
        private string _Id;
        private string _Description;
        private string _Value;
        private string _UserDefinedType;

        #endregion

        #region Properties 
        /// <summary>
        /// A user provided value that points to a user defined type. 
        /// </summary>
        [XmlAttribute("UDT")]
        [DataMember(Name="UDT")]
        public string UserDefinedType
        {
            get { return _UserDefinedType; }
            set { _UserDefinedType = value; }
        }

        [XmlAttribute("Name")]
        [DataMember(Name = "Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlAttribute("Value")]
        [DataMember(Name = "Value")]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        [XmlAttribute("Description")]
        [DataMember(Name = "Description")]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        [XmlAttribute("Id")]
        [DataMember(Name = "Id")]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        #endregion

        #region Constructors 
        public Attribute(Attribute atr)
        {
            _Name = atr._Name;
            _Id = atr.Id;
            _Description = atr._Description;
            _Value = atr._Value;
        }
        public Attribute() { }
        public Attribute(string name, string value)
        {
            _Name = name;
            _Value = value;
        }
        #endregion

        #region Methods 
        public Dictionary<string, string> GetAsKeysValues()
        {
            Dictionary<string, string> keysAndValues = new Dictionary<string, string>(5);

            keysAndValues.Add(PROPERTYKEY_DESCRIPTION, _Description);
            keysAndValues.Add(PROPERTYKEY_ID, _Id);
            keysAndValues.Add(PROPERTYKEY_NAME, _Name);
            keysAndValues.Add(PROPERTYKEY_USERDEFINEDTYPE, _UserDefinedType);
            keysAndValues.Add(PROPERTYKEY_VALUE, _Value);

            return keysAndValues;
        }

        public Dictionary<string, string> GetAsKeysValues(Dictionary<string, string> nameMap)
        {
            if (nameMap==null || nameMap.Count==0)
            {
                throw new ArgumentException("nameMap must not be null and must have at least one entry", "nameMap");
            }

            Dictionary<string, string> keysAndValues = new Dictionary<string, string>(5);

            string newKey;

            if (GetKeyForKeysValues(PROPERTYKEY_DESCRIPTION, nameMap, out newKey))
            {
                keysAndValues.Add(newKey, _Description);
            }

            if (GetKeyForKeysValues(PROPERTYKEY_ID, nameMap, out newKey))
            {
                keysAndValues.Add(newKey, _Id);
            }
            if (GetKeyForKeysValues(PROPERTYKEY_NAME, nameMap, out newKey))
            {
                keysAndValues.Add(newKey, _Name);
            }
            if (GetKeyForKeysValues(PROPERTYKEY_USERDEFINEDTYPE, nameMap, out newKey))
            {
                keysAndValues.Add(newKey, _UserDefinedType);
            }
            if (GetKeyForKeysValues(PROPERTYKEY_VALUE, nameMap, out newKey))
            {
                keysAndValues.Add(newKey, _Value);
            }

            return keysAndValues;
        }

        private bool GetKeyForKeysValues(string propertyKey,Dictionary<string, string> nameMap, out string newKey)
        {
            bool val = false;
            if (nameMap.ContainsKey(propertyKey))
            {
                newKey = nameMap[propertyKey];
                val = true;
            }
            else
            {
                newKey = null;
            }
            return val;
        }

        /// <summary>
        /// Based on the properties of this instance, derives a value and assigns that value to the Id property.
        /// The default implementation sets the value of the Name property to the Id.
        /// </summary>
        public virtual void SetId()
        {
            Id = Name;
        }
        /// <summary>
        /// Add Attributes from 'newAttributes' to 'currentAttributes' if currentAttributes doesn't already contain similarly named Attributes.
        /// <param name="NonUnique">all contain the attributes from 'newAttributes' that were not added</param>
        /// </summary>
        public static void AddUniqueAttributes<T>(List<T> currentAttributes, List<T> newAttributes, out List<T> NonUnique) where T:Attribute
        {
            NonUnique = new List<T>();
            foreach (T atr in newAttributes)
            {
                if (currentAttributes.Find(delegate(T atr1) { return atr1.Name.Equals(atr.Name, StringComparison.Ordinal); }) == null)
                {
                    currentAttributes.Add(atr);
                }
                else
                {
                    NonUnique.Add(atr);
                }
            }
        }
        /// <summary>
        /// Add Attributes from 'newAttributes' to 'currentAttributes' if currentAttributes doesn't already contain similarly named Attributes
        /// </summary>
        public static void AddUniqueAttributes<T>(List<T> currentAttributes, List<T> newAttributes) where T : Attribute
        {
            List<T> NonUnique;
            AddUniqueAttributes(currentAttributes, newAttributes, out NonUnique); 
        }
        /// <summary>
        /// Get Attributes in 'AttributesToCheck' that are not in the 'baseList'
        /// <param name="NonUnique">the attribute that are not unique</param>
        /// </summary>
        public static List<T> GetUniqueAttributes<T>(List<T> baseList, List<T> AttributesToCheck, out List<T> NonUnique) where T : Attribute
        {
            List<T> val = new List<T>(AttributesToCheck.Count);
            NonUnique = new List<T>();
            foreach (T cf in AttributesToCheck)
            {
                if (baseList.Find(delegate(T cf1) { return cf1.Name.Equals(cf.Name, StringComparison.Ordinal); }) == null)
                {
                    val.Add(cf);
                }
                else
                {
                    NonUnique.Add(cf);
                }
            }
            return val;
        }
        
        /// <summary>
        /// Get Attributes in 'AttributesToCheck' that are not in the 'baseList'
        /// </summary>
        public static List<T> GetUniqueAttributes<T>(List<T> baseList, List<T> AttributesToCheck) where T : Attribute
        {
            List<T> NonUnique = new List<T>();
            return GetUniqueAttributes(baseList, AttributesToCheck, out NonUnique);
        }

        public static List<Attribute> CopyAttributes(List<Attribute> attributes)
        {
            List<Attribute> newList = new List<Attribute>(attributes.Count);

            foreach (Attribute atr in attributes)
            {
                newList.Add(new Attribute(atr));
            }

            return newList;
        }

        public static Attribute GetAttribute(string attributeName, IEnumerable<Attribute> settings)
        {
            Attribute retval = null;

            foreach (Attribute a in settings)
            {
                if ( attributeName.Equals(a.Name, StringComparison.Ordinal) )
                {
                    retval = a;
                    break;
                }
            }

            return retval;
        }

        public static string GetAttributeValue(string attributeName, IEnumerable<Attribute> settings)
        {
            string retval = string.Empty;

            Attribute a = GetAttribute(attributeName, settings);

            if (a != null)
                retval = a.Value;

            return retval;
        }
        #endregion

        #region ITextSource Members 

        public virtual string GetText()
        {
            return ToString();
        }

        public virtual void WriteText(System.IO.TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            output.Write(GetText());
        }

        #endregion



    }
}
