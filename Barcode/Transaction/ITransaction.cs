using System;

namespace Barcode
{
    public interface ITransaction
    {
        uint Id { get; }
        User User { get; set; }
        DateTime Date { get; set; }
        decimal Amount { get; set; }
        bool Succeeded { get; }
        bool Undone { get; }

        string ToString();
    }
}