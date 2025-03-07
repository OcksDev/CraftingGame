using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            DisplaySegsmcnugget.gameObject.SetActive(CanInteract && wanker);
            if (wanker)
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
        switch(Type)
        {
            case "Crafter":
                g.checks[1] = !g.checks[1];
                g.checks[0] = g.checks[1];
                g.checks[2] = false;
                g.checks[11] = false;
                g.cuumer.Open();
                g.InitCraftMenu();
                g.UpdateMenus();
                break;
            case "Vault":
                g.checks[11] = true;
                g.checks[1] = false;
                g.checks[0] = true;
                g.checks[2] = false;
                g.UpdateMenus();
                g.OpenVault();
                Gamer.Instance.LoadVaultPage(0);
                break;
            case "Grafter":
                g.ToggleInventory();
                g.checks[2] = false;
                g.checks[18] = true;
                g.UpdateMenus();
                break;
            case "Aspect":
                g.ToggleInventory();
                g.checks[2] = false;
                g.checks[19] = true;
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
            case "Skilledman":
                Gamer.Instance.ToggleSkillMenu();
                break;
            case "RetoolBananman":
                Gamer.Instance.ToggleRefreshMenu();
                break;
            case "Printer":
                Gamer.Instance.PrinterYoinks = cuum;
                Gamer.Instance.TogglePrinterMenu();
                break;
            case "NextShop":
                if (PlayerFailsWeaponCheck()) return;
                Gamer.Instance.StartCoroutine(Gamer.Instance.StartFade("NextShop"));
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

}

