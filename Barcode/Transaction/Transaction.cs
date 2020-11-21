using System;

namespace Barcode
{
    public abstract class Transaction : ITransaction, ICommand, ILoggable<ITransaction>
    {
        private static uint _idTracker = 0;
        private uint _id;

        public uint Id
        {
            get => _id;
        }

        public User User { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool Succeeded { get; protected set; }
        public bool Undone { get; protected set; }
        public static event Action<Transaction> LogCommand;

        public Transaction(User user, decimal amount)
        {
            User = user;
            Amount = amount;
            _id = _idTracker++;
            Date = DateTime.Now;
            Undone = false;
        }

        public override string ToString()
        {
            return "";
        }

        public virtual void Execute()
        {
            LogCommand?.Invoke(this);
        }

        public virtual void Undo()
        {
            LogCommand?.Invoke(this);
        }
    }
}