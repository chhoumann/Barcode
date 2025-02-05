﻿using Barcode.Exceptions;

namespace Barcode
{
    public class BuyTransaction : Transaction
    {
        public BuyTransaction(User user, Product product, int amountPurchased = 1) : base(user,
            product.Price * amountPurchased)
        {
            Product = product;
            AmountPurchased = amountPurchased;
        }

        public Product Product { get; }
        public int AmountPurchased { get; }

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
            return $"{Date} - #{Id} | User: {User.FirstName} - " +
                   "Purchase " + (Succeeded ? "success" : "failed") +
                   $" for {AmountPurchased}x {Product.Name} for {Amount} credits";
        }
    }
}