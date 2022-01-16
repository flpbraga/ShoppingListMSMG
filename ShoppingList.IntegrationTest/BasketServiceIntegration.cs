using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingList.Model;
using ShoppingList.Offer;
using ShoppingList.Offers;
using ShoppingList.Services;
using ShoppingList.Services.Interface;
using System;

namespace ShoppingList.IntegrationTest
{
    [TestClass]
    public class BasketServiceIntegration
    {
        private IShoppingService _shoppingService;

        private ShoppingCalculatorService _shoppingCalculatorService;
        private IItemOffer _butterDiscountOffer;
        private IItemOffer _milkDiscountOffer;

        private readonly Guid _butterItemId = Guid.Parse("8b26ae2f-34c9-4ed5-b570-23e35ac0047c");
        private readonly Guid _breadItemId = Guid.Parse("4cf5c378-f618-4093-a575-4a6190a47772");
        private readonly Guid _milkItemId = Guid.Parse("75fabf66-c92c-4bde-b933-a7f55b5a2f83");

        [TestInitialize]
        public void Setup()
        {
            _butterDiscountOffer = new LookupItemDiscountOffer(new NullLogger<LookupItemDiscountOffer>(), _butterItemId, _breadItemId, 2, .5m);
            _milkDiscountOffer = new BuyAndGetFreeOffer(new NullLogger<BuyAndGetFreeOffer>(), _milkItemId, 4, 1);

            _shoppingCalculatorService = new ShoppingCalculatorService(new[] { _butterDiscountOffer, _milkDiscountOffer });
            _shoppingService = new ShoppingService(_shoppingCalculatorService);
        }


        [TestMethod]
        [Description("Given the basket has 1 bread, 1 butter and 1 milk when I total the basket then the total should be £2.95")]
        public void GivenShoppingWith1Bread1Milk1ButterShouldReturn2p95c()
        {
            //Given
            _shoppingService
                .AddItem(new Bread())
                .AddItem(new Butter())
                .AddItem(new Milk());

            //When
            var shoppingTotal = _shoppingService.CalculateTotal();

            //Then
            Assert.AreEqual(2.95m, shoppingTotal.Total);
            Assert.AreEqual(0.00m, shoppingTotal.Discount);
        }

        [TestMethod]
        [Description("Given the basket has 2 butter and 2 bread when I total the basket then the total should be £3.10")]
        public void GivenShoppingWith2Butters2BreadsShouldReturn3p10c()
        {
            //Given
            _shoppingService
                .AddItem(new Butter(), 2)
                .AddItem(new Bread(), 2);

            //When
            var shoppingTotal = _shoppingService.CalculateTotal();

            //Then
            Assert.AreEqual(3.10m, shoppingTotal.Total);
            Assert.AreEqual(0.5m, shoppingTotal.Discount);
        }

        [TestMethod]
        [Description("Given the basket has 4 milk when I total the basket then the total should be £3.45")]
        public void GivenShoppingWith4MilksShouldReturn3p45c()
        {
            //Given
            _shoppingService.AddItem(new Milk(), 4);

            //When
            var shoppingTotal = _shoppingService.CalculateTotal();

            //Then
            Assert.AreEqual(3.45m, shoppingTotal.Total);
            Assert.AreEqual(1.15m, shoppingTotal.Discount);
        }

        [TestMethod]
        [Description("Given the basket has 2 butter, 1 bread and 8 milk when I total the basket then the total should be £9.00")]
        public void GivenShoppingWith2Butter1Bread8MilkShouldReturn9p00c()
        {
            //Given
            _shoppingService
                .AddItem(new Butter(), 2)
                .AddItem(new Bread())
                .AddItem(new Milk(), 8);

            //When
            var shoppingTotal = _shoppingService.CalculateTotal();

            //Then
            Assert.AreEqual(9.00m, shoppingTotal.Total);
            Assert.AreEqual(2.8m, shoppingTotal.Discount);
        }

        // Note: C# DataRow attributes do not support `decimal` values, as a workaround (for testing) we use `double`
        [DataTestMethod]
        [DataRow(4, 3.45d, 1.15d)]
        [DataRow(0, 0d, 0d)]
        [DataRow(1, 1.15d, 0d)]
        [DataRow(2, 2.30d, 0d)]
        [DataRow(5, 4.60d, 1.15d)]
        [DataRow(8, 6.90d, 2.3d)]
        [DataRow(9, 8.05d, 2.3d)]
        [DataRow(12, 10.35d, 3.45d)]
        [DataRow(400, 345d, 115d)]
        [DataRow(4000, 3450d, 1150d)]
        [DataRow(40000, 34500d, 11500d)]
        [Description("Given the basket has `milkQty` (milk) when I total the shopping then the total should be `expectedTotal`")]
        public void GivenXMilkShouldReturnExpectedTotal(int milkQty, double expectedTotal, double expectedDiscount)
        {
            //Given
            _shoppingService.AddItem(new Milk(), milkQty);

            //When
            var shoppingTotal = _shoppingService.CalculateTotal();

            //Then
            Assert.AreEqual((decimal)expectedTotal, shoppingTotal.Total);
            Assert.AreEqual((decimal)expectedDiscount, shoppingTotal.Discount);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1.80d, 0d)]
        [DataRow(2, 2, 3.10d, 0.5d)]
        [DataRow(3, 2, 3.90d, 0.5d)]
        [DataRow(4, 2, 4.20d, 1d)]
        [DataRow(4, 3, 5.20d, 1d)]
        [DataRow(5, 2, 5.00d, 1d)]
        [DataRow(80, 40, 84.00d, 20d)]
        [DataRow(8000, 40, 6420.00d, 20d)]
        [DataRow(800000, 40001, 660000.5d, 20000.5d)]
        [Description("Given the basket has `butterQty` (butter) and `breadQty` (bread) when I total the shopping then the total should be `expectedTotal`")]
        public void GivenXButterYBreadShoulReturnExpectedTotal(int butterQty, int breadQty, double expectedTotal, double expectedDiscount)
        {
            //Given
            _shoppingService
                .AddItem(new Butter(), butterQty)
                .AddItem(new Bread(), breadQty);

            //When
            var shoppingTotal = _shoppingService.CalculateTotal();

            //Then
            Assert.AreEqual((decimal)expectedTotal, shoppingTotal.Total);
            Assert.AreEqual((decimal)expectedDiscount, shoppingTotal.Discount);
        }

    }
}
