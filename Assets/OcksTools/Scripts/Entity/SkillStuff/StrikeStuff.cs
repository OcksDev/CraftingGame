using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeStuff : MonoBehaviour
{
    public PlayerController playerController;
    public DamageProfile attackProfile;
    private SpriteRenderer bababna;
    public GameObject spsp;
    public GameObject duust;
    public SpriteRenderer zoomer;
    private void Start()
    {
        bababna = GetComponent<SpriteRenderer>();
        StartCoroutine(all());
    }

    public IEnumerator Beamnono()
    {
        var wee = Instantiate(spsp, transform.position + new Vector3(0,25,0), Quaternion.identity, transform.parent);
        for(int i = 0; i < 20; i++)
        {
            yield return new WaitForFixedUpdate();
            var a = wee.transform.localScale;
            a.x *= 0.84f;
            wee.transform.localScale = a;
        }
        Destroy(wee);
    }

    public IEnumerator all()
    {
        int times = 200;
        var sz = transform.localScale;
        for (int i = 0; i < times; i++)
        {
            var mod = Vector3.one * (i/(float)times);
            mod.z = 1;
            mod.x = 1 - mod.x;
            mod.y = 1 - mod.y;
            zoomer.transform.localScale = mod *0.874f;
            yield return new WaitForFixedUpdate();
        }


        EntityOXS.SpawnExplosion(10, transform.position, attackProfile, 50);
        CameraLol.Instance.Shake(0.25f, 0.84f);
        bababna.enabled = false;
        zoomer.enabled = false;
        StartCoroutine(Beamnono());
        Instantiate(duust, transform.position, Quaternion.identity, transform.parent);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
