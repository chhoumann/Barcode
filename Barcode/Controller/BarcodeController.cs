using System;
using System.Collections.Generic;
using Barcode.BarcodeCLI;
using Barcode.Controller.Commands;
using Barcode.Controller.Commands.AdminCommands;
using Barcode.Controller.Commands.UserCommands;
using Barcode.Controller.Commands.UserCommands.ProductSet;
using Barcode.Exceptions;
using Microsoft.VisualBasic;

namespace Barcode.Controller
{
    public class BarcodeController
    {
        private readonly Dictionary<string, Action<string[]>>
            adminCommands = new Dictionary<string, Action<string[]>>();

        private readonly IBarcodeCLI barcodeCli;
        private readonly IBarcodeSystem barcodeSystem;
        private readonly List<ICommand> commandsExecuted = new List<ICommand>();
        private readonly Dictionary<string, Action<string[]>> userCommands = new Dictionary<string, Action<string[]>>();

        public BarcodeController(IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem)
        {
            this.barcodeCli = barcodeCli;
            this.barcodeSystem = barcodeSystem;

            AddUserCommandsToDictionary(userCommands);
            AddAdminCommandsToDictionary(adminCommands);

            this.barcodeCli.CommandEntered += ParseCommand;
            User.UserBalanceNotification += barcodeCli.DisplayUserBalanceNotification;
            Command.CommandFired += SetLastCommandFired;

            this.barcodeCli.Start();
        }

        private void SetLastCommandFired(ICommand command)
        {
            commandsExecuted.Add(command);
        }

        private void ParseCommand(string inputCommand)
        {
            string formattedCommand = Strings.Trim(inputCommand).ToLower();

            if (string.IsNullOrEmpty(formattedCommand)) return;
            string[] command = formattedCommand.Split(' ');

            if (formattedCommand.StartsWith(':'))
                TryExecuteAdminCommand(command);
            else
                TryExecuteUserCommand(command);
        }

        private void TryExecuteUserCommand(string[] command)
        {
            bool isUserCommand = userCommands.ContainsKey(command[0]);

            if (isUserCommand)
                userCommands[command[0]]?.Invoke(command);
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
            bool isAdminCommand = adminCommands.ContainsKey(command[0]);

            if (isAdminCommand)
                adminCommands[command[0]]?.Invoke(command);
            else
                barcodeCli.DisplayAdminCommandNotFoundMessage(string.Join(" ", command));
        }

        private void TryPrintUser(string username)
        {
            try
            {
                User user = barcodeSystem.GetUserByUsername(username);
                barcodeCli.DisplayUserInfo(user);
            }
            catch (UserNotFoundException)
            {
                barcodeCli.DisplayUserNotFound(username);
            }
        }


        private void AddUserCommandsToDictionary(Dictionary<string, Action<string[]>> commandDictionary)
        {
            commandDictionary["close"] = command => new CloseCommand(barcodeCli).Execute();
            commandDictionary["undo"] = command => UndoLastCommand();
            commandDictionary["redo"] = command => RedoLastCommand();
        }

        private void AddAdminCommandsToDictionary(Dictionary<string, Action<string[]>> adminCommandDictionary)
        {
            adminCommandDictionary[":q"] = command => new
                CloseCommand(barcodeCli).Execute();
            adminCommandDictionary[":quit"] = command => new
                CloseCommand(barcodeCli).Execute();

            adminCommandDictionary[":activate"] = command => new
                ProductSetActiveState(command, true, barcodeCli, barcodeSystem).Execute();
            adminCommandDictionary[":deactivate"] = command => new
                ProductSetActiveState(command, false, barcodeCli, barcodeSystem).Execute();

            adminCommandDictionary[":crediton"] = command => new
                ProductSetCreditState(command, true, barcodeCli, barcodeSystem).Execute();
            adminCommandDictionary[":creditoff"] = command =>
                new ProductSetCreditState(command, false, barcodeCli, barcodeSystem).Execute();

            adminCommandDictionary[":addcredits"] = command => new
                AddCreditToUser(command, barcodeCli, barcodeSystem).Execute();
        }

        private void UndoLastCommand()
        {
            try
            {
                ICommand lastCommandExecuted =
                    commandsExecuted.FindLast(command => command.Undone == false && command.Succeeded);

                lastCommandExecuted?.Undo();
                barcodeCli.DisplayUndoCommand(lastCommandExecuted);
            }
            catch
            {
                barcodeCli.DisplayGeneralError("There is no command to undo.");
            }
        }

        private void RedoLastCommand()
        {
            try
            {
                ICommand lastUndoneCommand =
                    commandsExecuted.FindLast(command => command.Undone && command.Succeeded);

                lastUndoneCommand?.Execute();
            }
            catch
            {
                barcodeCli.DisplayGeneralError("There is no command to redo.");
            }
        }
    }
}