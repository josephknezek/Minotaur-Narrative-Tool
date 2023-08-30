using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtensions
{
    
    public static void FitAreaIntoCamera(this Camera cam, Bounds areaBounds, ref Transform transform)
    {
        Bounds camBounds = cam.GetOrthographicBounds();

        if (camBounds.Contains(areaBounds.min) && camBounds.Contains(areaBounds.max))
            return;

        bool inBoundsLeft = areaBounds.min.x >= camBounds.min.x;
        bool inBoundsRight = areaBounds.max.x <= camBounds.max.x;
        bool inBoundsDown = areaBounds.min.y >= camBounds.min.y;
        bool inBoundsUp = areaBounds.max.y <= camBounds.max.y;

        if (!inBoundsLeft)
            transform.position = new Vector2(camBounds.min.x + areaBounds.extents.x, transform.position.y);

        if (!inBoundsRight)
            transform.position = new Vector2(camBounds.max.x - areaBounds.extents.x, transform.position.y);

        if (!inBoundsDown)
            transform.position = new Vector2(transform.position.x, camBounds.min.y + areaBounds.extents.y);

        if (!inBoundsUp)
            transform.position = new Vector2(transform.position.x, camBounds.max.y - areaBounds.extents.y);
    }

    public static Bounds GetOrthographicBounds(this Camera cam)
    {
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;
        Vector3 pos = cam.transform.position;

        Bounds bounds = new Bounds(pos, new Vector3(width, height, int.MaxValue));

        return bounds;
    }

}
