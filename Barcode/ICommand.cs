namespace Barcode
{
    public interface ICommand
    {
        
        void Execute();
        void Undo();
        bool Succeeded { get; }
        bool Undone { get; }
        string ToString();
    }
}