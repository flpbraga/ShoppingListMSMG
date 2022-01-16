using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingList.Interface;
using ShoppingList.Model.Interface;
using ShoppingList.Services;
using ShoppingList.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.UnitTest.Services
{
    [TestClass]
    public class ShoppingServiceTests
    {
        private IShoppingService _sut;

        private Mock<IShoppingCalculatorService> _shoppingCalculatorServiceMock;
        private Mock<IItem> _itemMock;
        private readonly Guid _itemId = Guid.Parse("a449d654-2e8e-4e97-a138-43b18fb99c0a");

        [TestInitialize]
        public void SetUp()
        {
            _shoppingCalculatorServiceMock = new Mock<IShoppingCalculatorService>();
            _itemMock = new Mock<IItem>();

            _itemMock
                .Setup(item => item.ItemId)
                .Returns(_itemId);

            _sut = new ShoppingService(_shoppingCalculatorServiceMock.Object);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        public void AddItem_adds_a_item_to_the_items(int expectedQuantity)
        {
            //Given
            IItem item = _itemMock.Object;

            //When
            _sut.AddItem(item, expectedQuantity);

            //Then
            Assert.IsTrue(_sut.Items.ContainsKey(_itemId));
            Assert.AreEqual(expectedQuantity, _sut.Items[_itemId].Quantity);
        }

        [TestMethod]
        public void Clear_removes_all_items_from_the_shopping()
        {
            //Given
            IItem item = _itemMock.Object;
            _sut.AddItem(item);

            //When
            _sut.Clear();

            //Then
            Assert.AreEqual(0, _sut.Items.Count);
        }

        [TestMethod]
        public void RemoveItem_by_item_object_removes_the_specified_item_from_the_shopping()
        {
            //Given
            IItem item = _itemMock.Object;
            _sut.AddItem(item);

            //When
            _sut.RemoveItem(item);

            //Then
            Assert.IsFalse(_sut.Items.Any());
        }

        [TestMethod]
        public void RemoveItem_by_item_id_removes_the_specified_item_from_the_shopping()
        {
            //Given
            IItem item = _itemMock.Object;
            _sut.AddItem(item);

            //When
            _sut.RemoveItem(_itemId);

            //Then
            Assert.IsFalse(_sut.Items.Any());
        }

        [TestMethod]
        public void RemoveItem_by_invalid_item_id_does_not_remove_any_items_from_the_shopping()
        {
            //Given
            IItem item = _itemMock.Object;
            _sut.AddItem(item);

            //When
            _sut.RemoveItem(Guid.NewGuid());

            //Then
            Assert.IsTrue(_sut.Items.Count == 1);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        public void UpdateQuantity_by_item_object_updates_the_specified_item_quantity_in_the_shopping(int expectedQuantity)
        {
            //Given
            IItem item = _itemMock.Object;
            _sut.AddItem(item);

            //When
            _sut.UpdateQuantity(item, expectedQuantity);

            //Then
            Assert.AreEqual(expectedQuantity, _sut.Items[_itemId].Quantity);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        public void UpdateQuantity_by_item_id_updates_the_specified_item_quantity_in_the_shopping(int expectedQuantity)
        {
            //Given
            IItem item = _itemMock.Object;
            _sut.AddItem(item);

            //When
            _sut.UpdateQuantity(_itemId, expectedQuantity);

            //Then
            Assert.AreEqual(expectedQuantity, _sut.Items[_itemId].Quantity);
        }

        [DataTestMethod]
        [TestMethod]
        public void UpdateQuantity_by_item_id_and_quantity_of_zero_removes_the_specified_item_from_the_shopping()
        {
            //Given
            IItem item = _itemMock.Object;
            _sut.AddItem(item);

            //When
            _sut.UpdateQuantity(_itemId, 0);

            //Then
            Assert.IsFalse(_sut.Items.ContainsKey(_itemId));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        public void UpdateQuantity_by_invalid_item_id_does_not_update_any_items_from_the_shopping(int expectedQuantity)
        {
            //Given
            IItem item = _itemMock.Object;
            _sut.AddItem(item);

            //When
            _sut.UpdateQuantity(Guid.NewGuid(), expectedQuantity);

            //Then
            Assert.AreEqual(1, _sut.Items[_itemId].Quantity);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        public void CalculateTotal_by_invalid_item_id_does_not_update_any_items_from_the_shopping(int expectedQuantity)
        {
            //Given
            var rnd = new Random();
            var randomPrice = rnd.Next(1, 100);

            _itemMock
                .Setup(item => item.Price)
                .Returns(randomPrice);

            _shoppingCalculatorServiceMock
                .Setup(service => service.CalculateTotal(It.IsAny<IReadOnlyList<IShoppingItem>>()))
                .Returns<IReadOnlyList<IShoppingItem>>(list => new ShoppingTotalItems(list.Sum(item => item.Item.Price * item.Quantity), 0m));

            IItem item = _itemMock.Object;
            _sut.AddItem(item, expectedQuantity);

            //When
            var shoppingTotal = _sut.CalculateTotal();

            //Then
            var expectedTotal = item.Price * expectedQuantity;
            Assert.AreEqual(expectedTotal, shoppingTotal.Total);

            _shoppingCalculatorServiceMock
                .Verify(service => service.CalculateTotal(It.Is<IReadOnlyList<IShoppingItem>>(list => list.Count == 1 && list.All(a => a.Item == item))), Times.Once);
        }
    }
}
