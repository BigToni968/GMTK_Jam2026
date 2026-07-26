using Game.ObjectInteractable;
using UnityEngine;

namespace Game
{
    public class SpawnerStove : MonoBehaviour
    {
        [SerializeField] private SpawnerStoveData data;
        [SerializeField] private Recipe defaultRecipe;

        public Stove Curent { get; private set; }

        private void Awake()
        {
            if (Curent == null && data.TryUpdate(defaultRecipe.type, defaultRecipe.amount, out var stove))
            {
                Curent = Instantiate(stove, transform);
                Curent.Init(data.FireWoodTime);
            }
        }
    }
}