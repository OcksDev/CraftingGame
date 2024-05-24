
using System.Collections;
using System.Collections.Generic;
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
    private Vector3 move = new Vector3(0, 0, 0);
    public Transform SwordFart;
    public Transform dicksplit;
    public Transform MyAssHurts;
    public Transform[] cummers = new Transform[2];
    public GameObject[] SlashEffect;
    public GameObject HitCollider;
    public GameObject[] HitColliders;
    public Image healthshit;
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
    public OcksNetworkVar network_helditem = new OcksNetworkVar();
    private bool HasLoadedWeapon = false;

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
        if (isrealowner) Instance = this;
        selecteditem = 0;
        rigid= GetComponent<Rigidbody2D>();
        entit = GetComponent<EntityOXS>();
        dicksplay = dicksplit.GetComponent<SpriteRenderer>();   
        SetData();
    }

    public void Aids()
    {
        network_helditem.SetCreds(spawnData.Hidden_Data[0], "Held Item");
        Console.Log("GIGA CUUM " + spawnData.Hidden_Data[0]);
        if (isrealowner)
        {
            //network_helditem.SetValue(mainweapon.ItemToString());
        }
        else
        {
            network_helditem.Query();
            for (int i = 0; i < 1; i++) StartCoroutine(WaitFOrThing(i));
        }
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

    public void SetData()
    {
        helth = entit.Max_Health;
        if (GISLol.Instance.All_Containers.ContainsKey("Equips"))
        {
            if (isrealowner)
            {
                var c = GISLol.Instance.All_Containers["Equips"];
                mainweapon = c.slots[selecteditem].Held_Item;
                network_helditem.SetValue(mainweapon.ItemToString());
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
            if (isrealowner)
            {
                network_helditem.SetValue("");
            }
        }
        working_move_speed = 2;
        Damage = 5;
        AttacksPerSecond = 3;
        Spread = 15f;
        MaxBowMult = 1.5f;
        BowChargeSpeed = 1f;
        if(mainweapon != null)
        {
            switch (mainweapon.ItemIndex)
            {
                case 5:
                    AttacksPerSecond = 1.5f;
                    Damage = 10;
                    break;
                case 4:
                    AttacksPerSecond = 1.5f;
                    Damage = 4;
                    break;
            }
            foreach(var m in mainweapon.Materials)
            {
                ParseMaterial(m.index);
            }
        }
        entit.Max_Health = helth;
        entit.Max_Shield = 0;
    }
    public void ParseMaterial(int mat)
    {
        var m = GISLol.Instance.Materials[mat];
        switch (mat)
        {
            case 0:
                switch (mainweapon.ItemIndex)
                {
                    case 6:
                    case 3:
                        AttacksPerSecond += 0.5f;
                        break;
                    case 4:
                        AttacksPerSecond += 0.25f;
                        BowChargeSpeed += 0.25f;
                        break;
                    case 5:
                        AttacksPerSecond += 0.25f;
                        break;
                }
                break;
            case 1:
                switch (mainweapon.ItemIndex)
                {
                    case 6:
                    case 3:
                    case 4:
                        Damage += 2.5;
                        break;
                    case 5:
                        Damage += 5;
                        break;
                }
                break;
        }
    }
    private void LateUpdate()
    {
        healthshit.transform.rotation = Quaternion.identity;
        if (isrealowner)
        {
            if (InputManager.IsKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
            else if (InputManager.IsKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        }
    }

    public void SwitchWeapon(int s3x)
    {
        selecteditem = s3x;
        SetData();
    }
    private string oldval;
    void FixedUpdate()
    {
        if(!isrealowner && HasLoadedWeapon && network_helditem.GetValue() != oldval)
        {
            oldval = network_helditem.GetValue();
            SetData();
        }
        if (HitCollider != null) HitCollider.SetActive(false);
        healthshit.fillAmount = (float)(entit.Health / entit.Max_Health);
        if (isrealowner)
        {
            SetMoveSpeed();
            move *= decay;
            Vector3 dir = new Vector3(0, 0, 0);
            if (InputManager.IsKey(KeyCode.W)) dir += Vector3.up;
            if (InputManager.IsKey(KeyCode.S)) dir += Vector3.down;
            if (InputManager.IsKey(KeyCode.D)) dir += Vector3.right;
            if (InputManager.IsKey(KeyCode.A)) dir += Vector3.left;
            if (dir.magnitude > 0.5f)
            {
                dir.Normalize();
                move += dir;
            }

            if (mainweapon != null)
            {
                if (mainweapon.ItemIndex == 4)
                {
                    if (f2 <= 0 && f >= 1)
                    {
                        if (InputManager.IsKey(InputManager.gamekeys["shoot"])) bowsextimer += Time.deltaTime * BowChargeSpeed;
                        if (bowsextimer > MaxBowMult) bowsextimer = MaxBowMult;
                        if (bowsextimer > 0 && !InputManager.IsKey(InputManager.gamekeys["shoot"]))
                        {
                            //Debug.Log(bowsextimer + ", " + Damage);
                            StartAttack(Damage * (1 + bowsextimer));
                        }
                    }


                    //if (InputManager.IsKey(InputManager.gamekeys["shoot"]) && f2 <= 0 && f >= 1) StartAttack();
                }
                else
                {
                    if (InputManager.IsKey(InputManager.gamekeys["shoot"]) && f2 <= 0 && f >= 1) StartAttack();
                }
            }
            Vector3 bgalls = move * Time.deltaTime * move_speed * 20;
            rigid.velocity += new Vector2(bgalls.x, bgalls.y);
            if (CameraLol.Instance != null)
            {
                CameraLol.Instance.targetpos = transform.position;
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
        if (f >= 1)
        {
            if (isrealowner)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Point2D(-90, 0), 25f);
                dicksplay.flipX = (transform.position - RandomFunctions.Instance.MousePositon(Camera.main)).x < 0;
            }
            dicksplit.rotation = Quaternion.identity;
        }
        if (mainweapon != null)
        {
            SwordFart.localPosition = new Vector3(0, 0, 0);
            
            switch (mainweapon.ItemIndex)
            {
                case 3:
                    SwordFart.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(-121, 121, g) * reverse)) * transform.rotation;
                    break;
                case 6:
                    SwordFart.localScale = new Vector3(Mathf.Lerp(1, 0.8f, f2/ (1 / AttacksPerSecond) + ((0.2f * 3f) / AttacksPerSecond)), 1, 1);
                    SwordFart.rotation = transform.rotation;
                    break;
                case 4:
                    SwordFart.localPosition = new Vector3(0, -1f, 0);
                    SwordFart.rotation = transform.rotation;
                    break;
                case 5:
                    if (!sexed && g <= 0.5f)
                    {
                        reverse *= -1;
                        sexed = true;
                    }
                    g = Mathf.Sin(g * Mathf.PI);
                    g = 1 - g;
                    SwordFart.rotation = Quaternion.Euler(new Vector3(0, 0, 11 * reverse)) * transform.rotation;
                    SwordFart.localPosition = new Vector3(Mathf.Lerp(-0.5f, -1.5f, g)*-reverse, Mathf.Lerp(0.5f, -2.5f, g), 0);
                    break;
            }
            int reverse2 = (transform.position-MyAssHurts.transform.position).x < 0?1:-1;
            switch (mainweapon.ItemIndex)
            {
                case 6: SwordFart.localScale = new Vector3(Mathf.Lerp(1, 0.7f, bowsextimer / MaxBowMult) * reverse2, 1, 1); break;
                default: SwordFart.localScale = new Vector3(reverse2, 1, 1); break;
            }
        }
    }
    private bool sexed = false;
    public void SetMoveSpeed()
    {
        move_speed = working_move_speed;
        if (f < 1 || f2 > 0 || bowsextimer > 0) move_speed *= 0.35f;
    }

    public void StartAttack(double d = -1)
    {
        if (d == -1) d = Damage;
        if (mainweapon.ItemIndex == 0) return;
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
        switch (mainweapon.ItemIndex)
        {
            case 3:
                s = Instantiate(SlashEffect[0], transform.position + transform.up * 2.3f, transform.rotation);
                s.GetComponent<SpriteRenderer>().flipX = reverse > 0;
                s.GetComponent<Slasher>().wait = (0.1f * 3) / AttacksPerSecond;
                reverse *= -1;
                HitCollider = HitColliders[0];
                break;
            case 5:
                s = Instantiate(SlashEffect[1], transform.position + transform.up * 2.3f, transform.rotation);
                s.GetComponent<SpriteRenderer>().flipX = reverse > 0;
                s2 = s.GetComponent<Slasher>();
                s2.wait = (0.05f * 1.5f) / AttacksPerSecond;
                s2.speedmult = 8f;
                HitCollider = HitColliders[1];
                break;
            case 6:
                s = Instantiate(SlashEffect[2], SlashEffect[3].transform.position, transform.rotation * Quaternion.Euler(new Vector3(0, 0, Random.Range(Spread / 2, -Spread / 2))));
                s3 = s.GetComponent<HitBalls>();
                s3.playerController = this;
                s3.Damage = d;
                epe *= -0.5f;
                HitCollider = null;
                f = 1;
                f2 = (1 / AttacksPerSecond) + ((0.2f * 3f) / AttacksPerSecond);
                break;
            case 4:


                for(int i = -1; i < 2; i++)
                {
                    s = Instantiate(SlashEffect[2], SlashEffect[3].transform.position, transform.rotation * Quaternion.Euler(new Vector3(0, 0, (Random.Range(Spread / 2, -Spread / 2)) + (15*i))));
                    s3 = s.GetComponent<HitBalls>();
                    s3.playerController = this;
                    s3.Damage = d;
                }
                epe *= -0.5f;
                HitCollider = null;
                f = 1;
                f2 = (1 / AttacksPerSecond) + ((0.2f * 3f) / AttacksPerSecond);
                break;
            default:
                HitCollider = HitColliders[0];
                reverse *= -1;
                break;
        }
        rigid.velocity += new Vector2(epe.x, epe.y);
        if (HitCollider != null) { HitCollider.SetActive(true);
            HitCollider.GetComponent<HitBalls>().Damage = d;
        }
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
