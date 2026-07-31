using UnityEngine;
using Game.Save;

namespace Game.Tools.Setting
{
    public class ActionSettingFog : MonoBehaviour
    {
        [SerializeField] private Saver saver;

        private void Awake()
        {
            saver.OnSaveEv += () => RenderSettings.fog = saver.DTO.PreferencesGame.fogEnabled;
            RenderSettings.fog = saver.DTO.PreferencesGame.fogEnabled;
        }
    }
}