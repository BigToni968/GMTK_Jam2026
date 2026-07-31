using UnityEngine.SceneManagement;
using NaughtyAttributes;
using Game.Preferences;
using Game.ReadOnly;
using UnityEngine;
using System.IO;
using System;

namespace Game.Save
{
    public class Saver : MonoBehaviour
    {
        public event Action OnSaveEv;

        [Header("Preferences Game")]
        [SerializeField, Tooltip("Specify the file extension separated by a period.")] private string nameFile;

        [SerializeField] private PreferencesGameData data;
        [field: SerializeField] public DTO DTO { get; private set; }
        [SerializeField] private int indexSceneToMenu;
        [SerializeField] private bool isLoad = false;

        public bool IsLoaded { get; private set; } = false;

        private void Awake()
        {
            if (isLoad)
            {
                DTO.SetPreferences(new()
                {
                    sensitivity = data.Sensitivity,
                    resolution = data.Resolution,
                    screenMode = data.FullScreenMode,
                    audio = data.Audio,
                });
                Load();
                return;
            }

            if (SceneManager.GetActiveScene().buildIndex != indexSceneToMenu) return;
        }

        public void Save()
        {
            var str = JsonUtility.ToJson(DTO.PreferencesGame);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, nameFile), str);
            OnSaveEv?.Invoke();
        }

        public void Load()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, nameFile)))
                return;

            Debug.Log(Application.persistentDataPath);
            var str = File.ReadAllText(Path.Combine(Application.persistentDataPath, nameFile));
            DTO.SetPreferences(JsonUtility.FromJson<PreferencesGame>(str));

            IsLoaded = true;
        }

        [Button("Delete save file")]
        private void Delete()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, nameFile)))
                return;

            File.Delete(Path.Combine(Application.persistentDataPath, nameFile));
        }
    }
}