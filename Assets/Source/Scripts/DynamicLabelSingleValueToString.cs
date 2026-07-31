using UnityEngine;
using TMPro;

namespace Game.Tools
{
    public class DynamicLabelSingleValueToString : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        public void SetValue(float value)
        {
            label.SetText($"{value :F2}");
        }
    }
}