using System;
using System.Collections.Generic;

namespace Barcode
{
    public class BarcodeSystem
    {
        public void BuyProduct(User user, Product product)
        {
            // 
        }

        public void AddCreditsToAccount(User user, decimal amount)
        {
            
        }

        public void ExecuteTransaction(Transaction transaction)
        {
            
        }

        public void GetProductById(uint id)
        {
            
        }

        // TODO: Strategy Pattern
        public void GetUsers(Func<User, bool> predicate)
        {
            
        }

        public void GetUserByUsername(string username)
        {
            
        }

        public void GetTransactions(User user, int count)
        {
            
        }

        public List<Product> ActiveProducts;

    }
}