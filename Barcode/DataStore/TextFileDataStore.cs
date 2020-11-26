using System;
using System.Collections.Generic;
using System.IO;

namespace Barcode.DataStore
{
    public sealed class TextFileDataStore<T> : FileDataStore<T>
    {
        public TextFileDataStore(string directoryName, string fileName) : base(directoryName, fileName) { }

        public override IEnumerable<T> ReadData()
        {
            if (File.Exists(fullFilePath))
                return File.ReadAllLines(fullFilePath) as IEnumerable<T>;
            return new List<T>();
        }

        public override void AppendData(IEnumerable<T> data)
        {
            if (!DoesFolderExist())
                Directory.CreateDirectory(directoryPath);
            
            if (data is IEnumerable<string> lines)
                File.AppendAllLines(fullFilePath, lines);
            else throw new ArgumentException();
        }
    }
}