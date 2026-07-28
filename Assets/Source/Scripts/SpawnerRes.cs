using Random = UnityEngine.Random;
using Game.ObjectInteractable;
using System.Collections;
using UnityEngine;
using Game.Unit;

namespace Game
{
    public class SpawnerRes : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private ResPack[]  resPacks;
        [SerializeField] private float delay;
        [SerializeField] private float radius;
        [SerializeField] private Color colorGizmos;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = colorGizmos;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private IEnumerator Spawn()
        {
            while (player.Stats.GetHeat() >= 0f)
            {
                yield return new WaitForSeconds(delay);
                var res = Instantiate(resPacks[Random.Range(0, resPacks.Length)]);
                res.transform.position = transform.position + Random.insideUnitSphere * radius;
            }
        }
    }
}