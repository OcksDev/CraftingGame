using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverSexBalls : MonoBehaviour
{
    public float speed;
    public float rotationspeed;
    public Transform myballs;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        if (myballs != null) myballs.rotation *= Quaternion.Euler(0,0,rotationspeed);
    }
}
