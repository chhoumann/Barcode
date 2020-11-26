namespace Barcode
{
    public class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(User user, decimal amount) : base(user, amount)
        {
        }

        public override void Execute()
        {
            User.Balance += Amount;
            Succeeded = true;

            base.Execute();
        }

        public override void Undo()
        {
            if (Succeeded)
            {
                User.Balance -= Amount;
                Undone = true;
            }

            base.Undo();
        }

        public override string ToString()
        {
            return $"{Date} - #{Id} | Insert {Amount} credits into {User.Username}'s balance. New balance: {User.Balance} credits";
        }
    }
}