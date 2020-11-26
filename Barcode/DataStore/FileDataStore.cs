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

        public FileDataStore(string directoryName, string fileName)
        {
            this.directoryName = directoryName;
            this.fileName = fileName;
            fullFilePath = Path.Combine(rootDirectoryPath, directoryName, fileName);
            CreateFile();
        }

        public string fullFilePath { get; }

        public virtual IEnumerable<T> ReadData()
        {
            throw new NotImplementedException();
        }

        public virtual void AppendData(IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }

        private protected void CreateFile()
        {
            string directoryPath = Path.Combine(rootDirectoryPath, directoryName);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (!File.Exists(fullFilePath))
                File.Create(fullFilePath);
        }
    }
}