using System;
using System.Collections.Generic;

namespace Barcode.BarcodeCLI
{
    public class BarcodeCli : IBarcodeCLI
    {
        public event BarcodeEvent CommandEntered;
        
        private bool alive = true;

        public void DisplayUserNotFound(string username)
        {
            Console.WriteLine($"User \"{username}\" not found.");
        }

        public void DisplayProductNotFound(string product)
        {
            Console.WriteLine($"Product \"{product}\" not found.");
        }

        public void DisplayUserInfo(User user, List<Transaction> transactionsForUser)
        {
            Console.WriteLine(user.ToString());
            
            Console.WriteLine($"\n" +
                              $"This user has made {transactionsForUser.Count} " +
                              $"transaction{(transactionsForUser.Count > 1 || transactionsForUser.Count == 0 ? "s" : "")}.");
            
            foreach (Transaction transaction in transactionsForUser)
                Console.WriteLine($"    {transaction}");
        }

        public void DisplayTooManyArgumentsError(string[] command)
        {
            Console.WriteLine($"Too many arguments in command: {string.Join(" ", command)}");
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine($"Command not found: {adminCommand}");
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine(transaction.ToString());
        }

        public void DisplayAddCreditsTransaction(InsertCashTransaction transaction)
        {
            Console.WriteLine(transaction.ToString());
        }

        public void Close()
        {
            alive = false;
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine($"{user.Username} does not not have sufficient credits for {product.Name}.");
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine($"{errorString}");
        }

        public void DisplayUndoCommand(ICommand command)
        {
            Console.WriteLine($"Command \"{command}\" was {(command.Undone ? "" : "not ")}undone.");
        }

        public void DisplayProductActivatedChange(Product product)
        {
            Console.WriteLine($"{product.Name} is now {(product.Active ? "active" : "deactivated")}.");
        }

        public void DisplayProductOnCreditChange(Product product)
        {
            Console.WriteLine(
                $"{product.Name} is now {(product.CanBeBoughtOnCredit ? "able" : "unable")} to be bought on credit.");
        }

        public void DisplayNotEnoughArguments(string[] command)
        {
            Console.WriteLine($"Not enough arguments in command: {string.Join(" ", command)}");
        }

        public void DisplayUserBalanceNotification(User user)
        {
            Console.WriteLine($"User {user.Username} has a low balance: {user.Balance} credits");
        }

        public void DisplayProductLineup(List<Product> productLineup)
        {
            foreach (Product product in productLineup)
                Console.WriteLine(product);
        }

        public void ClearDisplay()
        {
            Console.Clear();
        }

        public void DisplayCommands(Dictionary<string, Action<string[]>> commands)
        {
            Console.WriteLine("Here are the available commands: ");
            foreach (string command in commands.Keys)
                Console.WriteLine($"    {command}");
        }

        public void DisplayCommandLog(List<ICommand> commandsExecuted)
        {
            Console.WriteLine($"The following {commandsExecuted.Count} commands have been executed:\n");
            foreach (ICommand command in commandsExecuted) 
                Console.WriteLine($"    {command}");
        }
        
        public IBarcodeCLI AwaitKeyPress()
        {
            Console.WriteLine("\n" +
                              "Please press a key to continue...");
            Console.ReadKey();

            return this;
        }

        private string GetUserCommandInput() => Console.ReadLine();
        
        public void Start()
        {
            while (alive)
            {
                ClearDisplay();
                
                Console.WriteLine("Welcome to the BarCode system." +
                                  "\n" +
                                  "Write \"help\" to see user commands or \":help\" to see admin commands.");
                
                
                Console.Write(" > ");

                CommandEntered?.Invoke(GetUserCommandInput());

                AwaitKeyPress();
            }
        }
    }
}