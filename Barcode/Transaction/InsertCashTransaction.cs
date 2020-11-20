namespace Barcode.Transaction
{
    public class InsertCashTransaction : Transaction, ICommand
    {
        public InsertCashTransaction(User.User user, decimal amount) : base(user, amount) { }

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