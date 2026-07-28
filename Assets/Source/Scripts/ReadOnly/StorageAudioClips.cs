using Random = UnityEngine.Random;
using UnityEngine;
using System;

namespace Game.ReadOnly
{
    public enum SoundType
    {
        AMBIENT,
        FOOTSTEPS,
        FIRE,
        SNOW,
        WIND,
        STORM
    }
    
    [CreateAssetMenu(menuName = "Game/Config/Storages/AudioStorages")]
    public class StorageAudioClips : ScriptableObject
    {
        [SerializeField] private SoundList[] soundlist;

        public AudioClip GetRandomClip(SoundType sound)
        {
            if ((int)sound >= soundlist.Length)
                return null;
            var clips = soundlist[(int)sound];
            if (clips.Length == 0)
                return null;
        
            return clips.Get(Random.Range(0, clips.Length));
        }
        
        [Serializable]
        public struct SoundList
        {
            [SerializeField] private AudioClip[] sounds;
        
            public int Length => sounds.Length;
            public AudioClip Get(int indexClip) => sounds[indexClip];
        
        }
    }
}