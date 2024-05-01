using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitShit : MonoBehaviour
{
    public string type = "rat";
    public double Damage = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pp = collision.GetComponent<PlayerController>();
        Debug.Log("COLL " + collision.gameObject.name);
        if (pp != null)
        {
            Debug.Log("HIG");
            var dam = new DamageProfile(type, Damage);
            dam.SpecificLocation = true;
            dam.AttackerPos = transform.parent.position;
            dam.Knockback = 1f;
            pp.entit.Hit(dam);
        }
        
    }
}
