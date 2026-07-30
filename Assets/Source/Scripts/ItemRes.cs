using Game.Abstraction;
using UnityEngine;
using Game.Unit;

namespace Game.ObjectInteractable
{
    public class ItemRes : InteractableObject
    {
        [field: SerializeField] public TypeRes ResType { get; private set; }
        [field: SerializeField] public Rigidbody Body { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }

        private Player _player;

        public void Init(Player player)
        {
            _player = player;
        }

        public override void Execute()
        {
            if (_player == null)
                _player = Player;
            if (_player.Stats.GetHeat() <= 0f) return; 
            
            _player.LHand.Add(this,1);
            Destroy(gameObject);
        }
    }
}