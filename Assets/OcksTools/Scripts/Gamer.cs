using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gamer : MonoBehaviour
{
    public bool[] checks = new bool[20];
    public Material[] sexex = new Material[2];
    public Image fader;
    public List<GISContainer> ballers = new List<GISContainer>();
    public List<GameObject> Enemies = new List<GameObject>();
    public List<PlayerController> Players = new List<PlayerController>();
    public List<NavMeshEntity> EnemiesExisting = new List<NavMeshEntity>();
    public GameObject HealerGFooFO;
    public NavMeshRefresher nmr;
    public static bool IsMultiplayer = false;
    public int CraftSex = 3;
    public Selector cuumer;
    public GameObject PlayerPrefab;
    public List<GameObject> healers = new List<GameObject>();
    public void UpdateMenus()
    {
        Tags.refs["Inventory"].SetActive(checks[0]);
        Tags.refs["Crafting"].SetActive(checks[1]);
        Tags.refs["Equips"].SetActive(checks[2]);
        Tags.refs["MainMenu"].SetActive(checks[3]);
        Tags.refs["PauseMenu"].SetActive(checks[4]);
    }

    public static Gamer Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        MainMenu();
        StartCoroutine(FUCK());
    }
    public static List<List<string>> Backup = new List<List<string>>();


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
    }

    public void LoadLobbyScene()
    {
        Tags.refs["NextFloor"].transform.position = new Vector3(11.51f, 0, 0);
        Tags.refs["Lobby"].SetActive(true);
        Tags.refs["Baller"].transform.position = new Vector3(5.12f, -6.6f, 17.68f);
        if (IsMultiplayer)
        {
            StartCoroutine(WaitForSexyGamer());
        }
    }
    public IEnumerator WaitForSexyGamer()
    {
        yield return new WaitUntil(() => { return ServerGamer.Instance != null; });
        foreach(var s in Backup)
        {
            OcksNetworkVar g = new OcksNetworkVar(s[1], s[0]);
            g.SetValue(s[2]);
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
        ClearMap();
        checks[3] = true;
        CraftSex = 3;
        Players.Clear();
        if (PlayerController.Instance != null) Destroy(PlayerController.Instance.gameObject);
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
    }

    public void Update()
    {
        if (InputManager.IsKeyDown(InputManager.gamekeys["close_menu"]))
        {
            checks[4] = !checks[4];
            UpdateMenus();  
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
    public IEnumerator NextFloor()
    {
        Tags.refs["Lobby"].SetActive(false);
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
        var rm = enders[Random.Range(0, enders.Count)];
        rm.isused = true;
        PlayerController.Instance.transform.position = rm.transform.position;
        var rmod = rm;
        enders.Remove(rm);
        List<float[]> pp = new List<float[]>();
        for(int i = 0; i < enders.Count; i++)
        {
            rm = enders[i];
            var e = new float[2] {i,(rm.transform.position-rmod.transform.position).magnitude};
            bool f = false;
            for(int j = 0; j < pp.Count; j++)
            {
                if (e[1] > pp[j][1])
                {
                    pp.Insert(j, e);
                    f = true;
                    break;
                }
            }
            if(!f)pp.Add(e);
        }
        rm = null;
        for (int i =0; i < pp.Count; i++)
        {
            if (Random.Range(0, 3) == 1) { rm = enders[(int)pp[i][0]]; break; }
        }
        if(rm==null) rm = enders[(int)pp[0][0]];
        rm.isused = true;
        Tags.refs["NextFloor"].transform.position = rm.transform.position;
        var e2 = CameraLol.Instance.transform.position;
        e2.x = PlayerController.Instance.transform.position.x;
        e2.y = PlayerController.Instance.transform.position.y;
        CameraLol.Instance.transform.position = e2;
        CameraLol.Instance.ppos = e2;

        yield return new WaitForFixedUpdate();

        nmr.BuildNavMesh();

        foreach (var e in RoomLol.Instance.SpawnedRoomsDos)
        {
            if (!e.isused)
            {
                availablerooms.Add(e);
            }
        }
        foreach(var e in availablerooms)
        {
            for(int i =0; i < 5; i++)
            {
                var s = e.gm.transform;
                var ss = Instantiate(Enemies[0], s.position, PlayerController.Instance.transform.rotation);
                var r = ss.GetComponent<NavMeshEntity>();
                r.originroom = e;
                EnemiesExisting.Add(r);
            }
        }
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
                StartCoroutine(NextFloor());
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
            e.ItemType = "Made";
            con.slots[0].Held_Item = new GISItem();
            con.slots[1].Held_Item = new GISItem();
            con.slots[2].Held_Item = new GISItem();
            con.slots[3].Held_Item = e;
        }
    }
}


