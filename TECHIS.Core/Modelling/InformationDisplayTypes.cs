using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public enum InformationDisplayTypes
    {
        Unknown,
        /// <summary>
        /// A display that enables edit
        /// </summary>
        Edit,
        /// <summary>
        /// a display that enables insert
        /// </summary>
        Insert,
        /// <summary>
        /// a display that enables delete
        /// </summary>
        Delete,
        /// <summary>
        /// Displays information about an entity
        /// </summary>
        Main,
        /// <summary>
        /// Displays information about an entity, but only the primitive attibutes
        /// </summary>
        Basic,
        /// <summary>
        /// multi faceted display that with Edit, insert, Delete and Update
        /// </summary>
        Composite,
        /// <summary>
        /// Like Composite, but includes a list of the entities. (With List Composite)
        /// </summary>
        WLComposite,
        /// <summary>
        /// A display that allows search
        /// </summary>
        Search,
        /// <summary>
        /// A display based on a user created query or procedure
        /// </summary>
        Custom,
        /// <summary>
        /// a user created display
        /// </summary>
        UserCreated,
        /// <summary>
        /// Lists 'A'll instances of an entity
        /// </summary>
        All,
        /// <summary>
        /// List instances of an entity, based on a 'F'ilter
        /// </summary>
        Filtered,
        /// <summary>
        /// List instances of an entity, based on a fixed 'C'onstraint
        /// </summary>
        Constrained
    }
}
