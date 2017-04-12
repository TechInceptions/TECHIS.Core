using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace TECHIS.Core.IO
{
    public class FileTransferInfo
    {
        private FileInfo _File;
        private TransferOptions _TransferOption;

        public TransferOptions TransferOption
        {
            get { return _TransferOption; }
            set { _TransferOption = value; }
        }

        public FileInfo File
        {
            get { return _File; }
            set { _File = value; }
        }

        public FileTransferInfo(FileInfo file, TransferOptions transferOption)
        {
            InputValidator.ArgumentNullCheck(file,"file");

            _File = file;
            _TransferOption = transferOption;
        }
    }
}
