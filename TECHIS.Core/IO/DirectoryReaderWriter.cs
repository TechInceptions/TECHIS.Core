using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace TECHIS.Core.IO
{
    /// <summary>
    /// Encapsulates the functionality needed to read and write from a single directory. 
    /// </summary>
    public class DirectoryReaderWriter
    {
        #region Fields 
        //private PublicationHistoryManager _PublicationHistoryManager;
        private DirectoryInfo _InitialFolder;
        private DirectoryInfo _OutputFolder;
        private FileInfo[] _Files;
        private static int DIMENSION_FILE_CHUNK = 65536*2;
        #endregion

        #region Properties 

        public FileInfo[] Files
        {
            get 
            {
                if (_Files == null)
                    _Files = _InitialFolder.GetFiles();

                return _Files; 
            }
        }

        public DirectoryInfo InitialFolder
        {
            get { return _InitialFolder; }
            set { _InitialFolder = value; }
        }

        public DirectoryInfo OutputFolder
        {
            get
            {
                return _OutputFolder;
            }

            set
            {
                _OutputFolder = value;
            }
        }

        public int FileCount
        {
            get
            {
                return Files.Length;
            }
        }
        #endregion

        #region Constructors 
        public DirectoryReaderWriter(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentException();

            if (!Directory.Exists(folderPath))
                throw new ArgumentException(folderPath + " doesn't exist");

            _InitialFolder = new DirectoryInfo(folderPath);

            _OutputFolder = _InitialFolder;
        }

        #endregion

        #region Private Methods 

        public static string MakeGUIDFileNameFriendly(Guid guid)
        {
            return (new StringBuilder(guid.ToString())).Replace("-", "").Replace("{", "").Replace("}", "").ToString();
        }

        /// <summary>
        /// If the fullNameAndPath is not specified, use the OriginalFilePosition ( the ordinal position of the file in the files collection ), to get the file name
        /// </summary>
        private string DeriveFileName(int OriginalFilePosition, string FullNameAndPath)
        {
            string filename;
            if (string.IsNullOrEmpty(FullNameAndPath))
            {
                if (OriginalFilePosition == int.MinValue)
                    filename = Path.Combine(_OutputFolder.FullName, MakeGUIDFileNameFriendly(Guid.NewGuid()));
                else
                {
                    if (OriginalFilePosition < 0 || OriginalFilePosition > Files.GetUpperBound(0))
                        throw new ArgumentException();

                    if (Files == null || Files.Length < 1)
                        throw new InvalidOperationException("original files don't exist");


                    filename = Path.Combine(_OutputFolder.FullName, Path.GetFileName(Files[OriginalFilePosition].FullName));
                }
            }
            else
            {
                filename = FullNameAndPath;
            }
            return filename;
        }

        private void TransferFile(FileInfo file, TransferOptions transferOption, AdditionalTextDelegate additionalTextDelegate)
        {
            //get existing file in the current destination
            FileInfo[] existing = _InitialFolder.GetFiles(file.Name);
            FileInfo destinationFile = null;
            bool fileExists = false;
            string destinationFullPath = null;

            if (existing != null && existing.Length > 0)
            {
                destinationFile = existing[0];
                if (destinationFile != null)
                {
                    destinationFullPath = destinationFile.FullName;
                    fileExists = true;
                }
            }

            if (!fileExists)
            {
                destinationFullPath = Path.Combine(_InitialFolder.FullName, file.Name);
            }
            bool transferOccured=false;
            switch (transferOption)
            {
                case TransferOptions.None:
                    Exceptions.ThrowInvalidOps("the current transfer option is 'None', please set a transfer option");
                    break;
                case TransferOptions.NewOrSelfCreatedOnly:
                    if (fileExists)
                    {
                        if (HasContentChanged(file, destinationFile) && CreatedBySelf(destinationFile))
                        {
                            File.Copy(file.FullName, destinationFullPath, true);
                            transferOccured = true;
                        }
                    }
                    else
                    {
                        File.Copy(file.FullName, destinationFullPath, false);
                        transferOccured = true;
                    }
                    break;
                case TransferOptions.NewOnly:
                    if (!fileExists)
                    {
                        File.Copy(file.FullName, destinationFullPath, false);
                        transferOccured = true;
                    }
                    break;
                case TransferOptions.OverwriteOnly:
                    if (fileExists)
                    {
                        File.Copy(file.FullName, destinationFullPath, true);
                        transferOccured = true;
                    }

                    break;
                case TransferOptions.NewAndOverwrite:
                    //always write
                    File.Copy(file.FullName, destinationFullPath, true);
                    transferOccured = true;
                    break;
                case TransferOptions.NewAndAppend:

                    if (!fileExists)
                    {
                        File.Copy(file.FullName, destinationFullPath, false);
                        transferOccured = true;
                    }
                    else
                    {
                        transferOccured = Append(destinationFile, file, additionalTextDelegate);
                    }
                    break;
                case TransferOptions.NewAndMerge:

                    if (!fileExists)
                    {
                        string newText;
                        if (additionalTextDelegate(null, DirectoryReaderWriter.ReadText(file.FullName), out newText))
                        {
                            WriteString(newText, destinationFullPath);
                            transferOccured = true;
                        }
                    }
                    else
                    {
                        transferOccured = Merge(destinationFile, file, additionalTextDelegate);
                    }
                    break;
                case TransferOptions.AppendOnly:

                    if (fileExists)
                    {
                        transferOccured = Append(destinationFile, file, additionalTextDelegate);
                    }
                    break;
                case TransferOptions.MergeOnly:
                    if (fileExists)
                    {
                        transferOccured = Merge(destinationFile, file, additionalTextDelegate);
                    }
                    break;
                case TransferOptions.OverwriteIfDiffOnly:
                    if (fileExists)
                    {
                        bool isDifferent = HasContentChanged(file, destinationFile);
                        if (isDifferent)
                        {
                            File.Copy(file.FullName, destinationFullPath, true);
                            transferOccured = true;
                        }
                    }
                    break;
                case TransferOptions.NewAndOverwriteIfDiff:
                    if (fileExists)
                    {
                        bool isDifferent = HasContentChanged(file, destinationFile);
                        if (isDifferent)
                        {
                            File.Copy(file.FullName, destinationFullPath, true);
                            transferOccured = true;
                        }
                    }
                    else
                    {
                        File.Copy(file.FullName, destinationFullPath, true);
                        transferOccured = true;
                    }
                    break;
                default:
                    Exceptions.ThrowInvalidOps("the current transfer option is not supported");
                    break;
            }

            //log publication
            if (transferOccured)
            {
                LogTransfer(file, destinationFile, destinationFullPath);
            }
        }

        protected virtual bool CreatedBySelf(FileInfo destinationFile)
        {
            return false;
        }

        protected virtual void LogTransfer(FileInfo file, FileInfo destinationFile, string destinationFullPath)
        {
        }

        private bool HasContentChanged(FileInfo file, FileInfo destinationFile)
        {
            bool hasChanged = false;

            //Check length
            if (file.Length != destinationFile.Length)
            {
                hasChanged = true;
            }

            //Compare
            if (! hasChanged)
            {
                string fileText = ReadText(file.FullName);
                string destinationText = ReadText(destinationFile.FullName);

                if ( ! fileText.Equals(destinationText, StringComparison.Ordinal))
                {
                    hasChanged = true;
                }
            }
            return hasChanged;
        }

        private bool  Append(FileInfo destinationFile, FileInfo file, AdditionalTextDelegate additionalTextDelegate)
        {
            InputValidator.ArgumentNullCheck(additionalTextDelegate, "additionalTextDelegate");

            string oldFileText = Read(destinationFile.Name);
            string newFileText = DirectoryReaderWriter.ReadText(file.FullName);
            string textToAppend;
            bool success = additionalTextDelegate(oldFileText, newFileText, out textToAppend);

            if (success)
                AppendString(textToAppend, destinationFile.FullName);

            return success;
        }


        private bool Merge(FileInfo destinationFile, FileInfo file, AdditionalTextDelegate additionalTextDelegate)
        {
            InputValidator.ArgumentNullCheck(additionalTextDelegate, "additionalTextDelegate");

            string oldFileText = Read(destinationFile.Name);
            string newFileText = DirectoryReaderWriter.ReadText(file.FullName);
            string mergedText;
            bool success = additionalTextDelegate(oldFileText, newFileText, out mergedText);

            if (success)
                WriteString(mergedText, destinationFile.FullName);

            return success;

        }
        #endregion

        #region Public Methods 
        public static string GetRelativePath(FileSystemInfo rootFolder, FileSystemInfo file)
        {
            return GetRelativePath(rootFolder.FullName, file.FullName);

        }

        public static string GetRelativePath(string rootFolder, string fileFullName)
        {
            string p = fileFullName.Replace(rootFolder, "").Trim('\\');
            return p;
        }
        public static void AppendString(string data, string fullNameAndPath)
        {
            if (!string.IsNullOrEmpty(data))
            {
                FileInfo fi = new FileInfo(fullNameAndPath);
                using (FileStream fs = fi.Open(FileMode.Append, FileAccess.Write, FileShare.Delete | FileShare.Read))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.AutoFlush = true;
                        sw.Write(data);
                    }
                }
            }
        }

        /// <summary>
        /// Appends data to the folder
        /// </summary>
        /// <param name="data">the data to append to the file</param>
        /// <param name="filename">This is only the file name, don't include the path</param>
        public void Append(string data, string filename)
        {
            AppendString(data, Path.Combine(_InitialFolder.FullName, filename));
        }

        //public void TransferFiles(IList<FileInfo> files, TransferOptions transferOption, AdditionalTextDelegate additionalTextDelegate) 
        //{
        //    if (files != null && files.Count > 0)
        //    {
        //        foreach (FileInfo fi in files)
        //        {
        //            TransferFile(fi, transferOption, additionalTextDelegate);
        //        }
        //    }
        //}

        public void TransferFiles(IList<FileTransferInfo> fileTransferInfoList, AdditionalTextDelegate additionalTextDelegate)
        {
            if (fileTransferInfoList != null && fileTransferInfoList.Count > 0)
            {
                foreach (FileTransferInfo fti in fileTransferInfoList)
                {
                    TransferFile(fti.File, fti.TransferOption, additionalTextDelegate);
                }
            }
        }

        public static void TransferFiles(DirectoryInfo source, DirectoryInfo destination, string inclusionFilters, string exclusionFilters,
                                        TransferOptions baseTransferOption, Dictionary<string, TransferOptions> specializedTransferOptions,
                                        AdditionalTextDelegate additionalTextDelegate)
        {
            InputValidator.ArgumentNullCheck(source, "source");
            InputValidator.ArgumentNullCheck(destination, "destination");

            List<FileTransferInfo> filesToTransfer = CreateFileTransferInfoBatch(source, inclusionFilters, exclusionFilters, baseTransferOption, specializedTransferOptions);

            DirectoryReaderWriter drw = new DirectoryReaderWriter(destination.FullName);

            drw.TransferFiles(filesToTransfer, additionalTextDelegate);

        }

        //public static void PublishFiles(DirectoryInfo source, DirectoryInfo destination, string inclusionFilters, string exclusionFilters,
        //                                TransferOptions baseTransferOption, Dictionary<string, TransferOptions> specializedTransferOptions,
        //                                AdditionalTextDelegate additionalTextDelegate)
        //{
        //    InputValidator.ArgumentNullCheck(source, "source");
        //    InputValidator.ArgumentNullCheck(destination, "destination");


        //    List<FileTransferInfo> filesToTransfer = CreateFileTransferInfoBatch(source, inclusionFilters, exclusionFilters, baseTransferOption, specializedTransferOptions);

        //    DirectoryReaderWriter drw = new DirectoryReaderWriter(destination.FullName);
        //    //drw.InitializeHistoryManager(historyStoreLocation,source);
        //    drw.TransferFiles(filesToTransfer, additionalTextDelegate);
        //    //destinationFilesRemoved=drw.FinalizeHistoryManager();

        //}

        protected static List<FileTransferInfo> CreateFileTransferInfoBatch(DirectoryInfo source, string inclusionFilters, string exclusionFilters, TransferOptions baseTransferOption, Dictionary<string, TransferOptions> specializedTransferOptions)
        {
            List<FileInfo> inclusionFiles = new List<FileInfo>();
            List<FileInfo> excludeFiles = new List<FileInfo>();
            List<FileInfo> allFiles = new List<FileInfo>();

            if (!string.IsNullOrEmpty(inclusionFilters))
            {
                inclusionFiles.AddRange(GetFilesFromDirectory(inclusionFilters, source, SearchOption.TopDirectoryOnly, ';'));
            }
            else
            {
                inclusionFiles = new List<FileInfo>(0);
            }

            if (!string.IsNullOrEmpty(exclusionFilters))
            {
                excludeFiles.AddRange(GetFilesFromDirectory(exclusionFilters, source, SearchOption.TopDirectoryOnly, ';'));
            }
            else
            {
                excludeFiles = new List<FileInfo>(0);
            }

            allFiles.AddRange(source.GetFiles());

            List<FileTransferInfo> filesToTransfer = new List<FileTransferInfo>(30);

            foreach (FileInfo fi in allFiles)
            {
                if (IsInCollection(inclusionFiles, fi) && !IsInCollection(excludeFiles, fi))
                {
                    filesToTransfer.Add(new FileTransferInfo(fi, baseTransferOption));
                }
            }

            if (specializedTransferOptions != null && specializedTransferOptions.Count > 0)
            {
                AssignSpecializedTransferOptions(source, filesToTransfer, specializedTransferOptions, baseTransferOption);
            }
            return filesToTransfer;
        }

        //private List<string> FinalizeHistoryManager()
        //{
        //    List<string> filesRemoved = null;
        //    if (_PublicationHistoryManager != null)
        //    {
        //        _PublicationHistoryManager.EndBatch();
        //        _PublicationHistoryManager.CleanUpDestination(out filesRemoved);
        //    }

        //    if (filesRemoved == null)
        //    {
        //        filesRemoved = new List<string>();
        //    }

        //    return filesRemoved;
        //}

        private static void AssignSpecializedTransferOptions(DirectoryInfo source, List<FileTransferInfo> filesToTransfer, Dictionary<string, TransferOptions> specializedTransferOptions, TransferOptions baseTransferOption)
        {
            foreach (KeyValuePair<string, TransferOptions> kvp in specializedTransferOptions)
            {
                string searchPattern = kvp.Key;
                TransferOptions transferOption = kvp.Value;

                FileInfo[] matchingSearch = source.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
                foreach (FileInfo fi in matchingSearch)
                {
                    foreach (FileTransferInfo fileToTransfer in filesToTransfer)
                    {
                        //If the file has a specialized transfer option, assign it
                        if (fi.Name.Equals(fileToTransfer.File.Name, StringComparison.Ordinal))
                        {
                            fileToTransfer.TransferOption = transferOption;
                        }
                    }
                }
            }
        }

        public static List<FileInfo> GetFilesFromDirectory(string filterList, DirectoryInfo target, SearchOption searchOption, char seperator)
        {
            InputValidator.ArgumentNullCheck(target, "target");

            List<FileInfo> val;
            if ( ! string.IsNullOrEmpty(filterList))
            {
                val = new List<FileInfo>();

                string[] parts = filterList.Split(new char[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in parts)
                {
                    val.AddRange(target.GetFiles(s, searchOption));
                }
            }
            else
            {
                val = new List<FileInfo>();
            }

            return val;
        }

        private static bool IsInCollection(IEnumerable<FileInfo> collection, FileInfo target)
        {
            bool val=false;
            foreach (FileInfo fi in collection)
            {
                if (fi.Name.Equals(target.Name))
                {
                    val = true;
                    break;
                }
            }

            return val;
        }

        public bool DeleteFolder(DirectoryInfo dInfo, out Exception Ex)
        {
            bool val;
            try
            {
                dInfo.Delete(true);
                val = true;
                Ex = null;
            }
            catch(Exception XC)
            {
                Ex = XC;
                val = false;
            }

            return val;
        }

        public bool DeleteFolder(string fullPath, out Exception Ex)
        {
            bool val;
            try
            {
                Directory.Delete(fullPath,true);
                val = true;
                Ex = null;
            }
            catch (Exception XC)
            {
                Ex = XC;
                val = false;
            }

            return val;
        }

        public virtual bool DeleteFile(FileInfo fi, out Exception Ex)
        {
            bool val;
            try
            {
                fi.Delete();
                val = true;
                Ex = null;
            }
            catch (Exception XC)
            {
                Ex = XC;
                val = false;
            }

            return val;
        }

        public virtual bool DeleteFile(string fullPath, out Exception Ex)
        {
            bool val;
            try
            {
                File.Delete(fullPath);
                val = true;
                Ex = null;
            }
            catch (Exception XC)
            {
                Ex = XC;
                val = false;
            }
            return val;
        }

        public virtual string Read(int position)
        {
            string retval = null;
            if (position > -1)
            {
                int pos = Files.GetUpperBound(0);

                if (position <= pos)
                {
                    FileInfo fi = Files[position];

                    using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            retval = sr.ReadToEnd();
                        }
                    }
                }
            }

            return retval;
        }

        public virtual string Read(string filename)
        {
            string retval = null;

            FileInfo[] files = _InitialFolder.GetFiles(filename);

            if (files.Length > 0)
            {
                using (FileStream fs = files[0].Open(FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        retval = sr.ReadToEnd();
                    }
                }
            }


            return retval;
        }

        /// <summary>
        /// Reads and returns the content of a file as text
        /// </summary>
        /// <param name="fullFilename">The fully qualified name of the file ( path + name )</param>
        public static string ReadText(string fullFilename)
        {
            InputValidator.ArgumentNullOrEmptyCheck(fullFilename, "fullFilename");

            string retval = null;
            FileInfo fi = new FileInfo(fullFilename);

            if (fi.Exists)
            {
                using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        retval = sr.ReadToEnd();
                    }
                }
            }

            return retval;
        }

        public virtual byte[] ReadBytes(string filename)
        {
            byte[] retval = null;

            FileInfo[] files = _InitialFolder.GetFiles(filename);

            if (files.Length > 0)
            {
                using (FileStream fs = files[0].Open(FileMode.Open, FileAccess.Read, FileShare.Delete| FileShare.Read ))
                {
                    retval = new byte[fs.Length];
                    fs.Read(retval, 0, (int)fs.Length);
                }
            }


            return retval;
        }

        public void CopyTo(string sourceName, string targetName, bool overwrite)
        {
            InputValidator.ArgumentNullOrEmptyCheck(sourceName,"sourceName");
            InputValidator.ArgumentNullOrEmptyCheck(targetName,"targetName");

            FileInfo[] files = _InitialFolder.GetFiles(sourceName);
            if (files.Length > 0)
            {
                File.Copy(files[0].FullName, targetName, overwrite);
            }
        }

        public static void CopyAs(DirectoryInfo sourceDir, DirectoryInfo targetDir, string filter, bool overWrite)
        {
            CopyAs(sourceDir, targetDir, filter, null, overWrite);
        }

        public static void CopyAs(DirectoryInfo sourceDir, DirectoryInfo targetDir, string filter, string newExtension, bool overWrite)
        {
            bool changeExtension = false;
            if (!string.IsNullOrEmpty(newExtension))
            {
                changeExtension = true;
                if (newExtension[0] == '*')
                {
                    newExtension = newExtension.Substring(1);
                }
            }

            FileInfo[] sources = sourceDir.GetFiles(filter, SearchOption.TopDirectoryOnly);

            foreach (FileInfo source in sources)
            {
                string newName = source.Name;
                if (changeExtension)
                {
                    newName = Path.ChangeExtension(source.Name, newExtension);
                }
                string targetFullPath = Path.Combine(targetDir.FullName, newName);
                File.Copy(source.FullName, targetFullPath, overWrite);
            }
 
        }

        public static void WriteStringBuilder(StringBuilder data, string FullNameAndPath)
        {
            string filename = FullNameAndPath;

            FileInfo fi = new FileInfo(filename);
            using (FileStream fs = fi.Open(FileMode.Create, FileAccess.Write, FileShare.Delete | FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.AutoFlush = false;
                    int len = DIMENSION_FILE_CHUNK;
                    int sIdx = 0;
                    int totalLength = data.Length;

                    if (totalLength > 0)
                    {
                        while (sIdx < totalLength)
                        {
                            len = (sIdx + len) <= totalLength ? len : totalLength - sIdx;

                            sw.Write(data.ToString(sIdx, len));
                            sIdx = len + sIdx;

                            sw.Flush();
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Serialize the object then create and write to the specified file.
        /// </summary>
        public static void Write(object data, string fullNameAndPath)
        {
            FileInfo fi = new FileInfo(fullNameAndPath);
            using (FileStream fs = fi.Open(FileMode.Create, FileAccess.Write, FileShare.Delete | FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(data.GetType());

                    sw.AutoFlush = true;
                    xs.Serialize(sw, data);
                }
            }
        }
        /// <summary>
        /// if the FullNameAndPath is provided, the file will be save as such. 
        /// To use the old file name, set the OriginalFilePosition.
        /// To make up a file name, pass-in int.Min value.
        /// </summary>
        /// <param name="OriginalFilePosition"></param>
        public virtual void Write(string data, int OriginalFilePosition, string FullNameAndPath)
        {
            string filename = DeriveFileName(OriginalFilePosition, FullNameAndPath);

            WriteString(data, filename);
        }

        public static void WriteString(string data, string fullNameAndPath)
        {
            FileInfo fi = new FileInfo(fullNameAndPath);
            using (FileStream fs = fi.Open(FileMode.Create, FileAccess.Write, FileShare.Delete | FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.AutoFlush = true;
                    sw.Write(data);
                }
            }
        }

        public virtual void Write(byte[] data, int originalFilePosition, string fullNameAndPath, System.Text.Encoding encoding)
        {
            string filename = DeriveFileName(originalFilePosition, fullNameAndPath);

            FileInfo fi = new FileInfo(filename);

            using (FileStream fs = fi.Open(FileMode.Create, FileAccess.Write, FileShare.Delete | FileShare.Read))
            {
                BinaryWriter bw = null;
                try
                {
                    if (encoding == null)
                    {
                        bw = new BinaryWriter(fs);
                    }
                    else
                    {
                        bw = new BinaryWriter(fs, encoding);
                    }
                    bw.Write(data);
                }
                finally
                {
                    bw?.Dispose();
                }
            }
        }

        public void Write(string data, int originalFilePosition)
        {
            Write(data, originalFilePosition, null);
        }
        
        public void Write(string data)
        {
            Write(data, int.MinValue, null);
        }

        public void Write(byte[] data)
        {
            Write(data, int.MinValue, null,null);
        }

        /// <summary>
        /// Writes data to the folder
        /// </summary>
        /// <param name="data">the data to write to the file</param>
        /// <param name="filename">This is only the file name, don't include the path</param>
        public void Write(string data, string filename)
        {
            Write(data,int.MinValue,Path.Combine(_InitialFolder.FullName, filename));
        }


        /// <summary>
        /// Writes data to the folder
        /// </summary>
        /// <param name="data">the data to write to the file</param>
        /// <param name="filename">This is only the file name, don't include the path</param>
        public void Write(StringBuilder data, string filename)
        {
            WriteStringBuilder(data, Path.Combine(_InitialFolder.FullName, filename));
        }
        
        public void Write(byte[] data, string filename, System.Text.Encoding encoding)
        {
            Write(data, int.MinValue, Path.Combine(_InitialFolder.FullName, filename),encoding);
        }
        #endregion

    }
}
