using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public enum SoundType
{
    AMBIENT,
    FOOTSTEPS,
    FIRE,
    SNOW,
    WIND,
    STORM
}




namespace Game
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]

    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundList[] soundlist;
        private static SoundManager instance;
        private AudioSource audioSource;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public static void PlaySound(SoundType sound, float volume = 1)
        {

            AudioClip[] clips = instance.soundlist[(int)sound].Sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
            instance.audioSource.PlayOneShot(randomClip, volume);


            //instance.audioSource.PlayOneShot(instance.soundlist[(int)sound], volume);
        }

        public static AudioClip GetRandomClip(SoundType sound)
        {
            if (instance == null || (int)sound >= instance.soundlist.Length) 
                return null;
            AudioClip[] clips = instance.soundlist[(int)sound].Sounds;
            if (clips == null || clips.Length == 0) 
                return null;
            
            return clips [UnityEngine.Random.Range(0, clips.Length)];
        }
#if UNITY_EDITOR
        private void OnEnable()
        {
            string[] names = Enum.GetNames(typeof(SoundType));
            Array.Resize(ref soundlist, names.Length);
            for (int i = 0; i < soundlist.Length; i++)
            {
                soundlist[i].name = names[i];
            }
        }
#endif
    }


    [Serializable]
    public struct SoundList
    {
        public AudioClip[] Sounds { get => sounds; }
        [HideInInspector] public string name;
        [SerializeField] private AudioClip[] sounds;
    }
}


