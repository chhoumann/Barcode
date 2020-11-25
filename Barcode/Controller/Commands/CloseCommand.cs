using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands
{
    public class CloseCommand : CliCommand
    {
        private IBarcodeCLI barcodeCli;

        public CloseCommand(IBarcodeCLI barcodeCli)
        {
            this.barcodeCli = barcodeCli;
            Succeeded = true;
        }

        public override void Execute()
        {
            barcodeCli.Close();
            base.Execute();
        }
    }
}