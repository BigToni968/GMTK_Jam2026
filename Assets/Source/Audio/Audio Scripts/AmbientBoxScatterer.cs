using UnityEngine;

public class AmbientBoxScatterer : AmbientScatterer
{
    public BoxCollider boundsBox;
    public Transform playerTransform;
    public bool followPlayer = true;
    public bool spawnOnSurfaceOnly = true;

    private void LateUpdate()
    {
        if (followPlayer && playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }

    protected override void PlayRandomSound()
    {
        if (boundsBox == null) return;

        // Calculate random spawn point inside or on the surface of the box
        Vector3 spawnPos = spawnOnSurfaceOnly ? GetRandomPointOnSurface(boundsBox) : GetRandomPointInside(boundsBox);
        Audio.transform.position = spawnPos;

        base.PlayRandomSound();
    }

    private Vector3 GetRandomPointInside(BoxCollider box)
    {
        Vector3 extents = box.size / 2f;
        Vector3 localPoint = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );
        return box.transform.TransformPoint(localPoint + box.center);
    }

    private Vector3 GetRandomPointOnSurface(BoxCollider box)
    {
        Vector3 extents = box.size / 2f;
        Vector3 localPoint = localPoint = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        // Pick one random face axis to snap to the outer surface boundary
        int face = Random.Range(0, 3);
        if (face == 0) localPoint.x = Random.value > 0.5f ? extents.x : -extents.x;
        else if (face == 1) localPoint.y = Random.value > 0.5f ? extents.y : -extents.y;
        else localPoint.z = Random.value > 0.5f ? extents.z : -extents.z;

        return box.transform.TransformPoint(localPoint + box.center);
    }
}