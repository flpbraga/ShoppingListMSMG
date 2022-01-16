namespace ShoppingList
{
    public class ShoppingTotalItems
    {
        public decimal Total { get; }

        public decimal Discount { get; }

        public ShoppingTotalItems(decimal total, decimal discount)
        {
            Total = total;
            Discount = discount;
        }
    }
}
