using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barcode
{
    public interface ILog
    {
        void AddLogEntry(object sender);
        List<string> LogEntries { get; }
    }
}