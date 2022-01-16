using ShoppingList.Model.Interface;
using System;

namespace ShoppingList.Interface
{
    public interface IShoppingItem
    {
        Guid ShoppingItemId { get; }

        /// <summary>
        /// The linked <see cref="IItem"/>
        /// </summary>
        IItem Item { get; }

        /// <summary>
        /// The quantity of <see cref="IItem"/> items
        /// </summary>
        int Quantity { get; set; }

        /// <summary>
        /// Calculates the overall value of the <see cref="IShoppingItem"/>
        /// </summary>
        /// <returns><see cref="IItem.Price"/> * <see cref="Quantity"/></returns>
        decimal Calculate();
    }
}
