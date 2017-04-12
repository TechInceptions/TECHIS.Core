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
    public class NamedGroups<T> : IAttribute//, IEnumerable<NamedGroup<T>>
    {

        #region Fields 
        private List<NamedGroup<T>> _Items;

        private string _Name;
        private string _Id;
        private string _Description;
        private string _Value;
        #endregion

        #region Properties
        [XmlArray("Items")]
        public List<NamedGroup<T>> Items
        {
          get { return _Items; }
          set { _Items = value; }
        }

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

        public NamedGroups(NamedGroups<T> namedGroups)
        {
            if (namedGroups != null)
            {
                _Name = namedGroups.Name;
                _Id = namedGroups.Id;
                _Value = namedGroups.Value;
                _Description = namedGroups.Description;
                _Items = new List<NamedGroup<T>>(namedGroups.Items);
            }
            else
            {
                _Items = new List<NamedGroup<T>>();
            }
        }

        public NamedGroups(int capacity) 
        {
            _Items = new List<NamedGroup<T>>(capacity);
        }

        public NamedGroups(IEnumerable<NamedGroup<T>> collection) 
        {
            _Items = new List<NamedGroup<T>>(collection);
        }

        public NamedGroups() 
        {
            _Items = new List<NamedGroup<T>>();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the group that the item belongs to. If item doesn't
        /// belong in a group, null is returned
        /// </summary>
        public virtual NamedGroup<T> FindItemGroup(T item)
        {

            NamedGroup<T> val = null;

            foreach (NamedGroup<T> group in _Items)
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
        public NamedGroup<T> GetGroup(string groupName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(groupName, "groupName");

            NamedGroup<T> val = null;
            foreach (NamedGroup<T> group in _Items)
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
        public NamedGroup<T> CreateGroup(string groupName)
        {
            NamedGroup<T> group = GetGroup(groupName);
            if (group == null)
            {
                group = new NamedGroup<T>(groupName);
                _Items.Add(group);
            }

            return group;
        }

        public int GetGroupCount(string groupName)
        {
            int val = -1;
            NamedGroup<T> group = GetGroup(groupName);
            if (group != null)
            {
                val = group.Items.Count;
            }

            return val;
        }

        /// <summary>
        /// Add a new group to this collection of groups.
        /// </summary>
        public void AddGroup(NamedGroup<T> group)
        {
            InputValidator.ArgumentNullCheck(group, "group");
            _Items.Add(group);
        }

        #endregion

        #region IList<T> Members

        public virtual int IndexOf(NamedGroup<T> item)
        {
            return Items.IndexOf(item);
        }

        public virtual void Insert(int index, NamedGroup<T> item)
        {
            Items.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            RemoveAt(index);
        }

        public virtual NamedGroup<T> this[int index]
        {
            get
            {
                return Items[index];
            }
            set
            {
                Items[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public virtual void Add(NamedGroup<T> item)
        {
            Items.Add(item);
        }

        public virtual void Clear()
        {
            Items.Clear();
        }

        public virtual bool Contains(NamedGroup<T> item)
        {
            return Items.Contains(item);
        }

        public virtual void CopyTo(NamedGroup<T>[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return Items.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(NamedGroup<T> item)
        {
            return Items.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<NamedGroup<T>> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion

        //#region IEnumerable Members

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    return ((System.Collections.IEnumerable)Items).GetEnumerator();
        //}

        //#endregion
    }
}
