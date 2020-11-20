using System;

namespace Barcode
{
    public abstract class Transaction 
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

        public Transaction(User user, decimal amount)
        {
            User = user;
            Amount = amount;
            _id = _idTracker++;
            Date = DateTime.Now;
            Undone = false;
        }
        
    }
}