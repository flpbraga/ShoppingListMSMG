using Microsoft.Extensions.Logging;
using ShoppingList.Interface;
using ShoppingList.Offers;
using ShoppingList.Static;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Offer
{
    public class LookupItemDiscountOffer : IItemOffer
    {
        private readonly ILogger<LookupItemDiscountOffer> _logger;
        private readonly Guid _primaryItemId;
        private readonly Guid _linkedItemId;
        private readonly int _requiredQuantity;
        private readonly decimal _discountMultiplier;

        public LookupItemDiscountOffer(ILogger<LookupItemDiscountOffer> logger, Guid primaryItemId, Guid linkedItemId, int requiredQuantity, decimal discountMultiplier)
        {
            _logger = logger;
            _primaryItemId = primaryItemId;
            _linkedItemId = linkedItemId;
            _requiredQuantity = requiredQuantity;
            _discountMultiplier = discountMultiplier;
        }

        public decimal CalculateDiscount(IReadOnlyList<IShoppingItem> items)
        {
            if (!HasRequiredItems(items))
                return 0m;

            var primaryShoppingItem = items.GetById(_primaryItemId);
            var linkedShoppingItem = items.GetById(_linkedItemId);

            _logger?.LogTrace(
                "Primary Item: {Name}; " +
                "Linked Item: {Name}; " +
                "Required Quantity: {RequiredQuantity}; " +
                "DiscountMultiplier: {Discount}",
                primaryShoppingItem.Item.Name, linkedShoppingItem.Item.Name, _requiredQuantity, _discountMultiplier);

            var discountQuantity = (primaryShoppingItem.Quantity / _requiredQuantity);
            var discountValue = Math.Min(discountQuantity, linkedShoppingItem.Quantity) * linkedShoppingItem.Item.Price * _discountMultiplier;

            _logger?.LogInformation("Discount Value: {DiscountValue}; Item: {Name}",
                discountValue, linkedShoppingItem.Item.Name);

            return discountValue;
        }

        /// <summary>
        /// Ensure we have required items to calculate the Discount.
        /// </summary>
        private bool HasRequiredItems(IEnumerable<IShoppingItem> items)
            => items?
                .Count(item =>
                    item.Item.ItemId == _primaryItemId &&
                    item.Quantity >= _requiredQuantity ||
                    item.Item.ItemId == _linkedItemId) == 2;
    }
}
