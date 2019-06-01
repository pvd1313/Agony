using System.Collections.Generic;
using System.Linq;
using System;

namespace Agony.Common
{
    public sealed class TechData : ITechData
    {
        public int craftAmount { get; private set; }
        public int ingredientCount => _ingredients.Count;
        public int linkedItemCount => _linkedItems.Count;

        private List<IIngredient> _ingredients;
        private List<TechType> _linkedItems;

        public TechData(int craftAmount, IEnumerable<IIngredient> ingredients, IEnumerable<TechType> linkedItems)
        {
            if (craftAmount < 0) throw new ArgumentException("craftAmount < 0");
            if (ingredients == null) throw new ArgumentNullException("ingredients is null");
            if (linkedItems == null) throw new ArgumentNullException("linkedItems is null");
            if (ingredients.Any(x => x == null)) throw new ArgumentNullException("One of ingredients is null");

            this.craftAmount = craftAmount;
            _ingredients = new List<IIngredient>(ingredients);
            _linkedItems = new List<TechType>(linkedItems);
        }

        public IIngredient GetIngredient(int index) => _ingredients[index];
        public TechType GetLinkedItem(int index) => _linkedItems[index];
    }   
}