using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamIndi : MonoBehaviour
{
    float t = 1;
    public TextMeshProUGUI sex;
    public float r = 0;
    public void Start()
    {
        float ee = 2;
        r = Random.Range(-ee, ee);
        transform.localScale = new Vector3(2, 2, 2);
    }
    private void FixedUpdate()
    {
        t += Time.deltaTime * 3;
        transform.Rotate(0, 0, r);
        transform.position += transform.up * Time.deltaTime;
        float g = 2 / (t);
        transform.localScale = new Vector3(g, g, g);
        var e = sex.color;
        e.a = 1 - ((t-1)/2);
        sex.color = e;
        if(t >= 3) Destroy(gameObject);
    }
}
