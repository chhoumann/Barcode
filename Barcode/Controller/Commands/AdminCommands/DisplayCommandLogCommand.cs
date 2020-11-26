using System;
using System.Collections.Generic;
using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands.AdminCommands
{
    public class DisplayCommandLogCommand : CliCommand
    {
        private readonly List<ICommand> commandsExecuted;
        private readonly IBarcodeCLI barcodeCli;

        public DisplayCommandLogCommand(List<ICommand> commandsExecuted, IBarcodeCLI barcodeCli)
        {
            this.commandsExecuted = commandsExecuted;
            this.barcodeCli = barcodeCli;
        }

        public override void Execute()
        {
            try
            {
                barcodeCli.DisplayCommandLog(commandsExecuted);
            }
            catch (Exception e)
            {
                barcodeCli.DisplayGeneralError(e.ToString());
            }
            
            base.Execute();
        }
    }
}