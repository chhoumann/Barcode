using System;
using Barcode.BarcodeCLI;
using Barcode.Exceptions;

namespace Barcode.Controller.Commands.UserCommands.ProductSet
{
    public abstract class ProductSetCommand : CliCommand
    {
        private protected IBarcodeCLI barcodeCli;
        private protected IBarcodeSystem barcodeSystem;
        private protected Product product;

        protected ProductSetCommand(string[] command, IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem) :
            base(command)
        {
            this.barcodeCli = barcodeCli;
            this.barcodeSystem = barcodeSystem;
        }

        protected bool TryGetProduct()
        {
            if (!HasEnoughArguments(2))
            {
                barcodeCli.DisplayNotEnoughArguments(Command);
                return false;
            }

            string productIdString = Command[1];

            try
            {
                uint productId = Convert.ToUInt32(productIdString);
                product = barcodeSystem.GetProductById(productId);
            }
            catch (ProductNotFoundException)
            {
                barcodeCli.DisplayProductNotFound(productIdString);
            }
            catch (Exception e)
            {
                barcodeCli.DisplayGeneralError(e.ToString());
            }

            return true;
        }
    }
}