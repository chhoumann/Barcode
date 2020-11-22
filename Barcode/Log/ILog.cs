using System.Collections.Generic;

namespace Barcode.Log
{
    public interface ILog
    {
        void AddLogEntry(object sender);
        List<string> LogEntries { get; }
    }
}