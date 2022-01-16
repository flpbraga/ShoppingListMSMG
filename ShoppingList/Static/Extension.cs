using ShoppingList.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Static
{
    public static class Extension
    {
        public static IShoppingItem GetById(this IEnumerable<IShoppingItem> items, Guid productId)
            => items?.Single(s => s.Item.ItemId == productId);
    }
}
