using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Paralaxis : MonoBehaviour
{
    public Transform origin;
    public float streng = 1f;
    public float attrenct = 1f;
    Vector3 off;
    private void Awake()
    {
        var ori = RandomFunctions.Instance.NoZ(transform.position);
        var mypos = RandomFunctions.Instance.NoZ(origin.transform.position);
        off = (ori - mypos)/2;
    }
    private void Update()
    {
        var mypos = RandomFunctions.Instance.NoZ(origin.transform.position);
        var mouse = RandomFunctions.Instance.NoZ(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var target = Vector3.Lerp(mypos, mouse-mypos, streng) + off;
        transform.position = Vector3.Lerp(transform.position, target, attrenct * Time.deltaTime);
    }

}
