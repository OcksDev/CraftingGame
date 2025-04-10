using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerCircleScrip : MonoBehaviour
{
    public string Type = "kik";
    float life = 0;
    public GameObject ToSpawn;
    public float Wait = 1f;
    public float izescale = 3f;
    private SpriteRenderer spr;
    private EnemyHitShit gam;
    private BallScrip bal;
    public Transform www;
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        gam = GetComponent<EnemyHitShit>();
        bal = GetComponent<BallScrip>();
    }
    // Update is called once per frame
    void Update()
    {
        life += Time.deltaTime;
        var wankperc = Mathf.Clamp01(life / Wait);
        var w = spr.color;
        w.a = wankperc/(Type=="fub"?1:2);
        spr.color = w;
        float pp = 0;
        if (Type == "bossrock")
        {
            pp = RandomFunctions.EaseIn(wankperc, 2);
            pp = pp = (pp / 2) + 0.5f;

        }
        else
        {
            pp = RandomFunctions.EaseOut(wankperc, 2);
        }
        float sz = pp * izescale;
        transform.localScale = new Vector3(sz, sz, sz);
        if (bal != null) bal.ChargePOerc = wankperc;
        if (wankperc >= 1)
        {
            switch (Type)
            {
                case "fub":
                    if(www != null)
                    bal.directionsons = (www.position - transform.position).normalized;
                    this.enabled = false;
                    break;
                case "bossrock":
                    Destroy(gameObject);
                    break;
                default:
                    var a = Instantiate(ToSpawn, transform.position - new Vector3(0, 0.5f, 0), transform.rotation).GetComponent<EnemyHitShit>();
                    a.overridedamage = gam.overridedamage;
                    a.Damage = gam.Damage;
                    a.sexballs = gam.sexballs;
                    Destroy(gameObject);
                    break;
            }
        }
        if(Type == "fub" && gam.sexballs != null)
        {
            transform.position = gam.sexballs.transform.position + new Vector3(0, 3.5f, 0);
        }
    }

}
