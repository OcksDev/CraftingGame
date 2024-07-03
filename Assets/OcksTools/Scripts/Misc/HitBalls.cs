using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBalls : MonoBehaviour
{
    public bool t = false;
    public PlayerController playerController;
    public AttackProfile attackProfile;
    public string type = "HitBox";
    public bool OnlyHitOne = false;
    private bool hite = false;
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
                var dam = new DamageProfile(type, attackProfile.CalcDamage());
                dam.Knockback = 1f;
                dam.attacker = playerController.gameObject;
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
                StartCoroutine(WaitForDIe(true));
            }
        }
    }

    public IEnumerator WaitForDIe(bool fart = false)
    {
        var e = GetComponent<Projectile>();
        if (fart && e != null) e.speed = 0;
        var f = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 50; i++)
        {
            if (f != null)
            {
                var c = f.color;
                c.a -= 0.02f;
                f.color = c;
            }
            if (e != null) e.speed *= 0.93f;
            yield return new WaitForFixedUpdate();
        }
        if (e != null) e.speed = 0;
        yield return new WaitForSeconds(0.5f);
        if (f != null) f.enabled = false;
        Destroy(gameObject);
    }
}
