using System.Collections;
using UnityEngine;
using Game; // References your SoundManager namespace

public class ContinuousAmbientLoop : MonoBehaviour
{
    [Header("Sound Manager Settings")]
    public SoundType soundType = SoundType.WIND;

    [Header("Transition Settings")]
    [Tooltip("How many seconds the fade transition takes between clips.")]
    public float crossfadeDuration = 3.0f;

    [Header("Volume Control")]
    [Range(0f, 1f)]
    public float masterVolume = 1.0f;

    private AudioSource sourceA;
    private AudioSource sourceB;
    private bool isSourceActive = false; // false = Source A, true = Source B

    private void Awake()
    {
        // Automatically create two 2D AudioSources for crossfading
        sourceA = gameObject.AddComponent<AudioSource>();
        sourceB = gameObject.AddComponent<AudioSource>();

        sourceA.spatialBlend = 0f; // Pure 2D
        sourceB.spatialBlend = 0f; // Pure 2D
        sourceA.playOnAwake = false;
        sourceB.playOnAwake = false;
    }

    private void Start()
    {
        StartCoroutine(ContinuousLoopRoutine());
    }

    private IEnumerator ContinuousLoopRoutine()
    {
        while (true)
        {
            // 1. Fetch next random clip from SoundManager
            AudioClip nextClip = SoundManager.GetRandomClip(soundType);

            if (nextClip == null)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            // 2. Pick current active source and incoming source
            AudioSource currentSource = isSourceActive ? sourceB : sourceA;
            AudioSource incomingSource = isSourceActive ? sourceA : sourceB;

            // 3. Prepare incoming source
            incomingSource.clip = nextClip;
            incomingSource.volume = 0f;
            incomingSource.Play();

            // 4. Crossfade between current and incoming sources
            float timer = 0f;
            float currentStartVol = currentSource.volume;

            while (timer < crossfadeDuration)
            {
                timer += Time.deltaTime;
                float progress = timer / crossfadeDuration;

                incomingSource.volume = Mathf.Lerp(0f, masterVolume, progress);
                if (currentSource.isPlaying)
                {
                    currentSource.volume = Mathf.Lerp(currentStartVol, 0f, progress);
                }

                yield return null;
            }

            // Ensure exact target volume
            incomingSource.volume = masterVolume;
            currentSource.Stop();

            // Swap active source status
            isSourceActive = !isSourceActive;

            // 5. Wait for the clip to finish (minus crossfade overlap time)
            float waitTime = Mathf.Max(0.1f, nextClip.length - crossfadeDuration);
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Call this from WinterStormCrossfader to adjust this layer's volume!
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        AudioSource activeSource = isSourceActive ? sourceB : sourceA;
        if (activeSource != null && activeSource.isPlaying)
        {
            activeSource.volume = masterVolume;
        }
    }
}
