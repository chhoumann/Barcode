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
}