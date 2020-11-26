using System;
using System.Collections.Generic;
using System.IO;

namespace Barcode.DataStore
{
    public abstract class FileDataStore<T> : IDataStore<T>
    {
        private protected readonly string rootDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
        private protected string directoryName;
        private protected string fileName;
        private protected string directoryPath;
        
        public string fullFilePath { get; }

        public FileDataStore(string directoryName, string fileName)
        {
            this.directoryName = directoryName;
            this.fileName = fileName;
            directoryPath = Path.Combine(rootDirectoryPath, directoryName);
            fullFilePath = Path.Combine(rootDirectoryPath, directoryName, fileName);
        }
        
        public abstract IEnumerable<T> ReadData();

        public virtual void AppendData(IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }

        private protected bool DoesFolderExist() => Directory.Exists(directoryPath);
    }
}