using System.Collections.Generic;
using Game.ObjectInteractable;
using UnityEngine;
using System;

namespace Game
{
    [Serializable]
    public enum TypeRes
    {
        Free,
        Wood,
        Stone,
        Iron,
        Clay
    }

    [Serializable]
    public struct FireWoodTime
    {
        public float time;
        public float heat;
    }

    [Serializable]
    public struct Recipe
    {
        public Stove prefab;
        public TypeRes type;
        public int amount;
    }

    [CreateAssetMenu(menuName = "Game/Config/SpawnerStove")]
    public class SpawnerStoveData : ScriptableObject
    {
        [field: SerializeField] public FireWoodTime FireWoodTime { get; private set; }
        [SerializeField] private Recipe[] recipes;

        private Dictionary<TypeRes, Recipe> _recipes;

        private void OnValidate()
        {
            if (_recipes == null || _recipes.Count == 0)
            {
                _recipes = new Dictionary<TypeRes, Recipe>(recipes.Length);
                foreach (var recipe in recipes)
                    _recipes[recipe.type] = recipe;
            }
        }
        
        public int GetAmount(TypeRes type) => _recipes[type].amount;

        public bool TryUpdate(TypeRes type, int amount, out Stove newStove)
        {
            OnValidate();
            newStove = null;

            _recipes.TryGetValue(type, out var recipe);

            if (recipe.amount <= amount)
            {
                newStove = recipe.prefab;
                return true;
            }

            return false;
        }

        public bool HasUpdate(TypeRes curent, out TypeRes? nextLevel)
        {
            nextLevel = null;
            var index = 0;

            foreach (var recipe in recipes)
            {
                if (recipe.type == curent && recipes.Length > index + 1)
                {
                    nextLevel = recipes[index + 1].type;
                    return true;
                }
                index ++;
            }
            
            return false;
        }
    }
}