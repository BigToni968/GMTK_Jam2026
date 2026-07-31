using UnityEngine;
using Game.Save;

namespace Game.Tools.Setting
{
    public class ActionSettingParticles : MonoBehaviour
    {
        [SerializeField] private Saver saver;

        private void Awake()
        {
            saver.OnSaveEv += () => gameObject.SetActive(saver.DTO.PreferencesGame.particlesEnabled);
            gameObject.SetActive(saver.DTO.PreferencesGame.particlesEnabled);
        }
    }
}