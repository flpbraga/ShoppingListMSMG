using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingList.Interface;
using ShoppingList.Model.Interface;
using ShoppingList.Offers;
using ShoppingList.Services;
using ShoppingList.Services.Interface;
using System;
using System.Collections.Generic;

namespace ShoppingList.UnitTest
{
    [TestClass]
    public class ShoppingCalculatorServiceTests
    {
        private IShoppingCalculatorService _shoppingCalculatorService;

        private Mock<IItemOffer> _itemOfferMock;
        private Mock<IShoppingItem> _shoppingItemMock;
        private Mock<IItem> _itemMock;

        private readonly Guid _itemId = Guid.Parse("a449d654-2e8e-4e97-a138-43b18fb99c0a");

        [TestInitialize]
        public void SetUp()
        {
            _itemOfferMock = new Mock<IItemOffer>();

            _itemMock = new Mock<IItem>();

            _itemMock
                .Setup(item => item.ItemId)
                .Returns(_itemId);

            _shoppingItemMock = new Mock<IShoppingItem>();
            _shoppingItemMock
                .Setup(item => item.Item)
                .Returns(_itemMock.Object);

            _shoppingCalculatorService = new ShoppingCalculatorService(new[] { _itemOfferMock.Object });
        }

        [TestMethod]
        public void CalculateTotal_returns_the_expected_discount_value()
        {
            //Given
            var rnd = new Random();
            var expectedDiscount = rnd.Next(1, 100);

            _itemOfferMock
                .Setup(offer => offer.CalculateDiscount(It.IsAny<IReadOnlyList<IShoppingItem>>()))
                .Returns(expectedDiscount);

            //When
            var basketTotal = _shoppingCalculatorService.CalculateTotal(new List<IShoppingItem>() { _shoppingItemMock.Object });

            //Then
            Assert.AreEqual(expectedDiscount, basketTotal.Discount);
        }
    }
}
