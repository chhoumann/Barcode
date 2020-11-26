using System.Collections.Generic;
using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands.AdminCommands
{
    public class DisplayAllProductsCommand : CliCommand
    {
        private readonly IBarcodeCLI barcodeCli;
        private readonly IBarcodeSystem barcodeSystem;

        public DisplayAllProductsCommand(string[] command, IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem)
            : base(command)
        {
            this.barcodeCli = barcodeCli;
            this.barcodeSystem = barcodeSystem;
        }

        public override void Execute()
        {
            List<Product> allProducts = barcodeSystem.Products;

            barcodeCli.DisplayProductLineup(allProducts);
            
            base.Execute();
        }
    }
}