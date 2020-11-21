namespace Barcode
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}