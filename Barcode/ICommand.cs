namespace Barcode
{
    public interface ICommand
    {
        bool Succeeded { get; }
        bool Undone { get; }

        void Execute();
        void Undo();
        string ToString();
    }
}