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
    public float xA = 34f;  //x-point 1
    public float xB = -34f; //x-point 2

    [Header("Motion")]
    public float speed = 3f;
    public float waitAtEnds = 0.25f;    //how long to pause for at each x-point

    int dir = 1;        //+1 toward xB, -1 toward xA
    float waitTimer = 0f; //Countdown while waiting at either end. When > 0, movement paused

    void Start()
    {
        var p = transform.position; //Fetches current position
        float dA = Mathf.Abs(p.x - xA), dB = Mathf.Abs(p.x - xB);   //Finds which x-point is closest and starts from there
        transform.position = new Vector3(dA <= dB ? xA : xB, p.y, p.z); //Sets position as location from line retrieved above
        dir = (Mathf.Abs(transform.position.x - xA) < 0.001f) ? +1 : -1; //Configures direction based on above computations
    }

    protected override void Update()
    {
        base.Update();              //keeps freeze timer ticking
        if (IsFrozen)
        {
            return;       //don't do anything if frozen
        }

        if (waitTimer > 0f) //Wait at end
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        float targetX = (dir > 0) ? xB : xA; //Determines current x-point dest based on direction
        var pos = transform.position;
        float newX = Mathf.MoveTowards(pos.x, targetX, speed * Time.deltaTime); //Moves toward dest x-point
        transform.position = new Vector3(newX, pos.y, pos.z); //Sets new x while keeping other axes constant

        if (Mathf.Abs(newX - targetX) <= 0.001f) //Once x-point dest reached, flip direction
        {
            dir *= -1;
            waitTimer = waitAtEnds;
        }
    }

    void FixedUpdate()  //May not be needed, added for debugging purposes
    {
        if (IsFrozen)
        {
            return; 
        }       
    }
}
