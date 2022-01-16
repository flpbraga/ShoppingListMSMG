using ShoppingList.Model.Interface;
using System;

namespace ShoppingList.Model
{
    public class Milk : IItem
    {
        public Guid ItemId => Guid.Parse("75fabf66-c92c-4bde-b933-a7f55b5a2f83");
        public string Name => "Milk";
        public decimal Price => 1.15m;
    }
}
