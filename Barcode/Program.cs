using System;
using Barcode.BarcodeCLI;
using Barcode.Controller;
using Barcode.DataStore;
using Barcode.Log;

namespace Barcode
{
    internal class Program
    {
        private static void Main()
        {
            ILog log = new FileLog();
            IBarcodeCLI barcodeCli = new BarcodeCli();
            IBarcodeSystem barcodeSystem;
            BarcodeController barcodeController;
            
            try
            {
                 barcodeSystem = new BarcodeSystem(log, barcodeCli)
                     .AddProductDataStore(new ProductCsvFileDataStore<Product>("Data", "products.csv", ";"))
                     .AddUserDataStore(new UserCsvFileDataStore<User>("Data", "users.csv", ","));

                 new BarcodeController(barcodeCli, barcodeSystem).Start();
            }
            catch (Exception e)
            {
                barcodeCli.DisplayGeneralError(e.ToString());
            }
        }
    }
}