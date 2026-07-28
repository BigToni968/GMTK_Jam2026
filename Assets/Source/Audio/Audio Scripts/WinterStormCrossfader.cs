using UnityEngine;

public class WinterStormCrossfader : MonoBehaviour
{
    [Header("Transforms")] public Transform playerTransform;
    public Transform baseTransform;

    [Header("Distance Boundaries")] public float safeRadius = 25f;
    public float stormRadius = 80f;

    [Header("2D Loop Controllers")] public ContinuousAmbientLoop lightWindLoop;
    public ContinuousAmbientLoop heavyStormLoop;

    [Header("3D Scatterer Integration")] public AmbientBoxScatterer windGustScatterer;
    public Vector2 calmGustIntervals = new Vector2(5f, 10f);
    public Vector2 stormGustIntervals = new Vector2(1f, 3f);

    private void Update()
    {
        if (playerTransform == null || baseTransform == null) return;

        var currentDistance = Vector3.Distance(playerTransform.position, baseTransform.position);
        var stormFactor = Mathf.InverseLerp(safeRadius, stormRadius, currentDistance);

        // Update Master Volumes on the 2D loop players
        lightWindLoop?.SetMasterVolume(1f - stormFactor);


        heavyStormLoop?.SetMasterVolume(stormFactor);

        // Adjust 3D gust intervals
        var interval = new Vector2(Mathf.Lerp(calmGustIntervals.x, stormGustIntervals.x, stormFactor),
            Mathf.Lerp(calmGustIntervals.y, stormGustIntervals.y, stormFactor));
        windGustScatterer?.SetInterval(interval);
    }
}