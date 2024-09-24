
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float move_speed = 2;
    private float working_move_speed = 2;
    private float decay = 0.8f;
    public double Damage = 20;
    public float AttacksPerSecond = 5;
    public float Spread = 15f;
    public float MaxBowMult = 2f;
    public float BowChargeSpeed = 1f;
    public float CritChance = 0.01f;
    public float MaxDashCooldown = 3f;
    private Vector3 move = new Vector3(0, 0, 0);
    public Transform SwordFart;
    public Transform dicksplit;
    public Transform MyAssHurts;
    public Transform ArrowInMyperkyAss;
    public SpriteRenderer ArrowInThyAss;
    public Transform[] cummers = new Transform[2];
    public GameObject[] SlashEffect;
    public GameObject HitCollider;
    public GameObject[] HitColliders;
    private float f = 0;
    private float f2 = 0;
    private int reverse = 1;
    public GISItem mainweapon;
    private double helth = 0;
    public EntityOXS entit;
    private SpriteRenderer dicksplay;
    public static PlayerController Instance;
    float bowsextimer = 0;
    public NetworkObject sexer;
    public int selecteditem = 0;
    public SpawnData spawnData;
    public bool isrealowner;
    public float DashCoolDown = 0f;
    public OcksNetworkVar network_helditem = new OcksNetworkVar();
    private bool HasLoadedWeapon = false;
    public static float BaseDashCooldown = 5f;
    public SpriteRenderer Underlay;
    public WeaponDisplay weewee;
    private void Awake()
    {
        Gamer.Instance.Players.Add(this);
    }

    private void Start()
    {
        
        spawnData = GetComponent<SpawnData>(); 
        if (Gamer.IsMultiplayer)
        {
            sexer = GetComponent<NetworkObject>();
            isrealowner = sexer.IsOwner;
            if (isrealowner && Gamer.Instance.IsHost)
            {
                spawnData.FardStart();
            }
            else if (isrealowner)
            {
                ServerGamer.Instance.MessageServerRpc(RandomFunctions.Instance.ClientID, "WhoAmI", sexer.NetworkObjectId.ToString());
            }
            else
            {
                ServerGamer.Instance.MessageServerRpc(RandomFunctions.Instance.ClientID, "WhoTheFuckIsThisYo", sexer.NetworkObjectId.ToString());
            }
        }
        else
        {
            isrealowner = true;
            spawnData.FardStart();
        }
        if (isrealowner)
        {
            Instance = this;
            StartCoroutine(ArrowSex());
        }
        selecteditem = 0;
        rigid= GetComponent<Rigidbody2D>();
        entit = GetComponent<EntityOXS>();
        dicksplay = dicksplit.GetComponent<SpriteRenderer>();
        oldsex = Underlay.color;
        entit.Shield = 0;
        SetData();
        DashCoolDown = MaxDashCooldown * 3;
        //SetData();


    }
    public bool hasaids = false;
    public void Aids()
    {
        StartCoroutine(AidsFix());
    }


    public IEnumerator AidsFix()
    {
        yield return null;
        network_helditem.SetCreds(spawnData.Hidden_Data[0], "Held Item");
        Console.Log("GIGA CUUM " + spawnData.Hidden_Data[0]);
        if (isrealowner)
        {
            network_helditem.SetValue(mainweapon.ItemToString());
        }
        else
        {
            network_helditem.Query();
            for (int i = 0; i < 1; i++) StartCoroutine(WaitFOrThing(i));
        }
        hasaids = true;
    }
    
    public IEnumerator WaitFOrThing(int i)
    {
        switch (i)
        {
            case 0:
                yield return new WaitUntil(() => { return network_helditem.GetValue() != "WAITING FOR DATA"; });
                SetData();
                HasLoadedWeapon = true;
                break;
        }
    }

    /*
    public IEnumerator WaitForIncomingData(int sex)
    {
        yield return null;
        switch (sex)
        {
            case 0:
                yield return new WaitUntil(() => { return PlayerID.Value.ToString() != "-"; });
                var e = RandomFunctions.Instance.GenerateBlankHiddenData();
                e[0] = PlayerID.Value.ToString();
                spawnData.Hidden_Data = e;
                spawnData.FardStart();
                break;
        }
    }
    */
    private Vector3 wankpos;
    private Vector3 targetpos;
    public IEnumerator ArrowSex()
    {
        ArrowInMyperkyAss.gameObject.SetActive(false);
        var c = ArrowInThyAss.color;
        c.a = 0;
        ArrowInThyAss.color = c;
        var ga = Gamer.Instance;
    boner:
        yield return new WaitUntil(() => { return !ga.InRoom && Gamer.CurrentFloor >= 1; });
        yield return new WaitForSeconds(0.5f);
        if (entit.Health <= 0) yield break;
        wankpos = transform.position;
        ArrowInMyperkyAss.gameObject.SetActive(true);
        bool wanker = !ga.InRoom && Gamer.CurrentFloor >= 1;
        while (wanker)
        {
            if (entit.Health <= 0) yield break;
            c = ArrowInThyAss.color;
            float max = 0.5f;
            if(targetpos != null)
            {
                var d = (RandomFunctions.Instance.Dist(RandomFunctions.Instance.NoZ(transform.position), RandomFunctions.Instance.NoZ(targetpos)) - 8)/8;
                max = Mathf.Min(max, d);
            }
            c.a = Mathf.Clamp(c.a + (1 * Time.deltaTime), 0, max);
            ArrowInThyAss.color = c;
            wankpos = Vector3.Lerp(wankpos, transform.position, Time.deltaTime*15);
            ArrowInMyperkyAss.position = wankpos;
            var room = ga.OldCurrentRoom != null ? ga.OldCurrentRoom : ga.LevelProgression[0];
            int index = ga.LevelProgression.IndexOf(room);
            if (index + 1 < ga.LevelProgression.Count)
            {
                var rm = ga.LevelProgression[index + 1];
                targetpos = rm.transform.position;
                ArrowInMyperkyAss.rotation = Point2DMod2(targetpos, -90, 0);
                if (max <= 0 && rm.isused == "End")
                {
                    goto shank;
                }
            }
            yield return null;
            wanker = !ga.InRoom && Gamer.CurrentFloor >= 1;
        }
        while(c.a > 0)
        {
            if (entit.Health <= 0) yield break;
            wankpos = Vector3.Lerp(wankpos, transform.position, Time.deltaTime * 15);
            ArrowInMyperkyAss.position = wankpos;
            ArrowInMyperkyAss.rotation = Point2DMod2(targetpos, -90, 0);
            c = ArrowInThyAss.color;
            c.a -= 2 * Time.deltaTime;
            ArrowInThyAss.color = c;
            yield return null;
        }
        ArrowInMyperkyAss.gameObject.SetActive(false);
        goto boner;
    shank:
        ArrowInMyperkyAss.gameObject.SetActive(false);
        int curfloor = Gamer.CurrentFloor;
        yield return new WaitUntil(() => { return curfloor != Gamer.CurrentFloor; });
        goto boner;
    }
    public void SetData()
    {
        helth = 100.0;
        if (GISLol.Instance.All_Containers.ContainsKey("Equips"))
        {
            if (isrealowner)
            {
                var c = GISLol.Instance.All_Containers["Equips"];
                mainweapon = c.slots[selecteditem].Held_Item;
                if(hasaids)network_helditem.SetValue(mainweapon.ItemToString());
            }
            else
            {
                try
                {
                    var s = network_helditem.GetValue();
                    GISItem se = new GISItem();
                    se.StringToItem(s);
                    mainweapon = se;
                }
                catch
                {

                }
            }
        }
        else
        {
            mainweapon = null;
            if (isrealowner && hasaids)
            {
                network_helditem.SetValue("");
            }
        }
        var OLDPERC = entit.Health / entit.Max_Health;
        var OLDPERCDASH = DashCoolDown / MaxDashCooldown;
        CritChance = 0.01f;
        working_move_speed = 1.5f;
        Damage = 7;
        AttacksPerSecond = 3.5f;
        Spread = 15f;
        MaxDashCooldown = BaseDashCooldown;
        //deprecated
        MaxBowMult = 1.5f;
        BowChargeSpeed = 1.5f;
        if(mainweapon != null)
        {
            switch (mainweapon.ItemIndex)
            {
                case "Spear":
                    AttacksPerSecond = 1.5f;
                    Damage = 12;
                    break;
                case "Crossbow":
                    AttacksPerSecond = 4f;
                    Damage = 4;
                    Spread = 15f;
                    break;
                case "Bow":
                    AttacksPerSecond = 1.5f;
                    Spread = 5f;
                    Damage = 1000f;
                    break;
                case "Shuriken":
                    AttacksPerSecond = 1.5f;
                    Damage = 6f;
                    break;
                case "Boomerang":
                    AttacksPerSecond = 1.5f;
                    Damage = 5f;
                    break;
                case "Axe":
                    AttacksPerSecond = 1.3f;
                    Damage = 5f;
                    break;
            }
            foreach(var m in mainweapon.Materials)
            {
                ParseMaterial(m);
            }
        }
        /*
        helth += GetItem("steak") * 10;
        Damage += GetItem("what") * 10;
        working_move_speed *= 1 + (GetItem("peed")*0.1f);
        Damage *= 1 + (GetItem("damag")*0.1f);
        AttacksPerSecond *= 1 + (GetItem("atkpeed")*0.1f);
        CritChance += (GetItem("critglass")*0.1f);*/
        entit.Max_Health = helth;
        entit.Health = helth * OLDPERC;
        entit.Max_Shield = helth/2;
        DashCoolDown = MaxDashCooldown * OLDPERCDASH;
        SetMoveSpeed();
        if (CritChance < 0) CritChance = 0;
    }
    public void ParseMaterial(GISMaterial matty)
    {
        switch (matty.index)
        {
            case "Emerald":
                AttacksPerSecond *= 1.15f;
                break;
            case "Gold":
                Damage *= 1.2f;
                AttacksPerSecond *= 0.95f;
                break;
            case "Glass":
                Damage *= 1.3f;
                helth *= 0.85f;
                break;
            case "Amethyst":
                MaxDashCooldown *= 0.85f;
                working_move_speed *= 0.90f;
                break;
            case "Slime":
                Damage *= 1.2f;
                working_move_speed *= 0.90f;
                break;
            case "Demonic Ingot":
                CritChance += 0.20f;
                Damage *= 0.90f;
                break;
            case "Angelic Ingot":
                CritChance -= 0.15f;
                Damage *= 1.15f;
                break;
        }
        switch (matty.itemindex)
        {
            case "DieBitch":
                switch (mainweapon.ItemIndex)
                {
                    default:
                        AttacksPerSecond *= 1.15f;
                        break;
                }
                break;
        }
    }

    private void Update()
    {
        InputBuffer.Instance.BufferListen(InputManager.gamekeys["dash"], "Dash", "player", 0.1f, true);
        InputBuffer.Instance.BufferListen(InputManager.gamekeys["shoot"], "Attack", "player", 0.1f, false);
    }
    float scrollcool;
    private void LateUpdate()
    {
        dicksplit.rotation = Quaternion.identity;
        if (isrealowner)
        {
            if (InputManager.IsKeyDown(KeyCode.Alpha1, "player")) SwitchWeapon(0);
            else if (InputManager.IsKeyDown(KeyCode.Alpha2, "player")) SwitchWeapon(1);
            scrollcool -= Time.deltaTime;
            if(Input.mouseScrollDelta.y != 0 && scrollcool <= 0)
            {
                scrollcool = 0.075f;
                SwitchWeapon(selecteditem + (int)Input.mouseScrollDelta.y);
            }
        }

        if (f < 1)
        {
            f += Time.deltaTime * AttacksPerSecond;
        }
        if (f >= 1)
        {
            f = 1;
            if (f2 > 0)
            {
                f2 -= Time.deltaTime;
            }
        }



        var g = 1 - f;
        g = g * g * g * g;
        if (mainweapon != null)
        {
            SwordFart.localPosition = new Vector3(0, 0, 0);
            MyAssHurts.rotation = SwordFart.rotation;
            switch (mainweapon.ItemIndex)
            {
                case "Sword":
                    SwordFart.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(-121, 121, g) * reverse)) * transform.rotation;
                    break;
                case "Shuriken":
                    SwordFart.rotation = Quaternion.Euler(new Vector3(0, 0, 121 * reverse)) * transform.rotation;
                    break;
                case "Boomerang":
                    SwordFart.rotation = Quaternion.Euler(new Vector3(0, 0, 121 * reverse)) * transform.rotation;
                    MyAssHurts.rotation = SwordFart.rotation * Quaternion.Euler(0, 0, 0);
                    break;
                case "Axe":
                    SwordFart.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(-121, 121, f) * reverse)) * transform.rotation;
                    SwordFart.localPosition = new Vector3(Mathf.Sin(f * Mathf.PI*2) * -0.5f*reverse, Mathf.Sin(f*Mathf.PI)*6f, 0);
                    var fff = Mathf.Cos(f * Mathf.PI);
                    fff *= fff;
                    if(f >= 0.5f)
                    {
                        MyAssHurts.rotation = SwordFart.rotation * Quaternion.Euler(0, 0, reverse * (f * 360 * 2) + Mathf.Lerp(0, (70 * -reverse), fff));
                    }
                    else
                    {
                        MyAssHurts.rotation = SwordFart.rotation * Quaternion.Euler(0, 0, reverse * (f * 360 * 2) + Mathf.Lerp(0, (70 * reverse), fff));
                    }
                    break;
                case "Crossbow":
                    SwordFart.rotation = transform.rotation;
                    break;
                case "Bow":
                    SwordFart.localPosition = new Vector3(0, -1f, 0);
                    SwordFart.rotation = transform.rotation;
                    break;
                case "Spear":
                    if (!sexed && g <= 0.5f)
                    {
                        reverse *= -1;
                        sexed = true;
                    }
                    g = Mathf.Sin(g * Mathf.PI);
                    g = 1 - g;
                    SwordFart.rotation = Quaternion.Euler(new Vector3(0, 0, 11 * reverse)) * transform.rotation;
                    SwordFart.localPosition = new Vector3(Mathf.Lerp(-0.5f, -1.5f, g) * -reverse, Mathf.Lerp(0.5f, -2.5f, g), 0);
                    break;
            }
            if (!Gamer.WithinAMenu)
            {
                int reverse2 = (transform.position - MyAssHurts.transform.position).x < 0 ? 1 : -1;
                switch (mainweapon.ItemIndex)
                {
                    case "Crossbow": SwordFart.localScale = new Vector3(Mathf.Lerp(1, 0.8f, f2 / (1 / AttacksPerSecond) + ((0.2f * 3f) / AttacksPerSecond)) * reverse2, 1, 1); break;
                    case "Shuriken": SwordFart.localScale = new Vector3(reverse2 * (1 - g), (1 - g), (1 - g)); break;
                    case "Boomerang": SwordFart.localScale = new Vector3((1 - g), (1 - g), (1 - g)); break;
                    default: SwordFart.localScale = new Vector3(reverse2, 1, 1); break;
                }
            }
        }
    }

    public void SwitchWeapon(int s3x)
    {
        s3x = RandomFunctions.Instance.Mod(s3x, 2);
        selecteditem = s3x;
        if(HitCollider!=null)HitCollider.SetActive(false);
        SetData();
        weewee.UpdateDisplay();
    }
    Color oldsex;
    [HideInInspector]
    public bool IsDashing = false;
    private string oldval;
    void FixedUpdate()
    {
        if(!isrealowner && HasLoadedWeapon && network_helditem.GetValue() != oldval)
        {
            oldval = network_helditem.GetValue();
            SetData();
        }
        if (entit.Shield > 0) entit.Shield -= 0.1;
        if (HitCollider != null)
        {
            switch (mainweapon.ItemIndex)
            {
                case "Axe":
                    if(f >= 1)
                    {
                        HitCollider.SetActive(false);
                    }
                    break;
                default:
                    HitCollider.SetActive(false);
                    break;
            }
        }
        if (isrealowner)
        {
            SetMoveSpeed();
            move *= decay;
            Vector3 dir = new Vector3(0, 0, 0);
            if (InputManager.IsKey(KeyCode.W, "player")) dir += Vector3.up;
            if (InputManager.IsKey(KeyCode.S, "player")) dir += Vector3.down;
            if (InputManager.IsKey(KeyCode.D, "player")) dir += Vector3.right;
            if (InputManager.IsKey(KeyCode.A, "player")) dir += Vector3.left;
            if (dir.magnitude > 0.5f)
            {
                dir.Normalize();
                move += dir;
            }

            if (mainweapon != null && isrealowner)
            {
                if (mainweapon.ItemIndex == "Bow")
                {
                    if (f2 <= 0 && f >= 1)
                    {
                        if (InputBuffer.Instance.GetBuffer("Attack")) bowsextimer += Time.deltaTime * BowChargeSpeed;
                        if (bowsextimer > MaxBowMult) bowsextimer = MaxBowMult;
                        if (bowsextimer > 0 && !InputBuffer.Instance.GetBuffer("Attack"))
                        {
                            //Debug.Log(bowsextimer + ", " + Damage);
                            StartAttack(Damage * (1 + bowsextimer));
                        }
                    }


                    //if (InputManager.IsKey(InputManager.gamekeys["shoot"]) && f2 <= 0 && f >= 1) StartAttack();
                }
                else
                {
                    if (InputBuffer.Instance.GetBuffer("Attack") && f2 <= 0 && f >= 1) StartAttack();
                }
            }
            Vector3 bgalls = move * Time.deltaTime * move_speed * 20;
            rigid.velocity += new Vector2(bgalls.x, bgalls.y);
            if (isrealowner)
            {
                float sex = Time.deltaTime;
                if (!Gamer.Instance.InRoom) sex *= 5;
                DashCoolDown = Mathf.Clamp(DashCoolDown + sex, 0, MaxDashCooldown * 3);
                bool candash = DashCoolDown >= MaxDashCooldown && !IsDashing;
                if (candash && InputBuffer.Instance.GetBuffer("Dash"))
                {
                    StartDash(dir);
                }
                var c = IsDashing ? (Color)new Color32(15, 140, 0, 255) : oldsex;
                c.a = candash||IsDashing?1:0.3f;
                Underlay.color = c;
            }
            if (CameraLol.Instance != null)
            {
                CameraLol.Instance.targetpos = transform.position;
            }
        }
        if (f >= 1)
        {
            if (isrealowner && !Gamer.WithinAMenu)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Point2D(-90, 0), 25f);
                dicksplay.transform.localScale = new Vector3((transform.position - RandomFunctions.Instance.MousePositon(Camera.main)).x > 0 ? 1 : -1, 1, 1);
            }
        }
    }
    private bool sexed = false;
    public void SetMoveSpeed()
    {
        move_speed = working_move_speed;
        switch (mainweapon.ItemIndex)
        {
            case "Crossbow":
                if (f < 1f || f2 > 0 || bowsextimer > 0) move_speed *= 0.35f;
                break;
            default:
                if (f < 0.85f) move_speed *= 0.35f;
                break;
        }
    }
    public void StartDash(Vector3 dir)
    {
        if (dir.magnitude < 0.5f) return;
        InputBuffer.Instance.RemoveBuffer("Dash");
        SoundSystem.Instance.PlaySound(3, false, 0.14f, 1f);
        SoundSystem.Instance.PlaySound(2, true, 0.2f, 1f);
        IsDashing = true;
        DashCoolDown -= MaxDashCooldown;
        Instantiate(Gamer.Instance.ParticleSpawns[4], transform.position, transform.rotation, transform);
        var c = Tags.refs["DashHolder"].transform;
        Instantiate(Gamer.Instance.ParticleSpawns[5], c.position, c.rotation, c);
        StartCoroutine(Dash(dir));
    }

    public IEnumerator Dash(Vector3 dir)
    {
        for(int i = 0; i < 7; i++)
        {
            rigid.velocity += (Vector2)(dir*17);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.1f);
        IsDashing = false;
    }
    public void StartAttack(double d = -1)
    {
        InputBuffer.Instance.RemoveBuffer("Attack");
        if (d == -1) d = Damage;
        if (mainweapon.ItemIndex == "Empty") return;
        sexed = false;
        f = 0;
        f2 = (0.2f * 3) / AttacksPerSecond;
        var ghghg = RandomFunctions.Instance.MousePositon(Camera.main);
        ghghg.z = 1;
        var fdsadfd = transform.position;
        fdsadfd.z = 1;
        var epe = (ghghg - fdsadfd).normalized * 30;
        GameObject s;
        Slasher s2;
        HitBalls s3;
        Projectile s4;

        DamageProfile Shart = new DamageProfile("PlayerAttack", Damage);
        Shart.CritChance = CritChance;
        Shart.controller = this;
        var ff = Random.Range(0f, 1f);
        int tt = Mathf.FloorToInt(CritChance);
        Shart.PreCritted = tt + (ff<(CritChance%1)?2:1);
        //Debug.Log($"D: {CritChance}, {GetItem("critglass")}, {Shart.PreCritted}, {Shart.CalcDamage()}");

        switch (mainweapon.ItemIndex)
        {
            case "Sword":
                s = Instantiate(SlashEffect[0], transform.position + transform.up * 2.3f, transform.rotation);
                s.GetComponent<SpriteRenderer>().flipX = reverse > 0;
                s.GetComponent<Slasher>().wait = (0.1f * 3) / AttacksPerSecond;
                reverse *= -1;
                HitCollider = HitColliders[0];
                Shart.PreCritted = -1;
                break;
            case "Axe":
                reverse *= -1;
                HitCollider = HitColliders[2];
                Shart.PreCritted = -1;
                epe *= -0.5f;
                f2 = (0.2f) / AttacksPerSecond;
                break;
            case "Spear":
                s = Instantiate(SlashEffect[1], transform.position + transform.up * 2.3f, transform.rotation);
                s.GetComponent<SpriteRenderer>().flipX = reverse > 0;
                s2 = s.GetComponent<Slasher>();
                s2.wait = (0.05f * 1.5f) / AttacksPerSecond;
                s2.speedmult = 8f;
                HitCollider = HitColliders[1];
                Shart.PreCritted = -1;
                break;
            case "Crossbow":
                s = Instantiate(SlashEffect[2], SlashEffect[3].transform.position, transform.rotation * Quaternion.Euler(new Vector3(0, 0, Random.Range(Spread / 2, -Spread / 2))));
                s3 = s.GetComponent<HitBalls>();
                s3.playerController = this;
                s3.attackProfile = Shart;
                epe *= -0.5f;
                HitCollider = null;
                f = 1;
                f2 = (1 / AttacksPerSecond) + ((0.2f * 3f) / AttacksPerSecond);
                break;
            case "Bow":
                for(int i = 0; i < 1; i++)
                {
                    s = Instantiate(SlashEffect[2], SlashEffect[3].transform.position, transform.rotation * Quaternion.Euler(new Vector3(0, 0, (Random.Range(Spread / 2, -Spread / 2)) + (15*i))));
                    s3 = s.GetComponent<HitBalls>();
                    s3.playerController = this;
                    s3.attackProfile = Shart;
                }
                epe *= -0.5f;
                HitCollider = null;
                f = 1;
                f2 = (1 / AttacksPerSecond) + ((0.2f * 3f) / AttacksPerSecond);
                break;
            case "Shuriken":
                s = Instantiate(SlashEffect[4], MyAssHurts.position, Point2DMod(MyAssHurts.position, -90, 0));
                s3 = s.GetComponent<HitBalls>();
                s3.playerController = this;

                var x = RandomFunctions.Instance.Dist(RandomFunctions.Instance.NoZ(Camera.main.ScreenToWorldPoint(Input.mousePosition)), RandomFunctions.Instance.NoZ(MyAssHurts.position));
                if (x < 10)
                {
                    x /= 10;
                    s.GetComponent<Projectile>().speed *= x;
                }

                s3.attackProfile = Shart;
                s3.hsh *= -reverse;
                epe *= -0.5f;
                HitCollider = null;
                reverse *= -1;
                var ra = GISDisplay.GetSprites(mainweapon);
                s3.spriteballs[0].sprite = ra.sprites[0];
                s3.spriteballs[0].color = ra.colormods[0];
                s3.spriteballs[1].sprite = ra.sprites[1];
                s3.spriteballs[1].color = ra.colormods[1];
                s3.spriteballs[2].sprite = ra.sprites[2];
                s3.spriteballs[2].color = ra.colormods[2];
                break;
            case "Boomerang":
                s = Instantiate(SlashEffect[4], MyAssHurts.position, Point2DMod(MyAssHurts.position, -90, 0));
                s3 = s.GetComponent<HitBalls>();
                s3.playerController = this;

                var x2 = RandomFunctions.Instance.Dist(RandomFunctions.Instance.NoZ(Camera.main.ScreenToWorldPoint(Input.mousePosition)), RandomFunctions.Instance.NoZ(MyAssHurts.position));
                var sss = s.GetComponent<Projectile>();
                sss.Banan = "Boomerang";
                s3.attackProfile = Shart;
                s3.hsh *= -reverse;
                s3.type = "Boomerang";
                epe *= -0.5f;
                HitCollider = null;
                reverse *= -1;
                var ra2 = GISDisplay.GetSprites(mainweapon);
                s3.spriteballs[0].sprite = ra2.sprites[0];
                s3.spriteballs[0].color = ra2.colormods[0];
                s3.spriteballs[1].sprite = ra2.sprites[1];
                s3.spriteballs[1].color = ra2.colormods[1];
                s3.spriteballs[2].sprite = ra2.sprites[2];
                s3.spriteballs[2].color = ra2.colormods[2];
                break;
            default:
                HitCollider = HitColliders[0];
                reverse *= -1;
                break;
        }
        if (isrealowner)
        {
            if(AttacksPerSecond >= 10)
            {
                var x = AttacksPerSecond / 10f;
                epe /= x;
            }
            rigid.velocity += new Vector2(epe.x, epe.y);
        }
        if (HitCollider != null)
        {
            HitCollider.SetActive(true);
            HitCollider.GetComponent<HitBalls>().attackProfile = Shart;
        }

        if (isrealowner && Gamer.IsMultiplayer)ServerGamer.Instance.MessageServerRpc(RandomFunctions.Instance.ClientID, "PAtt", spawnData.Hidden_Data[0]);
        bowsextimer = 0;
    }

    private Quaternion Point2D(float offset2, float spread)
    {
        //returns the rotation required to make the current gameobject point at the mouse, untested in 3D.
        var offset = UnityEngine.Random.Range(-spread, spread);
        offset += offset2;
        //Debug.Log(offset);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset);
        return sex;
    }
    private Quaternion Point2DMod(Vector3 pos, float offset2, float spread)
    {
        //returns the rotation required to make the current gameobject point at the mouse, untested in 3D.
        var offset = UnityEngine.Random.Range(-spread, spread);
        offset += offset2;
        //Debug.Log(offset);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset);
        return sex;
    }

    private Quaternion Point2DMod2(Vector3 pos, float offset2, float spread)
    {
        //returns the rotation required to make the current gameobject point at the mouse, untested in 3D.
        var offset = UnityEngine.Random.Range(-spread, spread);
        offset += offset2;
        //Debug.Log(offset);
        Vector3 difference = pos - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset);
        return sex;
    }


    public void HitEnemy(EntityOXS enem, DamageProfile dam)
    {
        if (!dam.SpecificLocation)
        {
            dam.SpecificLocation = true;
            dam.AttackerPos = transform.position;
        }
        enem.Hit(dam);
    }

}

