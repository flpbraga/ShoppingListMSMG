using ShoppingList.Model.Interface;
using System;

namespace ShoppingList.Model
{
    public class Butter : IItem
    {
        public Guid ItemId => Guid.Parse("8b26ae2f-34c9-4ed5-b570-23e35ac0047c");
        public string Name => "Butter";
        public decimal Price => 0.8m;
    }
}
