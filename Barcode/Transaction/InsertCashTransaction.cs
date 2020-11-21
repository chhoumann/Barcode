namespace Barcode
{
    public class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(User user, decimal amount) : base(user, amount) { }

        public void Execute()
        {
            User.Balance += Amount;
            Succeeded = true;
        }

        public void Undo()
        {
            if (Succeeded)
            {
                User.Balance -= Amount;
                Undone = true;
            }
        }

        public override string ToString() => Undone ? "" : $"{Date.Day} - #{Id} | Insert {Amount} into {User.Username}'s balance. New balance: {User.Balance}";
    }
}