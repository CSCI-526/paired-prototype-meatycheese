using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Bullet Behaviour

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    [Header("Motion")]
    public float speed = 25f;   //Bullet Travel Speed
    public float lifeTime = 2.0f; //Bullet lifetime before "self destructing"

    [Header("Freeze")]
    public float freezeDuration = 2.5f; //How long to freeze objects for

    Rigidbody rb;
    bool hasHit = false; //Prevents multiple hits from the same bullet

    void Awake()
    {
        //Rigidbody configurations
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;  //Prevents bullet drop (we're not making a hyper-realistic game)
        rb.isKinematic = false; 
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        //Collider configuration
        var col = GetComponent<Collider>();
        col.enabled = true; 
    }

    void OnEnable()
    {
        if (lifeTime > 0f)
        {
           Destroy(gameObject, lifeTime); 
        } 
    }

    void Start()
    {
        rb.velocity = transform.forward * speed; //Shoot bullet
    }

    void OnTriggerEnter(Collider other)
    {
        HandleHit(other, null);
    }

    void OnCollisionEnter(Collision c)
    {
        HandleHit(c.collider, c);
    }

    void HandleHit(Collider col, Collision c)
    {
        if (hasHit)
        {
            return; //Only processes the first bullet contact
        }
        hasHit = true;
        TryFreeze(col);
        Destroy(gameObject);
    }

    void TryFreeze(Collider col)
    {
        var f = col.GetComponentInParent<IFreezable>(); //Retrieves a freezable component in object
        if (f != null)
        {
            Debug.Log($"[Bullet] Freezing {((Component)f).gameObject.name} (hit collider {col.name})");
            f.Freeze(freezeDuration);
            return;
        }

        //Debug purposes if no freezable object was found (we hope this doesn't happen)
        Transform t = col.transform;
        string chain = t.name;
        while (t.parent) { t = t.parent; chain = t.name + " -> " + chain; }
        Debug.Log($"[Bullet] Hit {col.name} but no IFreezable found. Parent chain: {chain}");
    }
}