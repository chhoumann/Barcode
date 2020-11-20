using System;

namespace Barcode.Transaction
{
    public abstract class Transaction : ICommand
    {
        private static uint _idTracker = 0;
        private uint _id;

        public uint Id
        {
            get => _id;
        }

        public User.User User { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        
        public bool Succeeded { get; protected set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(User)}: {User}, {nameof(Date)}: {Date}, {nameof(Amount)}: {Amount}";
        }

        public Transaction(User.User user, decimal amount)
        {
            User = user;
            Amount = amount;
            _id = _idTracker++;
            Date = DateTime.Now;
            Undone = false;
        }
        
        public virtual void Execute() { }
        public virtual void Undo() { }
        public bool Undone { get; protected set; }
    }
}