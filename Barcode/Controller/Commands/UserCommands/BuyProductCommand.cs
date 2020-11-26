using Barcode.BarcodeCLI;
using Barcode.Exceptions;

namespace Barcode.Controller.Commands.UserCommands
{
    public class BuyProductCommand : CliCommand
    {
        private readonly IBarcodeCLI barcodeCli;
        private readonly IBarcodeSystem barcodeSystem;
        private BuyTransaction transaction;

        public BuyProductCommand(string[] command, IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem) : base(command)
        {
            this.barcodeCli = barcodeCli;
            this.barcodeSystem = barcodeSystem;
        }

        public override void Execute()
        {
            if (!HasEnoughArguments(2)) return;

            string username = Command[0];
            string productIdString = Command[1];
            int amountToPurchase = 1;
            Product product;
            User user;

            if (Command.Length == 3 && !int.TryParse(Command[2], out amountToPurchase))
            {
                barcodeCli.DisplayGeneralError($"{Command[2]} is not a valid amount.");
                return;
            }

            try
            {
                user = barcodeSystem.GetUserByUsername(username);
                if (uint.TryParse(productIdString, out uint productId))
                    try
                    {
                        product = barcodeSystem.GetProductById(productId);
                        try
                        {
                            BuyTransaction transaction = barcodeSystem.BuyProduct(user, product, amountToPurchase);

                            this.transaction = transaction;

                            barcodeCli.DisplayUserBuysProduct(transaction);
                        }
                        catch (InsufficientCreditsException)
                        {
                            barcodeCli.DisplayInsufficientCash(user, product);
                        }
                    }
                    catch (ProductNotFoundException)
                    {
                        barcodeCli.DisplayProductNotFound(productIdString);
                    }
            }
            catch (UserNotFoundException)
            {
                barcodeCli.DisplayUserNotFound(username);
            }

            Succeeded = transaction.Succeeded;
            base.Execute();
        }

        public override void Undo()
        {
            if (!Succeeded) return;

            transaction.Undo();
            Undone = transaction.Undone;

            base.Undo();
        }
    }
}