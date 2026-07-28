using System.Collections;
using UnityEngine;
using Game;

public class AmbientScatterer : MonoBehaviour
{
    [Header("Sound Type Reference")]
    public SoundType soundType = SoundType.STORM;
    public AudioSource audioSource;

    [Header("Interval Settings")]
    public float minInterval = 3f;
    public float maxInterval = 8f;

    [Header("Randomization")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public float minVolume = 0.7f;
    public float maxVolume = 1.0f;

    protected virtual void Start()
    {
        StartCoroutine(SpawnAudioRoutine());
    }

    private IEnumerator SpawnAudioRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            PlayRandomSound();
        }
    }

    protected virtual void PlayRandomSound()
    {
        if ( audioSource == null) 
            return;

        AudioClip clip = SoundManager.GetRandomClip(soundType);
        if (clip == null) 
            return;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = Random.Range(minVolume, maxVolume);

        audioSource.PlayOneShot(clip);
    }
}