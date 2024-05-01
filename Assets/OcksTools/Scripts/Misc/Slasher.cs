using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slasher : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    float A = 0;
    float B = 0;
    public float wait = 0.1f;
    public float speedmult = 1f;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.up * speedmult * Time.deltaTime;
        var e = spriteRenderer.color;
        int start = 5;
        int end= 10;
        B += Time.deltaTime;
        if(B >= wait)
        {
            A++;
            if (A <= start)
            {
                e.a = A / start;
                e.a *= 0.5f;
            }
            if (A > start)
            {
                e.a = (end + (start - A)) / end;
                e.a *= 0.5f;
            }
            if (A >= start + end)
            {
                Destroy(gameObject);
            }
            spriteRenderer.color = e;
        }
    }
}
