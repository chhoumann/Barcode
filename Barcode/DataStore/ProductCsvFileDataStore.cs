using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;

namespace Barcode.DataStore
{
    internal class ProductCsvFileDataStore<T> : CsvFileDataStore<T> where T : Product, new()
    {
        public ProductCsvFileDataStore(string directoryName, string fileName, string separator)
            : base(directoryName, fileName, separator)
        {
        }

        public override IEnumerable<T> ReadData()
        {
            return (IEnumerable<T>) File.ReadAllLines(fullFilePath)
                .Skip(1)
                .Select(x => x.Split(separator))
                .Select(dataRow => new Product
                {
                    Id = Convert.ToUInt32(dataRow[0]),
                    Name = Strings.Trim(RemoveCitationMarks(RemoveHtmlTagFromString(dataRow[1]))),
                    Price = Convert.ToDecimal(dataRow[2]),
                    Active = Convert.ToBoolean(Convert.ToInt32(dataRow[3]))
                });
        }

        private string RemoveHtmlTagFromString(string s)
        {
            if (!s.Contains('<')) return s;

            int tagStart = s.IndexOf('<');
            int tagEnd = s.IndexOf('>');

            return RemoveHtmlTagFromString(s.Remove(tagStart, tagEnd - tagStart + 1));
        }

        private string RemoveCitationMarks(string s)
        {
            if (!s.Contains('"')) return s;
            return s.Replace("\"", "");
        }
    }
}