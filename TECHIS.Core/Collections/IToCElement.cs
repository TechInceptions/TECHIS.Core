using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TECHIS.Core.Collections
{
    /// <summary>
    /// Properties of an Element of a Table of Content
    /// </summary>
    public interface IToCElement
    {
        /// <summary>
        /// Title of document or chapter
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Introductory text
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Parent relative link to the document or chapter
        /// </summary>
        string Ref { get; set; }

        /// <summary>
        /// Site relative link to document or chapter
        /// </summary>
        /// 
        string VirtualPath { get; set; }

        /// <summary>
        /// Document repository path
        /// </summary>
        string WebPath { get; set; }

        string Summary { get; set; }

        string KeyWords { get; set; }

        string Author { get; set; }

        DateTime CreatedTime { get; set; }

        DateTime LastUpdate { get; set; }

        DateTime ExpiresOn { get; set; }
    }
}
