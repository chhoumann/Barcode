using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands.AdminCommands
{
    public class ClearCommand : CliCommand
    {
        private readonly IBarcodeCLI barcodeCli;

        public ClearCommand(IBarcodeCLI barcodeCli)
        {
            this.barcodeCli = barcodeCli;
        }
        
        public override void Execute()
        {
            barcodeCli.ClearDisplay();    
            
            base.Execute();
        }
    }
}