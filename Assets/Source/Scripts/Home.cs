using System.Collections;
using UnityEngine;
using Game.Unit;

namespace Game
{
    public class Home : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private SpawnerStove _spawnerStove;
        [SerializeField] private float debuffHeat;
        [SerializeField] private float timeSecond;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Color colorDebug;

        private Player _player;

        private void Start()
        {
            StartCoroutine(ColdTreatment());
        }

        private void Update()
        {
            var colliders = Physics.OverlapSphere(transform.position, radius, layerMask);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                    if (collider.TryGetComponent(out _player))
                        return;
                return;
            }

            _player = null;
        }

        private IEnumerator ColdTreatment()
        {
            var debuff = debuffHeat / timeSecond;
            while (player.Stats.GetHeat() > 0f)
            { 
                yield return new WaitUntil(() => _player == null);
                player?.Stats.SetHeat(-debuff * Time.deltaTime);
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = colorDebug;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}