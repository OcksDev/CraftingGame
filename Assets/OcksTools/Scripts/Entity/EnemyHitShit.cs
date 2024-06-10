using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitShit : MonoBehaviour
{
    public string type = "rat";
    public double Damage = 10;

    public Transform balling = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pp = collision.GetComponent<PlayerController>();
        if (pp != null)
        {
            if (balling == null) balling = transform.parent;
            var dam = new DamageProfile(type, Damage);
            dam.SpecificLocation = true;
            if (balling == null) dam.AttackerPos = balling.position;
            dam.Knockback = 1f;
            pp.entit.Hit(dam);
        }
        
    }
}
