using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barcode.DataStore
{
    public interface IDataStore<T>
    {
        IEnumerable<T> ReadData();
        void AppendData(IEnumerable<T> data);
    }
}