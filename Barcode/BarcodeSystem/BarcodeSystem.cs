﻿using System;
using System.Collections.Generic;
using System.Linq;
using Barcode.BarcodeCLI;
using Barcode.DataStore;
using Barcode.Exceptions;
using Barcode.Log;

namespace Barcode
{
    public class BarcodeSystem : IBarcodeSystem
    {
        private readonly ILog log;
        private readonly IBarcodeCLI barcodeCli;
        private IDataStore<Product> productDataStore;
        private IDataStore<User> userDataStore;

        public BarcodeSystem(ILog log, IBarcodeCLI barcodeCli)
        {
            this.log = log;
            this.barcodeCli = barcodeCli;
            Transaction.LogTransaction += this.log.AddLogEntry;
        }

        public List<Product> Products { get; set; }

        public List<Transaction> Transactions { get; } = new List<Transaction>();
        public List<User> Users { get; private set; } = new List<User>();

        public BuyTransaction BuyProduct(User user, Product product, int amountToPurchase = 1)
        {
            BuyTransaction transaction = new BuyTransaction(user, product, amountToPurchase);

            return ExecuteTransaction(transaction);
        }

        public InsertCashTransaction AddCreditsToAccount(User user, decimal amount)
        {
            InsertCashTransaction transaction = new InsertCashTransaction(user, amount);

            return ExecuteTransaction(transaction);
        }

        public T ExecuteTransaction<T>(T transaction) where T : Transaction, ICommand
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
            try
            {
                return Products.First(p => p.Id.Equals(id));
            }
            catch
            {
                throw new ProductNotFoundException($"Product with ID: {id} not found.");
            }
        }

        public IEnumerable<User> GetUsers(Func<User, bool> predicate)
        {
            try
            {
                return Users.Where(predicate);
            }
            catch (Exception)
            {
                throw new UserNotFoundException("Users not found with given predicate.");
            }
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                return Users.First(u => u.Username == username);
            }
            catch
            {
                throw new UserNotFoundException($"User \"{username}\" not found.");
            }
        }

        public IEnumerable<Transaction> GetTransactionsForUser(User user, int count)
        {
            return Transactions
                .Where(transaction => transaction.User.Equals(user))
                .OrderBy(transaction => transaction.Date)
                .Take(count);
        }

        public BarcodeSystem AddProductDataStore(IDataStore<Product> productDataStore)
        {
            this.productDataStore = productDataStore;
            
            try
            {
                IEnumerable<Product> productStore = this.productDataStore.ReadData();
                Products = productStore.ToList();
            }
            catch
            {
                barcodeCli.DisplayGeneralError("Product data store not loaded." +
                                               "\n" +
                                               "Please make sure that you have provided the correct data." +
                                               "\n" +
                                               "Could not find:" +
                                               "\n" +
                                               $"{this.productDataStore.fullFilePath}");
                
                barcodeCli.AwaitKeyPress().Close();
            }
            
            return this;
        }

        public BarcodeSystem AddUserDataStore(IDataStore<User> userDataStore)
        {
            this.userDataStore = userDataStore;
            
            try
            {
                IEnumerable<User> userStore = this.userDataStore.ReadData();

                Users = userStore.ToList();
            }
            catch
            {
                barcodeCli.DisplayGeneralError("Data store not loaded." +
                                               "\n" +
                                               "Please make sure that you have provided the correct data." +
                                               "\n" +
                                               "Could not find:" +
                                               "\n" +
                                               $"{this.userDataStore.fullFilePath}");
                
                barcodeCli.AwaitKeyPress().Close();
            }

            return this;
        }
    }
}