using ShoppingList.Model.Interface;
using System;

namespace ShoppingList.Model
{
    public class Bread : IItem
    {
        public Guid ItemId => Guid.Parse("4cf5c378-f618-4093-a575-4a6190a47772");
        public string Name => "Bread";
        public decimal Price => 1.0m;
    }
}
