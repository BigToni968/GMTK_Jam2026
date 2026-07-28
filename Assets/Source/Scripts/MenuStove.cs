using UnityEngine;

namespace Game.Other
{
    public class MenuStove : MonoBehaviour
    {
        [SerializeField] private Audio audio;
        [SerializeField] private AudioClip clip;
        [SerializeField] private bool loop;

        private void Start()
        {
            audio.SoundSource.clip = clip;
            audio.SoundSource.loop = loop;
            audio.SoundSource.Play();
        }
    }
}