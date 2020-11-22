using System;

namespace Barcode.BarcodeCLI
{
    public class BarcodeCLI : IBarcodeCLI
    {
        private IBarcodeSystem _barcodeSystem;
        private bool _alive = true;
        
        public BarcodeCLI(IBarcodeSystem barcodeSystem)
        {
            _barcodeSystem = barcodeSystem;
        }

        public void DisplayUserNotFound(string username)
        {
            Console.WriteLine($"User \"{username}\" not found.");
        }

        public void DisplayProductNotFound(string product)
        {
            Console.WriteLine($"Product \"{product}\" not found.");
        }

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine(user.ToString());
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine($"Too many arguments in command: {command}");
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine($"Command not found: {adminCommand}");
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine(transaction.ToString());
        }

        public void Close()
        {
            _alive = false;
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine($"{user.Username} does not not have sufficient credits for {product.Name}.");
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine($"{errorString}");
        }

        public void DisplayUndoTransaction(Transaction transaction)
        {
            if (transaction.Undone)
                Console.WriteLine(transaction.ToString());
            else DisplayGeneralError($"Transaction #{transaction.Id} was not undone.");
        }

        public void Start()
        {
            Console.WriteLine($"Welcome to the BarCode system.");
            while (_alive)
            {
                CommandEntered?.Invoke(Console.ReadLine());
            }
        }

        public event BarcodeEvent CommandEntered;
    }
}