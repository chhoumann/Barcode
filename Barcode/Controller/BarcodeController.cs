using System;
using System.Collections.Generic;
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
        private static Dictionary<string, Action> commands = new Dictionary<string, Action>();

        public BarcodeController(IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem)
        {
            _barcodeCli = barcodeCli;
            _barcodeSystem = barcodeSystem;
            AddCommandsToDictionary();
            _barcodeCli.CommandEntered += ParseCommand;
            
            _barcodeCli.Start();
            
        }

        private void ParseCommand(string command)
        {
            string formattedCommand = (Strings.Trim(command)).ToLower();
            
            if (string.IsNullOrEmpty(formattedCommand)) return;
            String[] commandParts = formattedCommand.Split(' ');
            
            if (commands.ContainsKey(commandParts[0]))
                commands[formattedCommand].Invoke();
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


        private void AddCommandsToDictionary()
        {
            commands["close"] = () => { _barcodeCli.Close(); };
            commands["undo"] = UndoLatestTransaction;
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