using Game.Abstraction;
using UnityEngine.UI;
using Game.ReadOnly;
using System.Linq;
using UnityEngine;
using Game.Save;
using System;
using TMPro;

namespace Game.UI
{
    public class WindowSettings : MonoView
    {
        [SerializeField] private PreferencesGameData data;
        [SerializeField] private Saver saver;
        [Header("Display")] [SerializeField] private TMP_Dropdown screen;
        [SerializeField] private TMP_Dropdown resolution;
        [Header("Audio")] [SerializeField] private Slider generalVolume;
        [SerializeField] private Slider musicVolume;
        [SerializeField] private Slider soundVolume;

        private void Start()
        {
            var options = Enum.GetNames(typeof(FullScreenMode)).ToList();
            screen.AddOptions(options);
            screen.value = (int)data.FullScreenMode;

            options.Clear();
            var index = 0;
            var curentRes = Screen.currentResolution;

            foreach (var resolution in Screen.resolutions)
                options.Add($"{resolution.width} x {resolution.height}");
            resolution.AddOptions(options);

            if (saver.IsLoaded)
            {
                curentRes.width = saver.DTO.PreferencesGame.resolution.x;
                curentRes.height = saver.DTO.PreferencesGame.resolution.y;
                screen.value = (int)saver.DTO.PreferencesGame.screenMode;
            }

            foreach (var resolution in Screen.resolutions)
            {
                if (curentRes.width == resolution.width && curentRes.height == resolution.height) break;
                index++;
            }
            
            resolution.value = index;
            Screen.SetResolution(curentRes.width, curentRes.height, (FullScreenMode)screen.value);

            generalVolume.value = saver.DTO.PreferencesGame.audio.general;
            musicVolume.value = saver.DTO.PreferencesGame.audio.music;
            soundVolume.value = saver.DTO.PreferencesGame.audio.sound;
        }

        public void Apply()
        {
            var resolutionSelected = Screen.resolutions[resolution.value];
            saver.DTO.SetPreferences(new()
            {
                sensitivity = Vector2.one,
                resolution = new(resolutionSelected.width,resolutionSelected.height),
                screenMode = (FullScreenMode)screen.value,
                audio = new ()
                {
                    general = generalVolume.value,
                    music = musicVolume.value,
                    sound = soundVolume.value
                }
            });
            
            saver.Save();
        }
    }
}