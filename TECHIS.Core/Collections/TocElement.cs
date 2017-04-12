using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TECHIS.Core.Collections
{
    /// <summary>
    /// Properties of an Element of a Table of Content
    /// </summary>
    [Serializable]
    public class ToCElement: Modelling.Attribute,IToCElement
    {
        public ToCElement()
        {
            Childs = new List<ToCElement>();
        }

        /// <summary>
        /// Title of document or chapter
        /// </summary>
        public string  Title { get; set; }

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

        public List<ToCElement> Childs { get; set; }
    }
}
