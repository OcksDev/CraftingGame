using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int healerstospawn = 1;
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
        if(hit.Damage > 0)
        {
            var x = (transform.localScale.x / 2)-0.25f;
            var y = (transform.localScale.y / 2)-0.25f;
            var e = RandomFunctions.Instance.SpawnObject(0, Tags.refs["DIC"], transform.position + new Vector3(Random.Range(-x,x), Random.Range(-y, y), 0), Quaternion.identity);
            e.GetComponent<TextMeshProUGUI>().text = RandomFunctions.Instance.NumToRead(((System.Numerics.BigInteger)System.Math.Round(hit.Damage)).ToString());
            DamageTimer = 0.1f;
        }
        if (rg != null && hit.SpecificLocation)
        {
            var e = ((Vector2)transform.position - (Vector2)hit.AttackerPos).normalized * hit.Knockback * 2.5f;
            rg.velocity += e;
        }
        switch (EnemyType)
        {
            case "Enemy":
                //Debug.Log("Sexy" + hit.attacker.name);
                if(sexy != null)
                {
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
            case "Player":
                s2 = GetComponent<PlayerController>();
                if (PlayerController.Instance == s2) CameraLol.Instance.Shake(0.1f, 0.85f);
                break;
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
        switch (EnemyType)
        {
            case "Player":
                if (!s2.isrealowner) break;
                Shield -= hit.Damage;
                if (Shield < 0)
                {
                    Health += Shield;
                }
                break;
            default:
                Shield -= hit.Damage;
                if (Shield < 0)
                {
                    Health += Shield;
                }
                break;
        }
    }
    public void Kill()
    {
        switch (EnemyType)
        {
            case "Enemy":
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
                break;
            case "Player":
                return;
        }
        Destroy(gameObject);
    }
    private void Update()
    {
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
    public List<string> Procs = new List<string>();
    public bool SpecificLocation = false;
    public Vector3 AttackerPos = Vector3.zero;
    public float Knockback = 0f;
    public GameObject attacker = null;
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
