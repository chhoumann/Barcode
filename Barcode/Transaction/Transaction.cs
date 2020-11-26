using System;

namespace Barcode
{
    public abstract class Transaction : Command, ITransaction, ILoggable<ITransaction>
    {
        private static uint _idTracker;

        public Transaction(User user, decimal amount)
        {
            User = user;
            Amount = amount;
            Id = _idTracker++;
            Date = DateTime.Now;
            Undone = false;
        }

        public uint Id { get; }

        public User User { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool Succeeded { get; protected set; }
        public bool Undone { get; protected set; }

        public override string ToString()
        {
            return "";
        }

        public static event Action<Transaction> LogTransaction;

        public virtual void Execute()
        {
            LogTransaction?.Invoke(this);
        }

        public virtual void Undo()
        {
            LogTransaction?.Invoke(this);
        }
    }
}