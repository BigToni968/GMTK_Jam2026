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
            var res = Instantiate(this,_player.LHand);
            res.Collider.isTrigger  = true;
            res.gameObject.layer = 0;
            res.Body.isKinematic = true;
            res.Body.useGravity = false;
            res.transform.localScale = transform.localScale / 3;
            res.transform.localRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            res.transform.localPosition = Vector3.zero;
            _player.ItemsEditInHand();
            Destroy(gameObject);
        }
    }
}