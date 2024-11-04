using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Gamer : MonoBehaviour
{
    public bool[] checks = new bool[20];
    public Material[] sexex = new Material[2];
    public Image fader;
    public Camera mainnerddeingle;
    public GameObject textShuingite;
    public List<GISContainer> ballers = new List<GISContainer>();
    public List<EnemyHolder> EnemiesDos = new List<EnemyHolder>();
    public List<PlayerController> Players = new List<PlayerController>();
    public List<NavMeshEntity> EnemiesExisting = new List<NavMeshEntity>();
    public List<GameObject> Chests = new List<GameObject>();
    public List<EliteTypeHolder> EliteTypes = new List<EliteTypeHolder>();
    public GameObject HealerGFooFO;
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
    public List<string> ItemPool = new List<string>();
    public bool CanInteractThisFrame;
    public bool NextFloorButtonSexFuck = false;
    public bool NextShopButtonSexFuck = false;
    public GameObject ItemTranser;
    public List<Image> HitSexers = new List<Image>();
    public GameObject ItemAnimThing;
    public GameObject VaultThing;

    [HideInInspector]
    public bool InRoom = false;
    [HideInInspector]
    public I_Room CurrentRoom;

    public GISItem PickupItemCrossover;

    public delegate void JustFuckingRunTheMethods();
    public event JustFuckingRunTheMethods RefreshUIPos;
    public static bool WithinAMenu = false;
    public void UpdateMenus()
    {
        Tags.refs["Inventory"].SetActive(checks[0]);
        Tags.refs["Crafting"].SetActive(checks[1]);
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

        WithinAMenu = false;
        InputManager.SetLockLevel("");
        for(int i = 0; i < checks.Length; i++)
        {
            if (checks[i])
            {
                WithinAMenu = true;
                InputManager.SetLockLevel("menu");
                break;
            }
        }
        if (checks[3])
        {
            InputManager.SetLockLevel("main_menu");
        }
        if (checks[5])
        {
            InputManager.SetLockLevel("item_menu");
        }
        RefreshUIPos?.Invoke();
    }

    public static Gamer Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        foreach(var a in EliteTypes)
        {
            EliteTypesDict.Add(a.Name, a);
        }
        var c2 = fader.color;
        c2.a = 1;
        fader.color = c2;
        mainnerddeingle.Render();
    }
    private void Start()
    {
        Tags.refs["BGblack"].SetActive(true);
        MainMenu();
        StartCoroutine(FUCK());
    }
    public static List<List<string>> Backup = new List<List<string>>();
    public static List<List<string>> QBackup = new List<List<string>>();


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
        foreach (var sex in OXComponent.GetComponentsInChildren<HitBalls>(balls.gameObject))
        {
            Destroy(sex.gameObject);
        }
        foreach (var sex in StupidAssDoorDoohickies)
        {
            if (sex == null) continue;
            Destroy(sex);
        }
        OXComponent.CleanUp();
    }
    public bool IsHost;
    public void LoadLobbyScene()
    {
        InputManager.SetLockLevel("");
        GeneralFloorChange();
        Tags.refs["NextFloor"].GetComponent<INteractable>().Type = "StartGame";
        GameState = "Lobby";
        InRoom = false;
        CurrentRoom = null;
        OldCurrentRoom = null;
        Tags.refs["NextFloor"].transform.position = new Vector3(11.51f, 0, 0);
        Tags.refs["Lobby"].SetActive(true);
        Tags.refs["Baller"].transform.position = new Vector3(5.12f, -6.6f, 17.68f);
        if (IsMultiplayer)
        {
            IsHost = NetworkManager.Singleton.IsHost;
            StartCoroutine(WaitForSexyGamer());
        }


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
        enemybar.gameObject.SetActive(false);
        enemybaroutline.gameObject.SetActive(false);
        enemybar.BarParentSize.gameObject.SetActive(false);
        if (NextFloorBall != null) StopCoroutine(NextFloorBall);
        FloorHeader.transform.position = InitFloorHeadPos.position;
        for (int i = 0; i < checks.Length; i++)
        {
            checks[i] = false;
        }
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
        if (!IsFading && InputManager.IsKeyDown(InputManager.gamekeys["close_menu"]))
        {
            if (checks[0])
            {
                ToggleInventory();
            }
            else if (checks[8])
            {
                ToggleSettings();
            }
            else if (checks[10])
            {
                ToggleFuckPause();
            }
            else if (checks[5])
            {
                if(!anim)
                ToggleItemPickup();
            }
            else if (checks[3])
            {
            }
            else
            {
                SetPauseMenu(!checks[4]);
            }
        }
        if (InputManager.IsKeyDown(InputManager.gamekeys["inven"], "menu") && GameState == "Lobby")
        {
            ToggleInventory();
        }

#if UNITY_EDITOR
        if (InputManager.IsKeyDown(KeyCode.U, "menu"))
        {
            ToggleItemTrans();
        }
        if (InputManager.IsKeyDown(KeyCode.Space, "player"))
        {
            SpawnEnemy(EnemiesDos[6]);
        }
#endif

    }
    [HideInInspector]
    public int currentvault = 0;
    List<VaultitemDisplay> spawnednerds = new List<VaultitemDisplay>();
    public void LoadVaultPage(int page)
    {
        currentvault = page;
        int amount = 81;
        List<KeyValuePair<GISItem, int>> penis = new List<KeyValuePair<GISItem, int>>();
        for(int i = 0; i < amount; i++)
        {
            try
            {
                var wenor = GISLol.Instance.VaultItems.ElementAt(i + (amount * page));
                penis.Add(wenor);
            }
            catch
            {
                break;
            }
        }
        int offset = penis.Count - spawnednerds.Count;
        for(int i = 0; i < offset; i++)
        {
            var weenor = Instantiate(VaultThing, transform.position, Quaternion.identity, Tags.refs["VaultParent"].transform).GetComponent<VaultitemDisplay>();
            spawnednerds.Add(weenor);
        }
        for(int i = 0; i < -offset; i++)
        {
            Destroy(spawnednerds[0].gameObject);
            spawnednerds.RemoveAt(0);
        }
        for (int i = 0; i < penis.Count; i++)
        {
            spawnednerds[i].item = penis[i].Key;
            spawnednerds[i].UpdateDisplay();
        }
    }


    public static int EnemyCheckoffset = 0;
    public void ToggleInventory()
    {
        if (checks[0] && GISLol.Instance.Mouse_Held_Item.ItemIndex != "Empty") return;
        checks[0] = !checks[0];
        checks[1] = false;
        checks[11] = false;
        checks[2] = checks[0];
        UpdateMenus();
    }
    public void ToggleSettings()
    {
        checks[8] = !checks[8];
        UpdateMenus();
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
            ToggleItemTrans();
        }
    }

    public void ToggleItemTrans()
    {
        checks[9] = !checks[9];
        checks[6] = false;
        if (checks[9])
        {
            var strings = GetRunItems();
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
            if (GISLol.Instance.ItemsDict.TryGetValue(a.index, out GISItem_Data v))
            {
                if (v.IsCraftable)
                {
                    strings.Add(v.Name);
                }
            }
        }
        foreach (var a in c.slots[1].Held_Item.Run_Materials)
        {
            if (a == null) continue;
            if (GISLol.Instance.ItemsDict.TryGetValue(a.index, out GISItem_Data v))
            {
                if (v.IsCraftable)
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
        if (checks[5])
        {
            var c = GISLol.Instance.All_Containers["Equips"];

            var poopy = Tags.refs["InititemPickup"].GetComponent<GISContainer>();
            poopy.slots[0].Held_Item = PickupItemCrossover;
            poopy.slots[0].Displayer.UpdateDisplay();

            var leftnut = Tags.refs["LeftItemItems"].GetComponent<GISContainer>();
            leftnut.ClearSlots();
            leftnut.slots.Clear();
            leftnut.GenerateSlots(System.Math.Clamp(CurrentFloor*2,2,32));
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
            leftnut.GenerateSlots(System.Math.Clamp(CurrentFloor *2, 2, 32));
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

        }
        UpdateMenus();
    }
    bool anim = false;

    private bool AmIVeryFuckableToday()
    {
        if (anim) return true;
        var poopy = Tags.refs["InititemPickup"].GetComponent<GISContainer>();
        if (poopy.slots[0].Held_Item.ItemIndex != "Empty") return true;
        if (GISLol.Instance.Mouse_Held_Item.ItemIndex != "Empty") return true;
        return false;
    }

    public void ConfirmSexMenuSex()
    {
        if (AmIVeryFuckableToday()) return;

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
        Destroy(itemshite);
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
                    w2.img.color = GISLol.Instance.MaterialsDict[leftnut.slots[i].Held_Item.ItemIndex].ColorMod;
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
                    w2.img.color = GISLol.Instance.MaterialsDict[rightnut.slots[i].Held_Item.ItemIndex].ColorMod;
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
        UpdateMenus();
    }



    public GameObject itemshite;
    string a = "wank";
    public void TextModeEnter()
    {
        if("TextEntry" != InputManager.locklevel)
        {
            a = InputManager.locklevel;
            InputManager.SetLockLevel("TextEntry");
        }
    }
    public void TextModeExit()
    {
        if ("TextEntry" == InputManager.locklevel)
        {
            InputManager.SetLockLevel(a);
        }
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
            GISLol.Instance.AddVaultItem(item);
        }
        FadeToLobby();
    }
    public void FadeToLobby()
    {
        StartCoroutine(StartFade("LobDingle", 25));
    }

    public IEnumerator DeathAnim()
    {
        CameraLol.Instance.Shake(0.5f, 1f);
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
        OldCurrentRoom = null;
        Seed = Random.Range(-999999999, 999999999);
        GlobalRand = new System.Random(Seed);
        GameState = "Game";
        Tags.refs["Lobby"].SetActive(false);
        Tags.refs["ShopArea"].SetActive(false);
        Tags.refs["BlackBG"].SetActive(true);
        ClearMap();
        checks[0] = false;
        Sorters.Clear();
        LevelProgression.Clear();
        UpdateMenus();
        AssembleItemPool();
        if (titlething != null) StopCoroutine(titlething);
    }

    public IEnumerator NextShopLevel()
    {
        GeneralFloorChange();
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




        Tags.refs["NextFloor"].transform.position = new Vector3(11.5100002f, 0, -4.4000001f);
        Tags.refs["NextFloor"].GetComponent<INteractable>().Type = "StartGame";
        yield return new WaitForSeconds(0.5f);

        titlething = StartCoroutine(TitleText("Shop"));
    }
    Coroutine titlething;
    public GISItem GetItemForLevel()
    {
        return new GISItem(ItemPool[Random.Range(0, ItemPool.Count)]);
    }

    public void AssembleItemPool()
    {
        ItemPool.Clear();
        foreach(var a in GISLol.Instance.Items)
        {
            if(!a.CanSpawn) continue;
            if (a.IsCraftable)
            {
                ItemPool.Add(a.Name);
                continue;
            }
            if (a.IsRune)
            {
                ItemPool.Add(a.Name);
                continue;
            }
        }
        ItemPool.Remove("Rock");
    }


    public IEnumerator NextFloor()
    {
        GeneralFloorChange();
        CurrentFloor++;
        RoomLol.Instance.GenerateRandomLayout();
        
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
        var rm = endos[r.Next(0, endos.Count)];
        //Debug.Log("Endo Count: "+ endos.Count);
        rm.isused = "Start";
        PlayerController.Instance.transform.position = rm.transform.position;
        enders.Remove(rm);
        endos.Remove(rm);
        var rm2 = FindEndRoom(rm);
        rm2.isused = "End";
        Tags.refs["NextFloor"].transform.position = rm2.transform.position;
        Tags.refs["NextFloor"].GetComponent<INteractable>().Type = "NextShop";
        enders.Remove(rm2);
        endos.Remove(rm2);
        var e2 = CameraLol.Instance.transform.position;
        e2.x = PlayerController.Instance.transform.position.x;
        e2.y = PlayerController.Instance.transform.position.y;
        CameraLol.Instance.transform.position = e2;
        CameraLol.Instance.ppos = e2;
        CameraLol.Instance.targetpos = e2;

        foreach (var e in enders)
        {
            var c = Instantiate(GetChest(), e.transform.position, Quaternion.identity).GetComponent<INteractable>();
            e.isused = "Chest";
            var f = GetItemForLevel();
            c.cuum = f;
            spawnedchests.Add(c);
        }

        PlayerController.Instance.DashCoolDown = PlayerController.Instance.MaxDashCooldown * 3;

        yield return new WaitForFixedUpdate();

        nmr.BuildNavMesh();

        completetetge = true;

        //compile end list
        yield return new WaitForSeconds(0.7f);
        titlething = StartCoroutine(TitleText());
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
        for(int i = 0; i < waves; i++)
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
        CurrentRoom = null;
        PlayerController.Instance.DashCoolDown = PlayerController.Instance.MaxDashCooldown * 3;
        InRoom = false;
    }


    public List<NavMeshEntity> SpawnEnemyWave(long creditoverflow = 0)
    {
        List<NavMeshEntity> suck = new List<NavMeshEntity>();
        var w = CurrentRoom.room.RoomSize;
        if(creditcount <= 0)
        creditcount = (long)(25 * Mathf.Sqrt(w.x * w.y * ((1.5f*CurrentFloor)-0.5f))-1)+ CurrentFloor;
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
    public NavMeshEntity SpawnEnemy(EnemyHolder wank)
    {
        List<string> elitetypes = new List<string>();
        EliteTypesDict["Corrupted"].Enabled = false;
        EliteTypesDict["Lunar"].Enabled = false;
        EliteTypesDict["Unstable"].Enabled = CurrentFloor >= 6;
        EliteTypesDict["Splitting"].Enabled = CurrentFloor >= 6;
        foreach (var a in EliteTypes)
        {
            if (a.Enabled)
                elitetypes.Add(a.Name);
        }

        var ppos = wank.SpawnPos;
        if (ppos == Vector3.zero) ppos = FindValidPos(CurrentRoom, wank.EnemyObject.GetComponent<NavMeshEntity>());

        var ss = Instantiate(wank.EnemyObject, ppos, PlayerController.Instance.transform.rotation, Tags.refs["EnemyHolder"].transform);
        var rs = ss.GetComponent<NavMeshEntity>();
        rs.originroom = CurrentRoom;
        rs.EnemyHolder = wank;
        var e = wank.CreditCost;
        var dif = CurrentFloor - wank.MinFloor;
        if (wank.CanBeElite && Random.Range(0, 1f) < 0.2f * dif)
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
        creditcount -= e;
        rs.creditsspent = e;
        EnemiesExisting.Add(rs);
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
        var s = originroom.gm.transform;
        var s1 = s.localScale / 2;
        var x = s.position + new Vector3(Random.Range(-s1.x + 3f, s1.x - 3f), Random.Range(-s1.y + 3f, s1.y - 3f), 0);
        bool failed = false;
        if(RandomFunctions.Instance.Dist(x, PlayerController.Instance.transform.position) < 5)
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

        if (failed)
        {
            x = FindValidPos(originroom, wankw);
        }
        return x;
    }
    [HideInInspector]
    public I_Room OldCurrentRoom;
    public void BoomyRoomy()
    {
        OldCurrentRoom = CurrentRoom;
        var pp = new Vector2(CurrentRoom.transform.position.x,CurrentRoom.transform.position.y);
        var ppshex = CurrentRoom.room.RoomSize * 15f;
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
        if (shart.tag == "Sexy")
        {
            e.type = "Wall";
            e.BlocksSpawn = true;
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
        else if (shart.tag == "Enemy")
        {
            if (!noget)
            {
                e.entity = OXComponent.GetComponent<NavMeshEntity>(shart);
                e.entityoxs = e.entity.EntityOXS;
            }
            e.type = "Enemy";
        }
        else if (shart.tag == "Furniture")
        {
            if (!noget)
            {
                e.furniture = OXComponent.GetComponent<Furniture>(shart);
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
        return e;
    }

    public void SpawnGroundItem(Vector3 pos, GISItem cuum)
    {
        var itema = Instantiate(Gamer.Instance.GroundItemShit, pos, transform.rotation).GetComponent<GroundItem>();
        itema.sexyballer = cuum;
        Gamer.Instance.spawneditemsformymassivesexyballs.Add(itema);
    }

    public void SpawnHealers(Vector3 pos, int amount, PlayerController target)
    {
        List<GameObject> others = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            var wen = Instantiate(Gamer.Instance.HealerGFooFO, pos, transform.rotation, balls);
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
                completetetge = false;
                NextFloorBall = StartCoroutine(NextFloor());
                yield return new WaitUntil(() => { return completetetge; });
                break;
            case "NextShop":
                NextFloorBall = StartCoroutine(NextShopLevel());
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
    public void AttemptCraft()
    {
        var con = GISLol.Instance.All_Containers["Crafting"];
        if (ItemNameInput.text != "" && con.slots[0].Held_Item.CanCraft() && con.slots[1].Held_Item.CanCraft() && con.slots[2].Held_Item.CanCraft())
        {
            var e = new GISItem();
            e.ItemIndex = CraftSex;
            e.Amount = 1;
            e.CustomName = ItemNameInput.text;
            ItemNameInput.text = "";
            foreach (var ep in con.slots[0].Held_Item.Materials)
            {
                e.Materials.Add(ep);
            }
            foreach (var ep in con.slots[1].Held_Item.Materials)
            {
                e.Materials.Add(ep);
            }
            foreach (var ep in con.slots[2].Held_Item.Materials)
            {
                e.Materials.Add(ep);
            }
            con.slots[0].Held_Item = new GISItem();
            con.slots[1].Held_Item = new GISItem();
            con.slots[2].Held_Item = new GISItem();
            con.slots[3].Held_Item = e;
        }
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