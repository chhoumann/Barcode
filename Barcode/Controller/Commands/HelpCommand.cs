using System;
using System.Collections.Generic;
using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands
{
    public class HelpCommand : CliCommand
    {
        private readonly Dictionary<string, Action<string[]>> commands;
        private IBarcodeCLI barcodeCli;

        public HelpCommand(Dictionary<string, Action<string[]>> commands, IBarcodeCLI barcodeCli)
        {
            this.commands = commands;
            this.barcodeCli = barcodeCli;
        }
        

        public override void Execute()
        {
            barcodeCli.DisplayCommands(commands);
            
            base.Execute();
        }
    }
}