using System;
using Barcode;
using NSubstitute;
using NUnit.Framework;

namespace BarcodeTests.ProductTests
{
    [TestFixture]
    public class SeasonalProductTests
    {
        [Test]
        public void Constructor_CreateSeasonalProduct_Succeeds()
        {
            SeasonalProduct seasonalProduct = new SeasonalProduct("Product", 1m);

            Assert.That(seasonalProduct, Is.Not.Null);
        }

        [Test]
        public void SeasonStartDate_SetToDateTimeNow_Succeeds()
        {
            SeasonalProduct seasonalProduct = Substitute.For<SeasonalProduct>("Product", 1m);

            seasonalProduct.SeasonStartDate = DateTime.Now;

            Assert.That(seasonalProduct.SeasonStartDate, Is.Not.Null);
        }

        [Test]
        public void SeasonEndDate_SetToDateTimeNow_Succeeds()
        {
            SeasonalProduct seasonalProduct = Substitute.For<SeasonalProduct>("Product", 1m);

            seasonalProduct.SeasonEndDate = DateTime.Now;

            Assert.That(seasonalProduct.SeasonEndDate, Is.Not.Null);
        }
    }
}