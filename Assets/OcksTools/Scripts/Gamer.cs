using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Gamer : MonoBehaviour
{
    public bool DevNoTreeCost = false;
    public bool[] checks = new bool[30];
    public Material[] sexex = new Material[2];
    public Image fader;
    public Camera mainnerddeingle;
    public GameObject textShuingite;
    public List<GISContainer> ballers = new List<GISContainer>();
    public List<Sprite> floordinglers = new List<Sprite>();
    public List<EnemyHolder> EnemiesDos = new List<EnemyHolder>();
    public List<PlayerController> Players = new List<PlayerController>();
    public List<NavMeshEntity> EnemiesExisting = new List<NavMeshEntity>();
    public List<GameObject> Chests = new List<GameObject>();
    public List<EliteTypeHolder> EliteTypes = new List<EliteTypeHolder>();
    public GameObject HealerGFooFO;
    public GameObject CoinGFooFO;
    public NavMeshRefresher nmr;
    public Transform DoorHolder;
    public static bool IsMultiplayer = false;
    public GameObject GroundItemShit;
    public string CraftSex = "Sword";
    public Selector cuumer;
    public GameObject PlayerPrefab;
    public List<GameObject> healers = new List<GameObject>();
    public static int Seed = 0;
    public static string GameState = "Main Menu";
    public static System.Random GlobalRand = new System.Random();
    public Transform balls;
    public List<INteractable> spawnedchests = new List<INteractable>();
    public List<GroundItem> spawneditemsformymassivesexyballs = new List<GroundItem>();
    public Transform ItemDisplayParent;
    public GameObject ItemDisplay;
    public GameObject DoorFab;
    public GameObject SpawnFix;
    public Dictionary<string, EliteTypeHolder> EliteTypesDict = new Dictionary<string, EliteTypeHolder>();
    public List<GameObject> StupidAssDoorDoohickies = new List<GameObject>();
    public List<GameObject> ParticleSpawns = new List<GameObject>();
    public List<I_Room> LevelProgression = new List<I_Room>();
    public TextMeshProUGUI FloorHeader;
    public Transform InitFloorHeadPos;
    public NavMeshEntity LastHitEnemy;
    private float LastHitEnemyTimer;
    public EnemyBarOfAids enemybar;
    public GameObject enemybaroutline;
    public TMP_InputField ItemNameInput;
    public TextMeshProUGUI CoinCostDisplay;
    public List<string> ItemPoolMats = new List<string>();
    public List<string> ItemPoolRunes = new List<string>();
    public List<string> ItemPoolAspects = new List<string>();
    public bool CanInteractThisFrame;
    public int EnemySpawnNumber = 0;
    public string EnemySpawnElite = "";
    public bool NextFloorButtonSexFuck = false;
    public bool NextShopButtonSexFuck = false;
    public GameObject ItemTranser;
    public List<Image> HitSexers = new List<Image>();
    public static List<string> ActiveDrugs = new List<string>();
    public List<SkillCum> SkillCumSexers = new List<SkillCum>();
    public GameObject ItemAnimThing;
    public GameObject VaultThing;
    public GameObject LogbookThing;
    public GameObject EffectThing;
    public Volume volume;
    public Button FUCKYOUOHMYGOD;
    public List<Skill_Data> SkillOffers = new List<Skill_Data>();
    public double TimeOfQuest = 0;



    [HideInInspector]
    public bool InRoom = false;
    [HideInInspector]
    public I_Room CurrentRoom;

    private List<EffectorSexyBallzungussy> player_effect_prespawns = new List<EffectorSexyBallzungussy>();
    private List<EffectorSexyBallzungussy> Enemy_effect_prespawns = new List<EffectorSexyBallzungussy>();

    public GISItem PickupItemCrossover;

    public delegate void JustFuckingRunTheMethods();
    public event JustFuckingRunTheMethods RefreshUIPos;
    public static bool WithinAMenu = false;
    bool wasincraft = false;


    bool nexty = false;

    public void UpdateMenus()
    {
        Tags.refs["Inventory"].SetActive(checks[0]);
        Tags.refs["Crafting"].SetActive(checks[1]);
        Tags.refs["CrafterDarker"].SetActive(checks[1]);
        Tags.refs["Equips"].SetActive(checks[2]);
        Tags.refs["MainMenu"].SetActive(checks[3]);
        Tags.refs["MainMenu1"].SetActive(checks[3]);
        Tags.refs["MainMenu2"].SetActive(checks[3]);
        Tags.refs["PauseMenu"].SetActive(checks[4]);
        Tags.refs["ItemMenu"].SetActive(checks[5]);
        Tags.refs["DedMenu"].SetActive(checks[6]);
        Tags.refs["TempMatMenu"].SetActive(checks[7]);
        Tags.refs["SettingsMenu"].SetActive(checks[8]);
        Tags.refs["TransItems"].SetActive(checks[9]);
        Tags.refs["FuckPause"].SetActive(checks[10]);
        Tags.refs["Vault"].SetActive(checks[11]);
        Tags.refs["Logbook"].SetActive(checks[12]);
        Tags.refs["LogbookSubmenu"].SetActive(checks[13]);
        Tags.refs["QuestMenu"].SetActive(checks[14]);
        Tags.refs["SkillBuyMenu"].SetActive(checks[15]);
        Tags.refs["SkillSubBuy"].SetActive(checks[16]);
        Tags.refs["GrafterMenu"].SetActive(checks[18]);
        Tags.refs["AspectMenu"].SetActive(checks[19]);
        Tags.refs["Minigame"].SetActive(checks[21]);
        Tags.refs["Transmute"].SetActive(checks[22]);
        Tags.refs["RepairMenu"].SetActive(checks[24]);
        Tags.refs["UpgradeTree"].SetActive(checks[25]);
        Tags.refs["DrugMenu"].SetActive(checks[26]);
        if(nexty && !checks[25])
        {
            Gamer.Instance.UpdateLobbyStuff();
        }
        nexty = checks[25];
        Tags.refs["ItemeP"].SetActive(checks[20]);
        Tags.refs["ItemeR"].SetActive(checks[17]);
        Tags.refs["ItemeT"].SetActive(checks[23]);
        Tags.refs["Iteme"].SetActive(!(checks[17] || checks[20]|| checks[23]));

        Tags.refs["GameUI"].SetActive(GameState == "Game");
        Tags.refs["EnemiesRemaining"].SetActive(!IsInShop);

        if (wasincraft && !checks[1])
        {
            foreach(var a in GISLol.Instance.All_Containers["Crafting"].slots)
            {
                if(a.InteractFilter != "RockGive" && a.Held_Item.ItemIndex != "Empty")
                {
                    GISLol.Instance.GrantItem(a.Held_Item);
                    a.Held_Item = new GISItem();
                }
            }
        }
        foreach(var a in SkillCumSexers)
        {
            a.UpdateRare();
        }
        //SkillCumSexers[1].gameObject.SetActive(false);
        //SkillCumSexers[2].gameObject.SetActive(false);
        //SkillCumSexers[3].gameObject.SetActive(false);
        WithinAMenu = MenuAnim;
        InputManager.ResetLockLevel();
        if (!WithinAMenu)
        {
            for (int i = 0; i < checks.Length; i++)
            {
                if (checks[i])
                {
                    WithinAMenu = true;
                    InputManager.SetLockLevel("menu");
                    break;
                }
            }
        }
        if (checks[4])
        {
            InputManager.SetLockLevel("pause_menu");
        }
        if (checks[3])
        {
            InputManager.SetLockLevel("main_menu");
        }
        if (checks[5])
        {
            InputManager.SetLockLevel("item_menu");
        }
        InputManager.RemoveLockLevel("TextEntry");


        Tags.refs["Equippers"].SetActive(!WithinAMenu);

        FUCKYOUOHMYGOD.Select();

        UpdateShaders();
        RefreshUIPos?.Invoke();
        wasincraft = checks[1];
    }

    public void UpdateLobbyStuff()
    {
        Tags.refs["Lobby"].GetComponent<NodeEnablerLol>().Reebaka();
    }




    public static Gamer Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        checks = new bool[30];
        foreach (var a in EliteTypes)
        {
            EliteTypesDict.Add(a.Name, a);
        }
        var c2 = fader.color;
        c2.a = 1;
        fader.color = c2;
        mainnerddeingle.Render();
        AssembleRoomTypes();
    }

    public void AssembleRoomTypes()
    {
        ValidRoomTypes = new List<RoomTypeHolder> 
        { 
            new RoomTypeHolder("Chest"),
            new RoomTypeHolder("Chase The Orb"),
            //new RoomTypeHolder("Bullet Dodge"),
            new RoomTypeHolder("Passcode"),
            new RoomTypeHolder("Pick Three"),
            //new RoomTypeHolder("Monster Crystal"), //pendants?
            //new RoomTypeHolder("Shrine"),
            //new RoomTypeHolder("Cursed Item"),
        };
    }

    private void Start()
    {
        Tags.refs["BGblack"].SetActive(true);
        MainMenu();
        StartCoroutine(FUCK());
        StartCoroutine(SPawnPool());
    }
    public static List<List<string>> Backup = new List<List<string>>();
    public static List<List<string>> QBackup = new List<List<string>>();
    public static bool HasFinishedSpawningPool = false;
    public IEnumerator SPawnPool()
    {
        for(int i = 0; i < 63; i++)
        {
            yield return new WaitForFixedUpdate();
            var weenor = Instantiate(VaultThing, transform.position, Quaternion.identity, Tags.refs["VaultParent"].transform).GetComponent<VaultitemDisplay>();
            prespawnednerds.Add(weenor);
            weenor.gameObject.SetActive(false);
        }
        for(int i = 0; i < 30; i++)
        {
            yield return new WaitForFixedUpdate();
            var weenor1 = Instantiate(EffectThing, transform.position, Quaternion.identity, Tags.refs["PlayerEffects"].transform).GetComponent<EffectorSexyBallzungussy>();
            player_effect_prespawns.Add(weenor1);
            var weenor2 = Instantiate(EffectThing, transform.position, Quaternion.identity, Tags.refs["EnemyEffects"].transform).GetComponent<EffectorSexyBallzungussy>();
            Enemy_effect_prespawns.Add(weenor2);
        }
        HasFinishedSpawningPool = true;
    }
    public void ClearMap()
    {
        foreach (var sex in EnemiesExisting)
        {
            if (sex == null) continue;
            Destroy(sex.gameObject);
        }
        EnemiesExisting.Clear();
        foreach (var sex in healers)
        {
            if (sex == null) continue;
            Destroy(sex.gameObject);
        }
        healers.Clear();
        foreach (var room in RoomLol.Instance.SpawnedRooms)
        {
            if (room == null) continue;
            Destroy(room);
        }
        RoomLol.Instance.SpawnedRooms.Clear();
        foreach (var sex in spawnedchests)
        {
            if (sex == null) continue;
            Destroy(sex.gameObject);
        }
        spawnedchests.Clear();
        foreach (var sex in spawneditemsformymassivesexyballs)
        {
            if (sex == null) continue;
            Destroy(sex.gameObject);
        }
        spawneditemsformymassivesexyballs.Clear();
        foreach (var sex in OXComponent.GetComponentsInChildren<EnemyHitShit>(balls.gameObject))
        {
            Destroy(sex.gameObject);
        }
        foreach (var sex in balls.GetComponentsInChildren<Transform>())
        {
            if (sex != balls) Destroy(sex.gameObject);
        }
        foreach (var sex in OXComponent.GetComponentsInChildren<HitBalls>(balls.gameObject))
        {
            Destroy(sex.gameObject);
        }
        foreach (var sex in OXComponent.GetComponentsInChildren<partShitBall>(Tags.refs["ParticleHolder"]))
        {
            Destroy(sex.gameObject);
        }
        foreach (var sex in StupidAssDoorDoohickies)
        {
            if (sex == null) continue;
            Destroy(sex);
        }
        StupidAssDoorDoohickies.Clear();
        foreach (var sex in KillMeOnRoomSex)
        {
            if (sex == null) continue;
            Destroy(sex);
        }
        KillMeOnRoomSex.Clear();
        ShartPoop = 0;
        OXComponent.CleanUp();
        StartCoroutine(CorruptionCode.Instance.ClearAllNerds());
    }
    public List<GameObject> KillMeOnRoomSex = new List<GameObject>();
    public bool IsHost;
    public void LoadLobbyScene()
    {
        InputManager.SetLockLevel("");
        GeneralFloorChange();
        GameState = "Lobby";
        InRoom = false;
        CurrentRoom = null;
        OldCurrentRoom = null;
        Tags.refs["NextFloor"].transform.position = new Vector3(11.51f, 0, 0);
        Tags.refs["NextShop"].transform.position = new Vector3(100000, 100000, 0);
        Tags.refs["NextShop2"].transform.position = new Vector3(100000, 100000, 0);
        Tags.refs["Lobby"].SetActive(true);
        //Tags.refs["Baller"].transform.position = new Vector3(5.12f, -6.6f, 17.68f);
        if (IsMultiplayer)
        {
            IsHost = NetworkManager.Singleton.IsHost;
            StartCoroutine(WaitForSexyGamer());
        }
        foreach(var a in GISLol.Instance.Quests)
        {
            a.CheckComplete();
        }

        UpdateCurrentQuests();
        UpdateLobbyStuff();
    }

    public void UpdateCurrentQuests()
    {
        double f = 60 * 60 * 2;
        var weenor = (long)(RandomFunctions.Instance.GetUnixTime()/(f));
        if(weenor != TimeOfQuest)
        {
            TimeOfQuest = weenor;
            GISLol.Instance.Quests.Clear();
            for(int i = 0; i < 5; i++)
            {
                GISLol.Instance.Quests.Add(GetRandomQuest());
            }

        }
    }
    public QuestProgress GetRandomQuest()
    {
        weenis:
        Dictionary<string, List<string>> dat = new Dictionary<string, List<string>>()
        {
            {"mats", new List<string>(GISLol.Instance.AllCraftables) },
            {"weapons", GISLol.Instance.AllWeaponNames },
            {"runes", GISLol.Instance.AllRunes },
            {"rooms", new List<string>() },
        };
        dat["mats"].Remove("Rock");
        var weenor = new QuestProgress();
        List<string> list = new List<string>() 
        {
            "Collect",
            "Kill",
            "Craft",
            "Room",
        };
        weenor.Data["Name"] = list[Random.Range(0, list.Count)];
        var sexx = Random.Range(3, 6);
        switch (weenor.Data["Name"])
        {
            case "Room":
                foreach(var a in ValidRoomTypes)
                {
                    dat["rooms"].Add(a.Name);
                }
                break;
            default:
                break;
        }
        switch (weenor.Data["Name"])
        {
            case "Collect":
                weenor.Data["Target_Data"] = dat["mats"][Random.Range(0, dat["mats"].Count)];
                weenor.Data["Target_Amount"] = sexx.ToString();
                weenor.Data["Reward_Data"] = dat["mats"][Random.Range(0, dat["mats"].Count)];
                weenor.Data["Reward_Amount"] = sexx.ToString();
                break;
            case "Kill":
                weenor.Data["Target_Data"] = dat["weapons"][Random.Range(0, dat["weapons"].Count)];
                weenor.Data["Target_Amount"] = (sexx*100).ToString();
                weenor.Data["Reward_Data"] = dat["mats"][Random.Range(0, dat["mats"].Count)];
                weenor.Data["Reward_Amount"] = sexx.ToString();
                break;
            case "Craft":
                weenor.Data["Target_Data"] = dat["weapons"][Random.Range(0, dat["weapons"].Count)];
                weenor.Data["Target_Amount"] = (sexx).ToString();
                weenor.Data["Reward_Data"] = dat["mats"][Random.Range(0, dat["mats"].Count)];
                weenor.Data["Reward_Amount"] = (sexx*2).ToString();
                break;
            case "Room":
                weenor.Data["Target_Data"] = dat["rooms"][Random.Range(0, dat["rooms"].Count)];
                weenor.Data["Target_Amount"] = (sexx*2).ToString();
                weenor.Data["Reward_Data"] = dat["mats"][Random.Range(0, dat["mats"].Count)];
                weenor.Data["Reward_Amount"] = (sexx).ToString();
                break;
        }

        foreach(var e in GISLol.Instance.Quests)
        {
            if (e.Data["Name"] == weenor.Data["Name"] && e.Data["Target_Data"] == weenor.Data["Target_Data"])
            {
                goto weenis;
            }
        }

        return weenor;
    }


    public IEnumerator instancecoolmenus() // this is the most retarded fix for a thing I have made in a while
    {
        checks[0] = true;
        checks[1] = true;
        checks[2] = true;
        checks[7] = true;
        UpdateMenus();
        yield return new WaitForFixedUpdate();
        checks[0] = false;
        checks[1] = false;
        checks[2] = false;
        checks[7] = false;
        UpdateMenus();
#if UNITY_EDITOR
        StartCoroutine(StartFade("DingleBob", 5, true));
#else
        StartCoroutine(StartFade("DingleBob", 25, true));
#endif
    }




    public IEnumerator WaitForSexyGamer()
    {
        yield return new WaitUntil(() => { return ServerGamer.Instance != null; });
        foreach (var s in Backup)
        {
            OcksNetworkVar g = new OcksNetworkVar(s[1], s[0]);
            g.SetValue(s[2]);
        }
        foreach (var s in QBackup)
        {
            if (Tags.customdata.ContainsKey(s[0]))
            {
                if (!Tags.customdata[s[0]].ContainsKey(s[1]))
                {
                    OcksNetworkVar g = new OcksNetworkVar(s[1], s[0]);
                    g.Query();
                }
            }
            else
            {
                OcksNetworkVar g = new OcksNetworkVar(s[1], s[0]);
                g.Query();
            }
        }
    }
    public void ResetIntegreal()
    {
        if (titlething != null) StopCoroutine(titlething);
        CameraLol.Instance.shakeo.Clear();
        CurrentFloor = 0;
        LastHitEnemy = null;
        Edgemogging = false;
        CameraMouseMult = 1;
        enemybar.gameObject.SetActive(false);
        enemybaroutline.gameObject.SetActive(false);
        enemybar.BarParentSize.gameObject.SetActive(false);
        MenuAnim = false;
        if (NextFloorBall != null) StopCoroutine(NextFloorBall);
        FloorHeader.transform.position = InitFloorHeadPos.position;
        for (int i = 0; i < checks.Length; i++)
        {
            checks[i] = false;
        }
        Tags.refs["Lobby"].SetActive(false);
        Tags.refs["NextFloor"].SetActive(false);
        Tags.refs["NextShop"].SetActive(false);
        Tags.refs["NextShop2"].SetActive(false);
        Time.timeScale = 1;
        ShartPoop = 0f;
        ClearMap();
        Tags.refs["BlackBG"].SetActive(false);
        CraftSex = "Sword";
        GameState = "Main Menu";
        Players.Clear();
        if (PlayerController.Instance != null && !IsMultiplayer) Destroy(PlayerController.Instance.gameObject);
        UpdateMenus();
        var e2 = CameraLol.Instance.transform.position;
        e2.x = 0;
        e2.y = 0;
        InputManager.SetLockLevel("main_menu");
        foreach (var a in EliteTypes)
        {
            a.Enabled = true;
        }

        CameraLol.Instance.transform.position = e2;
        CameraLol.Instance.ppos = e2;
        CameraLol.Instance.targetpos = e2;
    }

    public void MainMenu()
    {
        if (IsMultiplayer) NetworkManager.Singleton.Shutdown();
        IsMultiplayer = false;
        ResetIntegreal();
        checks[3] = true;
        UpdateMenus();
    }


    public IEnumerator FUCK()
    {
        yield return new WaitForFixedUpdate();
        foreach (var e in ballers)
        {
            e.Start();
            e.LoadContents();
        }
        yield return null;
        StartCoroutine(instancecoolmenus());
    }

    public static bool GameInPlay()
    {
        return GameState == "Game" || GameState == "Lobby";
    }

    public void Update()
    {
        if (!IsFading && !MenuAnim && InputManager.IsKeyDown("close_menu"))
        {
             if (checks[14])
            {
                ToggleQuests();
            }
            else if(checks[21])
            {
                StartCoroutine(CloseMinigame());
            }
            else if(checks[0])
            {
                ToggleInventory(true);
            }
            else if (checks[8])
            {
                ToggleSettings();
            }
            else if (checks[13])
            {
                checks[13] = false;
                UpdateMenus();
            }
            else if (checks[26])
            {
                ToggleDrugs();
            }
            else if (checks[12])
            {
                ToggleLogbook();
            }
            else if (checks[25])
            {
                ToggleUpgradetree();
            }
            else if (checks[10])
            {
                ToggleFuckPause();
            }
            else if (checks[16])
            {
                ToggleSkillSubMenu();
            }
            else if (checks[15])
            {
                ToggleSkillMenu();
            }
            else if (checks[22])
            {
                SetTranStat(false);
            }
            else if (checks[5] || checks[17]|| checks[23])
            {
                if(!anim)
                ToggleItemPickup();
            }
            else if (ConsoleLol.Instance.enable)
            {
                ConsoleLol.Instance.CloseConsole();
            }
            else if (checks[3])
            {
            }
            else
            {
               if(!WithinAMenu || checks[4]) SetPauseMenu(!checks[4]);
            }
        }
        if ((InputManager.IsKeyDown("inven", "player") && GameState == "Lobby") || (checks[0] && InputManager.IsKeyDown("inven", "menu")))
        {
            if (checks[0])
            {
                ToggleInventory(true);
            }
            else
            {
                ToggleInventory();
            }
        }
        if (checks[11])
        {
            if (InputManager.IsKeyDown(KeyCode.RightArrow, "menu")) VaultIncrease();
            if (InputManager.IsKeyDown(KeyCode.LeftArrow, "menu")) VaultDecrease();
        }

#if UNITY_EDITOR
        if (InputManager.IsKeyDown(KeyCode.Space, "player"))
        {
            //SaveSystem.Instance.SaveGame();
            var a = SpawnEnemy(EnemiesDos[EnemySpawnNumber]);
            a.EliteType = EnemySpawnElite;
        }
#endif

    }
    [HideInInspector]
    public int currentvault = 0;
    [HideInInspector]
    public List<VaultitemDisplay> spawnednerds = new List<VaultitemDisplay>();
    [HideInInspector]
    public List<VaultitemDisplay> prespawnednerds = new List<VaultitemDisplay>();
    public int SortMethod = 0;

    public TMP_InputField VaultSearch;
    public void OpenVault()
    {
        VaultSearch.text = "";
    }
    private Dictionary<GISItem, int> memesex;
    public void LoadVaultPage(int page)
    {
        currentvault = page;
        int amount = 63;
        List<KeyValuePair<GISItem, int>> penis = new List<KeyValuePair<GISItem, int>>();
        List<KeyValuePair<GISItem, int>> penis2 = new List<KeyValuePair<GISItem, int>>();
        List<KeyValuePair<GISItem, int>> penis3 = new List<KeyValuePair<GISItem, int>>();

        memesex = new Dictionary<GISItem, int>(GISLol.Instance.VaultItems);
        if(VaultSearch.text != "")
        for(int i = 0; i < memesex.Count; i++)
        {
            var x = memesex.ElementAt(i);
            bool rem = false;
            if(x.Key.CustomName != "")
            {
                if(!x.Key.CustomName.Contains(VaultSearch.text)) rem = true;
            }
            else
            {
                if (!x.Key.ItemIndex.Contains(VaultSearch.text)) rem = true;
            }
            if (rem)
            {
                memesex.Remove(x.Key);
                i--;
            }
        }


        bool shungite = memesex.Count > 0;
        while (shungite)
        {
            try
            {
                var wenor = memesex.ElementAt(amount * page);
                shungite = false;
            }
            catch
            {
                page--;
            }
        }

        Tags.refs["VaultButt1"].SetActive(memesex.Count > (currentvault + 1) * 63);
        Tags.refs["VaultButt2"].SetActive(currentvault > 0);

        for (int i = 0; i < memesex.Count; i++)
        {
            var wenor = memesex.ElementAt(i);
            var rx = GISLol.Instance.ItemsDict[wenor.Key.ItemIndex];
            if (rx.IsCraftable)
            {
                penis.Add(wenor);
            }
            else if(rx.IsAspect)
            {
                penis3.Add(wenor);
            }
            else
            {
                penis2.Add(wenor);
            }
        }
        RandomFunctions.CombineListsNoDupe(penis, penis3);
        RandomFunctions.CombineListsNoDupe(penis, penis2);
        int offset = (penis.Count - (page*amount)) - spawnednerds.Count;
        if (offset > 0)
        {
            offset = Mathf.Min(offset, amount);
        }
        else
        {
            offset = Mathf.Max(offset, -amount);
        }
        int jjj = prespawnednerds.Count;
        for (int i = 0; i < offset && spawnednerds.Count < amount; i++)
        {
            //var weenor = Instantiate(VaultThing, transform.position, Quaternion.identity, Tags.refs["VaultParent"].transform).GetComponent<VaultitemDisplay>();
            var weenor = prespawnednerds[jjj-i-1];
            weenor.gameObject.SetActive(true);
            spawnednerds.Add(weenor);
            prespawnednerds.Remove(weenor);
        }
        int jj = spawnednerds.Count;
        for(int i = 0; i < -offset && i < amount; i++)
        {
            int j = jj - i - 1;
            var weenor = spawnednerds[j];
            weenor.gameObject.SetActive(false);
            prespawnednerds.Add(weenor);
            spawnednerds.RemoveAt(j);

        }
        jj = spawnednerds.Count-1;
        for (int i = 0; i < (penis.Count - (page * amount)) && i < amount; i++)
        {
            spawnednerds[jj - i].item = penis[i + (page*amount)].Key;
            spawnednerds[jj - i].UpdateDisplay();
        }
    }
    public float Highlights;
    public float Lowlights;
    public void UpdateShaders()
    {
        if (volume.profile.TryGet(out UnityEngine.Rendering.Universal.SplitToning spplit))
        {
            spplit.highlights.Override(new Color(Highlights, Highlights, Highlights));
            spplit.shadows.Override(new Color(Lowlights, Lowlights, Lowlights));
        }
    }
    public void VaultIncrease()
    {
        if(memesex.Count > (currentvault+1) * 63 )
        LoadVaultPage(currentvault + 1);
    }
    public void VaultDecrease()
    {
        if (currentvault > 0)
        LoadVaultPage(currentvault -1);
    }

    public static int EnemyCheckoffset = 0;
    public System.Action<float> ToggleInventory(bool overrides = false)
    {
        if (MenuAnim) return null;
        if (GISLol.Instance.Mouse_Held_Item.ItemIndex != "Empty")
        {
            GISLol.Instance.GrantItem(GISLol.Instance.Mouse_Held_Item);
            GISLol.Instance.Mouse_Held_Item = new GISItem();
        }

        if(!overrides && checks[0]) overrides = true;



        if (checks[18])
        {
            foreach (var a in GISLol.Instance.All_Containers["Grafter"].slots)
            {
                if (a.Held_Item.ItemIndex != "Empty")
                {
                    GISLol.Instance.GrantItem(a.Held_Item);
                    a.Held_Item = new GISItem();
                }
            }
        }
        if (checks[19])
        {
            foreach (var a in GISLol.Instance.All_Containers["Aspecter"].slots)
            {
                if (a.Held_Item.ItemIndex != "Empty")
                {
                    GISLol.Instance.GrantItem(a.Held_Item);
                    a.Held_Item = new GISItem();
                }
            }
        }
        System.Action bana = () => 
        {
            checks[1] = false;
            checks[11] = false;
            checks[18] = false;
            checks[19] = false;
            checks[24] = false;
            checks[2] = checks[0];
            UpdateMenus();
        };
        checks[0] = !checks[0];
        if (overrides) goto why;
        bana();
        why:
        var aa = Tags.refs["Inventory"].GetComponent<MenuMover>();
        aa.Initial();
        System.Action<float> y = (x) =>
        {
            aa.nerds[0].localPosition = Vector3.Lerp(new Vector3(aa.nerds_orig[0].x, -775, 0), aa.nerds_orig[0], RandomFunctions.EaseIn(x));
            if (checks[1]) aa.nerds[1].localPosition = Vector3.Lerp(new Vector3(aa.nerds_orig[1].x, 775, 0), aa.nerds_orig[1], RandomFunctions.EaseIn(x));
            if (checks[11]) aa.nerds[2].localPosition = Vector3.Lerp(new Vector3(aa.nerds_orig[2].x, 775, 0), aa.nerds_orig[2], RandomFunctions.EaseIn(x));
            if (checks[18]) aa.nerds[3].localPosition = Vector3.Lerp(new Vector3(aa.nerds_orig[3].x, 775, 0), aa.nerds_orig[3], RandomFunctions.EaseIn(x));
            if (checks[19]) aa.nerds[4].localPosition = Vector3.Lerp(new Vector3(aa.nerds_orig[4].x, 775, 0), aa.nerds_orig[4], RandomFunctions.EaseIn(x));
            if (checks[24]) aa.nerds[5].localPosition = Vector3.Lerp(new Vector3(aa.nerds_orig[5].x, 775, 0), aa.nerds_orig[5], RandomFunctions.EaseIn(x));
            aa.nerds_img[0].color = Color.Lerp(new Color(0, 0, 0, 0), aa.nerds_img_orig[0], x);
            aa.nerds_img[1].color = Color.Lerp(new Color(0, 0, 0, 0), aa.nerds_img_orig[1], x);
        };
        y(0);
        StartCoroutine(InvenAids(y, !checks[0], bana));
        return y;
    }

    public IEnumerator InvenAids(System.Action<float> y, bool fu, System.Action b)
    {
        yield return StartCoroutine(MenuAnimationLol(!checks[0], !checks[0], y));
        if (fu) b();
    }


    public void ToggleSettings()
    {
        checks[8] = !checks[8];
        UpdateMenus();
    }
    public void ToggleQuests()
    {
        checks[14] = !checks[14];
        var aa = Tags.refs["QuestMenu"].GetComponent<MenuMover>();
        aa.Initial();
        if (checks[14])
        {
            Tags.refs["QuestMenu"].GetComponent<QuestMenuUpdater>().OpenCum();
            UpdateMenus();
        }
        System.Action<float> y = (x) =>
        {
            aa.nerds[0].localPosition = Vector3.Lerp(new Vector3(0, -720, 0), aa.nerds_orig[0], RandomFunctions.EaseIn(x));
            aa.nerds_img[0].color = Color.Lerp(new Color(0,0,0,0), aa.nerds_img_orig[0], x);
        };
        y(0);
        StartCoroutine(MenuAnimationLol(!checks[14],!checks[14], y));
    }
    

    public void ToggleDrugs()
    {
        checks[26] = !checks[26];
        var aa = Tags.refs["DrugMenu"].GetComponent<MenuMover>();
        aa.Initial();
        if (checks[26])
        {
            var aaa = Tags.refs["DrugMenu"].GetComponent<DrugSex>();
            aaa.DoAll();
            UpdateMenus();
        }
        System.Action<float> y = (x) =>
        {
            aa.nerds[0].localPosition = Vector3.Lerp(new Vector3(0, -720, 0), aa.nerds_orig[0], RandomFunctions.EaseIn(x));
            aa.nerds_img[0].color = Color.Lerp(new Color(0,0,0,0), aa.nerds_img_orig[0], x);
        };
        y(0);
        StartCoroutine(MenuAnimationLol(!checks[26],!checks[26], y));
    }
    
    public void ToggleUpgradetree()
    {
        checks[25] = !checks[25];
        UpdateMenus();
        /*
        var aa = Tags.refs["UpgradeTree"].GetComponent<MenuMover>();
        aa.Initial();
        if (checks[25])
        {
            UpdateMenus();
        }
        System.Action<float> y = (x) =>
        {
            aa.nerds[0].localPosition = Vector3.Lerp(new Vector3(0, 550, 0), aa.nerds_orig[0], RandomFunctions.EaseIn(x));
            aa.nerds_img[0].color = Color.Lerp(new Color(0,0,0,0), aa.nerds_img_orig[0], x);
        };
        y(0);
        StartCoroutine(MenuAnimationLol(!checks[25],!checks[25], y));*/
    }

    public IEnumerator MenuAnimationLol(bool updateoncum, bool reversedir, System.Action<float> x)
    {
        MenuAnim = true;
        if (reversedir)
        {
            yield return StartCoroutine(OXLerp.Linear((y) => { x(1 - y); }, 0.5f));
        }
        else
        {
            yield return StartCoroutine(OXLerp.Linear(x, 0.5f));
        }
        MenuAnim = false;
        if (updateoncum) UpdateMenus();
    }

    public void ToggleLogbook()
    {
        checks[12] = !checks[12];
        checks[13] = false;
        if (checks[12])
        {
            ReloadLogbookItems();
        }

        UpdateMenus();
    }
    
    public void ToggleSkillMenu()
    {
        checks[15] = !checks[15];
        checks[16]= false;  
        if (checks[15])
        {
            var ggg = Tags.refs["SkillBuyMenu"].GetComponent<GAMBLING>();
            foreach (var item in ggg.displays)
            {
                item.transform.parent.gameObject.SetActive(true);
            }
            REFSkillOfferDisplay();
        }
        UpdateMenus();
    }
    public void ToggleRefreshMenu()
    {
        checks[5] = !checks[5];
        checks[17] = true;
        Ubdatebananasexballsfucucucuc();
        Shanana();
    }
    public void ToggleTransmutehMenu()
    {
        checks[5] = !checks[5];
        //checks[22] = true;
        hastrantempyes = false;
        checks[23] = true;
        Shanana();
    }

    public GISItem PrinterYoinks;
    public void TogglePrinterMenu()
    {
        checks[5] = !checks[5];
        checks[20] = true;
        Shanana();
        Tags.refs["ItemeP"].GetComponent<PrintRefHold>().ItemDick.item = PrinterYoinks;
    }
    
    public void AttemptSKillBuy(int index)
    {
        if(PlayerController.Instance.Coins >= 10)
        {
            var ggg = Tags.refs["SkillBuyMenu"].GetComponent<GAMBLING>();
            var ggg2 = Tags.refs["SkillSubBuy"].GetComponent<SkillThingbb>();
            if(ggg.displays[index].item.ItemIndex != "Empty")
            {
                ggg2.gup.item = ggg.displays[index].item;
                ggg2.gup.UpdateDisplay();
                ggg2.inititem = ggg2.gup.item;
                ggg2.initindex = index;
                for (int i = 0; i < 3; i++)
                {
                    ggg2.skills[i].item = new GISItem(PlayerController.Instance.Skills[i + 1].Name);
                }
                ToggleSkillSubMenu();

                foreach (var item in ggg.displays)
                {
                    item.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SpawnPrinter(Vector3 loc)
    {
        Instantiate(RandomFunctions.Instance.SpawnRefs[3], loc, Quaternion.identity, balls);
    }

    public void ToggleSkillSubMenu()
    {
        checks[16] = !checks[16];
        if (!checks[16])
        {
            var ggg = Tags.refs["SkillBuyMenu"].GetComponent<GAMBLING>();
            foreach (var item in ggg.displays)
            {
                item.transform.parent.gameObject.SetActive(true);
            }
        }
        UpdateMenus();
    }

    public void REFSkillOfferDisplay()
    {
        var ggg = Tags.refs["SkillBuyMenu"].GetComponent<GAMBLING>();
        ggg.displays[0].item = new GISItem(SkillOffers[0].Name);
        ggg.displays[1].item = new GISItem(SkillOffers[1].Name);
        ggg.displays[2].item = new GISItem(SkillOffers[2].Name);
        ggg.displays[0].UpdateDisplay();
        ggg.displays[1].UpdateDisplay();
        ggg.displays[2].UpdateDisplay();
        ggg.texts[0].text = $"{SkillOffers[0].GetCost()} Coins";
        ggg.texts[1].text = $"{SkillOffers[1].GetCost()} Coins";
        ggg.texts[2].text = $"{SkillOffers[2].GetCost()} Coins";
        ggg.texts[3].text = $"{skillrollamnt * 3} Coins";
    }
    int skillrollamnt = 0;
    int itemrollamnt = 0;
    public void RollNewSkillSex()
    {
        if (PlayerController.Instance.Coins < skillrollamnt * 3) return;
        PlayerController.Instance.Coins -= skillrollamnt * 3;
        skillrollamnt++;
        SkillOffers.Clear();
        var sk = new List<Skill_Data>(GISLol.Instance.Skills);
        for (int i = 0; i < sk.Count; i++)
        {
            if (!sk[i].CanSpawn)
            {
                sk.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            var index = Random.Range(0, sk.Count);
            SkillOffers.Add(sk[index]);
            sk.RemoveAt(index);
        }
        REFSkillOfferDisplay();
    }


    public void AttemptAddLogbookItem(string item)
    {
        if (!GISLol.Instance.LogbookDiscoveries.ContainsKey(item))
        {
            GISLol.Instance.LogbookDiscoveries.Add(item, "");
            if (item == "Rock") return;

            var ww = GISLol.Instance.ItemsDict[item];
            var notif = new OXNotif();
            if (ww.IsSkill)
            {
                notif.Title = "Skill Discovered";
            }
            else if (ww.IsEnemy)
            {
                notif.Title = "Enemy Discovered";
            }
            else
            {
                notif.Title = "Item Discovered";
            }
            notif.Item = new GISItem(item);
            //notif.Description = ww.GetDisplayName();
            notif.BackgroundColor1 = new Color32(50, 230, 227, 255);
            //notif.BackgroundColor2 = new Color32(50, 230, 227, 255);
            NotificationSystem.Instance.AddNotif(notif);
        }
    }

    List<I_penis> spawnsofmyballs1 = new List<I_penis>();
    List<I_penis> spawnsofmyballs2 = new List<I_penis>();
    List<I_penis> spawnsofmyballs3 = new List<I_penis>();
    public void ReloadLogbookItems()
    {
        List<string> items1 = new List<string>();
        List<string> items2 = new List<string>();
        List<string> items3 = new List<string>();
        List<string> skills = new List<string>();
        List<string> enems = new List<string>();

        foreach (var a in GISLol.Instance.Items)
        {
            if (a.CanSpawn || a.LogbookOverride)
            {
                if (a.IsCraftable) items1.Add(a.Name);
                else if (a.IsRune) items2.Add(a.Name);
                else if (a.IsSkill) skills.Add(a.Name);
                else if (a.IsEnemy) enems.Add(a.Name);
                else if (a.IsAspect) items3.Add(a.Name);
            }
        }
        items1 = RandomFunctions.CombineLists(items1, items2);
        items1 = RandomFunctions.CombineLists(items1, items3);
        int diff = items1.Count - spawnsofmyballs1.Count;
        for(int i = 0; i < diff; i++)
        {
            spawnsofmyballs1.Add(Instantiate(LogbookThing, transform.position, transform.rotation, Tags.refs["LogbookParent1"].transform).GetComponent<I_penis>());
        }
        for(int i = 0; i < -diff; i++)
        {
            Destroy(spawnsofmyballs1[0].gameObject);
            spawnsofmyballs1.RemoveAt(0);
        }
        for(int i = 0; i < items1.Count; i++)
        {
            spawnsofmyballs1[i].GISDisplay.item = new GISItem(items1[i]);
            spawnsofmyballs1[i].GISDisplay.UpdateDisplay("logbook");
        }

        diff = skills.Count - spawnsofmyballs2.Count;
        for(int i = 0; i < diff; i++)
        {
            spawnsofmyballs2.Add(Instantiate(LogbookThing, transform.position, transform.rotation, Tags.refs["LogbookParent2"].transform).GetComponent<I_penis>());
        }
        for(int i = 0; i < -diff; i++)
        {
            Destroy(spawnsofmyballs2[0].gameObject);
            spawnsofmyballs2.RemoveAt(0);
        }
        for(int i = 0; i < skills.Count; i++)
        {
            spawnsofmyballs2[i].GISDisplay.item = new GISItem(skills[i]);
            spawnsofmyballs2[i].GISDisplay.UpdateDisplay("logbook");
        }

        diff = enems.Count - spawnsofmyballs3.Count;
        for(int i = 0; i < diff; i++)
        {
            spawnsofmyballs3.Add(Instantiate(LogbookThing, transform.position, transform.rotation, Tags.refs["LogbookParent3"].transform).GetComponent<I_penis>());
        }
        for(int i = 0; i < -diff; i++)
        {
            Destroy(spawnsofmyballs3[0].gameObject);
            spawnsofmyballs3.RemoveAt(0);
        }
        for(int i = 0; i < enems.Count; i++)
        {
            spawnsofmyballs3[i].GISDisplay.item = new GISItem(enems[i]);
            spawnsofmyballs3[i].GISDisplay.UpdateDisplay("logbook");
        }
    }

    public List<string> GetSelfSimilar(string item)
    {
        var aa = GISLol.Instance.ItemsDict[item];
        if (aa.IsCraftable) return ItemPoolMats;
        if (aa.IsRune) return ItemPoolRunes;
        if (aa.IsAspect) return ItemPoolAspects;
        return null;
    }



    public void ToggleFuckPause()
    {
        if(GameState == "Lobby")
        {
            MainMenu();
        }
        else
        {
            checks[10] = !checks[10];
            UpdateMenus();
        }
    }
    List<MaterialTransfer> oldnerds = new List<MaterialTransfer>();

    public void ContinueFromDeathMenu()
    {
        var strings = GetRunItems();
        if(strings.Count == 0)
        {
            FadeToLobby();
        }
        else
        {
            checks[9] = false;
            ToggleItemTrans(strings);
        }
    }

    public void ToggleItemTrans(List<string> strings)
    {
        checks[9] = !checks[9];
        checks[6] = false;
        if (checks[9])
        {
            foreach (var a in oldnerds)
            {
                try
                {
                    Destroy(a.gameObject);
                }
                catch
                {

                }
            }
            foreach (var a in Tags.refs["RightTrans"].GetComponentsInChildren<MaterialTransfer>())
            {
                try
                {
                    Destroy(a.gameObject);
                }
                catch
                {

                }
            }
            foreach (var a in Tags.refs["LeftTrans"].GetComponentsInChildren<MaterialTransfer>())
            {
                try
                {
                    Destroy(a.gameObject);
                }
                catch
                {

                }
            }
            oldnerds.Clear();
            foreach (var a in strings)
            {
                try
                {
                    SpawnItemTranser(new GISItem(a), "FromRun");
                }
                catch
                {

                }
            }
        }
        UpdateMenus();
    }

    public List<string> GetRunItems()
    {
        List<string> strings = new List<string>();
        var c = GISLol.Instance.All_Containers["Equips"];
        foreach (var a in c.slots[0].Held_Item.Run_Materials)
        {
            if (a == null) continue;
            if (GISLol.Instance.ItemsDict.TryGetValue(a.GetName(), out GISItem_Data v))
            {
                if (v.IsCraftable || v.IsAspect)
                {
                    strings.Add(v.Name);
                }
            }
        }
        foreach (var a in c.slots[1].Held_Item.Run_Materials)
        {
            if (a == null) continue;
            if (GISLol.Instance.ItemsDict.TryGetValue(a.GetName(), out GISItem_Data v))
            {
                if (v.IsCraftable || v.IsAspect)
                {
                    strings.Add(v.Name);
                }
            }
        }
        return strings;
    }

    public void SpawnItemTranser(GISItem item, string side)
    {
        string weewee = "RightTrans";
        if (side == "FromRun") weewee = "LeftTrans";
        var transfer = Instantiate(ItemTranser, transform.position, transform.rotation, Tags.refs[weewee].transform).GetComponent<MaterialTransfer>();
        oldnerds.Add(transfer);
        transfer.dip.item = item;
        transfer.Type = side;
        transfer.dip.UpdateDisplay();
    }

    public void SetPauseMenu(bool a)
    {
        checks[4] = a;
        if (!IsMultiplayer) Time.timeScale = checks[4] ? 0 : 1;
        UpdateMenus();
    }
    public void ToggleItemPickup()
    {
        checks[5] = !checks[5];
        checks[17] = false;
        checks[22] = false;
        checks[23] = false;
        Shanana();
    }

    public void Shanana()
    {
        if (checks[5])
        {
            var c = GISLol.Instance.All_Containers["Equips"];

            var poopy = Tags.refs["InititemPickup"].GetComponent<GISContainer>();
            if (checks[17] || checks[20] || checks[23])
            {
                poopy.slots[0].Held_Item = new GISItem();
                poopy.slots[0].Displayer.UpdateDisplay();
            }
            else
            {
                poopy.slots[0].Held_Item = PickupItemCrossover;
                poopy.slots[0].Displayer.UpdateDisplay();
                AttemptAddLogbookItem(PickupItemCrossover.ItemIndex);
            }
            if(PickupItemCrossover != null && PickupItemCrossover.CoinCost > 0)
            {
                CoinCostDisplay.text = $"{PickupItemCrossover.CoinCost} Coins";
            }
            else
            {
                CoinCostDisplay.text = "";
            }


            var leftnut = Tags.refs["LeftItemItems"].GetComponent<GISContainer>();
            leftnut.ClearSlots();
            leftnut.slots.Clear();
            leftnut.GenerateSlots(System.Math.Clamp(CurrentFloor * 2, 2, 32));
            var leftnutitem = Tags.refs["LeftItemNut"].GetComponent<GISDisplay>();
            leftnutitem.item = c.slots[0].Held_Item;
            leftnutitem.UpdateDisplay();
            for (int i = 0; i < leftnutitem.item.Run_Materials.Count; i++)
            {
                var wank = leftnutitem.item.Run_Materials[i];
                string name = wank.itemindex != "" ? wank.itemindex : wank.index;
                if (name != "")
                {
                    leftnut.slots[i].Held_Item = new GISItem(name);
                }
                leftnut.slots[i].CanInteract = true;
            }

            leftnut = Tags.refs["RightItemItems"].GetComponent<GISContainer>();
            leftnut.ClearSlots();
            leftnut.GenerateSlots(System.Math.Clamp(CurrentFloor * 2, 2, 32));
            leftnutitem = Tags.refs["RightItemNut"].GetComponent<GISDisplay>();
            leftnutitem.item = c.slots[1].Held_Item;
            leftnutitem.UpdateDisplay();
            for (int i = 0; i < leftnutitem.item.Run_Materials.Count; i++)
            {
                var wank = leftnutitem.item.Run_Materials[i];
                string name = wank.itemindex != "" ? wank.itemindex : wank.index;
                if (name != "")
                {
                    leftnut.slots[i].Held_Item = new GISItem(name);
                }
            }
            StartCoroutine(FUCKYOU());
        }
        else
        {
            GISLol.Instance.Mouse_Held_Item = new GISItem();
            PickupItemCrossover.IAMSPECIL = null;
            checks[17] = false;
            checks[20] = false;
        }
        UpdateMenus();
    }
    bool hastrantempyes = false;
    public void AttemptOpenTransmute(GISItem meme)
    {
        if (hastrantempyes) return;
        SetTranStat(true, meme);
    }
    List<I_penis> spawndinglebobs = new List<I_penis>();
    public void SetTranStat(bool ree, GISItem a = null)
    {
        checks[22] = ree;
        if (ree)
        {
            var wankwank = GetSelfSimilar(a.ItemIndex);

            var diff = wankwank.Count - spawndinglebobs.Count;
            for (int i = 0; i < diff; i++)
            {
                spawndinglebobs.Add(Instantiate(LogbookThing, transform.position, transform.rotation, Tags.refs["TransmuteParent"].transform).GetComponent<I_penis>());
            }
            for (int i = 0; i < -diff; i++)
            {
                Destroy(spawndinglebobs[0].gameObject);
                spawndinglebobs.RemoveAt(0);
            }
            for (int i = 0; i < wankwank.Count; i++)
            {
                spawndinglebobs[i].GISDisplay.item = new GISItem(wankwank[i]);
                spawndinglebobs[i].GISDisplay.UpdateDisplay("logbook");
            }
        }
        else
        {
            if(a != null)
            {
                hastrantempyes = true;
                GISLol.Instance.All_Containers["ItemPickup"].slots[0].Held_Item = new GISItem(a);
            }
        }
        var aa = Tags.refs["Transmute"].GetComponent<MenuMover>();
        aa.Initial();
        System.Action<float> y = (x) =>
        {
            aa.nerds[0].localPosition = Vector3.Lerp(new Vector3(aa.nerds_orig[0].x, -775, 0), aa.nerds_orig[0], RandomFunctions.EaseIn(x));
            aa.nerds_img[0].color = Color.Lerp(new Color(0, 0, 0, 0), aa.nerds_img_orig[0], x);
        };
        y(0);
        if(ree) UpdateMenus();
        StartCoroutine(MenuAnimationLol(!ree, !ree, y));
    }
    public void Transselected(GISItem aa)
    {
        SetTranStat(false, aa);
    }

    bool anim = false;
    public bool MenuAnim = false;

    public IEnumerator FUCKYOU()
    {
        yield return new WaitUntil(() => { return GISLol.Instance.All_Containers.ContainsKey("LeftNut"); });
        GISSlot.Shungite();
    }

    public void ReroolIteme()
    {
        var poopy = Tags.refs["InititemPickup"].GetComponent<GISContainer>();
        if (poopy.slots[0].Held_Item.ItemIndex != "Empty")
        {
            int x = itemrollamnt * 2;
            if(PlayerController.Instance.Coins >= x)
            {
                PlayerController.Instance.Coins -= x;
                var y = GetItemForLevel();
                poopy.slots[0].Held_Item = y;
                poopy.slots[0].Displayer.item = y;
                AttemptAddLogbookItem(y.ItemIndex);
                itemrollamnt++;
                poopy.slots[0].Displayer.UpdateDisplay();
                Ubdatebananasexballsfucucucuc();
            }
        }
    }
    public void Ubdatebananasexballsfucucucuc()
    {
        var poopy = Tags.refs["ItemeR"].GetComponent<Quicky>();
        var x = itemrollamnt * 1;
        poopy.costy.text = $"{x} Coins";
        poopy.buyer.interactable = PlayerController.Instance.Coins >= x;
    }
    

    private bool AmIVeryFuckableToday()
    {
        if (anim) return true;
        if (MenuAnim) return true;
        var poopy = Tags.refs["InititemPickup"].GetComponent<GISContainer>();
        if (poopy.slots[0].Held_Item.ItemIndex != "Empty") return true;
        if (GISLol.Instance.Mouse_Held_Item.ItemIndex != "Empty") return true;
        if (checks[23] && (!hastrantempyes || PlayerController.Instance.Coins < 15)) return true;
        return false;
    }

    public void ConfirmSexMenuSex()
    {
        if (AmIVeryFuckableToday()) return;

        if(PickupItemCrossover != null)
        {
            PlayerController.Instance.Coins -= PickupItemCrossover.CoinCost;
            if (PickupItemCrossover.PickItems != null)
            {
                foreach (var a in PickupItemCrossover.PickItems)
                {
                    if (a == PickupItemCrossover) continue;
                    Destroy(a.SPEC2.gameObject);
                }
                Gamer.QuestProgressIncrease("Room", "Pick Three");
            }
        }
        if (checks[23])
        {
            PlayerController.Instance.Coins -= 15;
        }

        var leftnut = Tags.refs["LeftItemItems"].GetComponent<GISContainer>();
        var leftnutitem = Tags.refs["LeftItemNut"].GetComponent<GISDisplay>();
        leftnutitem.item.Run_Materials.Clear();
        for (int i = 0; i < leftnut.slots.Count; i++)
        {
            var x = leftnut.slots[i].Held_Item;
            if (leftnutitem.item.Run_Materials.Count <= i)
            {
                var w = new GISMaterial();
                leftnutitem.item.Run_Materials.Add(w);
            }
            if (x.ItemIndex != "Empty")
            {
                //leftnutitem.item.Run_Materials[i] = new GISMaterial();
                if (GISLol.Instance.ItemsDict[x.ItemIndex].IsCraftable)
                {
                    leftnutitem.item.Run_Materials[i].index = x.ItemIndex;
                }
                else
                {
                    leftnutitem.item.Run_Materials[i].itemindex = x.ItemIndex;
                }
            }
        }
        leftnutitem.item.CompileItems();
        var oldn = leftnutitem.item;
        leftnut = Tags.refs["RightItemItems"].GetComponent<GISContainer>();
        leftnutitem = Tags.refs["RightItemNut"].GetComponent<GISDisplay>();
        leftnutitem.item.Run_Materials.Clear();
        for (int i = 0; i < leftnut.slots.Count; i++)
        {
            var x = leftnut.slots[i].Held_Item;
            if (leftnutitem.item.Run_Materials.Count <= i)
            {
                var w = new GISMaterial();
                leftnutitem.item.Run_Materials.Add(w);
            }
            if (x.ItemIndex != "Empty")
            {
                //leftnutitem.item.Run_Materials[i] = new GISMaterial();
                if (GISLol.Instance.ItemsDict[x.ItemIndex].IsCraftable)
                {
                    leftnutitem.item.Run_Materials[i].index = x.ItemIndex;
                }
                else
                {
                    leftnutitem.item.Run_Materials[i].itemindex = x.ItemIndex;
                }
            }
        }
        leftnutitem.item.CompileItems();

        oldn.CompileBalance(leftnutitem.item);
        leftnutitem.item.CompileBalance(oldn);

        if (!checks[17])Destroy(itemshite);
        PlayerController.Instance.SetData();
        StartCoroutine(ConfirmAnimation());
    }
    List<GameObject> dienerds = new List<GameObject>();
    public IEnumerator ConfirmAnimation()
    {
        anim = true;
        var leftnut = Tags.refs["LeftItemItems"].GetComponent<GISContainer>();
        var rightnut = Tags.refs["RightItemItems"].GetComponent<GISContainer>();
        float a1 = 0;
        float a2 = 0;
        float m1 = 0;
        float m2 = 0;
        for (int i = 0; i < leftnut.slots.Count; i++)
        {
            if (leftnut.slots[i].Held_Item.ItemIndex != "Empty")
            {
                a1++;
                m1 = i;
            }
        }
        for (int i = 0; i < rightnut.slots.Count; i++)
        {
            if (rightnut.slots[i].Held_Item.ItemIndex != "Empty")
            {
                a2++;
                m2 = i;
            }
        }
        var max = Mathf.Max(a1, a2);
        var max2 = Mathf.Max(m1, m2);
        float time = 0.3f/max;
        time += 0.02f;
        for (int i = 0; i < leftnut.slots.Count; i++)
        {
            bool wee = false;
            if (leftnut.slots[i].Held_Item.ItemIndex != "Empty")
            {
                wee = true;
                var weenor = Instantiate(ItemAnimThing, leftnut.slots[i].transform.position, Quaternion.identity, Tags.refs["ItemAnimParent"].transform);
                dienerds.Add(weenor);
                var w2 = weenor.GetComponent<MeWhenYourMom>();
                w2.target = Tags.refs["LeftItemNut"].transform;
                if (GISLol.Instance.ItemsDict[leftnut.slots[i].Held_Item.ItemIndex].IsCraftable)
                {
                    w2.img.color = GISLol.Instance.MaterialsDict[leftnut.slots[i].Held_Item.ItemIndex].GetVisColor();
                }
                leftnut.slots[i].Held_Item = new GISItem();
                leftnut.slots[i].GetComponent<Image>().enabled = false;
            }
            if (rightnut.slots[i].Held_Item.ItemIndex != "Empty")
            {
                wee = true;
                var weenor = Instantiate(ItemAnimThing, rightnut.slots[i].transform.position, Quaternion.identity, Tags.refs["ItemAnimParent"].transform);
                dienerds.Add(weenor);
                var w2 = weenor.GetComponent<MeWhenYourMom>();
                w2.target = Tags.refs["RightItemNut"].transform;
                if (GISLol.Instance.ItemsDict[rightnut.slots[i].Held_Item.ItemIndex].IsCraftable)
                {
                    w2.img.color = GISLol.Instance.MaterialsDict[rightnut.slots[i].Held_Item.ItemIndex].GetVisColor();
                }
                rightnut.slots[i].Held_Item = new GISItem();
                rightnut.slots[i].GetComponent<Image>().enabled = false;
            }
            if (i >= max2) break;
            if (wee)
                yield return new WaitForSeconds(time);
        }
        yield return new WaitForSeconds(0.8f);
        anim = false;
        checks[5] = false;
        checks[17] = false;
        checks[20] = false;
        checks[22] = false;
        checks[23] = false;
        UpdateMenus();
    }



    public GameObject itemshite;
    string a = "wank";
    public void TextModeEnter()
    {
        Debug.Log("TextEntry");
        InputManager.AddLockLevel("TextEntry");
        InputManager.RemoveLockLevel("menu");

        string a = "";
        foreach(var b in InputManager.locklevel)
        {
            a += b + ", ";
        }
        Debug.Log("LL: " + a);
    }
    public void TextModeExit()
    {
        InputManager.RemoveLockLevel("TextEntry");
        InputManager.AddLockLevel("menu");
        Debug.Log("ExitedText");
    }

    public void ConfirmItemTrans()
    {
        List<GISItem> wankers = new List<GISItem>();
        foreach(var a in Tags.refs["RightTrans"].transform.GetComponentsInChildren<MaterialTransfer>())
        {
            wankers.Add(a.dip.item);
        }
        foreach (var item in wankers)
        {
            GISLol.Instance.GrantItem(item, true);
        }
        FadeToLobby();
    }

    public void CheckWeaponsBreak()
    {
        var c = GISLol.Instance.All_Containers["Equips"];
        if (c.slots[0].Held_Item.UsesRemaining <= 0)
        {
            var a = new OXNotif();
            a.Title = "A Weapon Has Broken";
            a.Description = c.slots[0].Held_Item.CustomName;
            a.BackgroundColor1 = new Color(0.5f, 0, 0);
            a.Item = c.slots[0].Held_Item;
            a.Descoffset = new Vector3(0, -2, 0);
            a.Time = 5;
            NotificationSystem.Instance.AddNotif(a);
            c.slots[0].Held_Item = new GISItem();
        }
        if (c.slots[1].Held_Item.UsesRemaining <= 0)
        {
            var a = new OXNotif();
            a.Title = "A Weapon Has Broken";
            a.Description = c.slots[1].Held_Item.CustomName;
            a.BackgroundColor1 = new Color(0.5f, 0, 0);
            a.Item = c.slots[1].Held_Item;
            a.Descoffset = new Vector3(0, -2, 0);
            a.Time = 5;
            NotificationSystem.Instance.AddNotif(a);
            c.slots[1].Held_Item = new GISItem();
        }
        c.SaveTempContents();
    }
    
    public void DurabilityHit()
    {
        var c = GISLol.Instance.All_Containers["Equips"];
        c.slots[0].Held_Item.UsesRemaining--;
        c.slots[1].Held_Item.UsesRemaining--;
        c.SaveTempContents();
    }


    public void FadeToLobby()
    {
        StartCoroutine(StartFade("LobDingle", 25));
    }

    public IEnumerator DeathAnim()
    {
        CameraLol.Instance.Shake(0.5f, 1f);
        SaveSystem.Instance.ResetFile("current_run");
        PlayerController.Instance.DeathDisable = true;
        StartCoroutine(DeathFlasher(40, 0.45f));
        StartCoroutine(StartFade("Death", 80));
        yield return null;
    }
    public Image Flasher;
    public IEnumerator DeathFlasher(int ticks, float perc)
    {
        for(int i = 0; i < ticks; i++)
        {
            var e2 = Flasher.color;
            e2.a = (perc)*((float)(ticks-i)/ticks);
            Flasher.color = e2;
            yield return new WaitForFixedUpdate();
        }
        var e = Flasher.color;
        e.a = 0;
        Flasher.color = e;
    }

    public void KillYourSelf()
    {
        ClearMap();
        checks[6] = true;
        GameState = "Dead";
        Time.timeScale = 1;
        SetPauseMenu(false);
    }

    public void StartLobby()
    {
        checks[3] = false;
        LoadLobbyScene();
        UpdateMenus();
        if (!IsMultiplayer)
        {
            var p = Instantiate(PlayerPrefab);
            Destroy(p.GetComponent<NetworkRigidbody2D>());
            Destroy(p.GetComponent<ClientNetworkTransform>());
            Destroy(p.GetComponent<NetworkObject>());
        }
    }
    public static int CurrentFloor = 0;
    public void GeneralFloorChange()
    {
        lastkillpos = Vector3.zero;
        hascorrupted = false;
        OldCurrentRoom = null;
        skillrollamnt = 0;
        roomsdeep = 0;
        itemrollamnt = 1;
        GameState = "Game";
        Tags.refs["Lobby"].SetActive(false);
        Tags.refs["ShopArea"].SetActive(false);
        Tags.refs["BlackBG"].SetActive(true);
        ClearMap();
        checks[0] = false;
        Sorters.Clear();
        LevelProgression.Clear();
        IsInShop = false;
        UpdateMenus();
        AssembleItemPool();
        Tags.refs["NextFloor"].SetActive(true);
        Tags.refs["NextShop"].SetActive(true);
        Tags.refs["NextShop2"].SetActive(true);
        if (titlething != null) StopCoroutine(titlething);
        if(PlayerController.Instance != null)PlayerController.Instance.SetData();
        Tags.refs["NextFloor"].GetComponent<INteractable>().UpdateText();
    }
    public bool IsInShop = false;
    public IEnumerator NextShopLevel(int sex)
    {
        GeneralFloorChange();
        IsInShop = true;
        SaveSystem.Instance.SaveCurrentRun();
        Tags.refs["ShopArea"].SetActive(true);
        PlayerController.Instance.transform.position = new Vector3(0,0,0);
        var e2 = CameraLol.Instance.transform.position;
        e2.x = PlayerController.Instance.transform.position.x;
        e2.y = PlayerController.Instance.transform.position.y;
        CameraLol.Instance.transform.position = e2;
        CameraLol.Instance.ppos = e2;
        CameraLol.Instance.targetpos = e2;

        for(int i = 0; i < 3; i++)
        {
            var c = Instantiate(GetChest(), new Vector3(-6, 7, 0) + (new Vector3(6, 0, 0) * i), Quaternion.identity, Tags.refs["ShopArea"].transform).GetComponent<INteractable>();
            var f = GetItemForLevel();
            c.cuum = f;
            spawnedchests.Add(c);
        }

        RollNewSkillSex();

        UpdateMenus();
        Tags.refs["NextFloor"].transform.position = new Vector3(11.5100002f, 0, -4.4000001f);
        Tags.refs["NextShop"].transform.position = new Vector3(100000, 100000, 0);
        Tags.refs["NextShop2"].transform.position = new Vector3(100000, 100000, 0);

        completetetge = true;
        yield return new WaitForSeconds(0.5f);

        titlething = StartCoroutine(TitleText("Market"));
    }
    Coroutine titlething;
    public GISItem GetItemForLevel()
    {
        if(CurrentFloor >= 5 && Random.Range(0, 101) == 0)
        {
            return new GISItem(ItemPoolAspects[Random.Range(0, ItemPoolAspects.Count)]);
        }
        if (Random.Range(0, 2) == 0)
        {
            return new GISItem(ItemPoolMats[Random.Range(0, ItemPoolMats.Count)]);
        }
        else
        {
            return new GISItem(ItemPoolRunes[Random.Range(0, ItemPoolRunes.Count)]);
        }
    }

    public void AssembleItemPool()
    {
        ItemPoolMats.Clear();
        ItemPoolRunes.Clear();
        ItemPoolAspects.Clear();
        foreach(var a in GISLol.Instance.Items)
        {
            if(!a.CanSpawn) continue;
            if (a.IsCraftable)
            {
                ItemPoolMats.Add(a.Name);
                continue;
            }
            if (a.IsRune)
            {
                ItemPoolRunes.Add(a.Name);
                continue;
            }
            if (a.IsAspect)
            {
                ItemPoolAspects.Add(a.Name);
                continue;
            }
        }
        ItemPoolMats.Remove("Rock");
    }

    public static List<RoomTypeHolder> ValidRoomTypes = new List<RoomTypeHolder>();

    bool skipped = false;
    public IEnumerator NextFloor(int seed = 0)
    {
        GeneralFloorChange();
        if(seed == 0)
        {
            Seed = Random.Range(-999999999, 999999999);
            if (Seed == 0) Seed = 1;
        }
        else
        {
            Seed = seed;
        }
        GlobalRand = new System.Random(Seed);
        CurrentFloor++;
        RoomLol.Instance.GenerateRandomLayout();
        if(CurrentFloor > 1 && seed == 0)
        {
            SaveSystem.Instance.SaveCurrentRun();
        }
        else if(CurrentFloor <= 1)
        {
            PlayerController.Instance.Coins = 0;
            SaveCurrentWeapons();
        }
        List<I_Room> enders = new List<I_Room>();
        foreach (var e in RoomLol.Instance.SpawnedRoomsDos)
        {
            if (e.room.IsEndpoint)
            {
                enders.Add(e);
            }
        }
        var r = GlobalRand;
        List<I_Room> endos = new List<I_Room>();
        int level = -1;
        foreach (var psex in enders)
        {
            //Debug.Log("Level:" + psex.level);
            if (psex.level > level) level = psex.level;
        }
        foreach (var psex in enders)
        {
            if (psex.level == level) endos.Add(psex);
        }
        I_Room rm = null;
        switch (CurrentFloor)
        {
            case 9:
                foreach(var a in endos)
                {
                    Debug.Log(a.gameObject.name);
                    if (a.gameObject.name == "End1(Clone)")
                    {
                        rm = a;
                        break;
                    }
                }
                break;
            default:
                rm = endos[r.Next(0, endos.Count)];
                break;
        }

        //Debug.Log("Endo Count: "+ endos.Count);
        rm.isused = "Start";
        PlayerController.Instance.transform.position = rm.transform.position;
        playerstpos = rm.transform.position;
        enders.Remove(rm);
        endos.Remove(rm);
        var rm2 = FindEndRoom(rm);
        rm2.isused = "End";
        Tags.refs["NextShop"].transform.position = rm2.transform.position;
        Tags.refs["NextFloor"].transform.position = new Vector3(100000, 100000, 0);
        Tags.refs["NextShop2"].transform.position = new Vector3(100000, 100000, 0);
        if(CurrentFloor > 1)
        {
            var meme = new Vector3(7.5f, -2, 0);
            var meme2 = new Vector3(-7.5f, -2, 0);
            var meme3 = new Vector3(0, 5, 0);

            if (rm2.room.HasLeftDoor || rm2.room.HasRightDoor)
            {
                meme = new Vector3(2, -7.5f, 0);
                meme2 = new Vector3(2, 7.5f, 0);
                meme3 = new Vector3(-5, 0, 0);
            }
            if (rm2.room.HasLeftDoor)
            {
                meme.x = -2;
                meme2.x = -2;
                meme3 = new Vector3(5, 0, 0);
            }
            if (rm2.room.HasTopDoor)
            {
                meme.y = 2;
                meme2.y = 2;
                meme3 = new Vector3(0, -5, 0);
            }

            Tags.refs["NextShop"].transform.position = rm2.transform.position + meme;
            Tags.refs["NextFloor"].transform.position = rm2.transform.position + meme3;
            Tags.refs["NextShop2"].transform.position = rm2.transform.position + meme2;
        }
        enders.Remove(rm2);
        endos.Remove(rm2);
        var e2 = CameraLol.Instance.transform.position;
        e2.x = PlayerController.Instance.transform.position.x;
        e2.y = PlayerController.Instance.transform.position.y;
        CameraLol.Instance.transform.position = e2;
        CameraLol.Instance.ppos = e2;
        CameraLol.Instance.targetpos = e2;

        skipped = !WasInShop && CurrentFloor > 1;
        if (skipped)
        {
            SpawnPrinter(rm.transform.position + new Vector3(7,7,0));
            SpawnPrinter(rm.transform.position + new Vector3(7,-7,0));
            SpawnPrinter(rm.transform.position + new Vector3(-7,7,0));
            SpawnPrinter(rm.transform.position + new Vector3(-7,-7,0));
        }
        PlayerController.Instance.DashCoolDown = PlayerController.Instance.MaxDashCooldown * 3;
        yield return new WaitForFixedUpdate();

        foreach (var e in enders)
        {
            AssignRoomStuff(e);
        }


        yield return new WaitForFixedUpdate();

        SexMeSomeGigaFuck();

        completetetge = true;

        //compile end list
        yield return new WaitForSeconds(0.7f);
        titlething = StartCoroutine(TitleText());
        if (skipped)
        {
            for(int i = 0; i < 10; i++)
            {
                SpawnCoins(PlayerController.Instance.transform.position + (Quaternion.Euler(0,0,i*36) * Vector3.up*10), 1, PlayerController.Instance);
            }
        }
    }
    [HideInInspector]
    public Vector3 playerstpos;

    public void SexMeSomeGigaFuck()
    {
        nmr.BuildNavMesh();
    }
    public GameObject OrbChaser;
    public GameObject PasscodeSex;
    public void AssignRoomStuff(I_Room e)
    {
        var tp = ValidRoomTypes[GlobalRand.Next(0, ValidRoomTypes.Count)];
        e.rth = tp;
        switch (tp.Name)
        {
            case "Chase The Orb":
                var c2 = Instantiate(OrbChaser, e.transform.position, Quaternion.identity).GetComponent<CumChasser>();
                c2.iroom = e;
                e.isused = "Orb";
                KillMeOnRoomSex.Add(c2.gameObject);
                break;
            case "Passcode":
                var c3 = Instantiate(PasscodeSex, e.transform.position, Quaternion.identity, Tags.refs["NavMesh"].transform);
                e.isused = "Passcode";
                KillMeOnRoomSex.Add(c3);
                break;
            case "Pick Three":

                Vector3[] memes = new Vector3[]
                {
                    new Vector3(0,0,0),
                    new Vector3(0,-5,0),
                    new Vector3(0,5,0),
                };

                if (!(e.room.HasLeftDoor || e.room.HasRightDoor))
                {
                    memes = new Vector3[]
                    {
                        new Vector3(0,0,0),
                        new Vector3(-5,0,0),
                        new Vector3(5,0,0),
                    };
                }

                GISItem[] items = new GISItem[]
                {
                    GetItemForLevel(),
                    GetItemForLevel(),
                    GetItemForLevel(),
                };
                for(int i = 0; i < 3; i++)
                {
                    items[i].CoinCost = 6;
                    items[i].PickItems = items;
                    SpawnGroundItem(memes[i] + e.transform.position, items[i]);
                }
                e.isused = "Pick3";
                break;
            default:
                var c = Instantiate(GetChest(), e.transform.position, Quaternion.identity).GetComponent<INteractable>();
                e.isused = "Chest";
                var f = GetItemForLevel();
                c.cuum = f;
                spawnedchests.Add(c);
                break;
        }
        
    }


    public void SaveCurrentWeapons()
    {
        string dict = "weapons";
        var c = GISLol.Instance.All_Containers["Equips"];
        SaveSystem.Instance.SetString("Weapon1", c.slots[0].Held_Item.ItemToString(), dict);
        SaveSystem.Instance.SetString("Weapon2", c.slots[1].Held_Item.ItemToString(), dict);
        SaveSystem.Instance.SetString("Drugs", Converter.ListToString(ActiveDrugs), dict);
        SaveSystem.Instance.SaveDataToFile(dict);
        //SaveSystem.Instance.SaveGame();
    }

    public IEnumerator TitleText(string ver = "")
    {
        float x = 0;
        var floopis = FloorHeader.GetComponent<CanvasGroup>();
        FloorHeader.text = $"Floor {CurrentFloor}";
        if (ver != "")
        {
            FloorHeader.text = ver;
        }
        while (x < 1)
        {
            x = Mathf.Clamp01(x + Time.deltaTime);
            var g = Mathf.Sin(Mathf.PI * x / 2);
            floopis.alpha = g;
            FloorHeader.transform.position = InitFloorHeadPos.position + new Vector3(0, -2, 0);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        while (x <= 2)
        {
            x += Time.deltaTime;
            var g = Mathf.Sin(Mathf.PI * x / 2);
            floopis.alpha = g;
            FloorHeader.transform.position = InitFloorHeadPos.position + new Vector3(0, -g * 2, 0);
            yield return null;
        }
    }

    List<I_Room> Sorters = new List<I_Room>();
    public I_Room FindEndRoom(I_Room start)
    {
        Sorters.Add(start);
        LevelProgression.Add(start);
        int hits = 0;
        foreach(var a in start.RelatedRooms)
        {
            if (Sorters.Contains(a)) continue;
            if(!a.room.IsEndpoint)
            {
                return FindEndRoom(a);
            }
            else
            {
                hits++;
            }
        }
        if(hits == start.RelatedRooms.Count-1)
        {
            var e = new List<I_Room>(start.RelatedRooms);
            e.Remove(start);
            for(int i =0; i < e.Count; i++)
            {
                if (LevelProgression.Contains(e[i]))
                {  
                    e.RemoveAt(i);
                    i--;
                }
            }
            var wank = e[Random.Range(0, e.Count)];
            LevelProgression.Add(wank);
            return wank;
        }
        return start;
    }


    long creditcount = 0;
    bool hascorrupted = false;
    public IEnumerator StartRoom()
    {
        BoomyRoomy();


        yield return new WaitForSeconds(1.5f);
        float time = 1.5f;
        int wavesex = 3;
        int adder = 4;
        if(CurrentFloor <= 3)
        {
            time = 2.5f;
            wavesex = 2;
        }
        if(CurrentFloor <= 1)
        {
            adder = 3;
        }
        int waves = Random.Range(0,wavesex)+adder;
        creditcount = 0;
        nmr.BuildNavMesh(true);

        if (ActiveDrugs.Contains("Meth")) time = 0.1f;

        switch (CurrentFloor)
        {
            case 9:
                StartCoroutine(HijackCamera(CurrentRoom.transform.position));
                yield return new WaitForSeconds(1f);
                EnemyHolder bossy = null;
                foreach(var a in EnemiesDos)
                {
                    if (a.EnemyObject.GetComponent<NavMeshEntity>().EnemyType == "Bossrocks")
                    {
                        bossy = a;
                        break;
                    }
                }
                SpawnEnemy(bossy, false, CurrentRoom.transform.position);
                yield return new WaitUntil(() => { return EnemiesExisting.Count == 0; });
                break;
            default:
                for (int i = 0; i < waves; i++)
                {
                    if (GameState != "Game") break;
                    SpawnEnemyWave(creditcount);
                    if (creditcount > 0)
                    {
                        i--;
                    }
                    yield return new WaitUntil(() => { return EnemiesExisting.Count <= 14; });
                    yield return new WaitForSeconds(time);
                }
                yield return new WaitUntil(() => { return EnemiesExisting.Count == 0; });
                break;
        }



        CurrentRoom = null;
        roomsdeep++;
        PlayerController.Instance.DashCoolDown = PlayerController.Instance.MaxDashCooldown * 3;
        for(int i = 1; i < PlayerController.Instance.Skills.Count; i++)
        {
            //if (GISLol.Instance.SkillsDict[PlayerController.Instance.Skills[i].Name].OnlyFillInCombat) continue;
            PlayerController.Instance.Skills[i].Stacks = GISLol.Instance.SkillsDict[PlayerController.Instance.Skills[i].Name].MaxStacks;
            PlayerController.Instance.Skills[i].Timer = 0;
        }
        InRoom = false;
        if(GameState == "Game")
        {
            if (!hascorrupted && CurrentFloor > 1 && PlayerController.Instance.entit.Health > 0)
            {
                if(skipped && roomsdeep < 2) goto WWW;
                hascorrupted = true;
                CorruptionCode.Instance.CorruptTile(new Vector3Int((int)playerstpos.x, (int)playerstpos.y));

                var notif = new OXNotif();
                notif.Title = "It Spreads...";
                notif.BackgroundColor1 = new Color32(143, 52, 235, 255);
                NotificationSystem.Instance.AddNotif(notif);
            }
            WWW:
            SpawnCoins(lastkillpos, 1, PlayerController.Instance);
        }

        //test
    }

    int roomsdeep = 0;

    [HideInInspector]
    public bool Edgemogging = false;
    public float CameraMouseMult = 1;
    public IEnumerator HijackCamera(Vector3 target)
    {
        Edgemogging = true;
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            CameraLol.Instance.targetpos = Vector3.Lerp(PlayerController.Instance.transform.position, target, RandomFunctions.EaseInAndOut(x));
            CameraMouseMult = 1 - RandomFunctions.EaseInAndOut(x);
        }));
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            CameraLol.Instance.targetpos = Vector3.Lerp(target, PlayerController.Instance.transform.position, RandomFunctions.EaseInAndOut(x));
            CameraMouseMult = RandomFunctions.EaseInAndOut(x);
        }));
        Edgemogging = false;
    }



    public List<NavMeshEntity> SpawnEnemyWave(long creditoverflow = 0)
    {
        List<NavMeshEntity> suck = new List<NavMeshEntity>();
        var w = CurrentRoom.room.RoomSize;
        var ww = CurrentRoom.CreditMod;
        if(creditcount <= 0)
        creditcount = (long)(((25 * Mathf.Sqrt(w.x * w.y * ((1.5f*CurrentFloor)-0.5f))-1)+ CurrentFloor)*ww);
        int x = 0;
        while(creditcount > 0)
        {
            if (x >= 4)
            {
                break;
            }
            var wank = GetEnemyForDiff(false);
            suck.Add(SpawnEnemy(wank));
            x++;
        }
        return suck;
    }
    public GameObject UnstableBullet;
    public GameObject HealBeam;
    public NavMeshEntity SpawnEnemy(EnemyHolder wank, bool canspendcredits = true)
    {
        var ppos = wank.SpawnPos;
        if (ppos == Vector3.zero) ppos = FindValidPos(CurrentRoom, wank.EnemyObject.GetComponent<NavMeshEntity>());

        return SpawnEnemy(wank, canspendcredits, ppos);
    }
    
    public NavMeshEntity SpawnEnemy(EnemyHolder wank, bool canspendcredits, Vector3 position = default)
    {
        List<string> elitetypes = new List<string>();
        if (wank.CanBeElite)
        {
            EliteTypesDict["Corrupted"].Enabled = false;
            EliteTypesDict["Lunar"].Enabled = false;
            EliteTypesDict["Unstable"].Enabled = CurrentFloor >= 6;
            EliteTypesDict["Splitting"].Enabled = CurrentFloor >= 6;
            foreach (var a in EliteTypes)
            {
                if (a.Enabled)
                    elitetypes.Add(a.Name);
            }
        }
        var ppos = position;

        var ss = Instantiate(wank.EnemyObject, ppos, PlayerController.Instance.transform.rotation, Tags.refs["EnemyHolder"].transform);
        var rs = ss.GetComponent<NavMeshEntity>();
        rs.originroom = CurrentRoom;
        rs.EnemyHolder = wank;
        var e = wank.CreditCost;
        var dif = CurrentFloor - wank.MinFloor;
        if (wank.CanBeElite && (Random.Range(0, 1f) < 0.2f * dif || ActiveDrugs.Contains("Fentanyl")))
        {
            if (dif < 8)
            {
                var el = new List<string>(elitetypes);
                if (dif <= 5) el.Remove("Perfected");
                rs.EliteType = el[Random.Range(0, el.Count)];
            }
            else
            {
                rs.EliteType = "Perfected";
            }
            e = (e * (long)(100 * EliteTypesDict[rs.EliteType].CostMod)) / 100;
        }
        if (canspendcredits)
        {
            creditcount -= e;
        }
        rs.creditsspent = e;
        EnemiesExisting.Add(rs);


        if (rs.IsBoss)
        {
            switch (CurrentFloor)
            {
                case 9: break;
                default:
                    rs.IsBoss = false; 
                    break;
            }
        }

        return rs;
    }


    public EnemyHolder GetEnemyForDiff(bool spend = true)
    {
        EnemyHolder eh = null; 
        for(int i = 0; i < EnemiesDos.Count; i++)
        {
            int j = (EnemiesDos.Count- 1)-i;
            var e = EnemiesDos[j];
            if(CurrentFloor >= e.MinFloor && (e.MaxFloor <= 0 || CurrentFloor <= e.MaxFloor))
            {
                if (Random.Range(0f, 1f) <= e.PickChance)
                {
                    eh = e;
                    if(spend)creditcount -= e.CreditCost;
                    break;
                }
            }
        }
        return eh;
        //return Enemies[0];
    }



    public Vector3 FindValidPos(I_Room originroom, NavMeshEntity wankw)
    {
        Transform s = null;
        Vector3 s1 = new Vector3(10,-10);
        Vector3 pos = new Vector3(0, 0);
        if(originroom != null && originroom.gm != null)
        {
            s = originroom.gm.transform;
            s1 = s.localScale / 2;
            pos = s.position;
        }
        var x = pos + new Vector3(Random.Range(-s1.x + 3f, s1.x - 3f), Random.Range(-s1.y + 3f, s1.y - 3f), 0);
        bool failed = false;
        switch (wankw.EnemyType)
        {
            case "Orb":
                if (RandomFunctions.Instance.Dist(x, PlayerController.Instance.transform.position) < 4 || RandomFunctions.Instance.Dist(x, PlayerController.Instance.transform.position) > 10)
                {
                    failed = true;
                }
                else
                {
                    var a = Physics2D.OverlapCircleAll((Vector2)x, wankw.SpawnOverlapRadius);
                    foreach (var b in a)
                    {
                        var ob = GetObjectType(b.gameObject, false);
                        if (ob.BlocksSpawn)
                        {
                            failed = true;
                            break;
                        }
                    }
                }
                break;
            default:
                if (RandomFunctions.Instance.Dist(x, PlayerController.Instance.transform.position) < 5)
                {
                    failed = true;
                }
                else
                {
                    var a = Physics2D.OverlapCircleAll((Vector2)x, wankw.SpawnOverlapRadius);
                    foreach (var b in a)
                    {
                        var ob = GetObjectType(b.gameObject, false);
                        if (ob.BlocksSpawn)
                        {
                            failed = true;
                            break;
                        }
                    }
                }
                break;
        }

        if (failed)
        {
            x = FindValidPos(originroom, wankw);
        }
        return x;
    }

    public bool IsPosInBounds(Vector3 pos, bool GlobalCheck = false, bool IgnoreDoors = false)
    {
        var a = Physics2D.RaycastAll(pos, Vector3.back);
        bool inbounds = false;
        foreach(var g in a)
        {
            var ob = GetObjectType(g.collider.gameObject);
            switch (ob.type)
            {
                case "RoomBG":
                    if(CurrentRoom != null && !GlobalCheck)
                    {
                        foreach(var aaa in CurrentRoom.floors)
                        {
                            if(aaa == ob.gm)
                            {
                                inbounds = true;
                                goto NextThing;
                            }
                        }
                    }
                    else
                    {
                        inbounds = true;
                    }
                    break;
                case "Wall":
                    if(IgnoreDoors && ob.isdoor) goto NextThing;
                    inbounds = false;
                    goto exit;
            }
        NextThing:;
        }
        exit:
        return inbounds;
    }


    [HideInInspector]
    public I_Room OldCurrentRoom;
    public void BoomyRoomy()
    {
        OldCurrentRoom = CurrentRoom;
        var pp = new Vector2(CurrentRoom.transform.position.x,CurrentRoom.transform.position.y);
        var ppshex = CurrentRoom.room.RoomSize * 15f;

        Debug.Log("Boomedyourroomt");
        Debug.Log(CurrentRoom.gameObject.name);

        if (CurrentRoom.room.HasBottomDoor)
        {
            var pz = pp - ppshex;
            var ppos = CurrentRoom.room.BottomDoor * 30f;
            ppos.x += 15f;
            Instantiate(DoorFab, new Vector3(pz.x + ppos.x, pz.y, 0), Quaternion.identity,DoorHolder);
        }
        if (CurrentRoom.room.HasTopDoor)
        {
            var pz = ppshex;
            pz.y *= -1;
            pz = pp - pz;
            var ppos = CurrentRoom.room.TopDoor * 30f;
            ppos.x += 15f;
            Instantiate(DoorFab, new Vector3(pz.x + ppos.x, pz.y, 0), Quaternion.Euler(0,0,180),DoorHolder);
        }
        if (CurrentRoom.room.HasLeftDoor)
        {
            var pz = ppshex;
            pz.y *= -1;
            pz = pp - pz;
            var ppos = CurrentRoom.room.LeftDoor * 30f;
            ppos.y += 15f;
            Instantiate(DoorFab, new Vector3(pz.x, pz.y - ppos.y, 0), Quaternion.Euler(0, 0, 270),DoorHolder);
        }
        if (CurrentRoom.room.HasRightDoor)
        {
            var pz = ppshex;
            pz = pp + pz;
            var ppos = CurrentRoom.room.RightDoor * 30f;
            ppos.y += 15f;
            Instantiate(DoorFab, new Vector3(pz.x, pz.y - ppos.y, 0), Quaternion.Euler(0, 0, 90),DoorHolder);
        }
    }
    public void SetLastEnemy(NavMeshEntity cum)
    {
        if(LastHitEnemyTimer <= 0)
        {
            LastHitEnemy = cum;
        }
    }
    public static void QuestProgressIncrease(string Name, string Target)
    {
        foreach(var quest in GISLol.Instance.Quests)
        {
            if (quest.Data["Completed"] == "True") continue;
            if (quest.Data["Name"]== Name && quest.Data["Target_Data"] == Target)
            {
                quest.Data["Progress"] = (int.Parse(quest.Data["Progress"]) + 1).ToString();
                quest.CheckComplete();
            }
        }
    }

    public float ShartPoop = 0f;
    private Button sexernuttyb;
    private void FixedUpdate()
    {
        LastHitEnemyTimer -= Time.deltaTime;


        bool a = GameState == "Game" || GameState == "Lobby";
        if (a)
        {
            if (NextFloorButtonSexFuck)
            {
                NextFloorButtonSexFuck = false;
                StartCoroutine(StartFade("NextFloor"));
            }
            if (NextShopButtonSexFuck)
            {
                NextShopButtonSexFuck = false;
                StartCoroutine(StartFade("NextShop"));
            }
            enemybar.gameObject.SetActive(LastHitEnemy != null);
            enemybaroutline.SetActive(LastHitEnemy != null);
            enemybar.BarParentSize.gameObject.SetActive(LastHitEnemy != null);
            if(LastHitEnemy != null)
            {
                enemybar.title.text = NavMeshEntity.GetName(LastHitEnemy);
                enemybar.title.color = LastHitEnemy.EliteType != "" ? EliteTypesDict[LastHitEnemy.EliteType].color : new Color32(255,255,255,255);
                var www = (float)System.Math.Clamp(System.Math.Sqrt(LastHitEnemy.EntityOXS.Max_Health), 3, 15);
                enemybar.BarParentSize.sizeDelta = new Vector2(www*40f,7);
                enemybar.BarItself.localScale = new Vector3((1-(float)System.Math.Clamp((LastHitEnemy.EntityOXS.Health)/LastHitEnemy.EntityOXS.Max_Health,0,1))*www, 1, 1);
                var weenor = LastHitEnemy.EntityOXS.Effects;
                for (int i = 0; i < Enemy_effect_prespawns.Count; i++)
                {
                    bool aa = i < weenor.Count;
                    Enemy_effect_prespawns[i].gameObject.SetActive(aa);
                    if (aa)
                    {
                        Enemy_effect_prespawns[i].UpdateRender(weenor[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Enemy_effect_prespawns.Count; i++)
                {
                    Enemy_effect_prespawns[i].gameObject.SetActive(false);
                }
            }
            var weenor2 = PlayerController.Instance.entit.Effects;
            for (int i = 0; i < player_effect_prespawns.Count; i++)
            {
                bool aa = i < weenor2.Count;
                player_effect_prespawns[i].gameObject.SetActive(aa);
                if (aa)
                {
                    player_effect_prespawns[i].UpdateRender(weenor2[i]);
                }
            }
            ShartPoop -= Time.deltaTime;
            ShartPoop = (float)System.Math.Max(Mathf.Clamp01(ShartPoop), 2 * (0.35f - (PlayerController.Instance.entit.Health / PlayerController.Instance.entit.Max_Health)));
            if (checks[5])
            {
                if(sexernuttyb == null)
                {
                    sexernuttyb = Tags.refs["ItemConfirm"].GetComponent<Button>();
                }
                sexernuttyb.interactable = !AmIVeryFuckableToday();
            }
        }
        else
        {
            ShartPoop = 0;
            checks[5] = false;
        }
        var c = HitSexers[0].color;
        c.a = ShartPoop;
        foreach (var ca in HitSexers)
        {
            ca.color = c;
        }
    }

    public ObjectType GetObjectType(GameObject shart, bool noget = false)
    {
        var e = new ObjectType();
        e.gm = shart;
        if (shart == null)
        {
            return e;
        }
        if (shart.tag == "Sexy" || shart.tag == "SexyDoor")
        {
            e.type = "Wall";
            e.BlocksSpawn = true; 
            e.isdoor = shart.tag == "SexyDoor";
        }
        else if (shart.tag == "PlayerNerd")
        {
            if (!noget)
            {
                e.playerController = OXComponent.GetComponent<PlayerController>(shart);
                e.entityoxs = e.playerController.entit;
            }
            e.type = "Player";
            e.BlocksSpawn = true;
        }
        else if (shart.tag == "PlayerIsMyDad")
        {
            if (!noget)
            {
                e.playerController = OXComponent.GetComponent<PlayerController>(shart.transform.parent.gameObject);
                e.entityoxs = e.playerController.entit;
            }
            e.type = "PlayerDaddy";
            e.BlocksSpawn = true;
        }
        else if (shart.tag == "Enemy")
        {
            var wankwan = shart.GetComponent<NavMeshEntity>();
            if (wankwan.HasSpawned)
            {
                e.entity = wankwan;
                e.entityoxs = e.entity.EntityOXS;
                e.type = "Enemy";
            }
        }
        else if (shart.tag == "Furniture")
        {
            if (!noget)
            {
                e.furniture = shart.GetComponent<Furniture>();
                e.DoOnTouch += e.furniture.OnTouch;
            }
            e.type = "Furniture";
        }
        else if (shart.tag == "Hitable")
        {
            if (!noget)
                e.entityoxs = shart.GetComponent<EntityOXS>();
            e.type = "Hitable";
            e.BlocksSpawn = true;
        }
        else if (shart.tag == "RoomBG")
        {
            e.type = "RoomBG";
        }
        else if (shart.tag == "Void")
        {
            e.type = "Void";
        }
        return e;
    }

    public void SpawnGroundItem(Vector3 pos, GISItem cuum)
    {
        var itema = Instantiate(Gamer.Instance.GroundItemShit, pos, transform.rotation).GetComponent<GroundItem>();
        itema.sexyballer = cuum;
        itema.sexyballer.SPEC2 = itema;
        Gamer.Instance.spawneditemsformymassivesexyballs.Add(itema);
        var aa = itema.GetComponent<INteractable>();
        aa.BananaMan = itema;
        aa.UpdateText();
    }

    [HideInInspector]
    public Vector3 lastkillpos = new Vector3();
    public List<HealerFollower> AllHealers = new List<HealerFollower>();
    public void SpawnHealers(Vector3 pos, int amount, PlayerController target)
    {
        List<GameObject> others = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            var wen = Instantiate(Gamer.Instance.HealerGFooFO, pos, transform.rotation, balls);
            others.Add(wen);
            OXComponent.StoreComponent<HealerFollower>(wen);
            var waa = OXComponent.GetComponent<HealerFollower>(wen);
            waa.SexChaser = target;
            AllHealers.Add(waa);
        }
        foreach (var other in others)
        {
            var h = OXComponent.GetComponent<HealerFollower>(other);
            h.others = others;
        }
    }
    public void SpawnCoins(Vector3 pos, int amount, PlayerController target)
    {
        List<GameObject> others = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            var wen = Instantiate(Gamer.Instance.CoinGFooFO, pos, transform.rotation, balls);
            others.Add(wen);
            OXComponent.StoreComponent<HealerFollower>(wen);
            OXComponent.GetComponent<HealerFollower>(wen).SexChaser = target;
        }
        foreach (var other in others)
        {
            var h = OXComponent.GetComponent<HealerFollower>(other);
            h.others = others;
        }
    }

    public void ToggleTabMenu()
    {
        checks[5] = !checks[5];
        UpdateMenus();
    }

    private Coroutine NextFloorBall;
    public bool IsFading = false;
    [HideInInspector]
    public bool WasInShop = false;
    [HideInInspector]
    public bool Skipper = false;
    public IEnumerator StartFade(string type, int steps = 50, bool startfake = false)
    {
        IsFading = true;
        fader.raycastTarget = true;
        if (startfake) goto skipdingle;
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForFixedUpdate();
            var c = fader.color;
            c.a = (float)i / steps;
            fader.color = c;
        }
        yield return new WaitForFixedUpdate();
    skipdingle:
        var c2 = fader.color;
        c2.a = 1;
        fader.color = c2;
        switch (type)
        {
            case "NextFloor":
                Skipper = true;
                completetetge = false;
                WasInShop = IsInShop;
                NextFloorBall = StartCoroutine(NextFloor());
                yield return new WaitUntil(() => { return completetetge; });
                break;
            case "NextFloor2":
                completetetge = false;
                Skipper = true;
                if (IsInShop)
                {
                    CurrentFloor++;
                    NextFloorBall = StartCoroutine(NextShopLevel(0));
                }
                else
                {
                    NextFloorBall = StartCoroutine(NextFloor(Seed));
                }
                SaveSystem.Instance.LoadCurRun2();
                yield return new WaitUntil(() => { return completetetge; });
                break;
            case "NextShop":
                Skipper = false;
                WasInShop = IsInShop;
                NextFloorBall = StartCoroutine(NextShopLevel(0));
                break;
            case "NextShop2":
                Skipper = false;
                WasInShop = IsInShop;
                NextFloorBall = StartCoroutine(NextShopLevel(1));
                break;
            case "LobDingle":
                ResetIntegreal();
                StartLobby();
                break;
            case "Death":
                KillYourSelf();
                goto wank;
        }
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForFixedUpdate();
            var c = fader.color;
            c.a = (float)(steps- i) / steps;
            fader.color = c;
        }
        wank:
        yield return new WaitForFixedUpdate();
        c2 = fader.color;
        c2.a = 0;
        fader.color = c2;

        fader.raycastTarget = false;
        IsFading = false;
        nono:
        yield return null;
    }
    public bool RandomChance(float percent01)
    {
        return Random.Range(0, 1f) < percent01;
    }

    public int Rand(int min, int max, int seedoffset)
    {
        var r = new System.Random(Seed + seedoffset);
        return r.Next(min, max);
    }

    public GameObject GetChest()
    {
        return Chests[0];
    }
    public bool completetetge = false;
    public bool CanCurrentCraft()
    {
        var con = GISLol.Instance.All_Containers["Crafting"];
        if (anim) return false;
        return ItemNameInput.text != "" && con.slots[0].Held_Item.CanCraft() && con.slots[1].Held_Item.CanCraft() && con.slots[2].Held_Item.CanCraft() && con.slots[3].Held_Item.ItemIndex == "Empty";
    }
    public bool CanCurrentGraft()
    {
        var con = GISLol.Instance.All_Containers["Grafter"];
        if (anim) return false;
        if (con.slots[0].Held_Item.ItemIndex == "Rock") return false;
        if(con.slots[0].Held_Item.ItemIndex != con.slots[1].Held_Item.ItemIndex || con.slots[1].Held_Item.ItemIndex != con.slots[2].Held_Item.ItemIndex) return false;
        if(con.slots[3].Held_Item.GraftedMaterial.IsSet()) return false;
        return con.slots[0].Held_Item.CanCraft() && con.slots[1].Held_Item.CanCraft() && con.slots[2].Held_Item.CanCraft() && con.slots[3].Held_Item.ItemIndex != "Empty";
    }
    public bool CanCurrentRepair()
    {
        var con = GISLol.Instance.All_Containers["Repair"];
        if (anim) return false;
        if (con.slots[0].Held_Item.ItemIndex == "Rock") return false;
        if (con.slots[1].Held_Item.ItemIndex == "Rock") return false;
        return con.slots[0].Held_Item.CanCraft() && con.slots[1].Held_Item.CanCraft() && con.slots[2].Held_Item.ItemIndex != "Empty";
    }
    public bool CanCurrentAspect()
    {
        var con = GISLol.Instance.All_Containers["Aspecter"];
        if (anim) return false;
        if (con.slots[1].Held_Item.AspectMaterial.IsSet()) return false;

        return con.slots[0].Held_Item.CanCraft() && con.slots[1].Held_Item.CanCraft();
    }
    public void AttemptCraft()
    {
        if (CanCurrentCraft())
        {
            thingl = StartCoroutine(OpenMinigame());
        }
    }
    public void AttemptRepair()
    {
        Debug.Log("Click");
        if (CanCurrentRepair())
        {
            Debug.Log("Sucky");
            thingl = StartCoroutine(OpenMinigame());
        }
    }
    [HideInInspector]
    public int MinigameScore = 0;
    [HideInInspector]
    public bool CompletedMinigame = false;
    private Coroutine thingl;
    private Color origcol;
    private bool alreadyfoundsex = false;
    [HideInInspector]
    public bool Reachedtop = false;
    public IEnumerator OpenMinigame()
    {
        if (animaim) yield break;
        Reachedtop = false;
        checks[21] = true;
        UpdateMenus();
        CompletedMinigame = false;
        var c = Tags.refs["Minigame"].GetComponent<Minigame>();
        c.StartGame();
        float x = 0;
        var cc = c.BG.color;
        if (alreadyfoundsex)
        {
            cc = origcol;
        }
        else
        {
            alreadyfoundsex = true;
            origcol = cc;
        }
        var origa = cc.a;
        cc.a = 0;
        c.BG.color = cc;
        while(x < 1)
        {
            x = Mathf.Clamp(x + (Time.deltaTime*2), 0, 1);
            cc.a = Mathf.Lerp(0, origa, x);
            c.BG.color = cc;
            c.thin.position = Vector3.Lerp(c.tg2.position, c.tg1.position, RandomFunctions.EaseIn(x));
            yield return null;
        }
        Reachedtop = true;
        yield return new WaitUntil(() => { return CompletedMinigame; });
        animaim = true;
        yield return new WaitForSeconds(0.25f);
        x = 0;
        cc = origcol;
        while (x < 1)
        {
            x = Mathf.Clamp(x + (Time.deltaTime * 2), 0, 1);
            cc.a = Mathf.Lerp(origa, 0, x);
            c.BG.color = cc;
            c.thin.position = Vector3.Lerp(c.tg1.position, c.tg2.position, RandomFunctions.EaseOut(x));
            yield return null;
        }
        animaim = false;
        checks[21] = false;
        UpdateMenus();
        if (checks[24])
        {
            Debug.Log("A");
            StartCoroutine(RepairAnim());
        }
        else
        {
            StartCoroutine(CraftAnim());
        }
    }
    [HideInInspector]
    public bool animaim=false;
    public IEnumerator CloseMinigame()
    {
        if (animaim) yield break;
        var c = Tags.refs["Minigame"].GetComponent<Minigame>();
        animaim = true;
        yield return new WaitUntil(() => { return Reachedtop; });
        if (thingl != null) StopCoroutine(thingl);
        float x = 0;
        var cc = origcol;
        var origa = cc.a;
        while (x < 1)
        {
            x = Mathf.Clamp(x + (Time.deltaTime * 2), 0, 1);
            cc.a = Mathf.Lerp(origa, 0, x);
            c.BG.color = cc;
            c.thin.position = Vector3.Lerp(c.tg1.position, c.tg2.position, RandomFunctions.EaseOut(x));
            yield return null;
        }



        animaim = false;
        checks[21] = false;
        UpdateMenus();
        yield return null;
    }


    public void AttemptGraft()
    {
        if (CanCurrentGraft())
        {
            StartCoroutine(GraftAnim());
        }
    }
    public void AttemptAspect()
    {
        if (CanCurrentAspect())
        {
            StartCoroutine(AspectAnim());
        }
    }
    List<GISItem> mattertyeysys = new List<GISItem>();
    public IEnumerator CraftAnim()
    {
        var con = GISLol.Instance.All_Containers["Crafting"];
        string a = ItemNameInput.text;
        mattertyeysys.Clear();
        anim = true;

        System.Func<int,int> SpawnAnim = (i) =>
        {
            var weenor = Instantiate(ItemAnimThing, con.slots[i].transform.position, Quaternion.identity, Tags.refs["CrafterAnimHolder"].transform);
            dienerds.Add(weenor);
            var w2 = weenor.GetComponent<MeWhenYourMom>();
            w2.target = con.slots[3].transform;
            w2.speed = 9f;
            if (GISLol.Instance.ItemsDict[con.slots[i].Held_Item.ItemIndex].IsCraftable)
            {
                w2.img.color = GISLol.Instance.MaterialsDict[con.slots[i].Held_Item.ItemIndex].GetVisColor();
            }
            var aa = con.slots[i].Held_Item;
            mattertyeysys.Add(aa);
            con.slots[i].Held_Item = new GISItem();
            return i;
        };


        SpawnAnim(0);
        SpawnAnim(1);
        SpawnAnim(2);
        yield return new WaitForSeconds(0.6f);
        var e = new GISItem();
        e.ItemIndex = CraftSex;
        e.Amount = 1;
        e.CustomName = a;
        e.Quality = MinigameScore + 2;
        ItemNameInput.text = "";
        foreach (var ep in mattertyeysys[0].Materials)
        {
            e.Materials.Add(ep);
        }
        foreach (var ep in mattertyeysys[1].Materials)
        {
            e.Materials.Add(ep);
        }
        foreach (var ep in mattertyeysys[2].Materials)
        {
            e.Materials.Add(ep);
        }
        e.UsesRemaining = e.Quality;
        con.slots[3].Held_Item = e;


        if (e.Materials[0].GetName() != "Rock" && e.Materials[1].GetName() != "Rock" && e.Materials[2].GetName() != "Rock")
        {
            QuestProgressIncrease("Craft", e.ItemIndex);
        }
        anim = false;
    }public IEnumerator GraftAnim()
    {
        var con = GISLol.Instance.All_Containers["Grafter"];
        mattertyeysys.Clear();
        anim = true;


        System.Func<int, int> SpawnAnim = (i) =>
        {
            var weenor = Instantiate(ItemAnimThing, con.slots[i].transform.position, Quaternion.identity, Tags.refs["GrafterAnimHolder"].transform);
            dienerds.Add(weenor);
            var w2 = weenor.GetComponent<MeWhenYourMom>();
            w2.target = con.slots[3].transform;
            w2.speed = 9f;
            if (GISLol.Instance.ItemsDict[con.slots[i].Held_Item.ItemIndex].IsCraftable)
            {
                w2.img.color = GISLol.Instance.MaterialsDict[con.slots[i].Held_Item.ItemIndex].GetVisColor();
            }
            var aa = con.slots[i].Held_Item;
            mattertyeysys.Add(aa);
            con.slots[i].Held_Item = new GISItem();
            return i;
        };


        SpawnAnim(0);
        SpawnAnim(1);
        SpawnAnim(2);
        yield return new WaitForSeconds(0.6f);
        con.slots[3].Held_Item.GraftedMaterial = mattertyeysys[0].Materials[0];
        /*if()
        {
            QuestProgressIncrease("Graft", );
        }*/
        anim = false;
    }public IEnumerator AspectAnim()
    {
        var con = GISLol.Instance.All_Containers["Aspecter"];
        mattertyeysys.Clear();
        anim = true;


        System.Func<int, int> SpawnAnim = (i) =>
        {
            var weenor = Instantiate(ItemAnimThing, con.slots[i].transform.position, Quaternion.identity, Tags.refs["AspectAnimHolder"].transform);
            dienerds.Add(weenor);
            var w2 = weenor.GetComponent<MeWhenYourMom>();
            w2.target = con.slots[1].transform;
            w2.speed = 9f;
            //if (GISLol.Instance.ItemsDict[con.slots[i].Held_Item.ItemIndex].IsCraftable)
            //{
            //    w2.img.color = GISLol.Instance.MaterialsDict[con.slots[i].Held_Item.ItemIndex].ColorMod;
            //}
            var aa = con.slots[i].Held_Item;
            mattertyeysys.Add(aa);
            con.slots[i].Held_Item = new GISItem();
            return i;
        };
        SpawnAnim(0);
        yield return new WaitForSeconds(0.6f);
        var cd = new GISMaterial();
        cd.itemindex = mattertyeysys[0].ItemIndex;
        con.slots[1].Held_Item.AspectMaterial = cd;
        /*if()
        {
            QuestProgressIncrease("Aspect", );
        }*/
        anim = false;
    }
    public IEnumerator RepairAnim()
    {
        var con = GISLol.Instance.All_Containers["Repair"];
        mattertyeysys.Clear();
        anim = true;


        System.Func<int, int> SpawnAnim = (i) =>
        {
            var weenor = Instantiate(ItemAnimThing, con.slots[i].transform.position, Quaternion.identity, Tags.refs["RepairAnimHolder"].transform);
            dienerds.Add(weenor);
            var w2 = weenor.GetComponent<MeWhenYourMom>();
            w2.target = con.slots[2].transform;
            w2.speed = 9f;
            if (GISLol.Instance.ItemsDict[con.slots[i].Held_Item.ItemIndex].IsCraftable)
            {
                w2.img.color = GISLol.Instance.MaterialsDict[con.slots[i].Held_Item.ItemIndex].GetVisColor();
            }
            var aa = con.slots[i].Held_Item;
            mattertyeysys.Add(aa);
            con.slots[i].Held_Item = new GISItem();
            return i;
        };


        SpawnAnim(0);
        SpawnAnim(1);
        yield return new WaitForSeconds(0.6f);
        con.slots[2].Held_Item.Quality = MinigameScore + 2;
        con.slots[2].Held_Item.UsesRemaining = con.slots[2].Held_Item.Quality;
        /*if()
        {
            QuestProgressIncrease("Graft", );
        }*/
        anim = false;
    }
    public void InitCraftMenu()
    {
        anim = false;
    }
}

