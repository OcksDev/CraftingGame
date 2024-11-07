using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitShit : MonoBehaviour
{
    public string type = "rat";
    public string subtype = "buy";
    public double Damage = 10;
    public double overridedamage = -1;
    float time = 10f;
    public NavMeshEntity sexballs;
    List<PlayerController> hits = new List<PlayerController>(); 
    private void Update()
    {
        if (isdea)
        {
            return;
        }
        if (type == "spitter" || type == "spik" || type == "edgworth")
        {
            time -= Time.deltaTime;
            if (time <= 0) Kill();
        }
    }

    public Transform balling = null;
    private void Awake()
    {
        OXComponent.StoreComponent(this);
    }
    public void OnSpawn()
    {
        hits.Clear();
    }
    public float nono = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (type)
        {
            case "ball":
            case "orb":
                if (nono <= 0)
                {
                    Tirggegg(collision, true);
                }
                break;
            default:
                Tirggegg(collision, false);
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (type)
        {
            case "ball":
            case "orb":
                if (nono <= 0)
                {
                    Tirggegg(collision, true);
                }
                break;
        }
    }
    private void FixedUpdate()
    {
        nono -= Time.deltaTime;
    }

    public void Tirggegg(Collider2D collision, bool ignorehits = false)
    {
        if (isdea) return;
        if (sexballs != null && sexballs.EntityOXS.AntiDieJuice) return;
        var e = Gamer.Instance.GetObjectType(collision.gameObject);
        if (sexballs != null)
            Damage = sexballs.Damage;
        if (overridedamage > 0) Damage = overridedamage;
        var pp = e.playerController;
        if (e.type == "Player" && (!hits.Contains(pp) || ignorehits))
        {
            if (balling == null) balling = transform.parent;
            var dam = new DamageProfile(type, Damage);
            dam.SpecificLocation = true;
            try { dam.AttackerPos = balling.position; } catch { };
            dam.Knockback = 1f;
            if(sexballs != null)dam.attacker = sexballs.gameObject;
            pp.entit.Hit(dam);
            hits.Add(pp);
            if (type == "spitter"|| type == "cloak" || type == "spik" || type == "edgworth") Kill();
            switch (type)
            {
                case "ball":
                case "orb":
                    nono = 0.20f;
                    break;
            }
        }
        else if ((subtype == "realbullet") && e.type == "Wall")
        {
            Kill();
        }
        e.FuckYouJustGodDamnRunTheShittyFuckingDoOnTouchMethodsAlreadyIWantToStabYourEyeballsWithAFork();
    }

    bool isdea = false;
    public IEnumerator sexdie()
    {
        if(subtype=="realbullet")
        {
            var wanks = GetComponentInChildren<SpriteRenderer>();
            if (wanks != null) wanks.gameObject.SetActive(false);
            if (TryGetComponent(out BoxCollider2D wank)) wank.enabled = false;
            if (TryGetComponent(out CircleCollider2D wankw)) wankw.enabled = false;
            if (TryGetComponent(out MoverSexBalls wankww)) wankww.enabled = false;
            var g = Gamer.Instance;
            switch (type)
            {
                case "cloak":
                    Instantiate(g.ParticleSpawns[8], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                    break;
                case "spik":
                    Instantiate(g.ParticleSpawns[12], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                    break;
                case "ball":
                    Instantiate(g.ParticleSpawns[14], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                    break;
                case "edgworth":
                    Instantiate(g.ParticleSpawns[18], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                    break;
                default:
                    Instantiate(g.ParticleSpawns[9], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                    break;
            }
            yield return new WaitForSeconds(3);
        }
        Destroy(gameObject);
    }

    public void Kill()
    {
        if (isdea) return;
        isdea = true;
        StartCoroutine(sexdie());
    }
}
