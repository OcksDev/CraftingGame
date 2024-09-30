using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerCircleScrip : MonoBehaviour
{
    float life = 0;
    public GameObject ToSpawn;
    public float Wait = 1f;
    public float izescale = 3f;
    private SpriteRenderer spr;
    private EnemyHitShit gam;
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        gam = GetComponent<EnemyHitShit>();
    }
    // Update is called once per frame
    void Update()
    {
        life += Time.deltaTime;
        var wankperc = Mathf.Clamp01(life / Wait);
        var w = spr.color;
        w.a = wankperc/2;
        spr.color = w;
        float sz = RandomFunctions.EaseOut(wankperc, 2) * izescale;
        transform.localScale = new Vector3(sz, sz, sz);
        if(wankperc >= 1)
        {
            var a = Instantiate(ToSpawn, transform.position - new Vector3(0, 0.5f, 0), transform.rotation).GetComponent<EnemyHitShit>();
            a.overridedamage = gam.overridedamage;
            a.Damage = gam.Damage;
            a.sexballs = gam.sexballs;
            Destroy(gameObject);
        }
    }
}
