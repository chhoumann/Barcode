using Barcode.Exceptions;

namespace Barcode
{
    public class BuyTransaction : Transaction, ICommand
    {
        public Product Product { get; }
        
        public BuyTransaction(User user, Product product) : base(user, product.Price)
        {
            Product = product;
        }

        public void Execute()
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

        public void Undo()
        {
            if (Succeeded)
            {
                User.Balance += Amount;
                Undone = true;
            }
        }
        
        public override string ToString() => Undone ? "" : $"{Date.Day} - #{Id} | {User.FirstName} Purchase {Product.Name} for {Amount}.";
    }
}