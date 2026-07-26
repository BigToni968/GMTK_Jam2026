using Game.Abstraction;
using UnityEngine.UI;
using UnityEngine;
using Game.Unit;
using System;

namespace Game.UI
{
    [Serializable]
    public struct HeatColorStatus
    {
        public float procent;
        public Color color;
    }
    
    public class WindowHUD : MonoView
    {
        [SerializeField] private Scrollbar heatProgress;
        [SerializeField] private Player player;
        [SerializeField] private HeatColorStatus[] heatStatuses;

        private void Start()
        {
            player.Stats.OnEditHeatEv += SetEditHeat;
            player.Stats.SetHeat(1f);
        }
        
        private void SetEditHeat()
        {
            heatProgress.size = player.Stats.GetHeat() / 100f;
            var procent = heatProgress.size * 100f;
            for (var i = heatStatuses.Length - 1; i >= 0; i--)
            {
                if (procent <= heatStatuses[i].procent)
                {
                    heatProgress.image.color = heatStatuses[i].color;
                    return;
                }
            }
        }
    }
}