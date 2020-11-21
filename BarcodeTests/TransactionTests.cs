using System;
using Barcode.Exceptions;
using Barcode;
using NSubstitute;
using NUnit.Framework;

namespace BarcodeTests
{
    [TestFixture]
    public class TransactionTests
    {
        private object[] userArgs;
        
        [SetUp]
        public void SetUp()
        {
            userArgs = new object[]
            {
                "Christian", "Houmann", "chbh", "christian@bagerbach.com" 
            };
        }
        
        [TestCase(10)]
        public void BuyTransaction_BuyProductWithSufficientCredits_Success(decimal productPrice)
        {
            Product product = Substitute.For<Product>("Milk", productPrice);
            User user = Substitute.For<User>(userArgs);
            user.Balance = productPrice;
            
            var transaction = new BuyTransaction(user, product);
            transaction.Execute();
            
            Assert.That(transaction.Succeeded, Is.True);
        }

        [TestCase(11, 10)]
        public void BuyTransaction_BuyProductWithInsufficientCredits_ThrowsException(decimal productPrice, decimal userBalance)
        {
            Product product = Substitute.For<Product>("Milk", productPrice);
            User user = Substitute.For<User>(userArgs);
            user.Balance = userBalance;

            var transaction = new BuyTransaction(user, product);
            
            Assert.Throws<InsufficientCreditsException>(transaction.Execute);
        }
        
        [Test]
        public void BuyTransaction_UndoProductPurchase_Success()
        {
            const decimal amount = 10;
            Product product = Substitute.For<Product>("Milk", amount);
            User user = Substitute.For<User>(userArgs);
            user.Balance = amount;

            var transaction = new BuyTransaction(user, product);
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
    
            var transaction = new BuyTransaction(user, product);
            
            Assert.That(transaction.ToString(), Is.TypeOf<String>());
        }

        [Test]
        public void InsertCashTransaction_InsertCash_Succeeds()
        {
            User user = Substitute.For<User>(userArgs);

            var transaction = new InsertCashTransaction(user, 50m);
            transaction.Execute();
            
            Assert.That(transaction.Succeeded, Is.True);
        }

        [Test]
        public void InsertCashTransaction_UndoInsertCash_Succeeds()
        {
            User user = Substitute.For<User>(userArgs);
            const decimal insertAmount = 50m;
            
            var transaction = new InsertCashTransaction(user, insertAmount);
            transaction.Execute();
            transaction.Undo();
            
            Assert.That(transaction.Undone, Is.True);
            Assert.That(user.Balance, Is.EqualTo(0));
        }

        [Test]
        public void InsertCashTransaction_CallToString_ReturnsString()
        {
            User user = Substitute.For<User>(userArgs);
                        
            var transaction = new InsertCashTransaction(user, 50m);
            
            Assert.That(transaction.ToString(), Is.TypeOf<String>());
        }
    }
}