using System;
using System.Collections.Generic;
using System.Linq;
using Barcode.DataStore;
using Barcode.Log;

namespace Barcode
{
    public class BarcodeSystem
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Product> ActiveProducts { get; } = new List<Product>();
        public List<Transaction> Transactions { get; } = new List<Transaction>();
        public List<User> Users { get; } = new List<User>();
        private ILog _log;
        // TODO: Add datareader that initializes lists / reads data into them.
        // Make it generic and make it use the strategy pattern

        public BarcodeSystem(ILog log)
        {
            _log = log;
            Transaction.LogCommand += _log.AddLogEntry;
            
            // var ds = new CsvFileDataStore<Product>("Data", "products.csv", ";");
            // Products = (List<Product>) ds.ReadData();
            // Console.WriteLine(Products);
        }

        public BuyTransaction BuyProduct(User user, Product product)
        {
            var transaction = new BuyTransaction(user, product);
            
            return ExecuteTransaction(transaction);
        }

        public InsertCashTransaction AddCreditsToAccount(User user, decimal amount)
        {
            var transaction = new InsertCashTransaction(user, amount);
            
            return ExecuteTransaction(transaction);
        }

        private T ExecuteTransaction<T>(T transaction) where T : Transaction, ICommand
        {
            transaction.Execute();
            if (transaction.Succeeded) Transactions.Add(transaction);
            
            return transaction;
        }

        public T UndoTransaction<T>(T transaction) where T : Transaction, ICommand
        {
            transaction.Undo();

            return transaction;
        }

        public Product GetProductById(uint id)
        {
            return Products.Find(p => p.Id.Equals(id));
        }

        // TODO: Strategy Pattern
        public IEnumerable<User> GetUsers(Func<User, bool> predicate)
        {
            return Users.Where(predicate);
        }

        public User GetUserByUsername(string username)
        {
            return Users.Find(u => u.Username == username);
        }

        public IEnumerable<Transaction> GetTransactionsForUser(User user, int count)
        {
            return Transactions
                .Where(transaction => transaction.User.Equals(user))
                .OrderBy(transaction => transaction.Date)
                .Take(count);
        }
    }
}