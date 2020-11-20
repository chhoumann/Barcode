namespace Barcode.Transaction
{
    public class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(User.User user, decimal amount) : base(user, amount) { }

        public override void Execute()
        {
            User.Balance += Amount;
            Succeeded = true;
        }

        public override void Undo()
        {
            if (!Succeeded) return;
            User.Balance -= Amount;
            Undone = true;
        }

        public override string ToString() => Undone ? "" : $"{Date.Day} - #{Id} | Insert {Amount} into {User.Username}'s balance. New balance: {User.Balance}";
    }
}