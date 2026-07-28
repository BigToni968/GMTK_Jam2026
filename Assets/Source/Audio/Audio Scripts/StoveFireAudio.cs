using System.Collections;
using Game.Other;
using Game.ReadOnly;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(AudioSource))]
    public class StoveFireAudio : MonoBehaviour
    {
        [System.Serializable]
        public struct FireLevelAudio
        {
            public int level;
            public AudioClip loopClip;
            [Range(0f, 1f)] public float volume;
        }

        [SerializeField] private Audio audio;
        [Header("Loop Configurations per Fire Level")]
        [SerializeField] private FireLevelAudio[] levelAudioProfiles;

        [Header("Random Wood Crackle/Pop Settings")]
        [SerializeField] private float minPopInterval = 2f;
        [SerializeField] private float maxPopInterval = 6f;

        private AudioSource fireLoopSource;
        private int currentLevel = 0;
        private Coroutine popCoroutine;
        private Coroutine transitionCoroutine;

        private void Awake()
        {
            fireLoopSource = GetComponent<AudioSource>();
            ConfigureAudioSource();
        }

        private void OnDisable()
        {
            ResetAudioState();
        }

        private void ConfigureAudioSource()
        {
            if (fireLoopSource == null) return;

            fireLoopSource.loop = true;
            fireLoopSource.playOnAwake = false;
            fireLoopSource.spatialBlend = 1f; // 3D sound
            fireLoopSource.rolloffMode = AudioRolloffMode.Logarithmic;
            fireLoopSource.minDistance = 1f;
            fireLoopSource.maxDistance = 15f;
        }

        /// <summary>
        /// Updates the fire level and triggers appropriate audio responses.
        /// </summary>
        public void SetFireLevel(int newLevel)
        {
            if (currentLevel == newLevel) return;

            int oldLevel = currentLevel;
            currentLevel = Mathf.Clamp(newLevel, 0, 5);

            if (currentLevel == 0)
            {
                Extinguish();
                return;
            }

            // 1. Play one-shot ignition sound using static SoundManager call
            if (oldLevel == 0)
            {
                var clip = audio.StorageClips.GetRandomClip(SoundType.FIRE);
                audio.SoundSource.PlayOneShot(clip,1f);
            }

            // 2. Crossfade local spatial AudioSource loop to match fire level
            var profile = GetProfileForLevel(currentLevel);
            if (profile.loopClip != null)
            {
                if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
                transitionCoroutine = StartCoroutine(ChangeLoopClipRoutine(profile.loopClip, audio.SoundSource.volume));
            }

            // 3. Trigger periodic crackles/pops
            if (popCoroutine != null) StopCoroutine(popCoroutine);
            popCoroutine = StartCoroutine(RandomPopRoutine());
        }

        public void Extinguish()
        {
            currentLevel = 0;

            if (popCoroutine != null) StopCoroutine(popCoroutine);

            if (fireLoopSource != null && fireLoopSource.isPlaying)
            {
                if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
                transitionCoroutine = StartCoroutine(FadeOutAndStop());
            }
        }

        public void ResetAudioState()
        {
            currentLevel = 0;
            StopAllCoroutines();

            if (fireLoopSource != null)
            {
                fireLoopSource.Stop();
                fireLoopSource.volume = 0f;
                fireLoopSource.clip = null;
            }
        }

        private FireLevelAudio GetProfileForLevel(int level)
        {
            foreach (var profile in levelAudioProfiles)
            {
                if (profile.level == level) return profile;
            }
            return levelAudioProfiles.Length > 0 ? levelAudioProfiles[levelAudioProfiles.Length - 1] : default;
        }

        private IEnumerator ChangeLoopClipRoutine(AudioClip newClip, float targetVolume)
        {
            if (!fireLoopSource.isPlaying)
            {
                fireLoopSource.clip = newClip;
                fireLoopSource.volume = 0f;
                fireLoopSource.Play();
            }

            if (fireLoopSource.clip != newClip)
            {
                while (fireLoopSource.volume > 0.05f)
                {
                    fireLoopSource.volume = Mathf.MoveTowards(fireLoopSource.volume, 0f, Time.deltaTime * 3f);
                    yield return null;
                }

                fireLoopSource.clip = newClip;
                fireLoopSource.Play();
            }

            while (!Mathf.Approximately(fireLoopSource.volume, targetVolume))
            {
                fireLoopSource.volume = Mathf.MoveTowards(fireLoopSource.volume, targetVolume, Time.deltaTime * 2f);
                yield return null;
            }
        }

        private IEnumerator FadeOutAndStop()
        {
            while (fireLoopSource.volume > 0f)
            {
                fireLoopSource.volume = Mathf.MoveTowards(fireLoopSource.volume, 0f, Time.deltaTime * 2f);
                yield return null;
            }
            fireLoopSource.Stop();
        }

        private IEnumerator RandomPopRoutine()
        {
            while (currentLevel > 0)
            {
                // Higher fire levels speed up pop frequency
                var intensityMultiplier = Mathf.Lerp(1.5f, 0.5f, currentLevel / 5f);
                var waitTime = Random.Range(minPopInterval, maxPopInterval) * intensityMultiplier;

                yield return new WaitForSeconds(waitTime);

                if (currentLevel > 0)
                {
                    var clip = audio.StorageClips.GetRandomClip(SoundType.FIRE);
                    audio.SoundSource.PlayOneShot(clip,.6f);
                }
            }
        }
    }
}