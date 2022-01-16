using Microsoft.Extensions.Logging;
using ShoppingList.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingList.Static;
using ShoppingList.Offers;

namespace ShoppingList.Offer
{
    public class BuyAndGetFreeOffer : IItemOffer
    {
        private readonly ILogger<BuyAndGetFreeOffer> _logger;
        private readonly Guid _itemId;
        private readonly int _requiredQuantity;
        private readonly int _freeQuantity;

        public BuyAndGetFreeOffer(ILogger<BuyAndGetFreeOffer> logger, Guid itemId, int requiredQuantity, int freeQuantity)
        {
            _logger = logger;
            _itemId = itemId;
            _requiredQuantity = requiredQuantity;
            _freeQuantity = freeQuantity;
        }

        public decimal CalculateDiscount(IReadOnlyList<IShoppingItem> items)
        {
            if (!HasRequiredItem(items))
                return 0m;

            var shoppingItem = items.GetById(_itemId);

            _logger?.LogTrace(
                "Item: {Name}; " +
                "Required Quantity: {RequiredQuantity}; " +
                "Free Quantity: {freeQuantity}",
                shoppingItem.Item.Name, _requiredQuantity, _freeQuantity);

            var discountQuantity = (shoppingItem.Quantity / _requiredQuantity) * _freeQuantity;
            var discountValue = (discountQuantity * shoppingItem.Item.Price);

            _logger?.LogInformation("Discount Value: {DiscountValue}; Item: {Name}",
                discountValue, shoppingItem.Item.Name);

            return discountValue;
        }

        private bool HasRequiredItem(IEnumerable<IShoppingItem> items)
        {
            return items?
                .Any(item => item.Item.ItemId == _itemId &&
                             item.Quantity >= _requiredQuantity) == true;
        }
    }
}
