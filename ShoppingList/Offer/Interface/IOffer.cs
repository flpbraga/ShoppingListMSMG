using ShoppingList.Interface;
using System.Collections.Generic;

namespace ShoppingList.Offers
{
    public interface IOffer
    {

        /// <summary>
        /// Calculates the discount price/value from the given <see cref="IShoppingItem"/> item(s).
        /// </summary>
        /// <param name="items"></param>
        /// <returns>The amount to be discounted</returns>
        decimal CalculateDiscount(IReadOnlyList<IShoppingItem> items);
    }
}
