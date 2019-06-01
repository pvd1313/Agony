using System;

namespace Agony.Common
{
    public sealed class Ingredient : IIngredient
    {
        public TechType techType { get; private set; }
        public int amount { get; private set; }

        public Ingredient(TechType techType, int amount)
        {
            if (amount < 1) throw new ArgumentException("amount < 1");
            this.techType = techType;
            this.amount = amount;
        }
    } 
}