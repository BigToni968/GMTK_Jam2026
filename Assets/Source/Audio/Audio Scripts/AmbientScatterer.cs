using System.Collections;
using Game.ReadOnly;
using UnityEngine;
using Game.Other;

public class AmbientScatterer : MonoBehaviour
{
    [Header("Sound Type Reference")]
    [SerializeField] private SoundType soundType = SoundType.STORM;
    [SerializeField] private protected Audio Audio;

    [Header("Interval Settings")]
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float maxInterval = 8f;

    [Header("Randomization")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    protected virtual void Start()
    {
        StartCoroutine(SpawnAudioRoutine());
    }

    public void SetInterval(Vector2 interval)
    {
        minInterval = interval.x;
        maxInterval = interval.y;
    }

    private IEnumerator SpawnAudioRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            PlayRandomSound();
        }
    }

    protected virtual void PlayRandomSound()
    {
        if ( Audio == null) 
            return;

        var clip = Audio.StorageClips.GetRandomClip(soundType);
        if (clip == null) 
            return;

        Audio.SoundSource.pitch = Random.Range(minPitch, maxPitch);
        Audio.SoundSource.PlayOneShot(clip);
    }
}