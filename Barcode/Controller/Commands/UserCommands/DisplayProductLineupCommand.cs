using System.Collections.Generic;
using System.Linq;
using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands.UserCommands
{
    public class DisplayProductLineupCommand : CliCommand
    {
        private readonly IBarcodeCLI barcodeCli;
        private readonly IBarcodeSystem barcodeSystem;

        public DisplayProductLineupCommand(string[] command, IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem)
            : base(command)
        {
            this.barcodeCli = barcodeCli;
            this.barcodeSystem = barcodeSystem;
        }

        public override void Execute()
        {
            List<Product> activeProducts = barcodeSystem.Products.Where(product => product.Active).ToList();

            barcodeCli.DisplayProductLineup(activeProducts);
            
            base.Execute();
        }
    }
}