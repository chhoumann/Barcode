using System.Collections.Generic;
using Barcode;
using Barcode.DataStore;
using Barcode.Log;
using NSubstitute;
using NUnit.Framework;

namespace BarcodeTests
{
    [TestFixture]
    public class BarcodeSystemTests
    {
        [SetUp]
        public void SetUp()
        {
            log = Substitute.For<ILog>();
            productDataStore = Substitute.For<IDataStore<Product>>();

            userArgs = new object[]
            {
                "test", "test", "test", "test@test.com"
            };

            productArgs = new object[]
            {
                "Milk", 1m
            };
        }

        private object[] userArgs;
        private object[] productArgs;
        private ILog log;
        private IDataStore<Product> productDataStore;

        [Test]
        public void BuyProduct_PurchaseWithEnoughCredit_Succeeds()
        {
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>(productArgs);
            user.Balance = 100m;

            BuyTransaction buyTransaction = barcodeSystem.BuyProduct(user, product);

            Assert.That(buyTransaction.Succeeded, Is.True);
        }

        [Test]
        public void AddCreditsToAccount_AddCredits_Success()
        {
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);

            InsertCashTransaction insertTransaction = barcodeSystem.AddCreditsToAccount(user, 100m);

            Assert.That(insertTransaction.Succeeded, Is.True);
            Assert.That(user.Balance, Is.EqualTo(100m));
        }

        [Test]
        public void UndoTransaction_UndoTransaction_Success()
        {
            const decimal productPrice = 10;
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>("Milk", productPrice);
            user.Balance = productPrice;

            BuyTransaction transaction = barcodeSystem.BuyProduct(user, product);
            barcodeSystem.UndoTransaction(transaction);

            Assert.That(transaction.Undone, Is.True);
            Assert.That(user.Balance, Is.EqualTo(productPrice));
        }

        [Test]
        public void ExecuteTransaction_AddsTransactionToListIfSuccessful_True()
        {
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>(productArgs);
            user.Balance = 100m;

            BuyTransaction successfulTransaction = barcodeSystem.BuyProduct(user, product);

            Assert.That(barcodeSystem.Transactions.Contains(successfulTransaction), Is.True);
        }

        [Test]
        public void GetProductById_ReturnsCorrectProduct_True()
        {
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            Product product = Substitute.For<Product>(productArgs);
            product.Active = true;
            barcodeSystem.ActiveProducts =
                new[]
                {
                    product
                }; // TODO: If you simply used the interface, you wouldn't have to make all this shit public.

            Product foundProduct = barcodeSystem.GetProductById(product.Id);

            Assert.That(foundProduct, Is.EqualTo(product));
        }

        // TODO: Deal with this.
        //[Test]
        /*public void GetUsers_FindCorrectUsers_True()
        {
            var users = new[]
            {
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
            };
            
            const decimal userBalance = Decimal.One;
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>();
            foreach (var user in users) user.Balance = userBalance;
            barcodeSystem.Users.Returns(users);

            Func<User, bool> findUsersWithABalanceOfZero = user => user.Balance.Equals(userBalance);
            var foundUsers = barcodeSystem.GetUsers(findUsersWithABalanceOfZero);

            Assert.That(foundUsers, Is.EquivalentTo(users));
        }

        [Test]
        public void GetUserByUsername_GetsCorrectUser_True()
        {
            User targetUser = Substitute.For<User>(userArgs);
            var users = new List<User>()
            {
                targetUser,
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
            };
            BarcodeSystem barcodeSystem = Substitute.For<BarcodeSystem>(log);
            Func<CallInfo, IEnumerable<User>> f = (CallInfo i) => users;
            barcodeSystem.Users.Returns(f);
            
            var foundUser = barcodeSystem.GetUserByUsername(targetUser.Username);

            Assert.That(foundUser.Username, Is.EqualTo(targetUser.Username));
        }*/

        [Test]
        public void GetTransactionsForUser_ReturnsOrderedTransactions_True()
        {
            IBarcodeSystem barcodeSystem = Substitute.For<IBarcodeSystem>();
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>(productArgs);
            user.Balance = 1000m;
            const int amountOfPurchases = 5;

            for (int i = 0; i < amountOfPurchases; i++) barcodeSystem.BuyProduct(user, product);
            IEnumerable<Transaction> transactionsForUser =
                barcodeSystem.GetTransactionsForUser(user, amountOfPurchases);

            Assert.That(transactionsForUser, Is.Ordered.By("Date"));
        }

        [Test]
        public void GetTransactionsForUser_ReturnsCorrectTransactions_True()
        {
            IBarcodeSystem barcodeSystem = Substitute.For<IBarcodeSystem>();
            User user = Substitute.For<User>(userArgs);
            Product product = Substitute.For<Product>(productArgs);
            user.Balance = 1000m;
            const int amountOfPurchases = 5;

            for (int i = 0; i < amountOfPurchases; i++) barcodeSystem.BuyProduct(user, product);
            IEnumerable<Transaction> transactionsForUser =
                barcodeSystem.GetTransactionsForUser(user, amountOfPurchases);

            Assert.That(transactionsForUser, Is.All.Property("User").EqualTo(user));
        }
    }
}