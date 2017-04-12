using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace TECHIS.Core.Modelling
{
    /// <summary>
    /// Represents a strongly typed list of objects with additional metadata like a name, a description and Id
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(Attribute))]
    [XmlRoot("Group")]
    [DataContract(Name="Group")]
    public class Group<TCollection,TItem> : Attribute, IList<TItem> where TCollection:IList<TItem>, new()
    {
        #region Types 
        public delegate bool EquatableDelegate(TItem instance, TItem other);
        #endregion

        #region Fields
        private TCollection _Items;
        #endregion

        #region Properties 
        /// <summary>
        /// The name of the Group
        /// </summary>
        public virtual string GroupName
        {
            get { return Name; }
            set { Name = value; }
        }

        /// <summary>
        /// The items in the group
        /// </summary>
        [DataMember]
        [XmlArray]
        public virtual TCollection Items
        {
            get { return _Items; }
            set { _Items = value; }
        } 
        #endregion

        #region Constructors 
        public Group():base() 
        {
            _Items = new TCollection();
        }

        public Group(string groupName) : this(groupName, -1)
        {
        }

        public Group(string groupName, int capacity)
        {
            InputValidator.ArgumentNullOrEmptyCheck(groupName, "groupName");
            Name = groupName;

            _Items = new TCollection();

        }

        public Group(string groupName, IEnumerable<TItem> list)
        {
            InputValidator.ArgumentNullOrEmptyCheck(groupName, "groupName");
            Name = groupName;

            _Items = new TCollection();
            if (list!=null)
            {
                foreach (TItem item in list)
                {
                    _Items.Add(item);
                }
            }
        }
        #endregion

        #region Public Methods 
        
        /// <summary>
        /// Copy attributes from 'additional' into 'target', if the attribute name doesn't exist in 'target'
        /// </summary>
        public static void MergeGroup(Group<TCollection,TItem> target, Group<TCollection,TItem> additional, EquatableDelegate equalityMethod)
        {
            InputValidator.ArgumentNullCheck(equalityMethod, "equalityMethod");

            TCollection targetItems = target.Items;
            int itemCount = targetItems.Count;

            for (int i = itemCount - 1; i >= 0; i--)
            {
                TItem current = targetItems[i];
                bool found = false;
                TItem itemToAdd=default(TItem);

                foreach (TItem item in additional.Items)
                {
                    if (equalityMethod(current,item))
                    {
                        found = true;
                        itemToAdd = item;
                        break;
                    }
                }

                if (! found)
                {
                    targetItems.Add(itemToAdd);
                }
            }
        }

        /// <summary>
        /// same as object.MemberwiseClone()
        /// </summary>
        /// <returns></returns>
        public Group<TCollection, TItem> Clone()
        {
            return (Group<TCollection, TItem>)(this.MemberwiseClone());
        }
        #endregion

        #region IList<T> Members

        public virtual int IndexOf(TItem item)
        {
            return Items.IndexOf(item);
        }

        public virtual void Insert(int index, TItem item)
        {
            Items.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            RemoveAt(index);
        }

        public virtual TItem this[int index]
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

        public virtual void Add(TItem item)
        {
            Items.Add(item);
        }

        public virtual void Clear()
        {
            Items.Clear();
        }

        public virtual bool Contains(TItem item)
        {
            return Items.Contains(item);
        }

        public virtual void CopyTo(TItem[] array, int arrayIndex)
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

        public virtual bool Remove(TItem item)
        {
            return Items.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<TItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Items).GetEnumerator();
        }

        #endregion
    }
}
