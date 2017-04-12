using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    /// <summary>
    /// Enumerates the reasons for the existance of entity to entity relationships
    /// </summary>
    public enum RelationshipReasonTypes
    {
        /// <summary>
        /// The reason is unknown because the instance has not been set
        /// </summary>
        Unknown,

        /// <summary>
        /// a normal entity to entity relationship in a relational schema (Primary key to Foreign key)
        /// </summary>
        Relational,

        /// <summary>
        /// The relationship is used as a conduit for Entity Inheritance
        /// </summary>
        Inheritance,

        /// <summary>
        /// a normal entity to entity relationship in a relational schema (Unique key (non-primary) to Foreign key)
        /// </summary>
        RelationalUniqueKeyNonPrimary,
    }
}
