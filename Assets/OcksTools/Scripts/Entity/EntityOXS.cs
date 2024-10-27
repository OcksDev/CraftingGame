using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    }
    private double damagefromhit;
    public void Hit(DamageProfile hit)
    {
        damagefromhit = hit.CalcDamage();
        if (AntiDieJuice) return;
        if (Gamer.GameState == "Dead") return;
        if (rg != null && hit.SpecificLocation)
        {
            var ewanker = ((Vector2)transform.position - (Vector2)hit.AttackerPos).normalized * hit.Knockback * 2.5f;
            rg.velocity += ewanker;
        }
        lasthit = hit;
        switch (EnemyType)
        {
            case "Enemy":
                if(sexy != null)
                {
                    SoundSystem.Instance.PlaySound(1, true, 0.2f, 3f);
                    sexy.target = hit.attacker;
                    sexy.MyAssChecker();
                }
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
                //hit.Damage -= s2.GetItem("repulse");
                if (damagefromhit < 1) damagefromhit = 1;

                if (s2.IsDashing)
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
                            CameraLol.Instance.Shake(0.4f, 0.87f);
                            Gamer.Instance.ShartPoop += 0.4f;
                        }
                        Shield -= damagefromhit;
                        if (Shield < 0)
                        {
                            Health += Shield;
                        }
                        var arr = s2.mainweapon.ReadItemAmount("Rune Of Retaliation");
                        if(arr >= 1)
                        {
                            NavMeshEntity cuumer = null;
                            float dist = 69696969;
                            foreach(var gw in Gamer.Instance.EnemiesExisting)
                            {
                                var dd = RandomFunctions.Instance.DistNoSQRT(transform.position, gw.transform.position);
                                if(dd < dist)
                                {
                                    dist = dd;
                                    cuumer = gw;
                                }
                            }
                            if(cuumer != null)
                            {
                                var we = s2.GetDamageProfile();
                                we.Damage = damagefromhit;
                                we.DamageMod = arr;
                                cuumer.EntityOXS.Hit(we);
                            }

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
                    SoundSystem.Instance.PlaySound(6, true, 0.5f, 0.75f);
                }
                else
                {
                    SoundSystem.Instance.PlaySound(4, true, 0.7f, 1f);
                }

                break;
            default:
                if (Gamer.Instance.GetObjectType(hit.attacker, false).type == "Player")
                {
                    PlayerController.Instance.DashCoolDown += PlayerController.BaseDashCooldown / 10f;
                    if (hit.WeaponOfAttack != null)
                    {
                        var arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Self") * 0.25f;
                        if (arr > 0)
                        {
                            var ff2 = Random.Range(0f, 1f);
                            int tt2 = Mathf.FloorToInt(arr);
                            if (ff2 <= (arr % 1)) tt2++;
                            if (tt2 > 0)
                            {
                                s.entit.Heal(tt2);
                            }
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Missile") * 0.1f;
                        if (arr > 0 && !hit.Procs.Contains("Missile"))
                        {
                            var ff2 = Random.Range(0f, 1f);
                            int tt2 = Mathf.FloorToInt(arr);
                            if (ff2 <= (arr % 1)) tt2++;
                            for(int i = 0; i < tt2; i++)
                            {
                                var attack = new DamageProfile(hit);
                                attack.Procs.Add("Missile");
                                attack.DamageMod *= 2;
                                var weenis = hit.attacker.transform;
                                var we = Instantiate(RandomFunctions.Instance.SpawnRefs[1], weenis.position, PointFromTo2D(hit.attacker.transform.position, transform.position,90+Random.Range(-90f,90f)), Gamer.Instance.balls).GetComponent<MissileMover>();
                                we.hitbal.attackProfile = attack;
                                we.target = gameObject;
                            }
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
            e = RandomFunctions.Instance.SpawnObject(0, Tags.refs["DIC"], transform.position + new Vector3(Random.Range(-xx, xx), Random.Range(-yy, yy), 0), Quaternion.identity);
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
    }

    public void SpawnExplosion(float size, Vector3 pos, DamageProfile dam)
    {
        var weenis = Instantiate(Gamer.Instance.ParticleSpawns[16], pos, Quaternion.identity).GetComponent<partShitBall>();
        float truesz = size / 5;
        weenis.Particicic.localScale = new Vector3(truesz, truesz, 1);
        var we = Physics2D.OverlapCircleAll((Vector2)pos, size/2);
        foreach(var nerd in we)
        {
            if (nerd.gameObject == gameObject) continue;
            var ob = Gamer.Instance.GetObjectType(nerd.gameObject);
            if(ob.type == "Enemy" && ob.entityoxs.Health > 0.5f)
            {
                var wank = new DamageProfile(dam);
                wank.Damage = 25;
                wank.DamageMod = 1;
                ob.entityoxs.Hit(wank);
            }
        }
    }



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
        if (playerdaddy != null)
        {
            var arr = playerdaddy.mainweapon.ReadItemAmount("Rune Of Confluence");
            Shield = System.Math.Clamp(Shield + arr, 0, Max_Shield);
        }
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

                Gamer.Instance.SpawnHealers(transform.position, healerstospawn, PlayerController.Instance);

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
                if(lasthit != null)
                {
                    var arr = lasthit.WeaponOfAttack.ReadItemAmount("Rune Of Kaboom")*2;
                    arr += 3;
                    if(arr > 4)
                    {
                        SpawnExplosion(arr, transform.position, lasthit);
                    }
                }
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
        if (ren != null)
        {
            curstatus = DamageTimer >= 0;
            if(curstatus != oldstatus)
            {
                if (sexy != null && sexy.EliteType != "")
                {
                    ren.material = Gamer.Instance.sexex[DamageTimer >= 0 ? 3 : 2];
                    ren.material.color = Gamer.Instance.EliteTypesDict[sexy.EliteType].color;
                    ren.material.SetFloat("_OutlineThickness", 1f / sexy.ImagePixelSize);
                }
                else
                {
                    ren.material = Gamer.Instance.sexex[DamageTimer >= 0 ? 1 : 0];
                }
                ren.color = DamageTimer >= 0 ? new Color32(255, 255, 255, 255) : col;
                oldstatus = curstatus;
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
    public DamageProfile(string name, double damage)
    {
        Damage = damage;
        Name = name;
    }
    public DamageProfile(DamageProfile pp)
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
        WeaponOfAttack = new GISItem(pp.WeaponOfAttack);
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


        return x * DamageMod;
    }
}

public class EffectProfile
{
    //data you pass in
    public int Type;
    public float Duration;
    public int CombineMethod;
    //other data
    public int Stack = 1;
    public float TimeRemaining;
    public int MaxStack;
    public string Name;
    public EffectProfile(int type, float time, int add_method, int stacks = 1)
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
        Name = "Error";
        switch (Type)
        {
            //some example effects
            case 0:
                Name = "Burning";
                break;
            case 1:
                Name = "Healing Energy";
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
        SetData();
    }

}
