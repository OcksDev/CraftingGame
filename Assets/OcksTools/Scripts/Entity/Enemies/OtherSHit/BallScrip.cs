using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScrip : MonoBehaviour
{
    public Vector3 directionsons = Vector3.zero;
    public float speed;
    public float PullStrnehgth = 1;
    float life;
    public float ChargePOerc = 0f;
    public GameObject WnakSpawn;
    private EnemyHitShit ehs;

    private void Start()
    {
        ehs = GetComponent<EnemyHitShit>();
        Instantiate(Gamer.Instance.ParticleSpawns[13], transform.position, transform.rotation, transform);
    }
    private void Update()
    {
        transform.position += directionsons * Time.deltaTime * speed;
        if ((life += Time.deltaTime) > 9)
        {
            ehs.StartCoroutine(ehs.sexdie());
            this.enabled = false;
            //speed = 0f;
        }
    }
    float zonk;
    float offs;
    private void FixedUpdate()
    {
        if (PlayerController.Instance != null)
        {
            var w = PlayerController.Instance.transform.position;
            float dist = RandomFunctions.Instance.Dist(transform.position, w);
            if(dist <= 13)
            PlayerController.Instance.rigid.velocity += (Vector2)(ChargePOerc * PullStrnehgth * (transform.position - w))/Mathf.Max(dist, 1f);
        }
        if(ChargePOerc >= 1 && (zonk -= Time.deltaTime) <= 0)
        {
            zonk = 0.2f;
            for(int i = 0; i < 4; i++)
            {
                var w = Instantiate(WnakSpawn, transform.position, Quaternion.identity * Quaternion.Euler(0, 0, (90 * i)-offs), Gamer.Instance.balls);
                var eh = w.GetComponent<EnemyHitShit>();
                eh.overridedamage = ehs.overridedamage;
                eh.Damage = ehs.Damage/2;
                eh.sexballs = ehs.sexballs;
            }
            offs += 11.25f;
        }
    }

}
