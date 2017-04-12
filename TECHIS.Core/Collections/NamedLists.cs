using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TECHIS.Core.Modelling;

namespace TECHIS.Core.Collections
{
    /// <summary>
    /// Represents a strongly typed list of objects with additional metadata like a name, a description and Id
    /// </summary>
    [Serializable]
    [XmlRoot("Groups")]
    public class NamedLists<T> : List<NamedList<T>>, IAttribute 
    {

        #region Fields
        private string _Name;
        private string _Id;
        private string _Description;
        private string _Value;
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
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        [XmlAttribute]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        #endregion

        #region Constructors 
        public NamedLists(int capacity) : base(capacity) { }
        public NamedLists(IEnumerable<NamedList<T>> collection) : base(collection) { }
        public NamedLists() : base() { }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the group that the item belongs to. If item doesn't
        /// belong in a group, null is returned
        /// </summary>
        public virtual NamedList<T> FindItemGroup(T item)
        {

            NamedList<T> val = null;

            foreach (NamedList<T> group in this)
            {
                foreach (T t in group.Items)
                {
                    if (ItemsEquals(item, t))
                    {
                        val = group;
                        break;
                    }
                }

                if (val != null)
                {
                    break;
                }
            }

            return val;
        }

        /// <summary>
        /// Determines is two items are equals. Inheritors may overide this to change
        /// how equality is determined
        /// </summary>
        protected virtual bool ItemsEquals(T item1, T item2)
        {
            return item1.Equals(item2);
        }

        /// <summary>
        /// Find the Group that has the specified name.
        /// If the group is not found, null will be returned
        /// </summary>
        public NamedList<T> GetGroup(string groupName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(groupName, "groupName");

            NamedList<T> val = null;
            foreach (NamedList<T> group in this)
            {
                if (groupName.Equals(group.Name, StringComparison.OrdinalIgnoreCase))
                {
                    val = group;
                    break;
                }
            }

            return val;
        }

        /// <summary>
        /// Creates a new instance of Group with the specified name then returns it. 
        /// If a group already exists, no Group is created
        /// </summary>
        public NamedList<T> CreateGroup(string groupName)
        {
            NamedList<T> group = GetGroup(groupName);
            if (group == null)
            {
                group = new NamedList<T>(groupName);
                this.Add(group);
            }

            return group;
        }

        public int GetGroupCount(string groupName)
        {
            int val = -1;
            NamedList<T> group = GetGroup(groupName);
            if (group != null)
            {
                val = group.Items.Count;
            }

            return val;
        }

        /// <summary>
        /// Add a new group to this collection of groups.
        /// </summary>
        public void AddGroup(NamedList<T> group)
        {
            InputValidator.ArgumentNullCheck(group, "group");
            this.Add(group);
        }

        #endregion
    }
}
