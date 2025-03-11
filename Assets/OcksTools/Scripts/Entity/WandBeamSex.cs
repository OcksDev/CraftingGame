using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandBeamSex : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer sp;
    public ParticleSystem pp;

    Vector3 ninininininini;
    void Start()
    {
        ninininininini = transform.localScale;
    }

    private float life = 0.2f;
    float targetlife = 0.2f;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetlife <= 0)
        {
            Destroy(gameObject);
            return;
        }
        targetlife -= Time.deltaTime;
        transform.localScale = new Vector3 (ninininininini.x, ninininininini.y*((targetlife/life)), 1);
    }
}
