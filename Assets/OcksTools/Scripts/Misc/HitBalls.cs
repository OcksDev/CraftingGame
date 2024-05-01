using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBalls : MonoBehaviour
{
    public bool t = false;
    public PlayerController playerController;
    public string type = "HitBox";
    public bool OnlyHitOne = false;
    private bool hite = false;
    public double Damage = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit " + collision.gameObject.name);
        if (!hite)
        {
            var e = collision.GetComponent<EntityOXS>();
            if (e != null && e.EnemyType == "Enemy" && !hite)
            {
                if (OnlyHitOne)
                {
                    hite = true;
                }
                var dam = new DamageProfile(type, Damage);
                dam.Knockback = 1f;
                if (type == "Arrow")
                {
                    dam.SpecificLocation = true;
                    dam.AttackerPos = transform.position;
                }
                playerController.HitEnemy(e, dam);
            }
            if ((collision.tag == "Sexy" && type == "Arrow") || (OnlyHitOne && hite))
            {
                //Debug.Log("AM DIE! " + collision.gameObject.name);
                hite = true;
                StartCoroutine(WaitForDIe());
            }
        }
    }

    public IEnumerator WaitForDIe()
    {
        var f = GetComponent<SpriteRenderer>();
            if (f != null) f.enabled = false;
        var e = GetComponent<Projectile>();
        if (e != null) e.speed = 0f;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
