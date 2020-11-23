using System;
using System.Collections.Generic;
using System.Linq;
using Barcode.BarcodeCLI;
using Barcode.Exceptions;
using Microsoft.VisualBasic;

namespace Barcode.Controller
{
    public class BarcodeController
    {
        private readonly IBarcodeCLI _barcodeCli;
        private readonly IBarcodeSystem _barcodeSystem;
        private Transaction latestTransaction;
        private Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>();
        private Dictionary<string, Action<string[]>> adminCommands = new Dictionary<string, Action<string[]>>();

        public BarcodeController(IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem)
        {
            _barcodeCli = barcodeCli;
            _barcodeSystem = barcodeSystem;
            
            AddCommandsToDictionary(commands);
            AddAdminCommandsToDictionary(adminCommands);
            
            _barcodeCli.CommandEntered += ParseCommand;

            _barcodeCli.Start();
            
        }

        private void ParseCommand(string command)
        {
            string formattedCommand = (Strings.Trim(command)).ToLower();
            
            if (string.IsNullOrEmpty(formattedCommand)) return;
            String[] commandParts = formattedCommand.Split(' ');
            
            if (commands.ContainsKey(commandParts[0]))
                commands[commandParts[0]].Invoke(commandParts);
            else if (adminCommands.ContainsKey(commandParts[0]))
                adminCommands[commandParts[0]].Invoke(commandParts);
            else if (commandParts.Length == 1)
                TryPrintUser(formattedCommand);
            else if (commandParts.Length == 2 || commandParts.Length == 3) 
                TryPurchaseProducts(commandParts);
            else if (commandParts.Length > 3) 
                _barcodeCli.DisplayTooManyArgumentsError(formattedCommand);
        }

        public void TryPurchaseProducts(string[] commandParts)
        {
            string usernameString = commandParts[0];
            string productIdString = commandParts[1];
            int amountToPurchase = 1;
            Product product;
            User user;

            if (commandParts.Length == 3 && !int.TryParse(commandParts[2], out amountToPurchase))
            {
                _barcodeCli.DisplayGeneralError($"{commandParts[2]} is not a valid amount.");
                return;
            }

            try
            {
                user = _barcodeSystem.GetUserByUsername(usernameString);
                if (uint.TryParse(productIdString, out var productId))
                {
                    product = _barcodeSystem.GetProductById(productId);
                    try
                    {
                        var transaction = _barcodeSystem.BuyProduct(user, product, amountToPurchase);

                        latestTransaction = transaction;
                        
                        _barcodeCli.DisplayUserBuysProduct(transaction);
                    }
                    catch (InsufficientCreditsException)
                    {
                        _barcodeCli.DisplayInsufficientCash(user, product);
                    }
                }
                else
                {
                    _barcodeCli.DisplayProductNotFound(productIdString);
                }
            }
            catch (UserNotFoundException)
            {
                _barcodeCli.DisplayUserNotFound(usernameString);
            }
        }

        private void TryPrintUser(string username)
        {
            try
            {
                User user = _barcodeSystem.GetUserByUsername(username);
                _barcodeCli.DisplayUserInfo(user);
            }
            catch (UserNotFoundException e)
            {
                _barcodeCli.DisplayUserNotFound(username);
            }
        }


        private void AddCommandsToDictionary(Dictionary<string, Action<string[]>> commandDictionary)
        {
            commandDictionary["close"] = (command) => { _barcodeCli.Close(); };
            commandDictionary["undo"] = (command) => UndoLatestTransaction();
        }

        private void AddAdminCommandsToDictionary(Dictionary<string, Action<string[]>> adminCommandDictionary)
        {
            adminCommandDictionary[":q"] = (command) => _barcodeCli.Close();
            adminCommandDictionary[":quit"] = (command) => _barcodeCli.Close();

            adminCommandDictionary[":activate"] = command => ProductManager(command, ProductSetable.Active, true);
            adminCommandDictionary[":deactivate"] = command => ProductManager(command, ProductSetable.Active, false);

            adminCommandDictionary[":crediton"] = command => ProductManager(command, ProductSetable.Credit, true);
            adminCommandDictionary[":creditoff"] = command => ProductManager(command, ProductSetable.Credit, false);

            adminCommandDictionary[":addcredits"] = command => AdminAddCreditToUser(command);
        }

        private void AdminAddCreditToUser(string[] command)
        {
            if (!EnoughArgumentsInCommand(command, 3)) return;
            string usernameString = command[1];
            string amountString = command[2];
            
            try
            {
                User user = _barcodeSystem.GetUserByUsername(usernameString);

                if (decimal.TryParse(amountString, out decimal amount))
                {
                    var transaction = _barcodeSystem.AddCreditsToAccount(user, amount);

                    latestTransaction = transaction;
                    
                    _barcodeCli.DisplayAddCreditsTransaction(transaction);
                }
            }
            catch (UserNotFoundException e)
            {
                _barcodeCli.DisplayUserNotFound(usernameString);
                throw;
            }
        }

        enum ProductSetable
        {
            Credit,
            Active
        }

        private void ProductManager(string[] command, ProductSetable set, bool active)
        {
            if (!EnoughArgumentsInCommand(command, 2)) return;
            
            string productIdString = command[1];
            
            if (uint.TryParse(productIdString, out var productId))
            {
                try
                {
                    var product = _barcodeSystem.GetProductById(productId);

                    switch (set)
                    {
                        case ProductSetable.Credit:
                            product.CanBeBoughtOnCredit = active;
                            _barcodeCli.DisplayProductOnCreditChange(product);
                            break;
                        case ProductSetable.Active:
                            product.Active = active;
                            _barcodeCli.DisplayProductActivatedChange(product);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(set), set, null);
                    }

                }
                catch (ProductNotFoundException)
                {
                    _barcodeCli.DisplayProductNotFound(productIdString);
                }
                catch (Exception e)
                {
                    _barcodeCli.DisplayGeneralError(e.ToString());
                }
            }
            else
            {
                _barcodeCli.DisplayProductNotFound(productIdString);
            }
        }

        private bool EnoughArgumentsInCommand(string[] command, int amountOfArgumentsExpected)
        {
            if (command.Length == amountOfArgumentsExpected) return true;
            
            _barcodeCli.DisplayNotEnoughArguments(command);
            return false;

        }

        private void UndoLatestTransaction()
        {
            if (latestTransaction != null)
            {
                latestTransaction.Undo();
                _barcodeCli.DisplayUndoTransaction(latestTransaction);
            }
            else
            {
                _barcodeCli.DisplayGeneralError("There is no transaction to undo.");
            }
        }
    }
}