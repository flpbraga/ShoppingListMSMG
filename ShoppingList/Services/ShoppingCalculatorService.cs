using ShoppingList.Interface;
using ShoppingList.Offers;
using ShoppingList.Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Services
{
    public class ShoppingCalculatorService : IShoppingCalculatorService
    {
        private readonly IEnumerable<IItemOffer> _offers;

        public ShoppingCalculatorService(IEnumerable<IItemOffer> offers)
            => _offers = offers;

        public ShoppingTotalItems CalculateTotal(IReadOnlyList<IShoppingItem> items)
        {
            var overallTotal = items.Sum(s => s.Calculate());
            var discount = CalculateDiscount(items);

            return new ShoppingTotalItems(overallTotal - discount, discount);
        }

        /// <summary>
        /// <para>Returns the discount value for offers.<br/>        
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private decimal CalculateDiscount(IReadOnlyList<IShoppingItem> items)
            => _offers.Aggregate(0m, (current, offer) => current + offer.CalculateDiscount(items));
    }
}
