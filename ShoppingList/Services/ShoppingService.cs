using ShoppingList.Interface;
using ShoppingList.Model.Interface;
using ShoppingList.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Services
{
    public class ShoppingService : IShoppingService
    {
        private readonly IShoppingCalculatorService _shoppingCalculatorService;

        public ShoppingService(IShoppingCalculatorService shoppingCalculatorService)
        {
            _shoppingCalculatorService = shoppingCalculatorService;
        }

        public IDictionary<Guid, IShoppingItem> Items { get; } = new Dictionary<Guid, IShoppingItem>();

        public IShoppingService AddItem(IItem item, int quantity = 1)
        {
            if (!Items.ContainsKey(item.ItemId))
            {
                Items.Add(item.ItemId, new ShoppingItem(item));
            }

            UpdateQuantity(item, quantity);

            return this;
        }

        public ShoppingTotalItems CalculateTotal()
        {
            var basketItems = Items?.Values.ToArray();
            return _shoppingCalculatorService.CalculateTotal(basketItems);
        }

        public IShoppingService Clear()
        {
            Items.Clear();

            return this;
        }

        public IShoppingService RemoveItem(IItem item)
        {
            return RemoveItem(item.ItemId);
        }

        public IShoppingService RemoveItem(Guid itemId)
        {
            if (!Items.ContainsKey(itemId))
            {
                return this;
            }

            Items.Remove(itemId);

            return this;
        }

        public IShoppingService UpdateQuantity(IItem item, int quantity)
        {
            return UpdateQuantity(item.ItemId, quantity);
        }

        public IShoppingService UpdateQuantity(Guid itemId, int quantity)
        {
            if (!Items.TryGetValue(itemId, out var item))
            {
                return this;
            }

            var updatedQuantity = item.Quantity = quantity;
            if (updatedQuantity <= 0)
            {
                RemoveItem(itemId);
            }

            return this;
        }
    }
}
