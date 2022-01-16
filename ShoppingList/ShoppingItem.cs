using ShoppingList.Interface;
using ShoppingList.Model.Interface;
using System;

namespace ShoppingList
{
    public class ShoppingItem : IShoppingItem
    {
        public ShoppingItem(IItem item) => Item = item;

        public Guid ShoppingItemId { get; } = Guid.NewGuid();

        public IItem Item { get; }

        public int Quantity { get; set; } = 0;

        public decimal Calculate() => Item.Price * Quantity;
    }
}