public class ObjectType
{
    public GameObject gm = null;
    public string type = "none";
    public PlayerController playerController = null;
    public NavMeshEntity entity = null;
    public EntityOXS entityoxs=null;
    public Furniture furniture = null;
    public bool BlocksSpawn = false;
    public bool isdoor=  false;
    public event Gamer.JustFuckingRunTheMethods DoOnTouch;

    public void FuckYouJustGodDamnRunTheShittyFuckingDoOnTouchMethodsAlreadyIWantToStabYourEyeballsWithAFork()
    {
        DoOnTouch?.Invoke();
    }

}




[System.Serializable]
public class EnemyHolder
{
    public GameObject EnemyObject;
    public long CreditCost = 1;
    public float PickChance = 1;
    public int MinFloor = 0;
    public int MaxFloor = 0;
    public bool CanBeElite = true;
    public bool InstantSpawn = false;
    [HideInInspector]
    public Vector3 SpawnPos = Vector3.zero;
    public EnemyHolder()
    {

    }
    public EnemyHolder(EnemyHolder sex)
    {
        EnemyObject = sex.EnemyObject;
        CreditCost = sex.CreditCost;
        PickChance = sex.PickChance;
        MinFloor = sex.MinFloor;
        MaxFloor = sex.MaxFloor;
        CanBeElite = sex.CanBeElite;
        SpawnPos= sex.SpawnPos;
        InstantSpawn = sex.InstantSpawn;
    }
}

[System.Serializable]
public class EliteTypeHolder
{
    public string Name = "Balls";
    public Color32 color;
    public float CostMod = 1;
    [HideInInspector]
    public bool Enabled = false;
}

[System.Serializable]
public class RoomTypeHolder
{
    public string Name = "Balls";
    public bool IsPuzzleRoom = false;
    public RoomTypeHolder(string name) 
    {
        Name = name;
        switch (Name)
        {
            case "Chase The Orb":
            case "Passcode":
            case "Bullet Dodge":
                IsPuzzleRoom = true;
                break;
        }
    }
}