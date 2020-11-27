using System.Collections.Generic;
using System.Linq;
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
                List<Transaction> transactionsForUser = new List<Transaction>();

                transactionsForUser = barcodeSystem
                                        .GetTransactionsForUser(user, 10)
                                        .Where(t => !t.Undone)
                                        .ToList();
                
                barcodeCli.DisplayUserInfo(user, transactionsForUser);
            }
            catch (UserNotFoundException)
            {
                barcodeCli.DisplayUserNotFound(username);
            }

            base.Execute();
        }
    }
}