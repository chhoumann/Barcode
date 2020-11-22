using System;
using Barcode.Exceptions;

namespace Barcode
{
    public class BuyTransaction : Transaction, ICommand
    {
        public Product Product { get; }
        public int AmountPurchased { get; private set; }

        public BuyTransaction(User user, Product product, int amountPurchased = 1) : base(user, product.Price * amountPurchased)
        {
            Product = product;
            AmountPurchased = amountPurchased;
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
                    $"{User.FirstName} does not have enough credits for {AmountPurchased}x {Product.Name}.");
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
                                 $" for {AmountPurchased}x {Product.Name} for {Amount}.";
        }
    }
}