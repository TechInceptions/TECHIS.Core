using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    /// <summary>
    /// The type of db statments generated
    /// </summary>
    public enum DataCentricOperationTypes
    {
        /// <summary>
        /// This is the default, implies no value was set
        /// </summary>
        Unknown,

        /// <summary>
        /// Query is based on the entity's primary key
        /// </summary>
        PrimaryKeysQuery,

        /// <summary>
        /// Query returns all rows/instances
        /// </summary>
        GetAllQuery,

        /// <summary>
        /// Query is based on a foreign key
        /// </summary>
        ForeignKeysQuery,

        /// <summary>
        /// Query returns all rows/instances. The resultset can be paged
        /// </summary>
        PagedResultsetGetAllQuery,

        /// <summary>
        /// Query is based on a foreign key. The resultset can be paged
        /// </summary>
        PagedResultsetForeignKeysQuery,

        /// <summary>
        /// Query returns all rows/instances. The resultset can be paged
        /// </summary>
        BatchedGetAllQuery,

        /// <summary>
        /// Query is based on a foreign key. The resultset can be paged
        /// </summary>
        BatchedForeignKeysQuery,

        /// <summary>
        /// Query is based on a preselected filters
        /// </summary>
        CustomQuery,

        /// <summary>
        /// Resolved many to many query
        /// </summary>
        ResolvedQuery,

        /// <summary>
        /// Query is based on a preselected filters. The resultset can be paged
        /// </summary>
        PagedCustomQuery,

        /// <summary>
        /// Delete all
        /// </summary>
        Delete,

        /// <summary>
        /// A delete statement based on the primary key
        /// </summary>
        PrimaryKeyDelete,

        /// <summary>
        /// A delete statement based on the oreign key
        /// </summary>
        ForeignKeysDelete,

        /// <summary>
        /// Update statement based on the primary key
        /// </summary>
        PrimaryKeyUpdate,

        /// <summary>
        /// Update statement based on the foreign key
        /// </summary>
        ForeignKeyUpdate,

        /// <summary>
        /// Update statement that depends on the initial information still being current by update time
        /// </summary>
        OptimisticUpdate,

        /// <summary>
        /// An insert statement
        /// </summary>
        Insert,

        /// <summary>
        /// This is a query that was designed to be run only at the database level
        /// </summary>
        DBOnly,

        /// <summary>
        /// Search query
        /// </summary>
        SearchQuery,

        /// <summary>
        /// Paged search query
        /// </summary>
        PagedSearchQuery,

        /// <summary>
        /// Counts an entity
        /// </summary>
        EntityCount,

        /// <summary>
        /// Counts the number of rows returned by a query
        /// </summary>
        QueryCount,

        /// <summary>
        /// A custom query that doesn't return data. Functions generated based on this activity will return void
        /// </summary>
        CustomNonQuery,

        /// <summary>
        /// A custom query that doesn't return data. Functions generated based on this activity will return a boolean
        /// that indicates if any rows were affected by the call.
        /// </summary>
        CustomNonQueryWithRowsAffected,

        /// <summary>
        /// A custom query that doesn't return data. Functions generated based on this activity will return an integer 
        /// that indicates the number of rows affected by the call.
        /// </summary>
        CustomNonQueryWithRowCount,

        /// <summary>
        /// Query is based on the entity's primary key that returns only a binary column
        /// </summary>
        PrimaryKeysBinaryColumn,




        #region Filter-set types 
        
        /// <summary>
        /// Query is based on a set of primary keys for the entity. The resultset can be paged
        /// </summary>
        PagedPrimaryKeysSetQuery,

        /// <summary>
        /// Query is based on a set of foreign keys for the entity. The resultset can be paged
        /// </summary>
        PagedForeignKeysSetQuery,

        /// <summary>
        /// Query is based on user configured fields. a set of entangled values is used as the input. The resultset can be paged
        /// </summary>
        PagedCustomSetQuery,
        #endregion
    }
}
