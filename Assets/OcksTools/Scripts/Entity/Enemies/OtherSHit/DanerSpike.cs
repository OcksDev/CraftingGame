using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanerSpike : MonoBehaviour
{
    float life = 0;
    public bool ishingite = false;
    public float Wait = 1f;
    public float izescale = 3f;
    private SpriteRenderer spr;
    private EnemyHitShit gam;
    private void Start()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        gam = GetComponent<EnemyHitShit>();
    }
    // Update is called once per frame
    float waitdin = 0;
    void Update()
    {
        var wankperc = Mathf.Clamp01(life / Wait);
        if (ishingite)
        {
            if(wankperc >= 0.5f && waitdin <= 1.9f)
            {
                waitdin += Time.deltaTime;
            }
            else
            {
                life += Time.deltaTime;
            }
        }
        else
        {
            life += Time.deltaTime;
        }


        float sz = 0;
        sz = RandomFunctions.EaseOutBad(wankperc * 2, 2) * izescale;
        transform.localScale = new Vector3(ishingite?1.4f: 4, sz, ishingite ? 1.4f : 4);
        if(wankperc >= 1)
        {
            Destroy(gameObject);
        }
    }
}
