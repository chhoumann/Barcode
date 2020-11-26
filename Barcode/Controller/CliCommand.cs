namespace Barcode.Controller
{
    public abstract class CliCommand : Command
    {
        private protected string[] Command;

        protected CliCommand(string[] command)
        {
            Command = command;
        }

        protected CliCommand()
        {
        }

        protected bool HasEnoughArguments(int amountOfArgumentsExpected)
        {
            return Command.Length == amountOfArgumentsExpected;
        }

        public override string ToString()
        {
            return string.Join(" ", Command);
        }
    }
}