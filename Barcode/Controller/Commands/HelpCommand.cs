using System;
using System.Collections.Generic;
using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands
{
    public class HelpCommand : CliCommand
    {
        private readonly List<(string command, string syntax, Action<string[]>)> commands;
        private IBarcodeCLI barcodeCli;

        public HelpCommand(List<(string command, string syntax, Action<string[]>)> commands, IBarcodeCLI barcodeCli)
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