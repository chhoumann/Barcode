using System;

namespace Barcode
{
    public abstract class Transaction : Command, ITransaction, ILoggable<ITransaction>
    {
        private static uint idTracker;
        public static event Action<Transaction> LogTransaction;

        public Transaction(User user, decimal amount)
        {
            User = user;
            Amount = amount;
            Id = idTracker++;
            Date = DateTime.Now;
            Undone = false;
        }

        public uint Id { get; }

        public User User { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public override void Execute()
        {
            LogTransaction?.Invoke(this);
        }

        public override void Undo()
        {
            LogTransaction?.Invoke(this);
        }
    }
}