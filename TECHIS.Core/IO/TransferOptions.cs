using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.IO
{
    /// <summary>
    /// 
    /// </summary>
    public enum TransferOptions
    {
        None,
        /// <summary>
        /// Only write files that don't exist in the destination
        /// </summary>
        NewOnly,
        /// <summary>
        /// Only append to files that exist
        /// </summary>
        AppendOnly,
        /// <summary>
        /// Only overwrite files that exist in the destination
        /// </summary>
        OverwriteOnly,
        /// <summary>
        /// Only merge files that exist
        /// </summary>
        MergeOnly,
        /// <summary>
        /// Write files that don't eixst, and overwrite the ones that do
        /// </summary>
        NewAndOverwrite,
        /// <summary>
        /// Write files that don't exist, and append to the ones that do
        /// </summary>
        NewAndAppend,
        /// <summary>
        /// Write files that don't exist, and merge the ones that do
        /// </summary>
        NewAndMerge,
        /// <summary>
        /// Only overwrite files that are different
        /// </summary>
        OverwriteIfDiffOnly,
        /// <summary>
        /// Write files that don't exist and overwrite files that are different
        /// </summary>
        NewAndOverwriteIfDiff,
        /// <summary>
        /// Only write files that don't exist in the destination OR that were last created or modified by this manager
        /// </summary>
        NewOrSelfCreatedOnly,

    }
}
