using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Moves platform closest to spawn back and forth along 2 x-axis points
Inherits from Freezable.cs to freeze movement when needed
*/

public class BackAndForthPlatform : Freezable
{
    [Header("World X positions to travel between")]
    public float xA = 34f;  // x-point 1
    public float xB = -34f; // x-point 2

    [Header("Motion")]
    public float speed = 3f;
    public float waitAtEnds = 0.25f;    // how long to pause for at each x-point

    int dir = 1;            // +1 toward xB, -1 toward xA
    float waitTimer = 0f;   // Countdown while waiting at either end

    void Start()
    {
        var p = transform.position;
        float dA = Mathf.Abs(p.x - xA);
        float dB = Mathf.Abs(p.x - xB);

        // Snap to the closest point (xA or xB)
        transform.position = new Vector3(dA <= dB ? xA : xB, p.y, p.z);

        // Determine initial direction
        dir = (Mathf.Abs(transform.position.x - xA) < 0.001f) ? +1 : -1;
    }

    protected override void Update()
    {
        base.Update();

        if (IsFrozen)
        {
            return;
        }

        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        float targetX = (dir > 0) ? xB : xA;
        var pos = transform.position;

        float newX = Mathf.MoveTowards(pos.x, targetX, speed * Time.deltaTime);
        transform.position = new Vector3(newX, pos.y, pos.z);

        if (Mathf.Abs(newX - targetX) <= 0.001f)
        {
            dir *= -1;
            waitTimer = waitAtEnds;
        }
    }

    protected override void OnFreeze()
    {
        base.OnFreeze();

        // Force FreezeTransformLock to snapshot position immediately
        var lockComponent = GetComponent<FreezeTransformLock>();
        if (lockComponent)
        {
            lockComponent.SnapshotNow();
        }
    }

    void FixedUpdate()
    {
        if (IsFrozen)
        {
            return;
        }
    }
}
