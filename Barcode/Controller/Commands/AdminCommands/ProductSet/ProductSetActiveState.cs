using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands.AdminCommands.ProductSet
{
    internal class ProductSetActiveState : ProductSetCommand
    {
        private readonly bool active;

        public ProductSetActiveState(string[] command, bool isActive, IBarcodeCLI barcodeCli,
            IBarcodeSystem barcodeSystem) : base(command, barcodeCli, barcodeSystem)
        {
            active = isActive;
        }

        public override void Execute()
        {
            if (!TryGetProduct()) return;

            product.Active = active;
            barcodeCli.DisplayProductActivatedChange(product);
            Succeeded = true;

            base.Execute();
        }

        public override void Undo()
        {
            if (!Succeeded) return;

            product.Active = !active;
            Undone = true;

            base.Undo();
        }
    }
}