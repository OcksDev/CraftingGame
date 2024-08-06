using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashExpender : MonoBehaviour
{
    public Image shex;
    public Transform cum;
    public float Times = 1f;
    private float elasped;
    // Update is called once per frame
    void Update()
    {
        elasped += Time.deltaTime;
        float perc = elasped/ Times;
        float x = 1-perc;
        var f = Mathf.Lerp(1f, 1.25f, 1-(x * x * x));
        var a = Mathf.Lerp(0.7f, 0f, perc);
        cum.localScale = new Vector3 (f, f, f);
        var c = shex.color;
        c.a = a;
        shex.color = c;
    }
}
