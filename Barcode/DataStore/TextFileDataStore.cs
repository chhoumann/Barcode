using System;
using System.Collections.Generic;
using System.IO;

namespace Barcode.DataStore
{
    public sealed class TextFileDataStore<T> : FileDataStore<T>
    {
        public TextFileDataStore(string directoryName, string fileName) : base(directoryName, fileName)
        {
        }

        public override IEnumerable<T> ReadData()
        {
            if (File.Exists(fullFilePath))
                return File.ReadAllLines(fullFilePath) as IEnumerable<T>;

            throw new FileNotFoundException($"{fileName} not found.");
        }

        public override void AppendData(IEnumerable<T> data)
        {
            if (data is IEnumerable<string> lines)
                File.AppendAllLines(fullFilePath, lines);
            else throw new ArgumentException();
        }
    }
}