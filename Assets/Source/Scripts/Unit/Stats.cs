using Game.ReadOnly;
using UnityEngine;
using System;

namespace Game.Unit.Component
{
    [Serializable]
    public class Stats
    {
        public event Action OnEditHeatEv;
        
        [SerializeField] private StatsData data;
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float SpeedMultiplier { get; private set; }
        [field: SerializeField] public float Heat { get; private set; }
        [field: SerializeField] public float HeatMultiplier { get; private set; }

        public void Init()
        {
            Speed = data.Speed;
            SpeedMultiplier = data.SpeedMultiplier;
            Heat = data.Heat;
            HeatMultiplier = data.HeatMultiplier;
        }

        public float GetSpeed() => Speed * SpeedMultiplier;
        public void SetSpeed(float multiplier) => SpeedMultiplier = multiplier;
        public float  GetHeat() => Heat * HeatMultiplier;
        
        public void SetHeat(float multiplier)
        {
            Heat += multiplier;
            Heat = Mathf.Clamp(Heat, 0, 100f);
            OnEditHeatEv?.Invoke();
        }
    }
}