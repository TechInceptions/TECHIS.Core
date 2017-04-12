using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace TECHIS.Core.Modelling
{
    [Serializable]
    [XmlInclude(typeof(IAttribute))]
    [DataContract]
    public class Groups<TCollection, TItem> : List<Group<TCollection, TItem>>, IAttribute where TCollection : IList<TItem>, new()
    {
        #region Fields 
        private string _Name;
        private string _Id;
        private string _Description;
        private string _Value;
        #endregion

        #region Properties 

        [XmlAttribute]
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlAttribute]
        [DataMember]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        [XmlAttribute]
        [DataMember]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        [XmlAttribute]
        [DataMember]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        #endregion

        #region Constructors 
        public Groups(int capacity) : base(capacity) { }
        public Groups(IEnumerable<Group<TCollection,TItem>> collection) : base(collection) { }
        public Groups() : base() { }
        #endregion

        #region Methods 

        /// <summary>
        /// Returns the group that the item belongs to. If item doesn't
        /// belong in a group, null is returned
        /// </summary>
        public virtual Group<TCollection,TItem> FindItemGroup(TItem item)
        {

            Group<TCollection,TItem> val = null;

            foreach (Group<TCollection,TItem> group in this)
            {
                foreach (TItem t in group.Items)
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
        protected virtual bool ItemsEquals(TItem item1, TItem item2)
        {
            return item1.Equals(item2);
        }

        /// <summary>
        /// Find the Group that has the specified name.
        /// If the group is not found, null will be returned
        /// </summary>
        public Group<TCollection,TItem> GetGroup(string groupName)
        {
            InputValidator.ArgumentNullOrEmptyCheck(groupName, "groupName");

            Group<TCollection,TItem> val = null;
            foreach (Group<TCollection,TItem> group in this)
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
        public Group<TCollection,TItem> CreateGroup(string groupName)
        {
            Group<TCollection,TItem> group = GetGroup(groupName);
            if (group == null)
            {
                group = new Group<TCollection,TItem>(groupName);
                this.Add(group);
            }

            return group;
        }

        public int GetGroupCount(string groupName)
        {
            int val = -1;
            Group<TCollection,TItem> group = GetGroup(groupName);
            if (group != null)
            {
                val = group.Items.Count;
            }

            return val;
        }

        /// <summary>
        /// Add a new group to this collection of groups.
        /// </summary>
        public void AddGroup(Group<TCollection,TItem> group)
        {
            InputValidator.ArgumentNullCheck(group, "group");
            this.Add(group);
        }

        #endregion
    }
}
