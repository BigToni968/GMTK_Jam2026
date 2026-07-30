using Game.Abstraction;
using UnityEngine.UI;
using UnityEngine;
using Game.Unit;
using System;
using TMPro;

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
        [SerializeField] private WindowLose losePanel;
        [SerializeField] private TextMeshProUGUI counter;

        private void Start()
        {
            player.Stats.OnEditHeatEv += SetEditHeat;
            player.OnItemsEditInHandEv += StartCountItems;
            player.Stats.SetHeat(1f);
        }

        private void SetEditHeat()
        {
            if (player.Stats.GetHeat() <= 0f)
            {
                player.Stats.OnEditHeatEv -= SetEditHeat;
                player.OnItemsEditInHandEv -= StartCountItems;
                player.Dead();
                Destroy(player);
                losePanel.Show();
            }

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

        private void StartCountItems()
        {
            var text =
                $"Number of items collected in the left hand:\nWood x {player.LHand.GetAmount(TypeRes.Wood)}," +
                $"\nStone x {player.LHand.GetAmount(TypeRes.Stone)}," +
                $"\nIron x {player.LHand.GetAmount(TypeRes.Iron)}," +
                $"\nClay x {player.LHand.GetAmount(TypeRes.Clay)}.";
            counter.SetText(text);
        }
    }
}