using System;
using Barcode;
using NSubstitute;
using NUnit.Framework;

namespace BarcodeTests.ProductTests
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void Product_OnCreate_IdIsUnique()
        {
            Product product1 = new Product("Product 1", 1m);
            Product product2 = new Product("Product 2", 1m);

            bool idsAreNotEqual = product1.Id != product2.Id;

            Assert.That(idsAreNotEqual, Is.True);
        }

        [Test]
        public void Name_SetNameToNull_Fails()
        {
            Product product = Substitute.For<Product>("Product", 1m);

            TestDelegate setProductNameToNull = () => product.Name = null;

            Assert.Throws<ArgumentException>(setProductNameToNull);
        }

        [Test]
        public void ToString_OnCall_ReturnsString()
        {
            Product product = Substitute.For<Product>("Product", 1m);
            
            Assert.That(product.ToString(), Is.TypeOf<String>());
        }

        [Test]
        public void CanBeBoughtOnCredit_SetToBooleanValue_Succeeds()
        {
            Product product = Substitute.For<Product>("Product", 1m);

            product.CanBeBoughtOnCredit = false;

            Assert.That(product.CanBeBoughtOnCredit, Is.False);
        }

        [Test]
        public void Active_SetToBooleanValue_Succeeds()
        {
            Product product = Substitute.For<Product>("Product", 1m);

            product.Active = true;

            Assert.That(product.Active, Is.True);
        }
    }
}