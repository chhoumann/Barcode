using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Barcode.DataStore;

namespace Barcode.Log
{
    public sealed class FileLog : ILog
    {
        private IDataStore<string> _dataStore;
        public List<string> LogEntries { get; private set; }

        public FileLog()
        {
            _dataStore = new TextFileDataStore<string>("Logs", "log.txt");
            LogEntries = new List<string>();
            LogEntries = _dataStore.ReadData().ToList();
        }

        public void AddLogEntry(object sender)
        {
            LogEntries.Add(sender.ToString());

            _dataStore.AppendData(new[] {sender.ToString()});
        }
    }
}