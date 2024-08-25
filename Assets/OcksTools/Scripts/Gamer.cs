using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class Gamer : MonoBehaviour
{
    public bool[] checks = new bool[20];
    public Material[] sexex = new Material[2];
    public Image fader;
    public List<GISContainer> ballers = new List<GISContainer>();
    public List<EnemyHolder> EnemiesDos = new List<EnemyHolder>();
    public List<PlayerController> Players = new List<PlayerController>();
    public List<NavMeshEntity> EnemiesExisting = new List<NavMeshEntity>();
    public List<GameObject> Chests = new List<GameObject>();
    public List<EliteTypeHolder> EliteTypes = new List<EliteTypeHolder>();
    public GameObject HealerGFooFO;
    public NavMeshRefresher nmr;
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
    public Transform ItemDisplayParent;
    public GameObject ItemDisplay;
    public GameObject DoorFab;
    public GameObject SpawnFix;
    public Dictionary<string, EliteTypeHolder> EliteTypesDict = new Dictionary<string, EliteTypeHolder>();

    public List<GameObject> ParticleSpawns = new List<GameObject>();

    public bool NextFloorButtonSexFuck = false;

    public List<Image> HitSexers = new List<Image>();

    [HideInInspector]
    public bool InRoom = false;
    [HideInInspector]
    public I_Room CurrentRoom;



    public delegate void JustFuckingRunTheMethods();
    public event JustFuckingRunTheMethods RefreshUIPos;
    public static bool WithinAMenu = false;
    public void UpdateMenus()
    {
        Tags.refs["Inventory"].SetActive(checks[0]);
        Tags.refs["Crafting"].SetActive(checks[1]);
        Tags.refs["Equips"].SetActive(checks[2]);
        Tags.refs["MainMenu"].SetActive(checks[3]);
        Tags.refs["PauseMenu"].SetActive(checks[4]);
        Tags.refs["ItemMenu"].SetActive(checks[5]);
        Tags.refs["DedMenu"].SetActive(checks[6]);
        Tags.refs["TempMatMenu"].SetActive(checks[7]);

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
    }
    private void Start()
    {
        Tags.refs["BGblack"].SetActive(true);
        MainMenu();
        CurrentFloor = 0;
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
        foreach (var sex in balls.GetComponentsInChildren<EnemyHitShit>())
        {
            Destroy(sex.gameObject);
        }
    }
    public bool IsHost;
    public void LoadLobbyScene()
    {
        GameState = "Lobby";
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
        yield return new WaitForSeconds(0.2f);
        checks[0] = false;
        checks[1] = false;
        checks[2] = false;
        checks[7] = false;
        UpdateMenus();
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
    public void MainMenu()
    {
        if (IsMultiplayer) NetworkManager.Singleton.Shutdown();
        IsMultiplayer = false;
        for (int i = 0;i < checks.Length;i++)
        {
            checks[i] = false;
        }
        Time.timeScale = 1;
        ShartPoop = 0f;
        ClearMap();
        Tags.refs["BlackBG"].SetActive(false);
        checks[3] = true;
        CraftSex = "Sword";
        Players.Clear();
        GameState = "Main Menu";
        if (PlayerController.Instance != null) Destroy(PlayerController.Instance.gameObject);
        UpdateMenus();
        var e2 = CameraLol.Instance.transform.position;
        e2.x = 0;
        e2.y = 0;

        foreach(var a in EliteTypes)
        {
            a.Enabled = true;
        }

        CameraLol.Instance.transform.position = e2;
        CameraLol.Instance.ppos = e2;
        CameraLol.Instance.targetpos = e2;
    }


    public IEnumerator FUCK()
    {
        yield return new WaitForFixedUpdate();
        foreach (var e in ballers)
        {
            e.Start();
            e.LoadContents();
        }
        yield return new WaitForFixedUpdate();
        StartCoroutine(instancecoolmenus());
    }

    public void Update()
    {
        if (InputManager.IsKeyDown(InputManager.gamekeys["close_menu"]))
        {
            SetPauseMenu(!checks[4]);
        }
        if (InputManager.IsKeyDown(InputManager.gamekeys["inven"]))
        {
            checks[0] = !checks[0];
            checks[1] = false;
            checks[2] = false;
            UpdateMenus();
        }
        if (checks[0] && InputManager.IsKeyDown(KeyCode.I))
        {
            checks[2] = !checks[2];
            checks[1] = false;
            UpdateMenus();
        }

    }

    public void SetPauseMenu(bool a)
    {
        checks[4] = a;
        if (!IsMultiplayer) Time.timeScale = checks[4] ? 0 : 1;
        UpdateMenus();
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
    public IEnumerator NextFloor()
    {
        Seed = Random.Range(-999999999, 999999999);
        GlobalRand = new System.Random(Seed);
        GameState = "Game";
        CurrentFloor++;
        Tags.refs["Lobby"].SetActive(false);
        Tags.refs["BlackBG"].SetActive(true);
        ClearMap();
        RoomLol.Instance.GenerateRandomLayout();
        
        List<I_Room> enders = new List<I_Room>();
        List<I_Room> availablerooms = new List<I_Room>();
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
            Debug.Log("Level:" + psex.level);
            if (psex.level > level) level = psex.level;
        }
        foreach (var psex in enders)
        {
            if (psex.level == level) endos.Add(psex);
        }
        var rm = endos[r.Next(0, endos.Count)];
        Debug.Log("Endo Count: "+ endos.Count);
        rm.isused = true;
        PlayerController.Instance.transform.position = rm.transform.position;
        enders.Remove(rm);
        endos.Remove(rm);
        var enbackup = new List<I_Room>(endos);
        for(int i = 0; i < endos.Count; i++)
        {
            if (endos[i].parent_room == rm.parent_room)
            {
                endos.RemoveAt(i);
                i--;
            }
        }
        if (endos.Count == 0) endos = enbackup;
        var rm2 = endos[r.Next(0,endos.Count)];
        rm2.isused = true;
        Tags.refs["NextFloor"].transform.position = rm2.transform.position;
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
            e.isused = true;
            var f = new GISItem("Rock");
            c.cuum = f;
            spawnedchests.Add(c);
        }

        PlayerController.Instance.DashCoolDown = PlayerController.Instance.MaxDashCooldown * 3;

        yield return new WaitForFixedUpdate();

        nmr.BuildNavMesh();

        foreach (var e in RoomLol.Instance.SpawnedRoomsDos)
        {
            if (!e.isused)
            {
                availablerooms.Add(e);
            }
        }
        completetetge = true;
    }
    long creditcount = 0;
    public IEnumerator StartRoom()
    {
        BoomyRoomy();
        yield return new WaitForSeconds(1.5f);
        int waves = Random.Range(3,6);
        for(int i = 0; i < waves; i++)
        {
            if (GameState != "Game") break;
            SpawnEnemyWave();
            yield return new WaitForSeconds(1.4f);
        }
        yield return new WaitUntil(() => { return EnemiesExisting.Count == 0; });
        CurrentRoom = null;
        PlayerController.Instance.DashCoolDown = PlayerController.Instance.MaxDashCooldown * 3;
        InRoom = false;
    }


    public List<GameObject> SpawnEnemyWave()
    {
        List<GameObject> suck = new List<GameObject>();
        var w = CurrentRoom.room.RoomSize;
        creditcount = (long)(25 * Mathf.Sqrt(w.x * w.y * CurrentFloor));
        int x = 0;
        while(creditcount > 0)
        {
            var wank = GetEnemyForDiff(false);
            suck.Add(SpawnEnemy(wank));
            x++;
            if (x >= 4) break;
        }
        return suck;
    }
    public GameObject UnstableBullet;
    public GameObject HealBeam;
    public GameObject SpawnEnemy(EnemyHolder wank)
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
        if (ppos == Vector3.zero) ppos = FindValidPos(CurrentRoom);

        var ss = Instantiate(wank.EnemyObject, ppos, PlayerController.Instance.transform.rotation, Tags.refs["EnemyHolder"].transform);
        var rs = ss.GetComponent<NavMeshEntity>();
        rs.originroom = CurrentRoom;
        rs.EnemyHolder = wank;
        var e = wank.CreditCost;
        var dif = CurrentFloor - wank.MinFloor;
        if (wank.CanBeElite && Random.Range(0, 1f) < 0.15f * dif)
        {
            if (dif < 10)
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
        EnemiesExisting.Add(rs);
        return ss;
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



    public Vector3 FindValidPos(I_Room originroom)
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
            var a = Physics2D.OverlapCircleAll((Vector2)x, 1);
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
            x = FindValidPos(originroom);
        }
        return x;
    }


    public void BoomyRoomy()
    {
        var pp = new Vector2(CurrentRoom.transform.position.x,CurrentRoom.transform.position.y);
        var ppshex = CurrentRoom.room.RoomSize * 15f;
        if (CurrentRoom.room.HasTopDoor)
        {
            var pz = pp - ppshex;
            var ppos = CurrentRoom.room.TopDoor * 30f;
            ppos.x += 15f;
            Instantiate(DoorFab, new Vector3(pz.x + ppos.x, pz.y, 0), Quaternion.identity, nmr.transform);
        }
        if (CurrentRoom.room.HasBottomDoor)
        {
            var pz = ppshex;
            pz.y *= -1;
            pz = pp - pz;
            var ppos = CurrentRoom.room.BottomDoor * 30f;
            ppos.x += 15f;
            Instantiate(DoorFab, new Vector3(pz.x + ppos.x, pz.y, 0), Quaternion.Euler(0,0,180), nmr.transform);
        }
        if (CurrentRoom.room.HasLeftDoor)
        {
            var pz = ppshex;
            pz.y *= -1;
            pz = pp - pz;
            var ppos = CurrentRoom.room.LeftDoor * 30f;
            ppos.y += 15f;
            Instantiate(DoorFab, new Vector3(pz.x, pz.y - ppos.y, 0), Quaternion.Euler(0, 0, 270), nmr.transform);
        }
        if (CurrentRoom.room.HasRightDoor)
        {
            var pz = ppshex;
            pz = pp + pz;
            var ppos = CurrentRoom.room.RightDoor * 30f;
            ppos.y += 15f;
            Instantiate(DoorFab, new Vector3(pz.x, pz.y - ppos.y, 0), Quaternion.Euler(0, 0, 90), nmr.transform);
        }
    }


    public float ShartPoop = 0f;
    private void FixedUpdate()
    {
        if (GameState == "Game" || GameState == "Lobby")
        {
            if (NextFloorButtonSexFuck)
            {
                NextFloorButtonSexFuck = false;
                StartCoroutine(StartFade("NextFloor"));
            }
            if (checks[5])
            {
                if (!InputManager.IsKey(KeyCode.Tab))
                {
                    ToggleTabMenu();
                    checks[5] = false;
                }
            }
            else
            {
                if (InputManager.IsKey(KeyCode.Tab))
                {
                    ToggleTabMenu();
                    checks[5] = true;
                }
            }
            ShartPoop -= Time.deltaTime;
            ShartPoop = Mathf.Clamp01(ShartPoop);
            var c = HitSexers[0].color;
            c.a = ShartPoop;
            foreach(var ca in HitSexers)
            {
                ca.color = c;
            }

        }
        else
        {
            checks[5] = false;
        }
    }

    public ObjectType GetObjectType(GameObject shart, bool noget = false)
    {
        var e = new ObjectType();
        e.gm = shart;
        if (shart.tag == "Sexy")
        {
            e.type = "Wall";
            e.BlocksSpawn = true;
        }
        else if (shart.tag == "PlayerNerd")
        {
            if (!noget)
            {
                e.playerController = shart.GetComponent<PlayerController>();
                e.entityoxs = e.playerController.entit;
            }
            e.type = "Player";
            e.BlocksSpawn = true;
        }
        else if (shart.tag == "Enemy")
        {
            if (!noget)
            {
                e.entity = shart.GetComponent<NavMeshEntity>();
                e.entityoxs = e.entity.EntityOXS;
            }
            e.type = "Enemy";
        }
        else if (shart.tag == "Furniture")
        {
            if (!noget)
                e.furniture = shart.GetComponent<Furniture>();
            e.type = "Furniture";
            e.DoOnTouch += e.furniture.OnTouch;
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

    public void ToggleTabMenu()
    {
        checks[5] = !checks[5];
        UpdateMenus();
    }


    public bool IsFading = false;
    public IEnumerator StartFade(string type)
    {
        IsFading = true;
        fader.raycastTarget = true;
        int steps = 50;
        for(int i = 0; i < steps; i++)
        {
            yield return new WaitForFixedUpdate();
            var c = fader.color;
            c.a = (float)i / steps;
            fader.color = c;
        }
        yield return new WaitForFixedUpdate();
        var c2 = fader.color;
        c2.a = 1;
        fader.color = c2;
        switch (type)
        {
            case "NextFloor":
                completetetge = false;
                StartCoroutine(NextFloor());
                yield return new WaitUntil(() => { return completetetge; });
                break;
        }
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForFixedUpdate();
            var c = fader.color;
            c.a = (float)(50- i) / steps;
            fader.color = c;
        }
        yield return new WaitForFixedUpdate();
        c2 = fader.color;
        c2.a = 0;
        fader.color = c2;

        fader.raycastTarget = false;
        IsFading = false;
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
        if (con.slots[0].Held_Item.CanCraft() && con.slots[1].Held_Item.CanCraft() && con.slots[2].Held_Item.CanCraft())
        {
            var e = new GISItem();
            e.ItemIndex = CraftSex;
            e.Amount = 1;
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