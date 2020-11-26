namespace Barcode.BarcodeCLI
{
    public interface IBarcodeCLI
    {
        void DisplayUserNotFound(string username);
        void DisplayProductNotFound(string product);
        void DisplayUserInfo(User user);
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
    }
}