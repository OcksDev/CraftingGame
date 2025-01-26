using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DUSTSTS : MonoBehaviour
{
    private SpriteRenderer meme;
    private void Start()
    {
        meme = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    float life = 0;
    void FixedUpdate()
    {
        life += Time.deltaTime;
        var a = transform.localScale;
        a.x += 0.35f;
        a.y += 0.35f;
        transform.localScale = a;
        var b = meme.color;
        b.a -= Time.deltaTime;
        meme.color = b;
        if (life > 2) Destroy(gameObject);
    }
}
