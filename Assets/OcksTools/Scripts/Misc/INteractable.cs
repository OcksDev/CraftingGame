using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class INteractable : MonoBehaviour
{
    public string Type = "Crafter";
    public float IneteractDistance = 3;
    public float TextOffsetDist = 3;
    private float workingtextoff = 3;
    public TextMeshProUGUI DisplaySegsmcnugget;
    public GameObject Parente;
    public bool CanInteract = true;
    public GroundItem BananaMan = null;
    public GISItem cuum;
    public SpriteRenderer[] memes;
    private void OnDisable()
    {
        if (DisplaySegsmcnugget != null && DisplaySegsmcnugget.gameObject != null)
        {
            Destroy(DisplaySegsmcnugget.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Gamer.Instance.CanInteractThisFrame || DisplaySegsmcnugget == null) return;
        var e = transform.position;
        if(PlayerController.Instance != null)
        {
            var e2 = PlayerController.Instance.transform.position;
            e.z = 0;
            e2.z = 0;
            bool wanker = (e - e2).magnitude <= IneteractDistance + transform.localScale.x;
            DisplaySegsmcnugget.gameObject.SetActive(CanInteract && wanker && !DialogLol.Instance.dialogmode);
            if (wanker && !DialogLol.Instance.dialogmode)
            {
                DisplaySegsmcnugget.transform.position = transform.position + new Vector3(0, workingtextoff, 0);
                if (CanInteract && InputManager.IsKeyDown("interact", "player"))
                {
                    Interact();
                }
            }
        }
    }
    private void OnDestroy()
    {
        if(DisplaySegsmcnugget != null && DisplaySegsmcnugget.gameObject != null)
        {
            Destroy(DisplaySegsmcnugget.gameObject);
        }
    }
    public void OnEnable()
    {
        if (Time.time < 0.2f) return;
        var w = Instantiate(Gamer.Instance.textShuingite, transform.position, Quaternion.identity, Tags.refs["DIC"].transform);
        var e = w.GetComponent<TextMeshProUGUI>();
        DisplaySegsmcnugget = e;
        UpdateText();

    }

    private void Start()
    {
        switch (Type)
        {
            case "Printer":
                PrinterItemSpawn(Gamer.Instance.GetItemForLevel());
                break;
        }
    }

    public void UpdateText()
    {
        var e = DisplaySegsmcnugget;
        workingtextoff = TextOffsetDist;
        string pon = InputManager.keynames[InputManager.gamekeys["interact"][0]];
        switch (Type)
        {
            case "Chest":
                if (Gamer.Instance.IsInShop)
                {
                    goto wank;
                }
                else
                {
                    e.text = $"3 Coins  [ {pon} ]";
                }
                break;
            case "Shrine":
                e.text = $"6 Coins  [ {pon} ]";
                break;
            case "Item":
                if (BananaMan == null || BananaMan.sexyballer.CoinCost <= 0)
                {
                    goto wank;
                }
                else
                {
                    e.text = $"{BananaMan.sexyballer.CoinCost} Coins  [ {pon} ]";
                }
                break;
            case "NextShop":
                e.text = $"Go to Market<br>[ {pon} ]";
                workingtextoff += 0.5f;
                break;
            case "NextShop2":
                e.text = $"Go to Emporium<br>[ {pon} ]";
                workingtextoff += 0.5f;
                break;
            case "StartGame":
                if (Gamer.Instance.Skipper && Gamer.CurrentFloor >= 1)
                {
                    e.text = $"Gain 10 Coins<br>[ {pon} ]";
                    workingtextoff += 0.5f;
                }
                else
                {
                    e.text = $"[ {pon} ]";
                }
                break;
            case "ContinueRun":
                e.text = $"Continue Previous Run<br>[ {pon} ]";
                break;
            default:
            wank:
                e.text = $"[ {pon} ]";
                break;
        }
    }
    public void Interact()
    {
        var g = Gamer.Instance;
        g.CanInteractThisFrame = false;
        if (g.IsFading) return;
        System.Action<float> aa;
        switch(Type)
        {
            case "Crafter":
                aa = g.ToggleInventory();
                if (aa == null || !Gamer.Instance.checks[0]) return;
                g.checks[2] = false;
                g.checks[1] = true;
                aa(0);
                g.UpdateMenus();
                g.cuumer.Open();
                g.InitCraftMenu();
                g.UpdateMenus();
                break;
            case "Vault":
                aa = g.ToggleInventory();
                if(aa==null || !Gamer.Instance.checks[0]) return;
                g.checks[11] = true;
                g.checks[2] = false;
                aa(0);
                g.UpdateMenus();
                g.OpenVault();
                Gamer.Instance.LoadVaultPage(0);
                break;
            case "Grafter":
                aa = g.ToggleInventory();
                if(aa==null || !Gamer.Instance.checks[0]) return;
                g.checks[2] = false;
                g.checks[18] = true;
                aa(0);
                g.UpdateMenus();
                break;
            case "Aspect":
                aa = g.ToggleInventory();
                if(aa==null || !Gamer.Instance.checks[0]) return;
                g.checks[2] = false;
                g.checks[19] = true;
                aa(0);
                g.UpdateMenus();
                break;
            case "TransmuterHub":
                aa = g.ToggleInventory();
                if(aa==null || !Gamer.Instance.checks[0]) return;
                g.checks[2] = false;
                g.checks[27] = true;
                aa(0);
                g.UpdateMenus();
                break;
            case "Repairer":
                aa = g.ToggleInventory();
                if(aa==null || !Gamer.Instance.checks[0]) return;
                g.checks[2] = false;
                g.checks[24] = true;
                aa(0);
                g.UpdateMenus();
                break;
            case "StartGame":
                if (PlayerFailsWeaponCheck()) return;
                Gamer.Instance.DurabilityHit();
                Gamer.Instance.StartCoroutine(Gamer.Instance.StartFade("NextFloor"));
                break;
            case "ContinueRun":
                if (PlayerFailsWeaponCheck()) return;
                SaveSystem.Instance.LoadCurrentRun();
                break;
            case "Quest":
                Gamer.Instance.ToggleQuests();
                break;
            case "Druggy":
                Gamer.Instance.ToggleDrugs();
                break;
            case "Skilledman":
                Gamer.Instance.ToggleSkillMenu();
                break;
            case "RetoolBananman":
                Gamer.Instance.ToggleRefreshMenu();
                break;
            case "Transmuter":
                Gamer.Instance.ToggleTransmutehMenu();
                break;
            case "Upgrader":
                Gamer.Instance.ToggleUpgradetree();
                break;
            case "Shrine":
                if(PlayerController.Instance.Coins >= 6)
                {
                    Shrine();
                    PlayerController.Instance.Coins -= 6;
                    Instantiate(Gamer.Instance.ParticleSpawns[10], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                    Destroy(gameObject);
                }
                break;
            case "Printer":
                Gamer.Instance.PrinterYoinks = cuum;
                Gamer.Instance.TogglePrinterMenu();
                break;
            case "NextShop":
                if (PlayerFailsWeaponCheck()) return;
                Gamer.Instance.StartCoroutine(Gamer.Instance.StartFade("NextShop"));
                break;
            case "NextShop2":
                if (PlayerFailsWeaponCheck()) return;
                Gamer.Instance.StartCoroutine(Gamer.Instance.StartFade("NextShop2"));
                break;
            case "Item":
                GetComponent<GroundItem>().AttemptPickup();
                break;
            case "ColorPass":
                var wank = Parente.GetComponent<ColorRoomBanana>().ClickityMe(this);
                break;
            case "ColorPassStart":
                Parente.GetComponent<ColorRoomBanana>().SrartThing();
                break;
            case "Chest":
                bool pass = Gamer.Instance.IsInShop;
                if (!pass && PlayerController.Instance.Coins >= 3)
                {
                    PlayerController.Instance.SpendCoin(3);
                    Gamer.QuestProgressIncrease("Room", "Chest");
                    pass = true;
                }
                if (pass)
                {
                    Gamer.Instance.SpawnGroundItem(transform.position, cuum);
                    Destroy(gameObject);
                }
                break;
        }
    }

    public static bool PlayerFailsWeaponCheck()
    {
        var c = GISLol.Instance.All_Containers["Equips"];
        return c.slots[0].Held_Item.ItemIndex == "Empty" || c.slots[1].Held_Item.ItemIndex == "Empty";
    }

    public void PrinterItemSpawn(GISItem item)
    {
        cuum = item;
        var ww = GISDisplay.GetSprites(item);
        memes[0].sprite = ww.sprites[0];
        memes[1].sprite = ww.sprites[1];
        memes[2].sprite = ww.sprites[2];
    }

    public void Shrine()
    {

        List<string> a = new List<string>();
        foreach(var b in GISLol.Instance.Effects)
        {
            if(b.Name.Contains("Shrine ")) a.Add(b.Name);
        }
        string chosen = a[Random.Range(0, a.Count)];

        var c = new EffectProfile(chosen, 180, 5, 1);
        PlayerController.Instance.entit.AddEffect(c);

        Gamer.QuestProgressIncrease("Room", "Shrine");

        var n = new OXNotif();
        n.Title = "ADA Has Sent Thanks";
        n.Description = chosen;
        n.Time = 5;
        n.BackgroundColor1 = new Color32(255, 199, 100,255);
        switch (chosen)
        {
            case "Shrine Attack Speed":
                n.Description = "x1.5 Attack Speed";
                break;
            case "Shrine Attack Damage":
                n.Description = "x1.5 Attack Damage";
                break;
            case "Shrine Movement Speed":
                n.Description = "x1.5 Movement Speed";
                break;
            case "Shrine Skill Cooldown":
                n.Description = "x0.65 Skill Cooldown";
                break;
            case "Shrine Max Health":
                n.Description = "x1.5 Max Health";
                break;
            case "Shrine Healing":
                n.Description = "x1.5 Healing";
                break;
            case "Shrine Crit Chance":
                n.Description = "x1.5 Crit Chance";
                break;
        }
        n.Description = $"<br>{n.Description}<br>(180s)";
        NotificationSystem.Instance.AddNotif(n);
    }


}

