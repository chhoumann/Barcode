using System;
using System.Collections.Generic;

namespace Barcode
{
    public interface IBarcodeSystem
    {
        List<Product> Products { get; }
        List<Transaction> Transactions { get; }
        List<User> Users { get; }
        BuyTransaction BuyProduct(User user, Product product, int amountToPurchase = 1);
        InsertCashTransaction AddCreditsToAccount(User user, decimal amount);
        T ExecuteTransaction<T>(T transaction) where T : Transaction, ICommand;
        T UndoTransaction<T>(T transaction) where T : Transaction, ICommand;
        Product GetProductById(uint id);
        IEnumerable<User> GetUsers(Func<User, bool> predicate);
        User GetUserByUsername(string username);
        IEnumerable<Transaction> GetTransactionsForUser(User user, int count);
    }
}