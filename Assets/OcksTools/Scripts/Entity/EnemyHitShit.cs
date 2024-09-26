using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitShit : MonoBehaviour
{
    public string type = "rat";
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
        if (type == "spitter")
        {
            time -= Time.deltaTime;
            if (time <= 0) Kill();
        }
    }

    public Transform balling = null;

    public void OnSpawn()
    {
        hits.Clear();
    }
    public float nono = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (type)
        {
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
            pp.entit.Hit(dam);
            hits.Add(pp);
            if (type == "spitter"|| type == "cloak") Kill();
            switch (type)
            {
                case "orb":
                    nono = 0.20f;
                    break;
            }
        }
        else if ((type == "spitter"||type == "cloak") && e.type == "Wall")
        {
            Kill();
        }
        e.FuckYouJustGodDamnRunTheShittyFuckingDoOnTouchMethodsAlreadyIWantToStabYourEyeballsWithAFork();
    }

    bool isdea = false;
    public IEnumerator sexdie()
    {
        if(type == "spitter"|| type == "cloak")
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<MoverSexBalls>().enabled = false;
            var g = Gamer.Instance;
            if(type=="cloak")
                Instantiate(g.ParticleSpawns[8], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
            else
                Instantiate(g.ParticleSpawns[9], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
            yield return new WaitForSeconds(3);
        }
        Destroy(gameObject);
    }

    public void Kill()
    {
        isdea = true;
        StartCoroutine(sexdie());
    }
}
