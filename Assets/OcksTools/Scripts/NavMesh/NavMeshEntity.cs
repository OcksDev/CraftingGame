
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

public class NavMeshEntity : MonoBehaviour
{
    public string Name = "Booty";
    public string EnemyType = "Rat";
    public string AttackType = "Melee";
    public int ImagePixelSize = 18;
    public bool FlipImage= false;
    public float SpawnOverlapRadius = 1;
    public float movespeed = 5f;
    public float SightRange = 15f;
    public float AttackCooldown = 1.5f;
    public float randommovetimer = 0f;
    [HideInInspector]
    public NavMeshAgent beans;
    public GameObject target;
    public I_Room originroom;
    public double Damage;
    public SpriteRenderer WantASpriteCranberry;
    public List<Sprite> SpriteVarients = new List<Sprite> ();
    public List<Sprite> SpriteMiscRefs = new List<Sprite> ();
    public List<GameObject> EnableOnTrueSpawn = new List<GameObject> ();
    private Rigidbody2D sex;
    public GameObject box;
    public EntityOXS EntityOXS;
    float timer = 0f;
    public float timer2 = 0f;
    public Vector3 spawn;
    public EnemyHitShit sex2;
    public bool HasSpawned=  false;
    public string EliteType = "";
    public EnemyHolder EnemyHolder;
    public long creditsspent = 0;
    int curcycycle = 0;
    public event Gamer.JustFuckingRunTheMethods CLearShit;
    [HideInInspector]
    public double BaldMaxHealth = 0f;
    [HideInInspector]
    public float BaldAttackCooldown = 0f;
    [HideInInspector]
    public float BaldMoveSpeed = 0f;
    [HideInInspector]
    public float BaldAltMoveSpeed = 0f;
    [HideInInspector]
    public double BaldDamage = 0f;
    private Animator anime;
    public float TurnSpeen = 0f;
    bool CanChangeIMg = false;
    // Start is called before the first frame update
    public void Start()
    {
        curcycycle = Gamer.EnemyCheckoffset;
        Gamer.EnemyCheckoffset = RandomFunctions.Instance.Mod(Gamer.EnemyCheckoffset,8);
        if (AttackType == "Melee")sex2.Damage = Damage;
        switch (EnemyType)
        {
            case "Worm":
                CanChangeIMg = false;
                break;
            default:
                CanChangeIMg = true;
                break;
        }
        if (!HasSpawned)
        {
            beans = GetComponent<NavMeshAgent>();
            sex = GetComponent<Rigidbody2D>();
            EntityOXS = GetComponent<EntityOXS>();
            anime = GetComponent<Animator>();
            WantASpriteCranberry = GetComponent<SpriteRenderer>();
            transform.rotation = Quaternion.identity;
            beans.updateRotation = false;
            beans.updateUpAxis = false;
            if(CanChangeIMg)WantASpriteCranberry.flipX = Random.Range(0, 2) == 1;
            WantASpriteCranberry.sprite = SpriteVarients[Random.Range(0, SpriteVarients.Count)];
            BaldMaxHealth = EntityOXS.Max_Health;
            BaldDamage = Damage;
            BaldAltMoveSpeed = alt_speed;
            BaldMoveSpeed = movespeed;
            BaldAttackCooldown = AttackCooldown;
        }
        else
        {
            AttackCooldown = BaldAttackCooldown;
            movespeed = BaldMoveSpeed;
            Damage = BaldDamage;
            alt_speed = BaldAltMoveSpeed;
            EntityOXS.Max_Health = BaldMaxHealth;
        }
        randommovetimer = 0;
        SightRange = 95f;
        if(!HasSpawned)
        switch (EnemyType)
        {
            case "Slimer":
            case "Charger":
                CLearShit += box.GetComponent<EnemyHitShit>().OnSpawn;
                break;
            case "Rat":
                CLearShit += box.GetComponentInChildren<EnemyHitShit>().OnSpawn;
                break;
        }
        if(EliteType != "")
        {
            EntityOXS.Max_Health *= 1.2f;
            EntityOXS.Health = EntityOXS.Max_Health;
        }
        switch (EliteType)
        {
            case "Hasty":
                AttackCooldown /= 1.5f;
                movespeed *= 1.5f;
                alt_speed *= 1.5f;
                break;
            case "Resilient":
                EntityOXS.Max_Health *= 1.5f;
                EntityOXS.Health = EntityOXS.Max_Health;
                break;
            case "Corrupted":
                EntityOXS.Max_Health *= 6f;
                EntityOXS.Health = EntityOXS.Max_Health;
                AttackCooldown /= 1.5f;
                movespeed *= 1.5f;
                alt_speed *= 1.5f;
                break;
            case "Perfected":
                EntityOXS.Max_Health *= 5f;
                EntityOXS.Health = EntityOXS.Max_Health;
                Damage *= 3f;
                break;
            case "Powerful":
                Damage *= 2f;
                break;
            case "Unstable":
                StartCoroutine(UnstableBalling());
                break;
            case "Mending":
                StartCoroutine(MendingBalling());
                break;
        }
        switch (EnemyType)
        {
            case "Orb":
                SightRange = 20f;
                timer2 = AttackCooldown / 2;
                break;
            case "Fwog":
                timer2 = AttackCooldown / 2;
                break;
            case "Worm":
                TurnSpeen = alt_speed * 0.00833333333f;
                break;
        }
        if(!HasSpawned)StartCoroutine(SpawningLol());
    }

