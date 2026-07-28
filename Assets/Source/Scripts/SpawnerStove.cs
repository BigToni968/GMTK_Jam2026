using Game.ObjectInteractable;
using System.Collections;
using UnityEngine;
using System;

namespace Game
{
    public class SpawnerStove : MonoBehaviour
    {
        public event Action OnInitEv;
        
        [field: SerializeField] public SpawnerStoveData Data { get; private set; }
        [SerializeField] private Recipe defaultRecipe;

        public Stove Curent { get; private set; }

        private void Start()
        {
            if (Curent == null && Data.TryUpdate(defaultRecipe.type, defaultRecipe.amount, out var stove))
            {
                Curent = Instantiate(stove, transform);
                Curent.Init(Data.FireWoodTime);
            }

            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitUntil(() => Curent != null);
            OnInitEv?.Invoke();
        }

        public void SetStove(Stove newStove)
        {
            Curent = newStove;
            Curent.Init(Data.FireWoodTime);
        }
    }
}