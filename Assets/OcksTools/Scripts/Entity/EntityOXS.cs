using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

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
    private DamageProfile lasthitfromplayer;
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
        List<System.Func<int, int>> stored_funcs = new List<System.Func<int, int>>();
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
        if (ContainsEffect("Protected").hasthing)
        {
            damagefromhit /= 2;
        }
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

                if (s2.IsDashingImmume || block)
                {
                    block = true;
                    var arr = s2.mainweapon.ReadItemAmount("Rune Of Blocked Healing");
                    if(arr > 0)
                    {
                        StartCoroutine(WaitCall(()=>{
                            Heal(3 * arr);
                        },0.05f));
                    }
                }
                else
                {
                    var aaaaa2 = Gamer.Instance.GetObjectType(hit.attacker, false);
                    //var y = s2.GetItem("blocker");
                    var y = 0;
                    float x = ((float)y) / (19f + y);
                    //Debug.Log("shar: " + x);
                    if (Random.Range(0f, 1f) > x)
                    {
                        if (PlayerController.Instance == s2)
                        {
                            if(SaveSystem.Instance.dam_shake) CameraLol.Instance.Shake(wasticked || hit.Name == "nono" ? 0.1f:0.4f, 0.87f);
                            if (SaveSystem.Instance.dam_red) Gamer.Instance.ShartPoop += wasticked || hit.Name == "nono" ? 0.1f:0.4f;
                        }


                        var arr = s2.mainweapon.ReadItemAmount("Rune Of Shielded Missiling");
                        if (arr >= 1 && (s2.entit.Shield > 0.01 || s2.entit.Health == s2.entit.Max_Health))
                        {
                            var Shart = s2.GetDamageProfile();
                            Shart.DamageMod *= arr;
                            s2.entit.SpawnMissile(null, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)), Shart);
                        }


                        damagefromhit *= s2.DamageTakenMod;
                        Shield -= damagefromhit;
                        if (aaaaa2.type == "Enemy") aaaaa2.entity.lsd_store += damagefromhit;
                        if (Shield < 0)
                        {
                            Health += Shield;
                        }
                        arr = s2.mainweapon.ReadItemAmount("Rune Of Retaliation");
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
                        arr = s2.mainweapon.ReadItemAmount("Rune Of Protection");
                        if(arr >= 1)
                        {
                            var ef = new EffectProfile("Protected", arr*0.5f, 5, 1);
                            ef.ItemOfInit = hit.WeaponOfAttack;
                            AddEffect(ef);
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
                    SoundSystem.Instance.PlaySound(6, true, wasticked || hit.Name == "nono" ? 0.2f:0.5f, 0.75f);
                }
                else
                {
                    SoundSystem.Instance.PlaySound(4, true, wasticked||hit.Name=="nono" ? 0.28f:0.7f, 1f);
                }

                break;
            default:
                var aaaaa = Gamer.Instance.GetObjectType(hit.attacker, false);
                if (aaaaa.type == "Player")
                {
                    PlayerController.Instance.DashCoolDown += PlayerController.BaseDashCooldown / 10f;
                    if (hit.WeaponOfAttack != null)
                    {
                        var arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Self") * 0.15f;
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

                        var memesex = ContainsEffect("Brittle");
                        if (memesex.hasthing)
                        {
                            damagefromhit *= 1 + (0.5 * memesex.susser.Stack);
                            Effects.Remove(memesex.susser);
                        }



                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Death");
                        if (arr > 0 && hit.controller != null)
                        {
                            int tt2 = hit.WeaponOfAttack.RollLuck(0.25f);
                            if (tt2 > 0) damagefromhit += arr * 3;
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Investment");
                        if (arr > 0 && hit.controller != null)
                        {
                            damagefromhit *= ((double)arr * hit.controller.Coins)/100 + 1;
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Drive-By");
                        if (arr > 0 && hit.controller != null && hit.controller.IsDashing)
                        {
                            damagefromhit *= 1 + (0.25 * arr);
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Weaponized Shielding");
                        if (arr > 0 && (hit.WeaponOfAttack.Player.entit.Shield > 0.01 || hit.WeaponOfAttack.Player.entit.Health == hit.WeaponOfAttack.Player.entit.Max_Health))
                        {
                            damagefromhit *= 1 + (arr*0.2);
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Criticality");
                        if (arr > 0 && hit.DType != DamageProfile.DamageType.Explosion && hit.WasCrit >= 0)
                        {
                            var nm = new DamageProfile(hit);
                            nm.DamageMod *= 0.3;
                            System.Func<int, int> me = (x) =>
                            {
                                SpawnExplosion((arr * 2) + 3, transform.position, nm, -1);
                                return x;
                            };
                            stored_funcs.Add(me);
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Conversion");
                        if (arr > 0)
                        {
                            aaaaa.playerController.entit.Shield += 0.1 * arr * damagefromhit;
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Bleed") * 0.2f;
                        if (arr > 0 && hit.controller != null && !hit.Procs.Contains("Bleed"))
                        {
                            hit.Procs.Add("Bleed");
                            int tt2 = hit.WeaponOfAttack.RollLuck(arr);
                            var ef = new EffectProfile("Bleed", 3, 7, tt2);
                            ef.storefloat = 0.95f;
                            ef.damprof = hit;
                            ef.ItemOfInit = hit.WeaponOfAttack;
                            if (tt2 > 0) AddEffect(ef);
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Freeze") * 0.3f;
                        if (arr > 0 && hit.controller != null)
                        {
                            int tt2 = hit.WeaponOfAttack.RollLuck(arr);
                            var ef = new EffectProfile("Freeze", 5, 7, tt2);
                            ef.ItemOfInit = hit.WeaponOfAttack;
                            if (tt2 > 0) AddEffect(ef);
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
                                var we = Instantiate(RandomFunctions.Instance.SpawnRefs[1], hit.GetTransform().position, PointFromTo2D(hit.GetTransform().position, transform.position, 90 + Random.Range(-90f, 90f)), Gamer.Instance.balls).GetComponent<MissileMover>();
                                we.hitbal.attackProfile = attack;
                                we.target = gameObject;
                            }
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Conduction");
                        if (arr > 0 && !hit.Procs.Contains("Conduct"))
                        {
                            if (hit.WeaponOfAttack.RollLuck(0.15f) > 0)
                            {
                                var attack = new DamageProfile(hit);
                                attack.Procs.Add("Conduct");
                                attack.DamageMod *= arr * 0.5;
                                hit.WeaponOfAttack.Player.ShootLightning(0, hit.GetTransform().position, attack);
                            }
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Aspect Of Lightning");
                        if (arr > 0 && !hit.Procs.Contains("LightningA"))
                        {
                            int tt2 = hit.WeaponOfAttack.RollLuck(0.5f);
                            for (int i = 0; i < tt2; i++)
                            {
                                var attack = new DamageProfile(hit);
                                attack.Procs.Add("LightningA");
                                attack.DamageMod *= 0.5;
                                aaaaa.playerController.ShootLightning(1, transform.position, attack);
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
                        arr = hit.WeaponOfAttack.ReadItemAmount("Aspect Of Duality");
                        if (arr > 0)
                        {
                            if(hit.WeaponOfAttack.RollLuck(0.2f) > 0)
                            {
                                OnKillEffect(hit.controller.secondweapon, true);
                            }
                        }
                        arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Collapse")*0.5f;
                        if (arr > 0)
                        {
                            var weenor = new EffectProfile("Collapse", 9999999, 7, 1);
                            weenor.storeint = aaaaa.playerController.selecteditem;
                            weenor.storedouble = hit.Damage;
                            weenor.storefloat = arr;
                            weenor.ItemOfInit = hit.WeaponOfAttack;
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
                        switch (hit.DType)
                        {
                            case DamageProfile.DamageType.Explosion:
                                arr = hit.WeaponOfAttack.ReadItemAmount("Rune Of Combustion");
                                if (arr > 0 && !hit.Procs.Contains("Fire"))
                                {
                                    hit.Procs.Add("Fire");
                                    var ef = new EffectProfile("Fire", 3, 7, (int)arr);
                                    ef.storefloat = 0.3f;
                                    ef.damprof = hit;
                                    ef.ItemOfInit = hit.WeaponOfAttack;
                                    AddEffect(ef);
                                }
                                break;
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
                fard.sex.text = RandomFunctions.Instance.NumToRead(((System.Numerics.BigInteger)System.Math.Round(damagefromhit)).ToString());
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
                if(weenor != null && weenor.EliteType != "Corrupted" && !weenor.IsBoss)
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
        foreach(var asexy in stored_funcs)
        {
            asexy(0);
        }
    }


    public void SpawnMissile(GameObject target, Vector3 pos, Quaternion rot, DamageProfile attack)
    {
        var atk = new DamageProfile(attack);
        var we = Instantiate(RandomFunctions.Instance.SpawnRefs[1], pos, rot, Gamer.Instance.balls).GetComponent<MissileMover>();
        we.hitbal.attackProfile = atk;
        we.target = target;
    }
    public IEnumerator WaitCall(System.Action banana, float tim)
    {
        yield return new WaitForSeconds(tim);
        banana();
    }
    public static void SpawnExplosion(float size, Vector3 pos, DamageProfile dam, double damage = 15, int particleid= 16)
    {
        var weenis = Instantiate(Gamer.Instance.ParticleSpawns[particleid], pos, Quaternion.identity).GetComponent<partShitBall>();
        float truesz = size / 5;
        weenis.Particicic.localScale = new Vector3(truesz, truesz, 1);
        var we = Physics2D.OverlapCircleAll((Vector2)pos, size/2);
        foreach(var nerd in we)
        {
            if (nerd.gameObject == null) continue;
            var ob = Gamer.Instance.GetObjectType(nerd.gameObject);
            if(ob != null && ob.gm != null && ob.type == "Enemy" && ob.entityoxs != null && ob.entityoxs.Health > 0.5f)
            {
                var wank = new DamageProfile(dam);
                wank.DType = DamageProfile.DamageType.Explosion;
                if (damage > 0)
                {
                    wank.Damage = damage;
                    wank.DamageMod = 1;
                }
                ob.entityoxs.Hit(wank);
            }
        }
    }
    [HideInInspector]
    public DamageProfile currentprof;

    public void Heal(double amount)
    {
        double DirectShieldPercent = 0;
        double ShieldChangeMult = 1;
        switch (EnemyType)
        {
            case "Player":
                var arr = playerdaddy.mainweapon.ReadItemAmount("Rune Of Life");
                if (arr > 0)
                {
                    int tt2 = playerdaddy.mainweapon.RollLuck(0.25f);
                    if (tt2 > 0) amount += arr;
                }
                arr = playerdaddy.mainweapon.ReadItemAmount("Aspect Of Healing");
                if (arr > 0)
                {
                    amount *= 2;
                }
                if(ContainsEffect("Shrine Healing").hasthing)
                {
                    amount *= 1.5;
                }
                arr = playerdaddy.mainweapon.ReadItemAmount("Rune Of Lasting");
                if (arr >= 1)
                {
                    var ef = new EffectProfile("Protected", arr * 0.5f, 5, 1);
                    ef.ItemOfInit = playerdaddy.mainweapon;
                    AddEffect(ef);
                }
                arr = playerdaddy.mainweapon.ReadItemAmount("Rune Of Trickle Down Economics");
                if (arr >= 1)
                {
                    amount *= 1 + (0.02 * arr * playerdaddy.Coins);
                }
                DirectShieldPercent = 1-playerdaddy.DirectShieldHeal;
                ShieldChangeMult = playerdaddy.ShieldHealingMod;
                break;
            case "Enemy":
                if(sexy != null && sexy.IsBoss) amount /= 4;
                break;
        }
        var oldh = Health;
        var direct = amount * DirectShieldPercent;
        var realamount = amount * (1-DirectShieldPercent);
        Health = System.Math.Clamp(Health + realamount, 0, Max_Health);
        var change = amount - ( Health - oldh);
        var olds = Shield;

        if(ShieldChangeMult != 1d)
        {
            amount -= change;
            change *= ShieldChangeMult;
            amount += change;
        }

        Shield = System.Math.Clamp(Shield + change, 0, Max_Shield);
        var change2 = (change) - (Shield - olds);
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
                            //we.DamageMod = 1;
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

                Gamer.Instance.lastkillpos = transform.position;

                int effect = -1;
                var aa = sexy;
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
                    case "Bossrocks":
                        effect = 39;
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

                if (aa.IsBoss)
                {
                    string zaza = GISLol.Instance.AllAspects[Random.Range(0, GISLol.Instance.AllAspects.Count)];
                    Gamer.Instance.SpawnGroundItem(transform.position, new GISItem(zaza));

                    Gamer.Instance.SpawnCoins(transform.position, 9, PlayerController.Instance);
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
                if(sexy != null) Gamer.Instance.AttemptAddLogbookItem(sexy.EnemyType);
                if (!ContainsEffect("Soulless").hasthing) DropKillReward(false);
                if(lasthit != null && lasthit.WeaponOfAttack != null) Gamer.QuestProgressIncrease("Kill", lasthit.WeaponOfAttack.ItemIndex);
                break;
            case "Player":
                if(!PlayerController.Instance.DeathDisable) Gamer.Instance.StartCoroutine(Gamer.Instance.DeathAnim());
                return;
        }
        Destroy(gameObject);
    }

    public void DropKillReward(bool useplayermainweapon, int amnt = 1)
    {


        int he = healerstospawn;

        GISItem inpu = null;
        if (lasthit != null)
        {
            inpu = lasthit.WeaponOfAttack;
        }
        if (useplayermainweapon)
        {
            inpu = PlayerController.Instance.mainweapon;
        }
        if (inpu != null)
        {
            if(lasthit != null && lasthit.WeaponOfAttack != null && lasthit.WasItemTransfered)
            {
                //do nothing
            }
            else
            {
                OnKillEffect(inpu);
            }
        }

        if (!Gamer.ActiveDrugs.Contains("MDMA"))
        {
            if (sexy.IsLSD)
            {
                if (sexy.lsd_store > 0)
                {
                    PlayerController.Instance.entit.Heal(sexy.lsd_store);
                }
            }
            else
            {
                for (int i = 0; i < amnt; i++)
                {
                    Gamer.Instance.SpawnHealers(transform.position, he, PlayerController.Instance);
                }
            }
        }
    }

    public void OnKillEffect(GISItem inpu, bool wankwank = false)
    {
        var arr2 = inpu.ReadItemAmount("Aspect Of Duality");
        if (arr2 > 0)
        {
            return;
        }

        System.Func<DamageProfile> GetDam = () =>
        {
            var a = inpu.Player.GetDamageProfile();
            a.WeaponOfAttack = inpu;
            if(wankwank) a.WasItemTransfered = true;
            return new DamageProfile(a);
        };

        arr2 = inpu.ReadItemAmount("Rune Of Kaboom") * 2;
        arr2 += 3;
        if (arr2 > 4)
        {
            SpawnExplosion(arr2, transform.position, GetDam());
        }
        arr2 = inpu.ReadItemAmount("Rune Of Swords");
        if (arr2 > 0)
        {
            inpu.Player.SpawnSword(arr2 * 5);
        }
        arr2 = inpu.ReadItemAmount("Rune Of Turret");
        if (arr2 > 0)
        {
            if (inpu.RollLuck(0.2f) > 0)
            {
                var nn = GetDam();
                nn.Damage = 3 * arr2;
                inpu.Player.SpawnTurret(nn, transform.position);
            }
        }
        arr2 = inpu.ReadItemAmount("Rune Of Acid");
        if (arr2 > 0)
        {
            if (inpu.RollLuck(0.2f) > 0)
            {
                var nn = GetDam();
                nn.Damage = 3 * arr2;
                inpu.Player.SpawnAcidPool(nn, transform.position);
            }
        }
        arr2 = inpu.ReadItemAmount("Rune Of Deathly Arrow");
        if (arr2 > 0)
        {
            bool found = false;
            GameObject sexgm = null;
            float fdist = 100000000;
            foreach (var a in Gamer.Instance.EnemiesExisting)
            {
                if (!a.HasSpawned || a.gameObject == gameObject) continue;
                found = true;
                var r = RandomFunctions.Instance.DistNoSQRT(transform.position, a.transform.position);
                if (fdist > r)
                {
                    sexgm = a.gameObject;
                    fdist = r;
                }
            }
            if (found && sexgm != null)
            {
                var wee = inpu.Player;
                var w = GetDam();
                w.Damage = 5 * arr2;
                var a = wee.SpawnArrow(w, transform.position, RandomFunctions.PointAtPoint2D(transform.position, sexgm.transform.position, 90), 1);
                a.hitlist.Add(gameObject);
            }
        }
        arr2 = inpu.ReadItemAmount("Rune Of Drain");
        if (arr2 > 0 && Effects.Count > 0)
        {
            inpu.Player.entit.Heal(arr2 * Effects.Count);
        }
        var arr = inpu.ReadItemAmount("Rune Of Soul") * 0.15f;
        if (arr > 0)
        {
            var x = inpu.RollLuck(arr);
            if(x > 0)
            {
                Gamer.Instance.SpawnHealers(transform.position, x, PlayerController.Instance);
            }
        }
        arr2 = inpu.ReadItemAmount("Rune Of Surge");
        if (arr2 > 0)
        {
            if (inpu.RollLuck(0.15f) > 0)
            {
                for (int i = 1; i < inpu.Player.Skills.Count; i++)
                {
                    var x = GISLol.Instance.SkillsDict[inpu.Player.Skills[i].Name].MaxStacks;
                    inpu.Player.Skills[i].Stacks = Mathf.Clamp(inpu.Player.Skills[i].Stacks + arr2, 0, x);
                    if (inpu.Player.Skills[i].Stacks == x) inpu.Player.Skills[i].Timer = 0;
                }
            }
        }
        arr2 = inpu.ReadItemAmount("Aspect Of Plague");
        if (arr2 > 0)
        {
            NavMeshEntity near = null;
            float nr = 100000000;
            foreach (var a in Gamer.Instance.EnemiesExisting)
            {
                if (a != null && a != sexy && a.HasSpawned)
                {
                    var de = RandomFunctions.Instance.DistNoSQRT(transform.position, a.transform.position);
                    if (de < nr)
                    {
                        nr = de;
                        near = a;
                    }
                }
            }
            if (near != null)
            {
                foreach (var a in Effects)
                {
                    var x = near.EntityOXS.ContainsEffect(a);
                    if (x.hasthing)
                    {
                        if (x.susser.Stack < a.Stack)
                        {
                            x.susser.Stack = a.Stack;
                        }
                        if (x.susser.TimeRemaining < a.TimeRemaining)
                        {
                            x.susser.TimeRemaining = a.TimeRemaining;
                        }
                    }
                    else
                    {
                        near.EntityOXS.AddEffect(a, true);
                    }
                }
            }
        }
        arr2 = inpu.ReadItemAmount("Aspect Of Commerce");
        if (arr2 > 0)
        {
            Gamer.Instance.SpawnCoins(transform.position, 1, PlayerController.Instance);
        }
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
                    var targetcol = DamageTimer >= 0 ? new Color32(255, 255, 255, 255) : col;
                    if(sexy != null && sexy.IsLSD) targetcol.a /= 2;
                    ren.color = targetcol;
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
                if (playerdaddy != null) PlayerController.Instance.SetData();
            }
        }
        if (EnemyType == "Player"? Health <= 0 : Health <= 0.5f)
        {
            Kill();
        }
    }

    public Dictionary<string,ParticleSystem> alreadyspawned = new Dictionary<string, ParticleSystem>();

    private void FixedUpdate()
    {
        switch(EnemyType)
        {
            case "Enemy":
                if(Effects.Count > 0)
                {
                    var cd= ContainsEffect("Bleed");
                    if (cd.hasthing)
                    {
                        if((cd.susser.storefloat -= Time.deltaTime) < 0)
                        {
                            cd.susser.storefloat = 0.98f;
                            var dmg = new DamageProfile(cd.susser.damprof);
                            dmg.Damage = (double)3 * cd.susser.Stack;
                            dmg.DamageMod = 1;
                            Hit(dmg);
                        }
                    }
                    cd= ContainsEffect("Fire");
                    if (cd.hasthing)
                    {
                        if((cd.susser.storefloat -= Time.deltaTime) < 0)
                        {
                            cd.susser.storefloat = 0.325f;
                            var dmg = new DamageProfile(cd.susser.damprof);
                            dmg.Damage = (double)cd.susser.Stack;
                            dmg.DamageMod = 1;
                            Hit(dmg);
                        }
                    }
                }
                break;
        }
        bool foundbounds = false;
        Vector2 bb = Vector2.zero;
        Transform eee = transform;
        switch (EnemyType)
        {
            case "Enemy":
                if (sexy!= null && sexy.WantASpriteCranberry != null)
                {
                    foundbounds = true;
                    bb = sexy.WantASpriteCranberry.bounds.size;
                    bb *= 0.85f;
                }
                break;
            case "Player":
                eee = playerdaddy.dicksplit;
                foundbounds = true;
                bb = playerdaddy.PlayerimageRemder.bounds.size;
                bb *= 0.9f;
                break;
        }
        if (foundbounds)
        {
            HashSet<string> tbd = new HashSet<string>();
            Dictionary<string, ParticleSystem> tbd2 = new Dictionary<string, ParticleSystem>(alreadyspawned);
            foreach(var a in Effects) tbd.Add(a.Type);
            //List<string> tbd2 = new List<string>(tbd);
            foreach(var a in tbd2)
            {
                if (alreadyspawned.ContainsKey(a.Key) && !tbd.Contains(a.Key))
                {
                    Destroy(a.Value.gameObject);
                    alreadyspawned.Remove(a.Key);
                }
            }
            foreach(var a in tbd)
            {
                if (alreadyspawned.ContainsKey(a)) continue;
                int index = -1;
                switch (a)
                {
                    case "Protected": index = 42; break;
                    case "Fire": index = 41; break;
                }
                if(index > -1)
                {
                    var ee = Instantiate(Gamer.Instance.ParticleSpawns[index], transform.position, Quaternion.Euler(-90,0,0), eee).GetComponent<ParticleSystem>();
                    alreadyspawned.Add(a, ee);
                    var e = ee.shape;
                    var e2 = ee.emission;
                    e2.rateOverTime = e2.rateOverTime.constant * (bb.x * bb.y);
                    e.scale = new Vector3(bb.x,bb.y,0);
                    if(EnemyType=="Player") ee.gameObject.layer = 5;
                }
            }
        }

    }
    public ret_cum_shenan ContainsEffect(EffectProfile eff)
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
    public ret_cum_shenan ContainsEffect(string eff)
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

    public void AddEffect(EffectProfile eff, bool ignore = false)
    {
        //ignore serves a double function here
        // 1 - For the debuff duration mod being skipped
        // 2 - For the player to not run SetData
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


        if (!ignore && eff.ItemOfInit != null)
        {
            var a = eff.ItemOfInit.ReadItemAmount("Aspect Of Plague");
            if (a > 0) eff.Duration /= 2;
            eff.Duration *= eff.ItemOfInit.Player.DebuffDurationMod;
        }

        eff.TimeRemaining = eff.Duration;
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
                    s.Stack += eff.Stack;
                    break;
                case 3:
                    //increase stack count, up to maximum value
                    s.Stack += eff.Stack;
                    if (s.Stack > s.MaxStack) s.Stack = s.MaxStack;
                    break;
                case 4:
                    //increase stack count, up to maximum value, refresh duration
                    s.Stack += eff.Stack;
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
                    s.Stack += eff.Stack;
                    break;
                case 7:
                    //increase stack count, refresh time remaining
                    s.Stack += eff.Stack;
                    s.TimeRemaining = eff.Duration;
                    break;
            }
        }
        else
        {
            Effects.Add(eff);
        }

        if (playerdaddy != null && !ignore) PlayerController.Instance.SetData();

    }


}
public class ret_cum_shenan
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
    public double TotalDamageMod = 1;
    public GISItem WeaponOfAttack;
    public GISItem storeditem;
    public int storedticks = -69;
    public float ticktimer = 0;
    public bool IsSpecificPointOfDamage = false;
    public Vector3 SpecificPointOfDamage = Vector3.zero;
    public DamageType DType = DamageType.Misc;
    public Transform HijackaleTransform = null;
    public Transform OriginalTransform = null;
    public bool WasItemTransfered = false;
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
        TotalDamageMod = pp.TotalDamageMod;
        HijackaleTransform = pp.HijackaleTransform;
        OriginalTransform = pp.OriginalTransform;
        WasItemTransfered = pp.WasItemTransfered;
    }
    public Transform GetTransform()
    {
        if (HijackaleTransform != null) return HijackaleTransform;
        return OriginalTransform;
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
            x *= TotalDamageMod;
        }

        return x * DamageMod;
    }
    public enum DamageType
    {
        Misc,
        Explosion,
        Lightning,
        Fire,
        Ice,
        Missile,
        Arrow,
    }
}
[System.Serializable]
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
    public DamageProfile damprof;
    public EffectProfile(string type, float time, int add_method, int stacks = 1)
    {
        Type = type;
        Duration = time;
        CombineMethod = add_method;
        Stack =stacks;
        SetData();
    }
    public EffectProfile(bool inin, string finaldat)
    {
        var a = Converter.StringToList(finaldat);   
        Type = a[0];
        Duration = float.Parse(a[1]);
        Stack = int.Parse(a[2]);
        storedouble = double.Parse(a[3]);
        storefloat = float.Parse(a[4]);
        storeint = int.Parse(a[5]);
        TimeRemaining = float.Parse(a[6]);
        SetData();
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
            case "Protected":
                MaxStack = 1;
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
        damprof = new DamageProfile(pp.damprof);
        SetData();
    }

    public string GetAsString()
    {
        if (damprof != null) return "-";
        string s = Converter.ListToString(new List<string>()
        {
            Type,
            Duration.ToString(),
            Stack.ToString(),
            storedouble.ToString(),
            storefloat.ToString(),
            storeint.ToString(),
            TimeRemaining.ToString(),
        });
        return s;
    }


}
