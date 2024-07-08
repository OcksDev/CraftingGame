using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HitBalls : MonoBehaviour
{
    public bool t = false;
    public PlayerController playerController;
    public AttackProfile attackProfile;
    public string type = "HitBox";
    public bool OnlyHitOne = false;
    private bool hite = false;
    public List<GameObject> hitlist = new List<GameObject>();
    public List<SpriteRenderer> spriteballs = new List<SpriteRenderer>();
    public List<GameObject> specialsharts = new List<GameObject>();
    public float hsh = 26.3f;
    private Dictionary<GameObject, int> hitdict = new Dictionary<GameObject, int>();
    private void FixedUpdate()
    {
        switch (type)
        {
            case "Shuriken":
                specialsharts[0].transform.rotation *= Quaternion.Euler(0,0,hsh);
                hsh *= 0.985f;
                for(int i = 0; i < hitdict.Count; i++)
                {
                    var x = hitdict.ElementAt(i);
                    hitdict[x.Key] = hitdict[x.Key] - 1;
                    if (x.Value <= 0)
                    {
                        hitdict.Remove(x.Key);
                        i--;
                    }
                }
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == "Shuriken")
        {
            if (!hitdict.ContainsKey(collision.gameObject))
            {
                Collisonsns(collision);
            }
        }
        else
            Collisonsns(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!hitdict.ContainsKey(collision.gameObject))
        {
            Collisonsns(collision);
        }
    }

    public void Collisonsns(Collider2D collision)
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
                    if (type == "Arrow" || type == "Shuriken")
                    {
                        dam.SpecificLocation = true;
                        dam.AttackerPos = transform.position;
                    }
                    playerController.HitEnemy(e, dam);
                    switch (type)
                    {
                        case "Arrow":
                            hitlist.Add(collision.gameObject);
                            break;
                        case "Shuriken":
                            hitdict.Add(collision.gameObject, 12);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (shunk.type == "Wall" && (type == "Arrow"))
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
            if (i > 40 && type != "Shuriken") NO = true;
            if (e != null)
            {
                if (type == "Shuriken")
                {
                    e.speed *= 0.96f;
                }
                else
                {
                    e.speed *= 0.93f;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        if (e != null) e.speed = 0;
        if (type == "Shuriken")
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 25 ; i++)
            {
                transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
                if (transform.localScale.x <= 0) break;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (f != null) f.enabled = false;
        Destroy(gameObject);
    }
}
