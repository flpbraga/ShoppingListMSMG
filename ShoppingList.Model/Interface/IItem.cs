using System;

namespace ShoppingList.Model.Interface
{
    public interface IItem
    {
        /// <summary>
        /// The id for the item.
        /// </summary>
        Guid ItemId { get; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The price of the item.
        /// </summary>
        decimal Price { get; }
    }
}
