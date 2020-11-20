namespace Barcode
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        public bool Undone { get; }
    }
}