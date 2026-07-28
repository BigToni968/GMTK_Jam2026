using System.Collections;
using Game.ReadOnly; // References your SoundManager namespace
using UnityEngine;
using Game.Other;

public class ContinuousAmbientLoop : MonoBehaviour
{
    [Header("Sound Manager Settings")]
    [SerializeField] private SoundType soundType = SoundType.WIND;
    [SerializeField] private Audio audio;

    [Header("Transition Settings")]
    [Tooltip("How many seconds the fade transition takes between clips.")]
    [SerializeField] private float crossfadeDuration = 3.0f;

    private AudioSource _sourceA;
    private AudioSource _sourceB;
    private bool _isSourceActive = false; // false = Source A, true = Source B

    private void Awake()
    {
        // Automatically create two 2D AudioSources for crossfading
        _sourceA = gameObject.AddComponent<AudioSource>();
        _sourceB = gameObject.AddComponent<AudioSource>();

        _sourceA.spatialBlend = 0f; // Pure 2D
        _sourceB.spatialBlend = 0f; // Pure 2D
        _sourceA.playOnAwake = false;
        _sourceB.playOnAwake = false;
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
            var nextClip = audio.StorageClips.GetRandomClip(soundType);

            if (nextClip == null)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            // 2. Pick current active source and incoming source
            var currentSource = _isSourceActive ? _sourceB : _sourceA;
            var incomingSource = _isSourceActive ? _sourceA : _sourceB;

            // 3. Prepare incoming source
            incomingSource.clip = nextClip;
            incomingSource.volume = 0f;
            incomingSource.Play();

            // 4. Crossfade between current and incoming sources
            var timer = 0f;
            var currentStartVol = currentSource.volume;

            while (timer < crossfadeDuration)
            {
                timer += Time.deltaTime;
                var progress = timer / crossfadeDuration;

                incomingSource.volume = Mathf.Lerp(0f, audio.SoundSource.volume, progress);
                if (currentSource.isPlaying)
                    currentSource.volume = Mathf.Lerp(currentStartVol, 0f, progress);

                yield return null;
            }

            // Ensure exact target volume
            incomingSource.volume = audio.SoundSource.volume;
            currentSource.Stop();

            // Swap active source status
            _isSourceActive = !_isSourceActive;

            // 5. Wait for the clip to finish (minus crossfade overlap time)
            var waitTime = Mathf.Max(0.1f, nextClip.length - crossfadeDuration);
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Call this from WinterStormCrossfader to adjust this layer's volume!
    public void SetMasterVolume(float volume)
    {
        var activeSource = _isSourceActive ? _sourceB : _sourceA;
        if (activeSource != null && activeSource.isPlaying)
            activeSource.volume = audio.SoundSource.volume;
    }
}