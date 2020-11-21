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
            
            base.Execute();
        }

        public override void Undo()
        {
            if (Succeeded)
            {
                User.Balance += Amount;
                Undone = true;
            }
            
            base.Undo();
        }
        
        public override string ToString()
        {
            return (Undone ? "UNDO: " : "") + $"{Date} - #{Id} | User: {User.FirstName} - " +
                                 $"Purchase " + (Succeeded ? "success" : "failed") +
                                 $" for {Product.Name} for {Amount}.";
        }
    }
}