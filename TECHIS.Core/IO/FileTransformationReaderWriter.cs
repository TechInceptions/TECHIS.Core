using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace TECHIS.Core.IO
{
    /// <summary>
    /// Encapsulates the functionality needed to read and write from a single directory. 
    /// Apart from the simple read/write against a single folder, It allows you to read data from an initial folder, transform it, then write the output to a transformation output folder using the original file names
    /// </summary>
    public class FileTransformationReaderWriter:IO.DirectoryReaderWriter
    {

        #region Fields
        //private DirectoryInfo _OutputFolder;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        public FileTransformationReaderWriter(string FolderPath, string outputFolderPath, string outputFolderPrefix, Guid instanceMarker):base(FolderPath)
        {

            try
            {
                if (string.IsNullOrEmpty(outputFolderPath))
                {
                    if (string.IsNullOrEmpty(outputFolderPrefix))
                        throw new ArgumentException("TransformedFolderPrefix");

                    if (instanceMarker.Equals(Guid.Empty))
                    {
                        instanceMarker = Guid.NewGuid();
                    }
                    outputFolderPath = Path.Combine(FolderPath, outputFolderPrefix + MakeGUIDFileNameFriendly(instanceMarker));
                }

                OutputFolder = new DirectoryInfo(outputFolderPath);

                if (!OutputFolder.Exists)
                    OutputFolder.Create();

            }
            catch (Exception Ex)
            {
                throw new Exception("Failed to create transformed folder", Ex);
            }
        }

        public FileTransformationReaderWriter(string FolderPath, string TransformedFolderPrefix, Guid instanceMarker) : this(FolderPath, null, TransformedFolderPrefix, instanceMarker) { }
        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion

    }
}
