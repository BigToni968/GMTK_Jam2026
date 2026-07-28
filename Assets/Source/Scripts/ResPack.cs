using System.Collections;
using Game.Abstraction;
using UnityEngine;

namespace Game.ObjectInteractable
{
    public class ResPack : InteractableObject
    {
        [SerializeField] private ItemRes prefab;
        [SerializeField] private  int _amountHit;
        [SerializeField] private float force;
        [SerializeField] private float timeLife = 3;

        private int _hits = 0;

        private void Start()
        {
            StartCoroutine(Delay());
        }

        public override void Execute()
        {
            if (_hits >= _amountHit)
            {
                Destroy(gameObject);
                return;
            }

            _hits++;
            var res = Instantiate(prefab);
            res.transform.position = transform.position + transform.up * 2;
            res.Init(Player);
            res.Body.AddForce(Vector3.up * force, ForceMode.Impulse);

            if (_hits >= _amountHit)
                Destroy(gameObject);
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(timeLife);
            if (gameObject) Destroy(gameObject);
        }
    }
}