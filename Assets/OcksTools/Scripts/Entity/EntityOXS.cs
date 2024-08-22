using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    public int healerstospawn = 1;
    public bool AntiDieJuice = false;
    private void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        sexy = GetComponent<NavMeshEntity>();
        rg = GetComponent<Rigidbody2D>();
        if (ren != null)
        {
            col = ren.color;
        }
    }
    public void Hit(DamageProfile hit)
    {
        if (AntiDieJuice) return;
        if (rg != null && hit.SpecificLocation)
        {
            var ewanker = ((Vector2)transform.position - (Vector2)hit.AttackerPos).normalized * hit.Knockback * 2.5f;
            rg.velocity += ewanker;
        }
        switch (EnemyType)
        {
            case "Enemy":
                if(sexy != null)
                {
                    SoundSystem.Instance.PlaySound(0, true, 0.5f, 0.4f);
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
        switch (EnemyType)
        {
            default:
                if (Gamer.IsMultiplayer)
                {
                    PlayerController s = null;
                    if (hit.attacker != null)
                    {
                        s = hit.attacker.GetComponent<PlayerController>();
                    }
                    if (s != null && !s.isrealowner) { return; }
                }
                break;
        }
        bool block = false;
        switch (EnemyType)
        {
            case "Player":
                s2 = GetComponent<PlayerController>();
                if (!s2.isrealowner) break;
                //hit.Damage -= s2.GetItem("repulse");
                if (hit.Damage < 1) hit.Damage = 1;

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
                        Shield -= hit.Damage;
                        if (Shield < 0)
                        {
                            Health += Shield;
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
                PlayerController.Instance.DashCoolDown += PlayerController.BaseDashCooldown/10f;
                Shield -= hit.Damage;
                if (Shield < 0)
                {
                    Health += Shield;
                }
                break;
        }

        var xx = (transform.localScale.x / 2) - 0.25f;
        var yy = (transform.localScale.y / 2) - 0.25f;
        GameObject e = null;
        if (hit.Damage > 0)
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
                fard.sex.text = RandomFunctions.Instance.NumToRead(((System.Numerics.BigInteger)System.Math.Round(hit.Damage)).ToString());
                fard.critlevel = hit.WasCrit;
                DamageTimer = 0.1f;
            }
        }
    }
    public void Kill()
    {
        switch (EnemyType)
        {
            case "Enemy":
                SoundSystem.Instance.PlaySound(1, true, 0.7f, 1f);
                List<GameObject> others = new List<GameObject>();
                for(int i = 0; i < healerstospawn; i++)
                {
                    others.Add(Instantiate(Gamer.Instance.HealerGFooFO, transform.position, transform.rotation));
                }
                foreach (var other in others)
                {
                    var h = other.GetComponent<HealerFollower>();
                    h.SexChaser = PlayerController.Instance;
                    h.others = others;
                }
                int effect = -1;
                var aa = GetComponent<NavMeshEntity>();
                switch (aa.EnemyType)
                {
                    case "Charger":
                        effect = 1;
                        break;
                    default:
                        effect = 0;
                        break;
                }
                if (Gamer.Instance.EnemiesExisting.Contains(aa)) Gamer.Instance.EnemiesExisting.Remove(aa);
                PlayerController.Instance.DashCoolDown += PlayerController.BaseDashCooldown;
                if (effect>-1)Instantiate(Gamer.Instance.ParticleSpawns[effect], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                CameraLol.Instance.Shake(0.25f, 0.80f);

                break;
            case "Player":
                Gamer.Instance.ClearMap();
                Gamer.Instance.checks[6] = true;
                Gamer.Instance.UpdateMenus();
                return;
        }
        Destroy(gameObject);
    }
    private void Update()
    {
        if (AntiDieJuice) return;
        if (ren != null)
        {
            ren.material = Gamer.Instance.sexex[DamageTimer >= 0 ? 1 : 0];
            ren.color = DamageTimer >= 0 ? new Color32(255, 255, 255, 255) : col;
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
        if (Health <= 0)
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
    public PlayerController PlayerController;
    public List<string> Procs = new List<string>();
    public bool SpecificLocation = false;
    public Vector3 AttackerPos = Vector3.zero;
    public float Knockback = 0f;
    public GameObject attacker = null;
    public int WasCrit = -1;
    public DamageProfile(string name, double damage)
    {
        Damage = damage;
        Name = name;
    }
    public List<string> GiveProcs()
    {
        var e = new List<string>(Procs);
        e.Add(Name);
        return e;
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
