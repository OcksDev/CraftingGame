using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitShit : MonoBehaviour
{
    public string type = "rat";
    public double Damage = 10;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isdea) return;
        if (sexballs != null && sexballs.EntityOXS.AntiDieJuice) return;
        var e = Gamer.Instance.GetObjectType(collision.gameObject);
        if(sexballs != null)
        Damage = sexballs.Damage;
        var pp = e.playerController;
        if (e.type == "Player" && !hits.Contains(pp))
        {
            if (balling == null) balling = transform.parent;
            var dam = new DamageProfile(type, Damage);
            dam.SpecificLocation = true;
            try { dam.AttackerPos = balling.position; } catch { };
            dam.Knockback = 1f;
            pp.entit.Hit(dam);
            hits.Add(pp);
            if(type=="spitter")Kill();
        }
        else if (type == "spitter" && e.type == "Wall")
        {
            Kill();
        }
        e.FuckYouJustGodDamnRunTheShittyFuckingDoOnTouchMethodsAlreadyIWantToStabYourEyeballsWithAFork();
    }
    bool isdea = false;
    public IEnumerator sexdie()
    {
        if(type == "spitter")
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<MoverSexBalls>().enabled = false;
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