    public IEnumerator SpawningLol()
    {
        if (EnemyHolder != null && EnemyHolder.InstantSpawn) goto balls;
        this.EntityOXS.AntiDieJuice = true;
        sex.bodyType = RigidbodyType2D.Static;
        var a = Instantiate(Gamer.Instance.SpawnFix, transform.position, transform.rotation, transform).GetComponent<SpriteRenderer>();
        var esex = Instantiate(Gamer.Instance.ParticleSpawns[3], transform.position, transform.rotation, transform).GetComponent<partShitBall>();
        var www = Mathf.Clamp(beans.radius, 0, 1.25f);
        esex.Particicic.localScale = new Vector3(www, www, www);
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
        Destroy(a.gameObject);
        balls:
        HasSpawned = true;
        WantASpriteCranberry.enabled = true;
        sex.bodyType = RigidbodyType2D.Dynamic;
        this.EntityOXS.AntiDieJuice = false;
        foreach(var aww in EnableOnTrueSpawn)
        {
            aww.SetActive(true);
        }
        switch (EnemyType)
        {
            case "Worm":
                chargedir = Quaternion.Euler(0,0,Random.Range(-360f,360f)) * new Vector3(1, 0, 0);
                GetComponent<BodyFollower>().SpawnNerds();
                //EnableOnTrueSpawn[0].GetComponent<ParticleSystem>().Play();
                break;
        }
    }



