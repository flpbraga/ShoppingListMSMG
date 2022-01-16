using ShoppingList.Interface;
using System.Collections.Generic;

namespace ShoppingList.Services.Interface
{
    public interface IShoppingCalculatorService
    {
        ShoppingTotalItems CalculateTotal(IReadOnlyList<IShoppingItem> items);
    }
}
