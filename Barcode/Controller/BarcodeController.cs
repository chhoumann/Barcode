using System;
using System.Collections.Generic;
using System.Linq;
using Barcode.BarcodeCLI;
using Barcode.Controller.Commands;
using Barcode.Controller.Commands.AdminCommands;
using Barcode.Controller.Commands.AdminCommands.ProductSet;
using Barcode.Controller.Commands.UserCommands;
using Microsoft.VisualBasic;
using static System.String;

namespace Barcode.Controller
{
    public class BarcodeController
    {
        private readonly IBarcodeCLI barcodeCli;
        private readonly IBarcodeSystem barcodeSystem;
        private readonly List<ICommand> commandsExecuted = new List<ICommand>();
        
        private List<(string Command, string Syntax, Action<string[]> Action)> userCommands = new List<(string Command, string Syntax, Action<string[]> Action)>();
        private List<(string Command, string Syntax, Action<string[]> Action)> adminCommands = new List<(string Command, string Syntax, Action<string[]> Action)>();

        public BarcodeController(IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem)
        {
            this.barcodeCli = barcodeCli;
            this.barcodeSystem = barcodeSystem;

            AddUserCommands(userCommands);
            AddAdminCommands(adminCommands);

            this.barcodeCli.CommandEntered += ParseCommand;
            User.UserBalanceNotification += barcodeCli.DisplayUserBalanceNotification;
            Command.CommandFired += SetLastCommandFired;
        }

        public void Start() => barcodeCli.Start();

        private void SetLastCommandFired(ICommand command) => commandsExecuted.Add(command);

        private void ParseCommand(string inputCommand)
        {
            string formattedCommand = Strings.Trim(inputCommand).ToLower();

            if (IsNullOrEmpty(formattedCommand))
            {
                barcodeCli.DisplayGeneralError("No command entered. Please enter a command.");
                return;
            }

            string[] command = formattedCommand.Split(' ');

            if (formattedCommand.StartsWith(':'))
                TryExecuteAdminCommand(command);
            else
                TryExecuteUserCommand(command);
        }

        private void TryExecuteUserCommand(string[] command)
        {
            bool isUserCommand = userCommands.Any(item => item.Command == command[0]);

            if (isUserCommand)
                userCommands.Find(item => item.Command == command[0]).Action(command);
            else
                switch (command.Length)
                {
                    case 1:
                        new DisplayUserInfoCommand(command, barcodeCli, barcodeSystem).Execute();
                        break;
                    case 2:
                    case 3:
                        new BuyProductCommand(command, barcodeCli, barcodeSystem).Execute();
                        break;
                    default:
                        barcodeCli.DisplayTooManyArgumentsError(command);
                        break;
                }
        }

        private void TryExecuteAdminCommand(string[] command)
        {
            bool isAdminCommand = adminCommands.Any(item => item.Command == command[0]);

            if (isAdminCommand)
                adminCommands.Find(item => item.Command == command[0]).Action(command);
            else
                barcodeCli.DisplayAdminCommandNotFoundMessage(Join(" ", command));
        }

        private void AddUserCommands(List<(string command, string syntax, Action<string[]>)> userCommandList)
        {
            userCommandList.Add(("[USERNAME]", "(displays user information for user with given username)",
                command => barcodeCli.DisplayGeneralError("Please enter a command.")));
            
            userCommandList.Add(("[USERNAME] [PRODUCT_ID] (amount)",
                "(purchase product for given user with given product Id and - optionally - amount)",
                command => barcodeCli.DisplayGeneralError("Please enter a command")));
            
            userCommandList.Add(("close", "no parameters (closes the program)", command => new CloseCommand(barcodeCli).Execute()));

            userCommandList.Add(("help", "no parameters (displays available commands)",
                command => new HelpCommand(userCommands, barcodeCli).Execute()));

            userCommandList.Add(("products", "no parameters (shows active products)",
                command => new DisplayProductLineupCommand(command, barcodeCli, barcodeSystem).Execute()));
        }

        private void AddAdminCommands(List<(string command, string syntax, Action<string[]>)> adminCommandList)
        {
            adminCommandList.Add((":q", "no parameters (closes the program)", command => new CloseCommand(barcodeCli).Execute()));
            adminCommandList.Add((":quit", "no parameters (closes the program)", command => new CloseCommand(barcodeCli).Execute()));
            
            adminCommandList.Add((":activate", "[PRODUCT_ID] (activates product with given id)",
                command => new ProductSetActiveState(command, true, barcodeCli, barcodeSystem).Execute()));
            adminCommandList.Add((":deactivate", "[PRODUCT_ID] (deactivates product with given id",
                command => new ProductSetActiveState(command, false, barcodeCli, barcodeSystem).Execute()));
            
            adminCommandList.Add((":crediton",
                "[PRODUCT_ID] (allows product to be purchased with insufficient credits)",
                command => new ProductSetCreditState(command, true, barcodeCli, barcodeSystem).Execute()));
            adminCommandList.Add((":creditoff",
                "[PRODUCT_ID] (disallows product from being purchased with insufficient credits)",
                command => new ProductSetCreditState(command, false, barcodeCli, barcodeSystem).Execute()));

            adminCommandList.Add((":addcredits",
                "[USERNAME] [CREDITS_TO_ADD] (adds given amount of credits to user with specified username)",
                command => new AddCreditToUser(command, barcodeCli, barcodeSystem).Execute()));
            
            adminCommandList.Add((":allproducts", "no parameters (displays all products - even inactive products)",
                command => new DisplayAllProductsCommand(command, barcodeCli, barcodeSystem).Execute()));
            
            adminCommandList.Add((":undo", "no parameters (undoes last command)",
                command => new UndoCommand(command, commandsExecuted, barcodeCli).Execute()));

            adminCommandList.Add((":help", "no parameters (displays admin commands)",
                command => new HelpCommand(adminCommands, barcodeCli).Execute()));

            adminCommandList.Add((":commandlog", "no parameters (displays executed commands)",
                command => new DisplayCommandLogCommand(commandsExecuted, barcodeCli).Execute()));
        }
    }
}