    public IEnumerator UnstableBalling()
    {
        yield return new WaitUntil(() => { return HasSpawned; });
        yield return new WaitUntil(() => { return target != null; });
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            var w = Instantiate(Gamer.Instance.UnstableBullet, transform.position, PointAtPoint2D(target.transform.position, 45), Gamer.Instance.balls);
            var e = w.GetComponent<EnemyHitShit>();
            e.balling = transform;
            e.sexballs = this;
            e.overridedamage = Damage / 2;
        }
    }
    public IEnumerator MendingBalling()
    {
        yield return new WaitUntil(() => { return HasSpawned; });
        while (true)
        {
            yield return new WaitForSeconds(1f);
            List<NavMeshEntity> nearbois = new List<NavMeshEntity>();
            foreach(var e in Gamer.Instance.EnemiesExisting)
            {
                if (e == this || e.EliteType == "Mending") continue;
                if(RandomFunctions.Instance.Dist(transform.position, e.transform.position) <= 8)
                {
                    nearbois.Add(e);
                }
            }
            foreach(var e2 in nearbois)
            {
                if (e2 == null || e2.EntityOXS == null) continue;
                var h = e2.EntityOXS.Health;
                e2.EntityOXS.Heal(e2.EntityOXS.Max_Health / 10);
                if(h != e2.EntityOXS.Health)
                {
                    var wankjob = Instantiate(Gamer.Instance.HealBeam);
                    var esus = wankjob.GetComponent<LineKiller>();
                    esus.coolpos = transform;
                    esus.coolerpos = e2.transform;
                }
            }
        }
    }

    public static string GetName(NavMeshEntity boner)
    {
        string ba = boner.Name;
        if(boner.EliteType != "")
        {
            ba = $"{boner.EliteType} {ba}";
        }
        return ba;
    }
    bool ANTICHARGEFART = false;
    public float distancetraveleleled = 0;
    private void Update()
    {
        if(!HasSpawned) return;
        switch (EnemyType)
        {
            case "Charger":
                if(ddist < SightRange)
                {
                    var WEEEE = alt_speed * Time.deltaTime;
                    transform.position += chargedir * WEEEE;
                    if(ANTICHARGEFART && (distancetraveleleled += WEEEE) >= 1.5f)
                    {
                        distancetraveleleled = 0;
                        var wenis = Instantiate(RandomFunctions.Instance.SpawnRefs[2], transform.position, Quaternion.identity, Gamer.Instance.balls);
                        var e = wenis.GetComponent<EnemyHitShit>();
                        e.Damage = Damage;
                        e.balling = transform;
                        e.sexballs = this;
                    }
                }
                break;
            case "Worm":
            case "Fwog":
            case "Slimer":
                transform.position += chargedir * alt_speed * Time.deltaTime;
                break;
        }
    }
    public bool existing = false;
    public float ddist;
    private Vector3 TotalVelocity = Vector3.zero;
    bool canrunattacktimer = true;
    const float sexcum = 180 / Mathf.PI;

    public void SetMoveSpeeds()
    {
        float mult = 1;

        if(EntityOXS.Effects.Count > 0)
        {
            var cd = EntityOXS.ContainsEffect("Freeze");
            if (cd.hasthing)
            {
                float bigG = 1;
                bigG += cd.susser.Stack * 0.5f;
                mult /= bigG;
            }
        }


        movespeed = BaldMoveSpeed * mult;
        alt_speed = BaldAltMoveSpeed * mult;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        curcycycle = (curcycycle + 1) % (EnemyType=="Worm"? 3: 8);
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
        SetMoveSpeeds();
        ddist= dist;
        if(nearestnerd != null && dist <= 100)
        {

            if (dist <= SightRange && curcycycle == 1)
            {
                CheckCanSee(true, PlayerController.Instance.gameObject);
            }
            switch (EnemyType)
            {
                case "Slimer":
                    if (!charging2)
                    {
                        chargedir *= 0.9f;
                    }
                    else
                    {
                        chargedir = (chargedir + (GetDir() - NoZ(transform.position)).normalized*0.08f).normalized;
                    }
                    break;
                case "Worm":
                    if(target != null && !Gamer.Instance.IsPosInBounds(transform.position))
                    {
                        var pos = target.transform.position - transform.position;
                        float curdie = Mathf.Atan2(chargedir.y, chargedir.x);
                        float curdie2 = curdie - Mathf.PI;
                        float curdie3 = curdie + Mathf.PI;
                        float targetangle = Mathf.Atan2(pos.y, pos.x);
                        float ta = targetangle - curdie;
                        float ta2 = targetangle - curdie2;
                        float ta3 = targetangle - curdie3;
                        if (Mathf.Abs(ta2) < Mathf.Abs(ta)) ta = -ta2;
                        if (Mathf.Abs(ta3) < Mathf.Abs(ta)) ta = -ta3;
                        var diff = Mathf.Clamp(ta, -0.5f, 0.5f) * sexcum * TurnSpeen;
                        var qwat = Quaternion.Euler(0, 0, diff);
                        chargedir = qwat * chargedir;
                    }

                    break;
                case "Fwog":
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
                case "Forever":
                    if (target != null && !charging && canrunattacktimer)
                    {
                        timer2 += Time.deltaTime;
                    }
                    if (timer2 > AttackCooldown)
                    {
                        canrunattacktimer = true;
                        switch (EnemyType)
                        {
                            case "Slimer":
                                timer2 = 0;
                                StartCoroutine(SlimerSex());
                                break;
                            case "Wraith":
                                timer2 = 0;
                                if(target != null)
                                StartCoroutine(WraithSex());
                                break;
                        }
                    }
                    break;
                case "Ranged":
                    if (target != null && canseemysexybooty && !charging && canrunattacktimer)
                    {
                        timer2 += Time.deltaTime;
                    }
                    if (timer2 > AttackCooldown)
                    {
                        canrunattacktimer = true;
                        switch (EnemyType)
                        {
                            case "Charger":
                                timer2 = 0;
                                StartCoroutine(ChargeSex());
                                break;
                            case "Orb":
                                if (ddist <= 20f)
                                {
                                    timer2 = 0;
                                    StartCoroutine(OrbSex());
                                }
                                break;
                            case "Fwog":
                                if (ddist <= 16f)
                                {
                                    timer2 = 0;
                                    StartCoroutine(FwogSex(false));
                                }
                                else
                                {
                                    timer2 = 0;
                                    StartCoroutine(FwogSex(true));
                                }
                                break;
                            case "Bat":
                                timer2 = 0;
                                StartCoroutine(BatSex());
                                break;
                            case "Cannon":
                                timer2 = 0;
                                StartCoroutine(CannonSex());
                                break;
                            case "Spitter2.0":
                                timer2 = 0;
                                StartCoroutine(Spiter2Sex());
                                break;
                            case "Spitter":
                                timer2 = 0;
                                StartCoroutine(SpiterSex());
                                break;
                            case "EyeOrb":
                                timer2 = 0;
                                StartCoroutine(SnormSex());
                                break;
                            case "Worm":
                                timer2 = 0;
                                StartCoroutine(EyeSex());
                                break;
                            case "Cloak":
                                timer2 = 0;
                                StartCoroutine(CloakSex());
                                break;
                            case "Handless":
                                timer2 = 0;
                                StartCoroutine(HandlessSex());
                                break;
                            case "Rocky":
                                timer2 = 0;
                                StartCoroutine(RockySex());
                                break;
                            default:
                                timer2 = 0;
                                var wank = PointAtPoint2D(target.transform.position, 0);
                                var wenis = Instantiate(box, transform.position, wank, Gamer.Instance.balls);
                                var e = wenis.GetComponent<EnemyHitShit>();
                                e.Damage = Damage;
                                e.balling = transform;
                                e.sexballs = this;
                                var w2 = wank * new Vector3(-5, 0, 0);
                                sex.velocity += (Vector2)w2;
                                break;
                        }

                    }
                    break;
            }
            TotalVelocity = beans.velocity + (Vector3)sex.velocity;
            if (CanChangeIMg)
            {
                if (charging)
                {
                    WantASpriteCranberry.flipX = chargedir.x > 0 ^ FlipImage;
                }
                else
                {
                    if (Mathf.Abs(beans.velocity.x) > 0.1f)
                        WantASpriteCranberry.flipX = beans.velocity.x > 0 ^ FlipImage;
                    switch (EnemyType)
                    {
                        case "Cannon":
                            anime.speed = TotalVelocity.magnitude / 2;
                            break;
                    }
                }
            }
            else
            {
                switch (EnemyType)
                {
                    case "Worm":
                        transform.rotation = RandomFunctions.PointAtPoint2D(transform.position, transform.position + chargedir, 0);
                        break;
                }
            }
            
            if (target != null)
            {
                beans.speed = movespeed;
                switch (EnemyType)
                {
                    case "Bat":
                        if (canseemysexybooty)
                        {
                            var e = (NoZ(target.transform.position) - NoZ(transform.position)).normalized * -6f + target.transform.position;
                            beans.SetDestination(e);
                        }
                        else
                        {
                            beans.SetDestination(target.transform.position);
                            beans.speed = movespeed * 3f;
                        }
                        if (dist >= 15f)
                        {
                            beans.speed *= 1.5f;
                        }
                        break;
                    case "Elec":
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
                    case "Cannon":
                        if (canseemysexybooty)
                        {
                            var e = (NoZ(target.transform.position) - NoZ(transform.position)).normalized*-11f + target.transform.position;
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
                    case "Worm":
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var weenor = Gamer.Instance.GetObjectType(collision.gameObject);
        if (weenor.type == "Furniture")
        {
            weenor.FuckYouJustGodDamnRunTheShittyFuckingDoOnTouchMethodsAlreadyIWantToStabYourEyeballsWithAFork();
        }
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        var weenor = Gamer.Instance.GetObjectType(collision.gameObject);
        if (weenor.type == "Void")
        {
            if(EliteType != "Corrupted")
            {
                EliteType = "Corrupted";
                EntityOXS.DamageTimer = 0.1f;
                Start();
            }
        }
    }


    public IEnumerator ChargeSex()
    {
        distancetraveleleled = 0;
        float premove = movespeed;
        movespeed = 0.5f;
        charging = true;
        yield return new WaitForSeconds(0.3f);
        ANTICHARGEFART = true;
        chargedir = (GetDir() - NoZ(transform.position)).normalized;
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
        yield return new WaitForSeconds(0.13f);
        ANTICHARGEFART = false;
    }
    public IEnumerator SlimerSex()
    {
        charging = true;
        chargedir = Quaternion.Euler(0,0,Random.Range(-15f,15f)) * (GetDir() - NoZ(transform.position)).normalized;
        charging2 = true;
        CLearShit?.Invoke();
        box.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        charging = false;
        charging2 = false;
        timer2 = Random.Range(-0.25f, 0.25f);
        box.SetActive(false);
    }
    public Vector3 GetDir()
    {
        return NoZ(beans.path.corners[1]);
    }
    public IEnumerator SpiterSex()
    {
        WantASpriteCranberry.sprite = SpriteMiscRefs[0];
        float f = movespeed;
        movespeed = 0;
        yield return new WaitForSeconds(0.25f);
        SoundSystem.Instance.PlaySound(16, true, 0.1f);
        yield return new WaitForSeconds(0.07f);
        if (target == null) yield break;
        var wank = PointAtPoint2D(target.transform.position, 0);
        var wenis = Instantiate(box, transform.position, wank, Gamer.Instance.balls);
        var e = wenis.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        WantASpriteCranberry.sprite = SpriteVarients[0];
        var w2 = wank * new Vector3(-5, 0, 0);
        sex.velocity += (Vector2)w2;
        movespeed = f;
        timer2 = Random.Range(-0.25f, 0.25f);
    }
    public IEnumerator Spiter2Sex()
    {
        WantASpriteCranberry.sprite = SpriteMiscRefs[0];
        float f = movespeed;
        movespeed = 0;
        yield return new WaitForSeconds(0.20f);
        SoundSystem.Instance.PlaySound(16, true, 0.1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        var wank = PointAtPoint2D(target.transform.position, 0);
        SpawnBox(transform.position, wank);
        SpawnBox(transform.position, wank * Quaternion.Euler(0, 0, 25));
        SpawnBox(transform.position, wank * Quaternion.Euler(0, 0, -25));
        SpawnBox(transform.position, wank * Quaternion.Euler(0, 0, 50));
        SpawnBox(transform.position, wank * Quaternion.Euler(0, 0, -50));

        WantASpriteCranberry.sprite = SpriteVarients[0];
        var w2 = wank * new Vector3(-5, 0, 0);
        sex.velocity += (Vector2)w2;
        movespeed = f;
        timer2 = Random.Range(-0.25f, 0.25f);
    }
    public IEnumerator CannonSex()
    {
        float f = movespeed;
        movespeed = 0;
        yield return new WaitForSeconds(0.25f);
        //SoundSystem.Instance.PlaySound(16, true, 0.1f);
        yield return new WaitForSeconds(0.05f);
        var wank = PointAtPoint2D(target.transform.position, 0);
        Vector3 off = new Vector3(0.782000005f, 0.144999996f, 0);
        if (WantASpriteCranberry.flipX) off.x *= -1;
        var wenis = Instantiate(box, transform.position+off, wank, Gamer.Instance.balls);
        var e = wenis.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        var w2 = wank * new Vector3(-5, 0, 0);
        sex.velocity += (Vector2)w2;
        movespeed = f;
        timer2 = Random.Range(-0.25f, 0.25f);
    }
    public IEnumerator WraithSex()
    {
        Instantiate(Gamer.Instance.ParticleSpawns[24], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
        var path = Instantiate(Gamer.Instance.ParticleSpawns[26], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
        var targetpos = GetDir();

        float dist = Mathf.Min(RandomFunctions.Instance.Dist(transform.position, targetpos)+2, alt_speed);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        transform.position = transform.position + ((targetpos-transform.position).normalized * dist);
        Instantiate(Gamer.Instance.ParticleSpawns[25], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
        path.transform.position = transform.position;
        if (!canseemysexybooty) goto weeno;
        yield return new WaitForSeconds(0.35f);
        var wank = PointAtPoint2D(target.transform.position, 0);
        Vector3 off = new Vector3(0, 0, 0);
        if (WantASpriteCranberry.flipX) off.x *= -1;
        var wenis = Instantiate(box, transform.position+off, wank, Gamer.Instance.balls);
        var e = wenis.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        var w2 = wank * new Vector3(-5, 0, 0);
        sex.velocity += (Vector2)w2;
        weeno:
        timer2 = Random.Range(-0.25f, 0.25f);
    }
    public IEnumerator EyeSex()
    {
        float f = movespeed;
        movespeed = 0;
        yield return new WaitForSeconds(0.25f);
        //SoundSystem.Instance.PlaySound(16, true, 0.1f);
        yield return new WaitForSeconds(0.05f);
        var wank = PointAtPoint2D(target.transform.position, 0);
        SpawnBox(transform.position, wank);
        SpawnBox(transform.position, wank * Quaternion.Euler(0, 0, 25));
        SpawnBox(transform.position, wank * Quaternion.Euler(0, 0, -25));
        var w2 = wank * new Vector3(-5, 0, 0);
        sex.velocity += (Vector2)w2;
        movespeed = f;
        timer2 = Random.Range(-0.25f, 0.25f);
    }
    public IEnumerator SnormSex()
    {
        float f = movespeed;
        movespeed = 0;
        var wank = PointAtPoint2D(target.transform.position, 0);
        SpawnBox(transform.position, wank);
        SpawnBox(transform.position, wank * Quaternion.Euler(0, 0, 25));
        SpawnBox(transform.position, wank * Quaternion.Euler(0, 0, -25));
        var w2 = wank * new Vector3(-5, 0, 0);
        sex.velocity += (Vector2)w2;
        movespeed = f;
        yield return null;
    }
    public IEnumerator BatSex()
    {
        var wank = PointAtPoint2D(target.transform.position, 0);

        var wenis = Instantiate(box, transform.position, wank, Gamer.Instance.balls);
        var e = wenis.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        wenis = Instantiate(box, transform.position, wank, Gamer.Instance.balls);
        e = wenis.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        e.GetComponent<MoverSexBalls>().sin_alphamale *= -1;
        var w2 = wank * new Vector3(-5, 0, 0);
        sex.velocity += (Vector2)w2;
        timer2 = Random.Range(-0.25f, 0.25f);
        yield return null;
    }
    public IEnumerator CloakSex()
    {
        float f = movespeed;
        movespeed = 0;
        canrunattacktimer = false;
        WantASpriteCranberry.sprite = SpriteMiscRefs[0];
        yield return new WaitForSeconds(0.15f);
        WantASpriteCranberry.sprite = SpriteMiscRefs[1];
        yield return new WaitForSeconds(0.15f);
        WantASpriteCranberry.sprite = SpriteMiscRefs[2];
        yield return new WaitForSeconds(0.15f);
        int i = Random.Range(0, 2);
        var wank = PointAtPoint2D(target.transform.position, 0);
        Vector3 pos = transform.position + new Vector3(0, 1.15f, 0);
        var wenis = Instantiate(box, pos, wank, Gamer.Instance.balls);
        var e = wenis.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        var w2 = wank * new Vector3(-5, 0, 0);
        sex.velocity += (Vector2)w2;
        switch (i)
        {
            case 1:
                wenis = Instantiate(box, pos, wank * Quaternion.Euler(0, 0, 45), Gamer.Instance.balls);
                e = wenis.GetComponent<EnemyHitShit>();
                e.Damage = Damage;
                e.balling = transform;
                e.sexballs = this;
                wenis = Instantiate(box, pos, wank * Quaternion.Euler(0, 0, -45), Gamer.Instance.balls);
                e = wenis.GetComponent<EnemyHitShit>();
                e.Damage = Damage;
                e.balling = transform;
                e.sexballs = this;
                break;
            default:
                yield return new WaitForSeconds(0.3f);
                pos = transform.position + new Vector3(0, 1.15f, 0);
                wenis = Instantiate(box, pos, wank, Gamer.Instance.balls);
                e = wenis.GetComponent<EnemyHitShit>();
                e.Damage = Damage;
                e.balling = transform;
                e.sexballs = this;
                w2 = wank * new Vector3(-5, 0, 0);
                sex.velocity += (Vector2)w2;
                yield return new WaitForSeconds(0.3f);
                pos = transform.position + new Vector3(0, 1.15f, 0);
                wenis = Instantiate(box, pos, wank, Gamer.Instance.balls);
                e = wenis.GetComponent<EnemyHitShit>();
                e.Damage = Damage;
                e.balling = transform;
                e.sexballs = this;
                w2 = wank * new Vector3(-5, 0, 0);
                sex.velocity += (Vector2)w2;
                break;
        }
        yield return new WaitForSeconds(0.15f);
        WantASpriteCranberry.sprite = SpriteMiscRefs[1];
        yield return new WaitForSeconds(0.15f);
        WantASpriteCranberry.sprite = SpriteMiscRefs[0];
        yield return new WaitForSeconds(0.15f);
        WantASpriteCranberry.sprite = SpriteVarients[0];
        movespeed = f;
        canrunattacktimer = true;
        timer2 = Random.Range(-0.25f, 0.25f);
    }
    public IEnumerator HandlessSex()
    {
        float f = movespeed;
        movespeed = 0;
        canrunattacktimer = false;
        WantASpriteCranberry.sprite = SpriteMiscRefs[0];
        yield return new WaitForSeconds(0.3f);
        WantASpriteCranberry.sprite = SpriteMiscRefs[1];

        yield return new WaitForSeconds(0.3f);

        var w = SpawnBox(transform.position + new Vector3(0,3.5f,0)).GetComponent<DangerCircleScrip>();
        w.www = target.transform;
        yield return new WaitForSeconds(2f);
        WantASpriteCranberry.sprite = SpriteMiscRefs[0];
        yield return new WaitForSeconds(0.30f);
        WantASpriteCranberry.sprite = SpriteVarients[0];
        movespeed = f;
        canrunattacktimer = true;
    }
    public IEnumerator RockySex()
    {
        float f = movespeed;
        movespeed = 0;
        canrunattacktimer = false;
        Instantiate(Gamer.Instance.ParticleSpawns[11], transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(0.30f);
        WantASpriteCranberry.sprite = SpriteMiscRefs[0];
        yield return new WaitForSeconds(0.3f);
        int i = Random.Range(0,2);
        Vector3 dir;
        Vector3 initpos;
        switch (i)
        {
            default:
                dir = (target.transform.position - transform.position).normalized;
                var dir2 = Quaternion.Euler(0, 0, 25) * dir;
                var dir3 = Quaternion.Euler(0, 0, -25) * dir;
                var dir4 = Quaternion.Euler(0, 0, 50) * dir;
                var dir5 = Quaternion.Euler(0, 0, -50) * dir;
                initpos = transform.position;
                for (int it = 1; it <= 7; it++)
                {
                    SpawnBox(initpos + (dir * (2.5f * it)));
                    SpawnBox(initpos + (dir2 * (2.5f * it)));
                    SpawnBox(initpos + (dir3 * (2.5f * it)));
                    SpawnBox(initpos + (dir4 * (2.5f * it)));
                    SpawnBox(initpos + (dir5 * (2.5f * it)));
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case 0:
                initpos = target.transform.position;
                int max = 8;
                float dist = 3f;
                SpawnBox(initpos);
                for (int aw = 0; aw < 4; aw++)
                {
                    for (int it = 1; it <= max; it++)
                    {
                        float perc = it / (float)max;
                        SpawnBox(initpos + (Quaternion.Euler(0, 0, perc * 360) * new Vector3(0, dist, 0)));
                    }
                    max += 0;
                    dist += 3f;
                    yield return new WaitForSeconds(0.1f);
                }
                break;
        }



        yield return new WaitForSeconds(0.30f);
        WantASpriteCranberry.sprite = SpriteVarients[0];
        yield return new WaitForSeconds(0.30f);
        movespeed = f;
        canrunattacktimer = true;
        timer2 = Random.Range(-0.25f, 0.25f);
    }
    private EnemyHitShit SpawnBox(Vector3 pos)
    {
        var wenis = Instantiate(box, pos, Quaternion.identity, Gamer.Instance.balls);
        var e = wenis.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        return e;
    }
    private EnemyHitShit SpawnBox(Vector3 pos, Quaternion rot)
    {
        var wenis = Instantiate(box, pos, rot, Gamer.Instance.balls);
        var e = wenis.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        return e;
    }


    public IEnumerator OrbSex()
    {
        SoundSystem.Instance.PlaySound(17, true, 0.05f);
        var wenis = Instantiate(box, transform.position, PointAtPoint2D(target.transform.position, 0), Gamer.Instance.balls);
        Instantiate(Gamer.Instance.ParticleSpawns[6], transform.position, Quaternion.identity, transform);

        var e2 = wenis.GetComponent<DeathBeamScript>();
        e2.Player = target.transform;
        e2.SorceNerd = transform;
        e2.UpdatePos();


        var e = e2.fardd.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        yield return new WaitForSeconds(0.9f);
        SoundSystem.Instance.PlaySound(18, false, 0.15f, 1f);
    }
    public IEnumerator FwogSex(bool ump = false)
    {
        if (ump) goto wank;
        canrunattacktimer = false;
        WantASpriteCranberry.sprite = SpriteMiscRefs[0];
        yield return new WaitForSeconds(0.2f);
        var wenis = Instantiate(box, transform.position, PointAtPoint2D(target.transform.position, 0), Gamer.Instance.balls);

        var e2 = wenis.GetComponent<DeathBeamScript>();
        e2.Player = target.transform;
        e2.SorceNerd = transform;
        e2.offset = new Vector3(0.363000005f, 0.465999991f, 0);
        if (WantASpriteCranberry.flipX) e2.offset.x *= -1;
        e2.UpdatePos();


        var e = e2.fardd.GetComponent<EnemyHitShit>();
        e.Damage = Damage;
        e.balling = transform;
        e.sexballs = this;
        yield return new WaitForSeconds(0.9f);
        WantASpriteCranberry.sprite = SpriteVarients[0];
        yield return new WaitForSeconds(0.2f);

        wank:

        canrunattacktimer = true;
        charging = true;
        chargedir = (GetDir() - NoZ(transform.position)).normalized;
        movespeed = 0;
        charging2 = true;
        CLearShit?.Invoke();
        yield return new WaitForSeconds(0.3f);
        charging = false;
        charging2 = false;
        timer2 = Random.Range(-0.25f, 0.25f);

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
        var hit = Physics2D.RaycastAll(transform.position, p.transform.position - transform.position, RandomFunctions.Instance.Dist(p.transform.position, transform.position));
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
        if (target == null) target = PlayerController.Instance.gameObject;
        if (sex)
        {
            if (OXComponent.GetComponent<PlayerController>(sexp) != null)
            {
                canseemysexybooty = true;
                fuckyouunity = (EnemyType == "Worm" ? 0 : 3);
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
        return;
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
