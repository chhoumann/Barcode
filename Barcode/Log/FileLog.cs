using System.Collections.Generic;
using System.Linq;
using Barcode.DataStore;

namespace Barcode.Log
{
    public sealed class FileLog : ILog
    {
        private readonly IDataStore<string> _dataStore;

        public FileLog()
        {
            _dataStore = new TextFileDataStore<string>("Logs", "log.txt");
            LogEntries = new List<string>();
            LogEntries = _dataStore.ReadData().ToList();
        }

        public List<string> LogEntries { get; }

        public void AddLogEntry(object sender)
        {
            LogEntries.Add(sender.ToString());

            _dataStore.AppendData(new[] {sender.ToString()});
        }
    }
}