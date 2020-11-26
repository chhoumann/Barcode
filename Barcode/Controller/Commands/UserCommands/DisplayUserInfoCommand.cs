using Barcode.BarcodeCLI;
using Barcode.Exceptions;

namespace Barcode.Controller.Commands.UserCommands
{
    public class DisplayUserInfoCommand : CliCommand
    {
        private readonly IBarcodeCLI barcodeCli;
        private readonly IBarcodeSystem barcodeSystem;

        public DisplayUserInfoCommand(string[] command, IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem) :
            base(command)
        {
            this.barcodeCli = barcodeCli;
            this.barcodeSystem = barcodeSystem;
        }

        public override void Execute()
        {
            string username = Command[0];

            try
            {
                User user = barcodeSystem.GetUserByUsername(username);
                barcodeCli.DisplayUserInfo(user);
            }
            catch (UserNotFoundException)
            {
                barcodeCli.DisplayUserNotFound(username);
            }

            base.Execute();
        }
    }
}