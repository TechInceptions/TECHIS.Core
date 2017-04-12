using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using TECHIS.Core.Collections;

namespace TECHIS.Core.Modelling
{
    [XmlRoot]
    public class TextContainerGroup : Attribute
    {
        #region Types
        public class HeaderInfo
        {
            private NamedGroup<Attribute> _HeaderGroups;

            //[XmlArray("HeaderGroups")]
            //[XmlArrayItem("headerGroup")]
            public NamedGroup<Attribute> HeaderGroups
            {
                get { return _HeaderGroups; }
                set { _HeaderGroups = value; }
            }

            public HeaderInfo()
            {
                _HeaderGroups = new NamedGroup<Attribute>();
            }

        }
        #endregion

        #region Private Fields 
        private List<TextContainer> _Items;
        private HeaderInfo _Header;
        private Dictionary<string, TextContainer> _InternalDictionary;
        #endregion

        #region Properties 
        public HeaderInfo Header
        {
            get { return _Header; }
            set { _Header = value; }
        }
        [XmlArray("Body")]
        [XmlArrayItem("DataItem", typeof(TextContainer))]
        public List<TextContainer> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        #endregion

        #region Constructors 
        public TextContainerGroup()
        {
            _Items = new List<TextContainer>();
        }
        public TextContainerGroup(IEnumerable<TextContainer> items)
        {
            InputValidator.ArgumentNullCheck(items, "items");

            _Items.AddRange(items);
        }
        #endregion

        #region Public Methods 

        /// <summary>
        /// Creates an internal dictionary object that makes finding TextContainers faster
        /// </summary>
        public Dictionary<string, TextContainer> GetAsDictionary()
        {
            if (_InternalDictionary == null && _Items != null)
            {
                _InternalDictionary = new Dictionary<string, TextContainer>(_Items.Count);

                foreach (TextContainer tc in _Items)
                {
                    _InternalDictionary.Add(tc.Name, tc);
                }
            }
            return _InternalDictionary;
        }
        /// <summary>
        /// Resets the internal dictionary so that when 'GetAsDictionary' is called, the internal dictionary is recreated from the Items
        /// </summary>
        public void ResetDictionary()
        {
            _InternalDictionary = null;
        }

        public void Add(TextContainer textContainer)
        {
            InputValidator.ArgumentNullCheck(textContainer, "textContainer");
            InputValidator.ArgumentNullOrEmptyCheck(textContainer.Name, "textContainer.Name");


            //Remove previous TemplateContainers with similar identifiers
            List<TextContainer> similarTargets;

            similarTargets = _Items.FindAll(delegate(TextContainer tc)
            {
                return (textContainer.Name.Equals(tc.Name, StringComparison.Ordinal));
            });

            //Remove previous
            foreach (TextContainer tc in similarTargets)
            {
                _Items.Remove(tc);
            }


            //Add 
            _Items.Add(textContainer);
        }

        public void AddRange(IEnumerable<TextContainer> templateContainers)
        {
            foreach (TextContainer tc in templateContainers)
            {
                Add(tc);
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
