using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TECHIS.Core.Collections
{
    /// <summary>
    /// Represents a strongly typed list of objects with additional metadata like a name, a description and Id
    /// </summary>
    [Serializable]
    [XmlRoot("Group")]
    public class ToCElements : NamedGroup<ToCElement>,IToCElement
    {
        /// <summary>
        /// Title of document or chapter
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Introductory text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Parent relative link to the document or chapter
        /// </summary>
        public string Ref { get; set; }
        /// <summary>
        /// Site relative link to document or chapter
        /// </summary>
        /// 
        public string VirtualPath { get; set; }
        /// <summary>
        /// Document repository path
        /// </summary>
        public string WebPath { get; set; }
        public string Summary { get; set; }
        public string KeyWords { get; set; }
        public string Author { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
