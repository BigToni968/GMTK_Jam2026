using UnityEngine;

namespace Game.Other
{
    public class MenuLight : MonoBehaviour
    {
        [SerializeField] private Light directionalLight;
        [SerializeField] private int[] colorParams;
        public void OnChangeLight(float value)
        {
            var colorValue = (byte)colorParams[(int)value];
            directionalLight.color = new Color32(colorValue, colorValue, colorValue,255);
        }
    }
}