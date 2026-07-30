using Game.Other;
using Game.ReadOnly;
using Unity.VisualScripting;
using UnityEngine;

public class WinterStormCrossfader : MonoBehaviour
{
    [Header("Transforms")] public Transform playerTransform;
    public Transform baseTransform;

    [Header("Distance Boundaries")] public float safeRadius = 25f;
    public float stormRadius = 80f;

    [Header("2D Loop Controllers")]
    [SerializeField] private SoundType[] soundTypes;

    [SerializeField] private Audio audio;

    private SoundType _currentType;
    private SoundType _playType;

    private void Update()
    {
        if (playerTransform == null || baseTransform == null) return;

        var currentDistance = Vector3.Distance(playerTransform.position, baseTransform.position);

        if (currentDistance < safeRadius)
            _currentType = soundTypes[0];
        else if (currentDistance > safeRadius && currentDistance < stormRadius)
            _currentType = soundTypes[1];
        else if (currentDistance > stormRadius)
            _currentType = soundTypes[2];

        if (_playType != _currentType)
        {
           _playType = _currentType;
            audio.SoundSource.clip = audio.StorageClips.GetRandomClip(_playType);
            audio.SoundSource.Play();
        }
    }
}