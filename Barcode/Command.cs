using System;

namespace Barcode
{
    public abstract class Command : ICommand
    {
        public static event Action<ICommand> CommandFired;
        public virtual void Execute()
        {
            CommandFired?.Invoke(this);
        }

        public virtual void Undo() { }

        public bool Succeeded { get; private protected set; }
        public bool Undone { get; private protected set; }
    }
}