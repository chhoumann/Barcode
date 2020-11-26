using System;
using System.Collections.Generic;
using System.Linq;
using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands.AdminCommands
{
    public class DisplayCommandLogCommand : CliCommand
    {
        private readonly List<ICommand> commandsEntered;
        private readonly IBarcodeCLI barcodeCli;

        public DisplayCommandLogCommand(List<ICommand> commandsEntered, IBarcodeCLI barcodeCli)
        {
            this.commandsEntered = commandsEntered;
            this.barcodeCli = barcodeCli;
        }

        public override void Execute()
        {
            try
            {
                List<ICommand> commands = commandsEntered.Where(command => command.Succeeded).ToList();
                barcodeCli.DisplayCommandLog(commands);
            }
            catch (Exception e)
            {
                barcodeCli.DisplayGeneralError(e.ToString());
            }
            
            base.Execute();
        }
    }
}