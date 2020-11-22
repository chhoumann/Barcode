using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Barcode.DataStore
{
    public abstract class CsvFileDataStore<T> : FileDataStore<T> where T : class, new()
    {
        private protected string separator;

        public CsvFileDataStore(string directoryName, string fileName, string separator) : base(directoryName, fileName)
        {
            this.separator = separator;
        }
    }

    class UserCsvFileDataStore<T> : CsvFileDataStore<T> where T : User, new()
    {
        public UserCsvFileDataStore(string directoryName, string fileName, string separator) : base(directoryName,
            fileName, separator)
        {
        }

        public override IEnumerable<T> ReadData()
        {
            return (IEnumerable<T>) File.ReadAllLines(fullFilePath)
                .Skip(1)
                .Select(x => x.Split(separator))
                .Select(dataRow => new User(dataRow[1], dataRow[2], dataRow[3], dataRow[5])
                {
                    Id = Convert.ToUInt32(dataRow[0]),
                    Balance = Convert.ToDecimal(dataRow[4])
                });
        }
    }
}