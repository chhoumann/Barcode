using System.Collections.Generic;

namespace Barcode.Log
{
    public interface ILog
    {
        List<string> LogEntries { get; }
        void AddLogEntry(object sender);
    }
}