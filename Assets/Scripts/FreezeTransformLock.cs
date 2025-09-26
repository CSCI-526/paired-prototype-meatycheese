using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Freezes an object's position
*/

public class FreezeTransformLock : MonoBehaviour
{
    public Freezable target; //leave empty to auto-find on this object/parents

    //Determines whether to lock a GO's position/rotation
    public bool lockPosition = true;
    public bool lockRotation = true;

    //Saves the position and rotation of an object when it's first frozen
    Vector3 frozenPos;
    Quaternion frozenRot;

    bool snapshotTaken = false;

    void Awake()
    {
        if (!target)
        {
            target = GetComponentInParent<Freezable>();
        }
    }

    void LateUpdate()
    {
        if (!target)
        {
            return;
        }

        if (target.IsFrozen)
        {
            //Take a snapshot once, then enforce it every frame
            if (!snapshotTaken)
            {
                frozenPos = transform.position;
                frozenRot = transform.rotation;
                snapshotTaken = true;
            }

            if (lockPosition)
            {
                transform.position = frozenPos;
            }
            if (lockRotation)
            {
                transform.rotation = frozenRot;
            }
        else
        {
            snapshotTaken = false; //allow new snapshot next time we freeze
        }
        }
    }
}