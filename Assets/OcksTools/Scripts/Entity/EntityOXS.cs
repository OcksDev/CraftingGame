using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class EntityOXS : MonoBehaviour
{
    public string EnemyType = "Enemy";
    public double Health = 100;
    public double Shield = 0f;
    public double Max_Health = 100f;
    public double Max_Shield = 100f;
    public List<EffectProfile> Effects = new List<EffectProfile>();
    public float DamageTimer = 0.1f;
    private SpriteRenderer ren;
    private Rigidbody2D rg;
    public Color32 col;
    public NavMeshEntity sexy;
    private PlayerController playerdaddy;
    public int healerstospawn = 1;
    public bool AntiDieJuice = false;
    private DamageProfile lasthit;
    [HideInInspector]
    public List<DamageProfile> ricks = new List<DamageProfile>();
    public List<SpriteRenderer> additionalnerds = new List<SpriteRenderer>();
    private void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        if(EnemyType == "Player")
        {
            DamageTimer = -1f;
            playerdaddy = OXComponent.GetComponent<PlayerController>(gameObject);
            ren = playerdaddy.dicksplit.GetComponent<SpriteRenderer>();
        }
        else
        {
            sexy = GetComponent<NavMeshEntity>();
        }
        rg = GetComponent<Rigidbody2D>();
        if (ren != null)
        {
            col = ren.color;
        }
        additionalnerds.Insert(0, ren);
    }
    private double damagefromhit;
    public void Hit(DamageProfile hit)
    {
        List<DamageProfile> stored_hits = new List<DamageProfile>();
        damagefromhit = hit.CalcDamage();
        if (AntiDieJuice) return;
        if (Gamer.GameState == "Dead") return;
        if (rg != null && hit.SpecificLocation)
        {
            var ewanker = ((Vector2)transform.position - (Vector2)hit.AttackerPos).normalized * hit.Knockback * 2.5f;
            rg.velocity += ewanker;
        }
        bool wasticked = false;
        switch (EnemyType)
        {
            case "Enemy":
                lasthit = hit;
                if (sexy != null)
                {
                    SoundSystem.Instance.PlaySound(1, true, 0.2f, 3f);
                    sexy.target = hit.attacker;
                    sexy.MyAssChecker();
                }
                break;
            case "Player":
                int xxx = playerdaddy.mainweapon.ReadItemAmount("Rune Of Splitting")+1;
                if(xxx > 1 && hit.storedticks == -69)
                {
                    hit.storeditem = playerdaddy.mainweapon;
                    hit.storedticks = xxx-1;
                    hit.ticktimer = playerdaddy.DamageTickTime;
                    lasthit = hit;
                    ricks.Add(hit);
                    wasticked = true;
                }
                else
                {
                    lasthit = hit;
                }
                if(hit.storeditem != null)
                {
                    xxx = hit.storeditem.ReadItemAmount("Rune Of Splitting")+1;
                    hit.ticktimer = playerdaddy.DamageTickTime;
                    damagefromhit /= xxx;
                    wasticked = !wasticked;
                }
                if (!wasticked && DamageTimer > 0) return;
                break;
            default:
                lasthit = hit;
                break;
        }
        foreach (var effect in hit.Effects)
        {
            AddEffect(effect);
        }
        PlayerController s2 = null;
        PlayerController s = null;
        if (hit.attacker != null)
        {
            s = OXComponent.GetComponent<PlayerController>(hit.attacker);
        }
        switch (EnemyType)
        {
            case "Enemy":
                Gamer.Instance.SetLastEnemy(sexy);
                if (Gamer.IsMultiplayer)
                {
                    if (s != null && !s.isrealowner) { return; }
                }
                break;
        }
        bool block = false;
        switch (EnemyType)
        {
            case "Player":
                s2 = OXComponent.GetComponent<PlayerController>(gameObject);
                if (!s2.isrealowner) break;
                damagefromhit -= s2.mainweapon.ReadItemAmount("Shungite");
                if (damagefromhit < 1) damagefromhit = 1;

                if(Shield > 0.1)
                {
                    var weenor = s2.mainweapon.RollLuck(s2.BarrierBlockChance, true);
                    if (weenor == 0) block = true;
                }

                if (s2.IsDashing || block)
                {
                    block = true;
                }
                else
                {
                    //var y = s2.GetItem("blocker");
                    var y = 0;
                    float x = ((float)y) / (19f + y);
                    //Debug.Log("shar: " + x);
                    if (Random.Range(0f, 1f) > x)
                    {
                        if (PlayerController.Instance == s2)
                        {
                            CameraLol.Instance.Shake(wasticked?0.1f:0.4f, 0.87f);
                            Gamer.Instance.ShartPoop += wasticked ? 0.1f:0.4f;
                        }
                        Shield -= damagefromhit;
                        if (Shield < 0)
                        {
                            Health += Shield;
                        }
                        var arr = s2.mainweapon.ReadItemAmount("Rune Of Retaliation");
                        if(arr >= 1)
                        {
                            NavMeshEntity cuumer = NearestEnemyFrom(transform.position);
                            if(cuumer != null)
                            {
                                var we = s2.GetDamageProfile();
                                we.Damage = damagefromhit;
                                we.DamageMod = arr/2f;
                                cuumer.EntityOXS.Hit(we);
                            }

                        }
                        arr = s2.mainweapon.ReadItemAmount("Rune Of Retribution");
                        if(arr >= 1)
                        {
                            var we = s2.GetDamageProfile();
                            SpawnExplosion((arr*2)+4, transform.position, we, 10);
                        }
                    }
                    else
                    {
                        block = true;
                        //blocked!
                    }

                }
                if (block)
                {
                    SoundSystem.Instance.PlaySound(6, true, wasticked? 0.2f:0.5f, 0.75f);
                }
                else
                {
                    SoundSystem.Instance.PlaySound(4, true, wasticked ? 0.28f:0.7f, 1f);
                }

                break;
            default:
                var aaaaa = Gamer.Instance.GetObjectType(hit.attacker, false);
                if (aaaaa.type == "Player")
                {
                    PlayerController.Instance.DashCoolDown += PlayerController.BaseDashCooldown / 10f;
                    if (hit.WeaponOfAttack != null)
                    {
                        var arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Self") * 0.25f;
                        if (arr > 0 && !hit.Procs.Contains("HOH"))
                        {
                            int tt2 = hit.WeaponOfAttack.RollLuck(arr);
                            if (tt2 > 0)
                            {
                                hit.Procs.Add("HOH");
                                s.entit.currentprof = hit;
                                s.entit.Heal(tt2);
                            }
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Missile") * 0.1f;
                        if (arr > 0 && !hit.Procs.Contains("Missile"))
                        {
                            int tt2 = hit.WeaponOfAttack.RollLuck(arr);
                            for (int i = 0; i < tt2; i++)
                            {
                                var attack = new DamageProfile(hit);
                                attack.Procs.Add("Missile");
                                attack.DamageMod *= 2;
                                var weenis = hit.attacker.transform;
                                var we = Instantiate(RandomFunctions.Instance.SpawnRefs[1], weenis.position, PointFromTo2D(hit.attacker.transform.position, transform.position, 90 + Random.Range(-90f, 90f)), Gamer.Instance.balls).GetComponent<MissileMover>();
                                we.hitbal.attackProfile = attack;
                                we.target = gameObject;
                            }
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Excitation");
                        if (arr > 0)
                        {
                            if ((aaaaa.playerController.MaxTimeSinceDamageDealt-aaaaa.playerController.timersincedamage)>0.1f)
                            {
                                Instantiate(Gamer.Instance.ParticleSpawns[21], hit.attacker.transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                            }
                            aaaaa.playerController.timersincedamage = aaaaa.playerController.MaxTimeSinceDamageDealt;
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Collapse")*0.5f;
                        if (arr > 0)
                        {
                            var weenor = new EffectProfile("Collapse", 9999999, 7, 1);
                            weenor.storeint = aaaaa.playerController.selecteditem;
                            weenor.storedouble = hit.Damage;
                            weenor.storefloat = arr;
                            AddEffect(weenor);
                        }
                        var ee = ContainsEffect("Collapse");
                        if (ee.hasthing)
                        {
                            for(int i = 0; i < Effects.Count; i++)
                            {
                                if (Effects[i].Type == "Collapse")
                                {
                                    Effects.RemoveAt(i);
                                    i--;
                                }
                            }
                            var attack = new DamageProfile(hit, true);
                            attack.Damage = (ee.susser.Stack * ee.susser.storedouble) * ee.susser.storefloat;
                            stored_hits.Add(attack);
                        }
                    }
                }
                Shield -= damagefromhit;
                if (Shield < 0)
                {
                    Health += Shield;
                }
                break;
        }

        var xx = (transform.localScale.x / 2) - 0.25f;
        var yy = (transform.localScale.y / 2) - 0.25f;
        GameObject e = null;
        if (damagefromhit > 0)
        {
            e = RandomFunctions.Instance.SpawnObject(0, Tags.refs["DIC"], (hit.IsSpecificPointOfDamage? transform.position + (transform.rotation * hit.SpecificPointOfDamage): transform.position) + new Vector3(Random.Range(-xx, xx), Random.Range(-yy, yy), 0), Quaternion.identity);
        }
        if (e != null)
        {
            var fard = e.GetComponent<DamIndi>();
            if (block)
            {
                fard.sex.text = "Blocked";
                fard.critlevel = -1;
                fard.NoCLor = true;
                fard.sex.color = new Color32(109, 147, 207,255);
            }
            else
            {
                fard.sex.text = RandomFunctions.Instance.NumToRead(((System.Numerics.BigInteger)System.Math.Round(damagefromhit )).ToString());
                fard.critlevel = hit.WasCrit;
                DamageTimer = 0.1f;
            }
        }
        if(EnemyType == "Enemy" && s != null)
        {
            var arr = s.mainweapon.ReadItemAmount("Void") * 0.01f;
            if(s.mainweapon.RollLuck(arr, true) > 0)
            {
                var weenor = OXComponent.GetComponent<NavMeshEntity>(gameObject);
                if(weenor != null && weenor.EliteType != "Corrupted")
                {
                    Health = 1;
                    weenor.EliteType = "Corrupted";
                    weenor.Start();
                }
            }
        }
        currentprof = null;
        foreach(var asexy in stored_hits)
        {
            Hit(asexy);
        }
    }


    public void SpawnExplosion(float size, Vector3 pos, DamageProfile dam, double damage = 15)
    {
        var weenis = Instantiate(Gamer.Instance.ParticleSpawns[16], pos, Quaternion.identity).GetComponent<partShitBall>();
        float truesz = size / 5;
        weenis.Particicic.localScale = new Vector3(truesz, truesz, 1);
        var we = Physics2D.OverlapCircleAll((Vector2)pos, size/2);
        foreach(var nerd in we)
        {
            if (nerd.gameObject == gameObject || nerd.gameObject == null) continue;
            var ob = Gamer.Instance.GetObjectType(nerd.gameObject);
            if(ob != null && ob.gm != null && ob.type == "Enemy" && ob.entityoxs != null && ob.entityoxs.Health > 0.5f)
            {
                var wank = new DamageProfile(dam);
                wank.Damage = damage;
                wank.DamageMod = 1;
                ob.entityoxs.Hit(wank);
            }
        }
    }
    [HideInInspector]
    public DamageProfile currentprof;

    public void Heal(double amount)
    {
        var oldh = Health;
        Health = System.Math.Clamp(Health + amount, 0, Max_Health);
        var change = amount-( Health - oldh);
        var olds = Shield;
        Shield = System.Math.Clamp(Shield + change, 0, Max_Shield);
        var change2 = change - (Shield - olds);
        if (Health != oldh || Shield != olds)
        {
            var xx = (transform.localScale.x / 2) - 0.25f;
            var yy = (transform.localScale.y / 2) - 0.25f;
            GameObject e = RandomFunctions.Instance.SpawnObject(0, Tags.refs["DIC"], transform.position + new Vector3(Random.Range(-xx, xx), Random.Range(-yy, yy), 0), Quaternion.identity);
            if (e != null)
            {
                var fard = e.GetComponent<DamIndi>();
                fard.sex.text = RandomFunctions.Instance.NumToRead(((System.Numerics.BigInteger)System.Math.Round(amount-change2)).ToString());
                fard.critlevel = -1;
                fard.NoCLor = true;
                fard.sex.color = new Color32(26, 217, 61, 255);
            }
        }
        switch (EnemyType)
        {
            case "Player":
                var arr = playerdaddy.mainweapon.ReadItemAmount("Rune Of Confluence") * 0.35f;
                if (arr > 0)
                {
                    var tt2 = playerdaddy.mainweapon.RollLuck(arr);
                    if (tt2 > 0)
                    {
                        NavMeshEntity cuumer = NearestEnemyFrom(transform.position);
                        if (cuumer != null)
                        {
                            var we = currentprof == null ? playerdaddy.GetDamageProfile() : currentprof;
                            we.Damage = amount * (tt2 * 2);
                            we.DamageMod = 1;
                            cuumer.EntityOXS.Hit(we);
                        }
                    }
                }
                break;
        }
        currentprof = null;
    }

    public NavMeshEntity NearestEnemyFrom(Vector3 pos)
    {
        NavMeshEntity cuumer = null;
        float dist = 69696969;
        foreach (var gw in Gamer.Instance.EnemiesExisting)
        {
            if (!gw.HasSpawned || gw.EntityOXS.Health <= 0.5f)
            {
                continue;
            }
            var dd = RandomFunctions.Instance.DistNoSQRT(pos, gw.transform.position);
            if (dd < dist)
            {
                dist = dd;
                cuumer = gw;
            }
        }
        return cuumer;
    }

    private Quaternion PointFromTo2D(Vector3 from_pos, Vector3 to_pos, float offset2)
    {
        //returns the rotation required to make the current gameobject point at the mouse, this method is 2D only.
        Vector3 difference = from_pos - to_pos;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset2);
        return sex;
    }
    public void Kill()
    {
        switch (EnemyType)
        {
            case "Enemy":
                SoundSystem.Instance.PlaySound(1, true, 0.7f, 1f);


                int effect = -1;
                var aa = OXComponent.GetComponent<NavMeshEntity>(gameObject);
                switch (aa.EnemyType)
                {
                    case "Charger":
                        effect = 1;
                        break;
                    case "Rocky":
                        effect = 10;
                        break;
                    case "EyeOrb":
                        effect = 19;
                        break;
                    case "Cannon":
                        effect = 20;
                        break;
                    case "Worm":
                        effect = 23;
                        break;
                    case "Slimer":
                        aa.EnableOnTrueSpawn[0].transform.parent = Tags.refs["ParticleHolder"].transform;
                        aa.EnableOnTrueSpawn[0].GetComponent<ParticleSystem>().Stop();
                        var ww = aa.EnableOnTrueSpawn[0].AddComponent<partShitBall>();
                        ww.lifetime = 1f;
                        effect = 7;
                        break;
                    default:
                        effect = 0;
                        break;
                }
                switch (aa.EliteType)
                {
                    case "Splitting":
                        var w1 = new EnemyHolder( aa.EnemyHolder);
                        w1.InstantSpawn = true;
                        w1.CanBeElite = false;
                        var w2 = new EnemyHolder(w1);
                        w1.SpawnPos = transform.position + new Vector3(0.5f, 0, 0);
                        w2.SpawnPos = transform.position - new Vector3(0.5f, 0, 0);
                        Gamer.Instance.SpawnEnemy(w1);
                        Gamer.Instance.SpawnEnemy(w2);
                        break;
                }
                if (Gamer.Instance.EnemiesExisting.Contains(aa)) Gamer.Instance.EnemiesExisting.Remove(aa);
                PlayerController.Instance.DashCoolDown += PlayerController.BaseDashCooldown;
                if (effect>-1)Instantiate(Gamer.Instance.ParticleSpawns[effect], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                CameraLol.Instance.Shake(0.25f, 0.80f);
                int he = healerstospawn;
                if(lasthit != null)
                {
                    var arr2 = lasthit.WeaponOfAttack.ReadItemAmount("Rune Of Kaboom") * 2;
                    arr2 += 3;
                    if (arr2 > 4)
                    {
                        SpawnExplosion(arr2, transform.position, lasthit);
                    }
                    var arr = lasthit.WeaponOfAttack.ReadItemAmount("Rune Of Soul") * 0.15f;
                    if (arr > 0)
                    {
                        he += lasthit.WeaponOfAttack.RollLuck(arr);
                    }
                }
                Gamer.Instance.SpawnHealers(transform.position, he, PlayerController.Instance);
                if(lasthit.WeaponOfAttack != null) Gamer.QuestProgressIncrease("Kill", lasthit.WeaponOfAttack.ItemIndex);
                break;
            case "Player":
                if(!PlayerController.Instance.DeathDisable) Gamer.Instance.StartCoroutine(Gamer.Instance.DeathAnim());
                return;
        }
        Destroy(gameObject);
    }
    bool oldstatus = false;
    bool curstatus = false;
    private void Update()
    {
        if (AntiDieJuice) return;
        if (additionalnerds.Count > 0)
        {
            curstatus = DamageTimer >= 0;
            if (curstatus != oldstatus)
            {
                var w1 = Gamer.Instance.sexex[DamageTimer >= 0 ? 3 : 2];
                var w3 = Gamer.Instance.sexex[DamageTimer >= 0 ? 1 : 0];
                foreach (var ren in additionalnerds)
                {
                    if (sexy != null && sexy.EliteType != "")
                    {
                        var w2 = 1f / sexy.ImagePixelSize;
                        ren.material = w1;
                        ren.material.color = Gamer.Instance.EliteTypesDict[sexy.EliteType].color;
                        ren.material.SetFloat("_OutlineThickness", w2);
                    }
                    else
                    {
                        ren.material = w3;
                    }
                    ren.color = DamageTimer >= 0 ? new Color32(255, 255, 255, 255) : col;
                }
                oldstatus = curstatus;
                if (!curstatus && sexy != null)
                {
                    switch (sexy.EnemyType)
                    {
                        case "Worm":
                            GetComponent<BodyFollower>().ForceUpdateStatus();
                            break;
                    }
                }
            }
        }
        DamageTimer -= Time.deltaTime;
        Health = System.Math.Clamp(Health, 0, Max_Health);
        Shield = System.Math.Clamp(Shield, 0, Max_Shield);
        for(int i = 0; i < Effects.Count; i++)
        {
            Effects[i].TimeRemaining -= Time.deltaTime;
            if (Effects[i].TimeRemaining <= 0)
            {
                Effects.RemoveAt(i);
                i--;
            }
        }
        if (EnemyType == "Player"? Health <= 0 : Health <= 0.5f)
        {
            Kill();
        }
    }

    private ret_cum_shenan ContainsEffect(EffectProfile eff)
    {
        bool alreadyhaseffect = false;
        EffectProfile s = null;
        foreach (var ef in Effects)
        {
            if (eff.Type == ef.Type)
            {
                s = ef;
                alreadyhaseffect = true;
                break;
            }
        }
        var ee = new ret_cum_shenan();
        ee.hasthing = alreadyhaseffect;
        ee.susser = s;
        return ee;
    }
    private ret_cum_shenan ContainsEffect(string eff)
    {
        bool alreadyhaseffect = false;
        EffectProfile s = null;
        foreach (var ef in Effects)
        {
            if (eff == ef.Type)
            {
                switch (eff)
                {
                    case "Collapse":
                        if(ef.storeint == lasthit.controller.selecteditem) goto ahh;
                        break;
                }
                s = ef;
                alreadyhaseffect = true;
                break;
            }
        ahh:;
        }
        var ee = new ret_cum_shenan();
        ee.hasthing = alreadyhaseffect;
        ee.susser = s;
        return ee;
    }

    public void AddEffect(EffectProfile eff)
    {
        eff.TimeRemaining = eff.Duration;
        bool alreadyhaseffect = false;
        EffectProfile s = null;
        foreach(var ef in Effects)
        {
            if (eff.Type == ef.Type)
            {
                s = ef;
                alreadyhaseffect = true;
                break;
            }
        }
        if (alreadyhaseffect)
        {
            switch (eff.CombineMethod)
            {
                default:
                    //replace existing effect with new
                    Effects.Remove(s);
                    Effects.Add(eff);
                    break;
                case 1:
                    //apply effect as new
                    Effects.Add(eff);
                    break;
                case 2:
                    //increase stack count
                    s.Stack++;
                    break;
                case 3:
                    //increase stack count, up to maximum value
                    s.Stack++;
                    if (s.Stack > s.MaxStack) s.Stack = s.MaxStack;
                    break;
                case 4:
                    //increase stack count, up to maximum value, refresh duration
                    s.Stack++;
                    s.TimeRemaining = eff.Duration;
                    if (s.Stack > s.MaxStack) s.Stack = s.MaxStack;
                    break;
                case 5:
                    //add old time remaining with new time (2s + 5s = 7s)
                    s.TimeRemaining += eff.Duration;
                    break;
                case 6:
                    //add old time remaining with new time (2s + 5s = 7s), also increase stack count
                    s.TimeRemaining += eff.Duration;
                    s.Stack++;
                    break;
                case 7:
                    //increase stack count, refresh time remaining
                    s.Stack++;
                    s.TimeRemaining = eff.Duration;
                    break;
            }
        }
        else
        {
            Effects.Add(eff);
        }


    }


}
class ret_cum_shenan
{
    public bool hasthing;
    public EffectProfile susser;
}

public class DamageProfile
{
    public string Name = "";
    public double Damage;
    public List<EffectProfile> Effects = new List<EffectProfile>();
    public List<string> Procs = new List<string>();
    public bool SpecificLocation = false;
    public Vector3 AttackerPos = Vector3.zero;
    public float Knockback = 0f;
    public GameObject attacker = null;
    public string NerdType = "Player";
    public PlayerController controller = null;
    public NavMeshEntity entity = null;
    public double CritChance = 0;
    public int PreCritted = -1;
    public int WasCrit = -1;
    public double DamageMod = 1;
    public GISItem WeaponOfAttack;
    public GISItem storeditem;
    public int storedticks = -69;
    public float ticktimer = 0;
    public bool IsSpecificPointOfDamage = false;
    public Vector3 SpecificPointOfDamage = Vector3.zero;
    public DamageProfile(string name, double damage)
    {
        Damage = damage;
        Name = name;
    }
    public DamageProfile(DamageProfile pp)
    {
        dinkle(pp);
        WeaponOfAttack = new GISItem(pp.WeaponOfAttack);
    }
    public DamageProfile(DamageProfile pp, bool ahh)
    {
        dinkle(pp);
        WeaponOfAttack = pp.WeaponOfAttack;
    }
    private void dinkle(DamageProfile pp)
    {
        Name = pp.Name;
        Damage = pp.Damage;
        Procs = new List<string>(pp.Procs);
        Effects = new List<EffectProfile>(pp.Effects);
        AttackerPos = pp.AttackerPos;
        SpecificLocation = pp.SpecificLocation;
        Knockback = pp.Knockback;
        attacker = pp.attacker;
        WasCrit = pp.WasCrit;
        PreCritted = pp.PreCritted;
        CritChance = pp.CritChance;
        entity = pp.entity;
        controller = pp.controller;
        NerdType = pp.NerdType;
        DamageMod = pp.DamageMod;
    }



    public double CalcDamage()
    {
        var x = Damage;
        WasCrit = -1;
        if (PreCritted > -1)
        {
            x *= PreCritted;
            if (PreCritted > 1)
            {
                WasCrit = PreCritted - 2;
            }
        }
        else
        {
            var ff = Random.Range(0f, 1f);
            int tt = (int)System.Math.Floor(CritChance);
            var shex = tt + (ff < (CritChance % 1) ? 2 : 1);
            if (shex > 1)
            {
                WasCrit = shex - 2;
            }
            x *= shex;

        }
        if(controller != null)
        {
            x *= controller.TotalDamageMod;
        }

        return x * DamageMod;
    }
}

public class EffectProfile
{
    //data you pass in
    public string Type;
    public float Duration;
    public int CombineMethod;
    //other data
    public int Stack = 1;
    public float TimeRemaining;
    public int MaxStack;
    public GISItem ItemOfInit;
    public int storeint;
    public double storedouble;
    public float storefloat;
    public EffectProfile(string type, float time, int add_method, int stacks = 1)
    {
        SetData();
        Type = type;
        Duration = time;
        CombineMethod = add_method;
        Stack =stacks;
    }
    public EffectProfile()
    {
        SetData();
    }

    public void SetData()
    {
        MaxStack = 0;
        switch (Type)
        {
            //some example effects
            case "Healing Energy":
                MaxStack = 6;
                break;
        }
    }
    public EffectProfile(EffectProfile pp)
    {
        Type = pp.Type;
        Duration = pp.Duration;
        CombineMethod = pp.CombineMethod;
        Stack = pp.Stack;
        TimeRemaining = pp.TimeRemaining;
        ItemOfInit = pp.ItemOfInit;
        storedouble = pp.storedouble;
        storeint = pp.storeint;
        storefloat = pp.storefloat;
        SetData();
    }

}
