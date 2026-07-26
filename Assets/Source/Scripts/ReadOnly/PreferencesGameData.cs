using UnityEngine;

namespace Game.ReadOnly
{
    [CreateAssetMenu(menuName = "Game/Config/PreferencesGame")]
    public class PreferencesGameData : ScriptableObject
    {
        [field: SerializeField] public Vector2 Sensitivity { get; private set; } = Vector2.one;
        [field: SerializeField] public FullScreenMode  FullScreenMode { get; private set; } = FullScreenMode.ExclusiveFullScreen;
        [field: SerializeField] public Vector2 Resolution { get; private set; } = new Vector2(1920,1080);
    }
}