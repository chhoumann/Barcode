using System.Collections.Generic;
using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands.AdminCommands
{
    public class UndoCommand : CliCommand
    {
        private readonly List<ICommand> commandsExecuted;
        private readonly IBarcodeCLI barcodeCli;
        private ICommand CommandToUndo;
        
        public UndoCommand(string[] command, List<ICommand> commandsExecuted, IBarcodeCLI barcodeCli) : base(command)
        {
            this.commandsExecuted = commandsExecuted;
            this.barcodeCli = barcodeCli;
        }

        public override void Execute()
        {
            try
            {
                CommandToUndo = commandsExecuted.FindLast(command => !command.Undone && command.Succeeded);

                CommandToUndo?.Undo();
                Succeeded = true;

                barcodeCli.DisplayUndoCommand(CommandToUndo);
            }
            catch
            {
                barcodeCli.DisplayGeneralError("There is no command to undo.");
            }
            
            base.Execute();
        }
    }
}