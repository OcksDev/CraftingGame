using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitShit : MonoBehaviour
{
    public string type = "rat";
    public double Damage = 10;
    float time = 10f;
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
        var pp = collision.GetComponent<PlayerController>();
        if (pp != null && !hits.Contains(pp))
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
        else if (type == "spitter" && collision.transform.parent != null)
        {
            if (collision.transform.parent.GetComponent<I_Room>() != null && !collision.GetComponent<BoxCollider2D>().isTrigger)
            {
                Kill();
            }
            else if(collision.transform.parent.GetComponent<I_RoomWall>() != null)
            {
                Kill();
            }
        }
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
