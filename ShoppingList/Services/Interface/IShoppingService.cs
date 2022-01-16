using ShoppingList.Interface;
using ShoppingList.Model.Interface;
using System;
using System.Collections.Generic;

namespace ShoppingList.Services.Interface
{
    public interface IShoppingService
    {
        IDictionary<Guid, IShoppingItem> Items { get; }

        /// <summary>
        /// Clears the Basket of all products
        /// </summary>
        /// <returns></returns>
        IShoppingService Clear();

        /// <summary>
        /// Adds a (single) <see cref="IItem"/> to the Basket, quantity of the <see cref="IItem"/> is incremented by one (1)
        /// </summary>
        /// <param name="item">The <see cref="IItem"/> to add and/or update</param>
        /// <param name="quantity">Item quantity</param>
        /// <returns></returns>
        IShoppingService AddItem(IItem item, int quantity = 1);

        /// <summary>
        /// Removes the specified <see cref="IItem"/> from the basket
        /// </summary>
        /// <param name="item">The <see cref="IItem"/> to remove</param>
        /// <returns></returns>
        IShoppingService RemoveItem(IItem item);

        /// <summary>
        /// Removes the specified <see cref="IItem"/> from the basket
        /// </summary>
        /// <param name="itemId">The <see cref="IItem.ItemId"/> of the item to remove</param>
        /// <returns></returns>
        IShoppingService RemoveItem(Guid itemId);

        /// <summary>
        /// <para>Updates the <see cref="IItem"/> quantity<br/>
        /// If the quantity is less than zero (0) the item is removed from the basket</para>
        /// </summary>
        /// <param name="item">The <see cref="IItem"/> to update</param>
        /// <param name="quantity">The quantity</param>
        /// <returns></returns>
        IShoppingService UpdateQuantity(IItem item, int quantity);

        /// <summary>
        /// <para>Updates the <see cref="IItem"/> quantity<br/>
        /// If the quantity is less than zero (0) the item is removed from the basket</para>
        /// </summary>
        /// <param name="itemId">The <see cref="IItem.itemID"/> to update</param>
        /// <param name="quantity">The quantity</param>
        /// <returns></returns>
        IShoppingService UpdateQuantity(Guid itemId, int quantity);

        /// <summary>
        /// Calculates the total of the <see cref="IBasketItem"/> including all offers
        /// </summary>
        /// <returns>Total including applicable offers</returns>
        ShoppingTotalItems CalculateTotal();
    }
}
