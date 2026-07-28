using System.Collections.Generic;
using Game.ObjectInteractable;
using System.Collections;
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
            StartCoroutine(CountTheItems());
        }

        private IEnumerator CountTheItems()
        {
            yield return new WaitForSeconds(1f);
            
            var list = new List<int>(4) { 0, 0, 0, 0 };

            foreach (var VARIABLE in player.LHand.GetComponentsInChildren<ItemRes>())
            {
                switch (VARIABLE.ResType)
                {
                    case TypeRes.Wood:
                        list[0]++;
                        break;
                    case TypeRes.Stone:
                        list[1]++;
                        break;
                    case TypeRes.Iron:
                        list[2]++;
                        break;
                    case TypeRes.Clay:
                        list[3]++;
                        break;
                }
            }

            var text =
                $"Number of items collected in the left hand:\nWood x {list[0]},\nStone x {list[1]},\nIron x {list[2]},\nClay x {list[3]}.";
            counter.SetText(text);
        }
    }
}