using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshEntity : MonoBehaviour
{
    private NavMeshAgent beans;
    public GameObject target;
    public I_Room originroom;
    public double Damage;
    public SpriteRenderer WantASpriteCranberry;
    public List<Sprite> SpriteVarients = new List<Sprite> ();
    public float movespeed = 5f;
    private Rigidbody2D sex;
    public GameObject box;
    float timer = 0f;
    float timer2 = 0f;
    public float SightRange = 15f;
    public float randommovetimer = 0f;
    public Vector3 spawn;
    public EnemyHitShit sex2;
    // Start is called before the first frame update
    void Start()
    {
        sex2.Damage = Damage;
        beans = GetComponent<NavMeshAgent>();
        sex = GetComponent<Rigidbody2D>();
        WantASpriteCranberry = GetComponent<SpriteRenderer>();
        transform.rotation = Quaternion.identity;
        beans.updateRotation= false;
        beans.updateUpAxis= false;
        ReRollPos();
        WantASpriteCranberry.flipX = Random.Range(0, 2) == 1;
        WantASpriteCranberry.sprite = SpriteVarients[Random.Range(0, SpriteVarients.Count)];
        randommovetimer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer <= 0.1f)
        {
            ReRollPos();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (timer <= 0.1f)
        {
            ReRollPos();
        }
    }

    public void ReRollPos()
    {
        var s = originroom.gm.transform;
        var s1 = s.localScale / 2;
        transform.position = s.position + new Vector3(Random.Range(-s1.x + 3f, s1.x - 3f), Random.Range(-s1.y + 3f, s1.y - 3f), 0);

        var hits = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward));
        bool sex = false;
        foreach(var e in hits)
        {
            var s2 = e.transform.GetComponent<SpriteRenderer>();
            if (s2 != null && s2.sortingOrder == -9999)
            {
                sex = true; break;
            }
        }
        if(!sex) ReRollPos();
    }
    public bool existing = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer > 0.1f && !existing)
        {
            existing = true;
            WantASpriteCranberry.enabled = true;
            spawn = transform.position;
            randommovetimer = Random.Range(1f, 5f);
        }
        else
        {
            timer += Time.deltaTime;
        }
        float dist=69;
        PlayerController nearestnerd = null;
        foreach(var p in Gamer.Instance.Players)
        {
            if(p == null || p.gameObject==null) continue;
            var x = (p.transform.position - transform.position).magnitude;
            if(x < dist)
            {
                nearestnerd = p;
                dist = x;
            }
        }

        if(nearestnerd != null && dist <= 30)
        {

            if (dist <= SightRange)
            {
                CheckCanSee(true, PlayerController.Instance.gameObject);
            }

            if (target != null && dist <= 2.5f)
            {
                timer2 += Time.deltaTime;
            }


            box.SetActive(false);
            if (timer2 > 1.5f)
            {
                timer2 = 0;
                //Debug.Log("SHONK");
                box.transform.rotation = Point2D(-180, 0);
                box.SetActive(true);
            }
            if (Mathf.Abs(beans.velocity.x) > 0.1f)
                WantASpriteCranberry.flipX = beans.velocity.x > 0;
            if (target != null)
            {
                beans.SetDestination(target.transform.position);
                beans.speed = movespeed;
            }
            else if(existing)
            {
                beans.speed = movespeed / 2;
                randommovetimer -= Time.deltaTime;
                if (randommovetimer <= 0)
                {
                    RandomMove();
                }
            }
        }
    }

    public void RandomMove()
    {
        randommovetimer = Random.Range(1f, 7f);
        float wanderrange = 3f;
        beans.SetDestination(spawn + new Vector3(Random.Range(-wanderrange, wanderrange), Random.Range(-wanderrange, wanderrange),0));
    }

    public void CheckCanSee(bool range, GameObject shart)
    {
        if (target != null) return;
        var p = shart;
        var hit = Physics2D.RaycastAll(transform.position, p.transform.position - transform.position, range?SightRange:(SightRange*1.5f));
        // Does the ray intersect any objects excluding the player layer
        bool sex = false;
        float dist = 69;
        foreach (var h in hit)
        {
            if (h.distance <= dist)
            {
                if (h.transform == p.transform)
                {
                    sex = true;
                    dist = h.distance;
                }
                if (h.transform.parent != null && !h.transform.GetComponent<BoxCollider2D>().isTrigger && h.transform.parent.GetComponent<I_Room>() != null)
                {
                    sex = false;
                    dist = h.distance;
                }
            }
        }
        //Debug.Log(hits);
        if (sex)
        {
            if(p.GetComponent<PlayerController>() != null)
            {
                target = p.gameObject;
            }
            else
            {
                target = p.GetComponent<NavMeshEntity>().target;
            }
        }
        if (range)
        {
            MyAssChecker();
        }
        range = false;
    }

    public void MyAssChecker()
    {
        for (int i = 0; i < Gamer.Instance.EnemiesExisting.Count; i++)
        {
            var pp = Gamer.Instance.EnemiesExisting[i];
            if (pp == null)
            {
                Gamer.Instance.EnemiesExisting.RemoveAt(i);
                i--;
                continue;
            }
            if ((transform.position - pp.transform.position).magnitude <= 10)
            {
                //Debug.Log("Checking for player via spread");
                pp.CheckCanSee(false, gameObject);
            }
        }
    }

    private Quaternion Point2D(float offset2, float spread)
    {
        //returns the rotation required to make the current gameobject point at the mouse, untested in 3D.
        var offset = UnityEngine.Random.Range(-spread, spread);
        offset += offset2;
        //Debug.Log(offset);
        Vector3 difference = NoZ(target.transform.position) - NoZ(transform.position);
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset);
        return sex;
    }
    public Vector3 NoZ(Vector3 s)
    {
        s.z = 0;
        return s;
    }


}
