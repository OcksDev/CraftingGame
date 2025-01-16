using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elecdier : MonoBehaviour
{
    public ParticleSystem pp;
    public CircleCollider2D col;
    public SpriteRenderer sex;

    float life = 6f;
    private void FixedUpdate()
    {
        if((life -= Time.deltaTime) <= 0)
        {
            StartCoroutine(diedie());
            life = 5959595;
        }
    }

    public IEnumerator diedie()
    {
        pp.Stop();
        yield return new WaitForSeconds(0.5f);
        col.enabled = false;
        sex.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
