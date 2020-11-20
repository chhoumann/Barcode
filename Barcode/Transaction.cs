using System;

namespace Barcode
{
    public class Transaction
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(User)}: {User}, {nameof(Date)}: {Date}, {nameof(Amount)}: {Amount}";
        }
        
        // TODO: Command pattern
        void Execute() {}
    }
}