
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshEntity : MonoBehaviour
{
    public string EnemyType = "Rat";
    public string AttackType = "Melee";
    public int ImagePixelSize = 18;
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
    public EntityOXS EntityOXS;
    float timer = 0f;
    public float timer2 = 0f;
    public Vector3 spawn;
    public EnemyHitShit sex2;
    public bool HasSpawned=  false;
    public string EliteType = "";
    public event Gamer.JustFuckingRunTheMethods CLearShit;
    // Start is called before the first frame update
    void Start()
    {
        if(AttackType == "Melee")sex2.Damage = Damage;
        beans = GetComponent<NavMeshAgent>();
        sex = GetComponent<Rigidbody2D>();
        EntityOXS = GetComponent<EntityOXS>();
        WantASpriteCranberry = GetComponent<SpriteRenderer>();
        transform.rotation = Quaternion.identity;
        beans.updateRotation= false;
        beans.updateUpAxis= false;
        WantASpriteCranberry.flipX = Random.Range(0, 2) == 1;
        WantASpriteCranberry.sprite = SpriteVarients[Random.Range(0, SpriteVarients.Count)];
        randommovetimer = 0;
        switch (EnemyType)
        {
            case "Charger":
                CLearShit += box.GetComponent<EnemyHitShit>().OnSpawn;
                break;
            case "Rat":
                CLearShit += box.GetComponentInChildren<EnemyHitShit>().OnSpawn;
                break;
        }
        SightRange = 95f;
        StartCoroutine(SpawningLol());
    }

    public IEnumerator SpawningLol()
    {
        this.EntityOXS.AntiDieJuice = true;
        sex.bodyType = RigidbodyType2D.Static;
        var a = Instantiate(Gamer.Instance.SpawnFix, transform.position, transform.rotation, transform).GetComponent<SpriteRenderer>();
        var esex = Instantiate(Gamer.Instance.ParticleSpawns[3], transform.position, transform.rotation, transform).GetComponent<partShitBall>();
        esex.Particicic.localScale = new Vector3(beans.radius, beans.radius, beans.radius);
        a.sprite = WantASpriteCranberry.sprite;
        a.material = Gamer.Instance.sexex[1];
        var c = (Color)new Color32(120, 0, 255, 0);
        a.color = c;
        WantASpriteCranberry.enabled = false;
        for (int i = 0; i < 80; i++)
        {
            yield return new WaitForFixedUpdate();
            c.a += 0.0125f;
            a.color = c;
        }
        //yield return new WaitForSeconds(10f);
        HasSpawned = true;
        WantASpriteCranberry.enabled = true;
        Destroy(a.gameObject);
        sex.bodyType = RigidbodyType2D.Dynamic;
        this.EntityOXS.AntiDieJuice = false;
    }


    private void Update()
    {
        if(!HasSpawned) return;
        switch (EnemyType)
        {
            case "Charger":
                if(ddist < SightRange)
                {
                    transform.position += chargedir * alt_speed * Time.deltaTime;
                }
                break;
        }
    }
    public bool existing = false;
    public float ddist;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!HasSpawned) return;
        if (!existing)
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
        float dist=690;
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
        ddist= dist;
        if(nearestnerd != null && dist <= 100)
        {

            if (dist <= SightRange)
            {
                CheckCanSee(true, PlayerController.Instance.gameObject);
            }
            switch (EnemyType)
            {
                case "Charger":
                    if (!charging2)
                    {
                        chargedir *= 0.9f;
                    }
                    else
                    { 
                    }
                    break;
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
                        CLearShit?.Invoke();
                        box.SetActive(true);
                    }
                    break;
                case "Ranged":
                    if (target != null && canseemysexybooty && !charging)
                    {
                        timer2 += Time.deltaTime;
                    }
                    if (timer2 > AttackCooldown)
                    {
                        timer2 = 0;
                        switch (EnemyType)
                        {
                            case "Charger":
                                StartCoroutine(ChargeSex());
                                break;
                            default:

                                var wenis = Instantiate(box, transform.position, PointAtPoint2D(target.transform.position, 0), Gamer.Instance.balls);
                                var e = wenis.GetComponent<EnemyHitShit>();
                                e.Damage = Damage;
                                e.balling = transform;
                                break;
                        }

                    }
                    break;
            }
            if (charging)
            {
                WantASpriteCranberry.flipX = chargedir.x > 0;
            }
            else
            {
                if (Mathf.Abs(beans.velocity.x) > 0.1f)
                    WantASpriteCranberry.flipX = beans.velocity.x > 0;
            }
            if (target != null)
            {
                beans.speed = movespeed;
                switch (EnemyType)
                {
                    case "Spitter":
                        if (canseemysexybooty)
                        {
                            var e = (NoZ(target.transform.position) - NoZ(transform.position)).normalized*-7f + target.transform.position;
                            beans.SetDestination(e);
                        }
                        else
                        {
                            beans.SetDestination(target.transform.position);
                            beans.speed = movespeed*2f;
                        }
                        if (dist >= 22f)
                        {
                            beans.speed *= 1.5f;
                        }
                        break;
                    case "Charger":
                        beans.SetDestination(target.transform.position);
                        if (dist >= 15f)
                        {
                            beans.speed = movespeed * 2.5f;
                        }
                        break;
                    default:
                        beans.SetDestination(target.transform.position);
                        if (dist >= 22f)
                        {
                            beans.speed = movespeed * 1.5f;
                        }
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
    public bool charging = false;
    public bool charging2 = false;
    public float alt_speed = 10f;
    private Vector3 chargedir = Vector3.zero;
    public IEnumerator ChargeSex()
    {
        float premove = movespeed;
        movespeed = 0.5f;
        charging = true;
        yield return new WaitForSeconds(0.3f);
        chargedir = (NoZ(target.transform.position) - NoZ(transform.position)).normalized;
        movespeed = 0;
        charging2 = true;
        CLearShit?.Invoke();
        box.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        charging = false;
        charging2 = false;
        movespeed = premove;
        timer2 = Random.Range(-0.25f, 0.25f);
        box.SetActive(false);
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
        var hit = Physics2D.RaycastAll(transform.position, p.transform.position - transform.position, SightRange);
        // Does the ray intersect any objects excluding the player layer
        bool sex = false;
        float dist = 69;
        GameObject sexp = null;
        foreach (var h in hit)
        {
            if (h.distance <= dist)
            {
                var obj = Gamer.Instance.GetObjectType(h.collider.gameObject, true);
                if (obj.type == "Enemy") continue;
                if (h.transform == p.transform)
                {
                    sex = true;
                    dist = h.distance;
                    sexp = h.collider.gameObject;
                }
                if (obj.type == "Wall")
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

    private Quaternion RotateTowards(GameObject target, float max_angle_change)
    {
        Vector3 a = target.transform.position;
        var b = Quaternion.LookRotation((a - transform.position).normalized);
        return Quaternion.RotateTowards(transform.rotation, b, max_angle_change);
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
