using System;

namespace Barcode
{
    public interface ITransaction
    {
        uint Id { get; }
        User User { get; set; }
        DateTime Date { get; set; }
        decimal Amount { get; set; }
        public bool Succeeded { get; }
        public bool Undone { get; }

        string ToString();
    }
}