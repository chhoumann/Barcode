using System;
using System.Collections.Generic;

namespace Barcode.BarcodeCLI
{
    public interface IBarcodeCLI
    {
        void DisplayUserNotFound(string username);
        void DisplayProductNotFound(string product);
        void DisplayTooManyArgumentsError(string[] command);
        void DisplayAdminCommandNotFoundMessage(string adminCommand);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void Close();
        void DisplayInsufficientCash(User user, Product product);
        void DisplayGeneralError(string errorString);
        void Start();
        event BarcodeEvent CommandEntered;
        void DisplayUndoCommand(ICommand command);
        void DisplayProductActivatedChange(Product product);
        void DisplayProductOnCreditChange(Product product);
        void DisplayNotEnoughArguments(string[] command);
        void DisplayAddCreditsTransaction(InsertCashTransaction transaction);
        void DisplayUserBalanceNotification(User user);
        void DisplayProductLineup(List<Product> productLineup);
        void DisplayCommands(List<(string command, string syntax, Action<string[]>)> commands);
        void DisplayCommandLog(List<ICommand> commandsExecuted);
        IBarcodeCLI AwaitKeyPress();
        void DisplayUserInfo(User user, List<Transaction> transactionsForUser);
    }
}