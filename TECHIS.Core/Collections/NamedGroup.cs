using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TECHIS.Core.Collections
{
    /// <summary>
    /// Represents a strongly typed list of objects with additional metadata like a name, a description and Id
    /// </summary>
    [Serializable]
    [XmlRoot("Group")]
    public class NamedGroup<T> : Modelling.Attribute
    {
        #region Types
        public delegate bool EquatableDelegate(T instance, T other);
        #endregion

        #region Fields
        private List<T> _Items;
        #endregion

        #region Properties
        /// <summary>
        /// The name of the Group
        /// </summary>
        [XmlAttribute]
        public string GroupName
        {
            get { return Name; }
            set { Name = value; }
        }

        /// <summary>
        /// The items in the group
        /// </summary>
        [XmlArray]
        public List<T> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        #endregion

        #region Constructors
        public NamedGroup() { }

        public NamedGroup(string groupName)
            : this(groupName, -1)
        {
        }

        public NamedGroup(string groupName, int capacity)
        {
            InputValidator.ArgumentNullOrEmptyCheck(groupName, "groupName");
            Name = groupName;

            if (capacity < 1)
            {
                _Items = new List<T>();
            }
            else
            {
                _Items = new List<T>(capacity);
            }
        }

        public NamedGroup(string groupName, IEnumerable<T> list)
        {
            InputValidator.ArgumentNullOrEmptyCheck(groupName, "groupName");
            Name = groupName;

            _Items = new List<T>(list);
        }
        #endregion

        /// <summary>
        /// Copy attributes from 'additional' into 'target', if the attribute name doesn't exist in 'target'
        /// </summary>
        public static void MergeGroup(NamedGroup<T> target, NamedGroup<T> additional, EquatableDelegate equalityMethod)
        {
            InputValidator.ArgumentNullCheck(equalityMethod, "equalityMethod");

            List<T> targetItems = target.Items;
            int itemCount = targetItems.Count;

            for (int i = itemCount - 1; i >= 0; i--)
            {
                T current = targetItems[i];
                bool found = false;
                T itemToAdd = default(T);

                foreach (T item in additional.Items)
                {
                    if (equalityMethod(current, item))
                    {
                        found = true;
                        itemToAdd = item;
                        break;
                    }
                }

                if (!found)
                {
                    targetItems.Add(itemToAdd);
                }
            }
        }

        /// <summary>
        /// same as object.MemberwiseClone()
        /// </summary>
        /// <returns></returns>
        public NamedGroup<T> Clone()
        {
            return (NamedGroup<T>)(this.MemberwiseClone());
        }
    }
}
