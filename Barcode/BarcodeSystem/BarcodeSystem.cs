using System;
using System.Collections.Generic;
using System.Linq;

namespace Barcode
{
    public class BarcodeSystem
    {
        public IEnumerable<Product> Products;
        public IEnumerable<Product> ActiveProducts;
        public IEnumerable<Transaction> Transactions;
        public IEnumerable<User> Users;
        private Log _log;

        public BarcodeSystem()
        {
            _log = new Log();
            Transaction.LogCommand += _log.AddLog;
        }

        public BuyTransaction BuyProduct(User user, Product product)
        {
            var transaction = new BuyTransaction(user, product);
            
            return ExecuteTransaction(transaction);
        }

        public InsertCashTransaction AddCreditsToAccount(User user, decimal amount)
        {
            var transaction = new InsertCashTransaction(user, amount);
            
            return ExecuteTransaction(transaction) as InsertCashTransaction;
        }

        private T ExecuteTransaction<T>(T transaction) where T : Transaction, ICommand
        {
            // TODO: All transactions must be logged to a logfile.
            
            transaction.Execute();
            
            return transaction;
        }

        public T UndoTransaction<T>(T transaction) where T : Transaction, ICommand
        {
            // TODO: Add logging

            transaction.Undo();

            return transaction;
        }

        public IEnumerable<Product> GetProductById(uint id)
        {
            return Products.Where(p => p.Id == id);
        }

        // TODO: Strategy Pattern
        public IEnumerable<User> GetUsers(Func<User, bool> predicate)
        {
            return Users.Where(predicate);
        }

        public IEnumerable<User> GetUserByUsername(string username)
        {
            return Users.Where(u => u.Username == username);
        }

        public IEnumerable<Transaction> GetTransactions(User user, int count)
        {
            return Transactions
                .Where(transaction => transaction.User.Equals(user))
                .OrderBy(transaction => transaction.Date)
                .Take(count);
        }
    }
}