using UnityEngine;

namespace Game.ReadOnly
{
    [CreateAssetMenu(menuName = "Game/Config/Stats")]
    public class StatsData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float SpeedMultiplier { get; private set; }
        [field: SerializeField] public float Heat { get; private set; }
        [field: SerializeField] public float HeatMultiplier { get; private set; }
    }
}