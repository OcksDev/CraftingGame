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
    public List<GameObject> hitlist = new List<GameObject>();




    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit " + collision.gameObject.name);
        if (!hite && !NO)
        {
            var shunk = Gamer.Instance.GetObjectType(collision.gameObject);

            if ((shunk.type == "Enemy" || shunk.type == "Hitable") && !hite && !hitlist.Contains(collision.gameObject))
            {
                bool sex = true;
                float dist = 69;
                if (type == "HitBox")
                {
                    var p = collision.gameObject;
                    var hit = Physics2D.RaycastAll(playerController.transform.position, p.transform.position - playerController.transform.position, RandomFunctions.Instance.Dist(playerController.transform.position, p.transform.position));
                    // Does the ray intersect any objects excluding the player layer
                    GameObject sexp = null;
                    foreach (var h in hit)
                    {
                        if (h.distance <= dist)
                        {
                            var obj = Gamer.Instance.GetObjectType(h.collider.gameObject, true);
                            if (h.transform == p.transform)
                            {
                                sex = true;
                                dist = h.distance;
                                sexp = h.collider.gameObject;
                            }
                            else if (obj.type == "Wall")
                            {
                                sex = false;
                                dist = h.distance;
                                sexp = h.collider.gameObject;
                            }
                        }
                    }

                }
                if (sex)
                {
                    var e = shunk.entityoxs;
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
                    if (type != "HitBox") hitlist.Add(collision.gameObject);
                }
            }
            else if ((shunk.type == "Wall" && type == "Arrow") || (OnlyHitOne && hite))
            {
                //Debug.Log("AM DIE! " + collision.gameObject.name);
                hite = true;
                StartCoroutine(WaitForDIe(true));
            }
            shunk.FuckYouJustGodDamnRunTheShittyFuckingDoOnTouchMethodsAlreadyIWantToStabYourEyeballsWithAFork();
        }
    }
    bool NO = false;
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
            if (i > 40) NO = true;
            if (e != null) e.speed *= 0.93f;
            yield return new WaitForFixedUpdate();
        }
        if (e != null) e.speed = 0;
        yield return new WaitForSeconds(0.5f);
        if (f != null) f.enabled = false;
        Destroy(gameObject);
    }
}
