using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbFollowerLol : MonoBehaviour
{
    public Transform target;
    Vector3 oldpos;
    float wait = 0.1f;
    private void Awake()
    {
        oldpos = transform.position;
    }
    private void FixedUpdate()
    {
        oldpos = Vector3.Lerp(oldpos, target.position + Vector3.up* Mathf.Sin(Time.time*5)/10f, 0.3f);
        transform.position = oldpos;
    }
}
