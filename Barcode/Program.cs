using Barcode.BarcodeCLI;
using Barcode.Controller;
using Barcode.DataStore;
using Barcode.Log;

namespace Barcode
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog log = new FileLog();
            IBarcodeSystem barcodeSystem = new BarcodeSystem(log)
                .AddProductDataStore(new ProductCsvFileDataStore<Product>("Data", "products.csv", ";"))
                .AddUserDataStore(new UserCsvFileDataStore<User>("Data", "users.csv", ","));
            
            IBarcodeCLI barcodeCli = new BarcodeCLI.BarcodeCLI(barcodeSystem);
            BarcodeController barcodeController = new BarcodeController(barcodeCli, barcodeSystem);
        }
    }
}
