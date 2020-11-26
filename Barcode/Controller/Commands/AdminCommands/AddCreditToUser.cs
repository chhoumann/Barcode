using Barcode.BarcodeCLI;
using Barcode.Exceptions;

namespace Barcode.Controller.Commands.AdminCommands
{
    public class AddCreditToUser : CliCommand
    {
        private readonly IBarcodeCLI barcodeCli;
        private readonly IBarcodeSystem barcodeSystem;

        private InsertCashTransaction transaction;

        public AddCreditToUser(string[] command, IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem) : base(command)
        {
            this.barcodeCli = barcodeCli;
            this.barcodeSystem = barcodeSystem;
        }

        public override void Execute()
        {
            if (!HasEnoughArguments(3))
            {
                barcodeCli.DisplayNotEnoughArguments(Command);
                return;
            }

            string username = Command[1];
            string amountString = Command[2];

            try
            {
                User user = barcodeSystem.GetUserByUsername(username);

                if (decimal.TryParse(amountString, out decimal amount))
                {
                    transaction = barcodeSystem.AddCreditsToAccount(user, amount);

                    barcodeCli.DisplayAddCreditsTransaction(transaction);

                    Succeeded = true;
                }
            }
            catch (UserNotFoundException)
            {
                barcodeCli.DisplayUserNotFound(username);
            }

            base.Execute();
        }

        public override void Undo()
        {
            barcodeSystem.UndoTransaction(transaction);
            Undone = transaction.Undone;
        }
    }
}