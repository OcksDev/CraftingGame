using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanerSpike : MonoBehaviour
{
    float life = 0;
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
    void Update()
    {
        life += Time.deltaTime;
        var wankperc = Mathf.Clamp01(life / Wait);
        float sz = 0;
        sz = RandomFunctions.EaseOut(wankperc * 2, 2) * izescale;
        transform.localScale = new Vector3(4, sz, 4);
        if(wankperc >= 1)
        {
            Destroy(gameObject);
        }
    }
}
