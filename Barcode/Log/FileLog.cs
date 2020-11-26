using System.Collections.Generic;
using System.Linq;
using Barcode.DataStore;

namespace Barcode.Log
{
    public sealed class FileLog : ILog
    {
        private readonly IDataStore<string> dataStore;

        public FileLog()
        {
            dataStore = new TextFileDataStore<string>("Logs", "log.txt");

            LogEntries = dataStore.ReadData().ToList();
        }

        public List<string> LogEntries { get; }

        public void AddLogEntry(object sender)
        {
            LogEntries.Add(sender.ToString());

            dataStore.AppendData(new[] {sender.ToString()});
        }
    }
}