using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshEntity : MonoBehaviour
{
    public string EnemyType = "Rat";
    public string AttackType = "Melee";
    public float movespeed = 5f;
    public float SightRange = 15f;
    public float AttackCooldown = 1.5f;
    public float randommovetimer = 0f;
    private NavMeshAgent beans;
    public GameObject target;
    public I_Room originroom;
    public double Damage;
    public SpriteRenderer WantASpriteCranberry;
    public List<Sprite> SpriteVarients = new List<Sprite> ();
    private Rigidbody2D sex;
    public GameObject box;
    float timer = 0f;
    public float timer2 = 0f;
    public Vector3 spawn;
    public EnemyHitShit sex2;
    // Start is called before the first frame update
    void Start()
    {
        if(AttackType == "Melee")sex2.Damage = Damage;
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


            switch (AttackType)
            {
                case "Melee":
                    if (target != null && dist <= 2.5f)
                    {
                        timer2 += Time.deltaTime;
                    }
                    box.SetActive(false);
                    if (timer2 > AttackCooldown)
                    {
                        timer2 = 0;
                        //Debug.Log("SHONK");
                        box.transform.rotation = Point2D(-180, 0);
                        box.SetActive(true);
                    }
                    break;
                case "Ranged":
                    if (target != null && canseemysexybooty)
                    {
                        timer2 += Time.deltaTime;
                    }
                    if (timer2 > AttackCooldown)
                    {
                        timer2 = 0;
                        Debug.Log("AttaemptSpawn sex!");

                        var wenis = Instantiate(box, transform.position, PointAtPoint2D(target.transform.position, 0), Gamer.Instance.balls);
                        var e = wenis.GetComponent<EnemyHitShit>();
                        e.Damage = Damage;
                        e.balling = transform;

                    }
                    break;
            }
            if (Mathf.Abs(beans.velocity.x) > 0.1f)
                WantASpriteCranberry.flipX = beans.velocity.x > 0;
            if (target != null)
            {
                beans.speed = movespeed;
                switch (AttackType)
                {
                    case "Ranged":
                        if (canseemysexybooty)
                        {
                            var e = (NoZ(target.transform.position) - NoZ(transform.position)).normalized*-13f + target.transform.position;
                            beans.SetDestination(e);
                        }
                        else
                        {
                            beans.SetDestination(target.transform.position);
                        }
                        break;
                    default:
                        beans.SetDestination(target.transform.position);
                        break;
                }
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
    public bool canseemysexybooty = false;
    public void RandomMove()
    {
        randommovetimer = Random.Range(1f, 7f);
        float wanderrange = 3f;
        beans.SetDestination(spawn + new Vector3(Random.Range(-wanderrange, wanderrange), Random.Range(-wanderrange, wanderrange),0));
    }
    int fuckyouunity = 0;
    public void CheckCanSee(bool range, GameObject shart)
    {
        var p = shart;                                                                                                                                              
        var hit = Physics2D.RaycastAll(transform.position, p.transform.position - transform.position, range?SightRange:(SightRange*1.5f));
        // Does the ray intersect any objects excluding the player layer
        bool sex = false;
        float dist = 69;
        GameObject sexp = null;
        foreach (var h in hit)
        {
            if (h.distance <= dist)
            {
                if (h.collider.GetComponent<NavMeshEntity>() != null) continue;
                if (h.transform == p.transform)
                {
                    sex = true;
                    dist = h.distance;
                    sexp = h.collider.gameObject;
                }
                if (h.transform.parent != null && !h.transform.GetComponent<BoxCollider2D>().isTrigger && h.transform.parent.GetComponent<I_Room>() != null)
                {
                    sex = false;
                    dist = h.distance;
                    sexp = h.collider.gameObject;
                }
            }
        }
        //Debug.Log(hits);
        if (sex)
        {
            //Debug.Log("Assert my balls");
            if (sexp.GetComponent<PlayerController>() != null)
            {
                canseemysexybooty = true;
                fuckyouunity = 4;
                //Debug.Log("Sexyboooty");
                if (target == null) target = p.gameObject;
            }
            else
            {
                var ss = p.GetComponent<NavMeshEntity>();
                if (target == null && ss != null) target = ss.target;
            }
        }
        else
        {
            fuckyouunity--;
            if(canseemysexybooty && fuckyouunity < 0)
            {
                canseemysexybooty = false;
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


    private Quaternion PointAtPoint(Vector3 start_location, Vector3 location)
    {
        Quaternion _lookRotation =
            Quaternion.LookRotation((location - start_location).normalized, Vector3.forward);
        return _lookRotation;
    }
    private Quaternion PointAtPoint2D(Vector3 location, float spread)
    {
        // a different version of PointAtPoint with some extra shtuff
        //returns the rotation the gameobject requires to point at a specific location
        var offset = UnityEngine.Random.Range(-spread, spread);

        //Debug.Log(offset);
        Vector3 difference = NoZ(location) - NoZ(transform.position);
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset);
        return sex;
    }
}
