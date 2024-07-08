using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShexCmaera : MonoBehaviour
{
    public Transform cameraballer;
    // Update is called once per frame
    Vector3 oldp =  Vector3.zero;
    void LateUpdate()
    {
        var e = oldp;
        oldp = cameraballer.transform.position;
        e = new Vector3(Mathf.Round(e.x * 16) / 16f, Mathf.Round(e.y * 16) / 16f, e.z);
        transform.position = e;
    }
}
