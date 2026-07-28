using Game.ReadOnly;
using UnityEngine;
using Game.Other;
using Game;

public class AmbienceSound : MonoBehaviour
{
    [SerializeField] private Audio audio;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name); 
        if (other.TryGetComponent(out GhostMusic ghost))
        {
            var clips = audio.StorageClips.GetRandomClip(SoundType.STORM);
            audio.SoundSource.PlayOneShot(clips);
        }
    }
}