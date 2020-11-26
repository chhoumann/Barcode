using Barcode;
using Barcode.Exceptions;
using NSubstitute;
using NUnit.Framework;

namespace BarcodeTests
{
    [TestFixture]
    public class TransactionTests
    {
        [SetUp]
        public void SetUp()
        {
            userArgs = new object[]
            {
                "Christian", "Houmann", "chbh", "christian@bagerbach.com"
            };
        }

        private object[] userArgs;

        [TestCase(10)]
        public void BuyTransaction_BuyProductWithSufficientCredits_Success(decimal productPrice)
        {
            Product product = Substitute.For<Product>("Milk", productPrice);
            User user = Substitute.For<User>(userArgs);
            user.Balance = productPrice;

            BuyTransaction transaction = new BuyTransaction(user, product);
            transaction.Execute();

            Assert.That(transaction.Succeeded, Is.True);
        }

        [TestCase(11, 10)]
        public void BuyTransaction_BuyProductWithInsufficientCredits_ThrowsException(decimal productPrice,
            decimal userBalance)
        {
            Product product = Substitute.For<Product>("Milk", productPrice);
            User user = Substitute.For<User>(userArgs);
            user.Balance = userBalance;

            BuyTransaction transaction = new BuyTransaction(user, product);

            Assert.Throws<InsufficientCreditsException>(transaction.Execute);
        }

        [Test]
        public void BuyTransaction_UndoProductPurchase_Success()
        {
            const decimal amount = 10;
            Product product = Substitute.For<Product>("Milk", amount);
            User user = Substitute.For<User>(userArgs);
            user.Balance = amount;

            BuyTransaction transaction = new BuyTransaction(user, product);
            transaction.Execute();
            transaction.Undo();

            Assert.That(transaction.Undone, Is.True);
            Assert.That(user.Balance, Is.EqualTo(amount));
        }

        [Test]
        public void BuyTransaction_CallToString_ReturnsString()
        {
            const decimal amount = 10;
            Product product = Substitute.For<Product>("Milk", amount);
            User user = Substitute.For<User>(userArgs);

            BuyTransaction transaction = new BuyTransaction(user, product);

            Assert.That(transaction.ToString(), Is.TypeOf<string>());
        }

        [Test]
        public void InsertCashTransaction_InsertCash_Succeeds()
        {
            User user = Substitute.For<User>(userArgs);
            decimal previousUserBalance = user.Balance;

            InsertCashTransaction transaction = new InsertCashTransaction(user, 50m);
            transaction.Execute();

            Assert.That(user.Balance, Is.EqualTo(previousUserBalance + 50m));
            Assert.That(transaction.Succeeded, Is.True);
        }

        [Test]
        public void InsertCashTransaction_UndoInsertCash_Succeeds()
        {
            User user = Substitute.For<User>(userArgs);
            const decimal insertAmount = 50m;

            InsertCashTransaction transaction = new InsertCashTransaction(user, insertAmount);
            transaction.Execute();
            transaction.Undo();

            Assert.That(transaction.Undone, Is.True);
            Assert.That(user.Balance, Is.EqualTo(0));
        }

        [Test]
        public void InsertCashTransaction_CallToString_ReturnsString()
        {
            User user = Substitute.For<User>(userArgs);

            InsertCashTransaction transaction = new InsertCashTransaction(user, 50m);

            Assert.That(transaction.ToString(), Is.TypeOf<string>());
        }
    }
}