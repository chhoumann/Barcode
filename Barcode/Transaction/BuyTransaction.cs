using Barcode.Exceptions;

namespace Barcode.Transaction
{
    public class BuyTransaction : Transaction
    {
        public Product.Product Product { get; }
        
        public BuyTransaction(User.User user, Product.Product product) : base(user, product.Price)
        {
            Product = product;
        }

        public override void Execute()
        {
            if (Product.CanBeBoughtOnCredit || User.Balance >= Amount)
            {
                User.Balance -= Amount;
                Succeeded = true;
            }
            else
            {
                Succeeded = false;
                throw new InsufficientCreditsException(
                    $"{User.FirstName} does not have enough credits for {Product.Name}.");
            }
        }

        public override void Undo()
        {
            if (!Succeeded) return;
            User.Balance += Amount;
            Undone = true;
        }
        
        public override string ToString() => Undone ? "" : $"{Date.Day} - #{Id} | {User.FirstName} Purchase {Product.Name} for {Amount}.";
    }
}