using System;
using Barcode;
using NSubstitute;
using NUnit.Framework;

namespace BarcodeTests
{
    [TestFixture]
    public class BarcodeSystemTests
    {
        private object[] userArgs;
        private object[] productArgs;

        [SetUp]
        public void SetUp()
        {
            userArgs = new object[]
            {
                "test", "test", "test", "test@test.com"
            };

            productArgs = new object[]
            {
                "Milk", 1m
            };
        }

        [Test]
        public void BuyProduct_PurchaseWithEnoughCredit_Succeeds()
        {
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>(productArgs);
            user.Balance = 100m;

            var buyTransaction = barcodeSystem.BuyProduct(user, product);

            Assert.That(buyTransaction.Succeeded, Is.True);
        }

        [Test]
        public void AddCreditsToAccount_AddCredits_Success()
        {
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);

            var insertTransaction = barcodeSystem.AddCreditsToAccount(user, 100m);

            Assert.That(insertTransaction.Succeeded, Is.True);
            Assert.That(user.Balance, Is.EqualTo(100m));
        }

        [Test]
        public void UndoTransaction_UndoTransaction_Success()
        {
            const decimal productPrice = 10;
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>("Milk", productPrice);
            user.Balance = productPrice;

            var transaction = barcodeSystem.BuyProduct(user, product);
            barcodeSystem.UndoTransaction(transaction);

            Assert.That(transaction.Undone, Is.True);
            Assert.That(user.Balance, Is.EqualTo(productPrice));
        }

        [Test]
        public void ExecuteTransaction_AddsTransactionToListIfSuccessful_True()
        {
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>(productArgs);
            user.Balance = 100m;

            var successfulTransaction = barcodeSystem.BuyProduct(user, product);

            Assert.That(barcodeSystem.Transactions.Contains(successfulTransaction), Is.True);
        }

        [Test]
        public void GetProductById_ReturnsCorrectProduct_True()
        {
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            Product product = Substitute.For<Product>(productArgs);
            barcodeSystem.Products.Add(product);

            var foundProduct = barcodeSystem.GetProductById(product.Id);

            Assert.That(foundProduct, Is.EqualTo(product));
        }

        [Test]
        public void GetUsers_FindCorrectUsers_True()
        {
            var users = new[]
            {
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
            };
            const decimal userBalance = Decimal.One;
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            foreach (var user in users) user.Balance = userBalance;
            barcodeSystem.Users.AddRange(users);

            Func<User, bool> findUsersWithABalanceOfZero = user => user.Balance.Equals(userBalance);
            var foundUsers = barcodeSystem.GetUsers(findUsersWithABalanceOfZero);

            Assert.That(foundUsers, Is.EquivalentTo(users));
        }

        [Test]
        public void GetUserByUsername_GetsCorrectUser_True()
        {
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            barcodeSystem.Users.Add(user);

            var foundUser = barcodeSystem.GetUserByUsername(user.Username);

            Assert.That(foundUser.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public void GetTransactionsForUser_ReturnsOrderedTransactions_True()
        {
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>(productArgs);
            user.Balance = 1000m;
            const int amountOfPurchases = 5;
            
            for (int i = 0; i < amountOfPurchases; i++) barcodeSystem.BuyProduct(user, product);
            var transactionsForUser = barcodeSystem.GetTransactionsForUser(user, amountOfPurchases);

            Assert.That(transactionsForUser, Is.Ordered.By("Date"));
        }
        
        [Test]
        public void GetTransactionsForUser_ReturnsCorrectTransactions_True()
        {
            ILog log = Substitute.For<ILog>();
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>(productArgs);
            user.Balance = 1000m;
            const int amountOfPurchases = 5;
            
            for (int i = 0; i < amountOfPurchases; i++) barcodeSystem.BuyProduct(user, product);
            var transactionsForUser = barcodeSystem.GetTransactionsForUser(user, amountOfPurchases);

            Assert.That(transactionsForUser, Is.All.Property("User").EqualTo(user));

        }
    }
}