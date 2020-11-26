using System.Collections.Generic;

namespace Barcode.DataStore
{
    public interface IDataStore<T>
    {
        IEnumerable<T> ReadData();
        void AppendData(IEnumerable<T> data);
    }
}