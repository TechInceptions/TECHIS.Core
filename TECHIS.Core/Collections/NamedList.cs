using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHIS.Core.Modelling;
using System.Xml.Serialization;

namespace TECHIS.Core.Collections
{
    /// <summary>
    /// Represents a strongly typed list of objects with additional metadata like a name, a description and Id
    /// </summary>
    [Serializable]
    [XmlRoot("Group")]
    public class NamedList<T> : Group<List<T>, T>
    {
        public NamedList() { }
        

        public NamedList(string groupName) : this(groupName, -1)
        {
        }

        public NamedList(string groupName, int capacity):base(groupName,capacity)
        {

        }

        public NamedList(string groupName, IEnumerable<T> list):base(groupName,list)
        {
        }
    }
}
