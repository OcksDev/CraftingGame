using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem instance;
    public bool UseFileSystem = true;
    //idk how needed this is tbh
    private string UniqueGamePrefix = "oxt";
    public int test = 0;

    public bool NoScroll = false;
    public bool DashSkillShow = false;

    public delegate void JustFuckingRunTheMethods();
    public static event JustFuckingRunTheMethods SaveAllData;
    public static event JustFuckingRunTheMethods LoadAllData;

    public Dictionary<string, Dictionary<string, string>> HoldingData = new Dictionary<string, Dictionary<string, string>>();

    public static SaveSystem Instance
    {
        get { return instance; }
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private void Awake()
    {
        if (Instance == null) instance = this;
    }
    private void Start()
    {
        LoadGame();
    }
    [HideInInspector]
    public bool LoadedData = false;
    public void LoadGame(string dict = "def")
    {
        LoadedData = true;


        InputManager.AssembleTheCodes();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        List<string> list = new List<string>();



        var s = SoundSystem.Instance;

        int x = 0;


        GetDataFromFile(dict);

        var ghghgg = GetString("keybinds", "fuck", dict);
        dic = Converter.StringToDictionary(ghghgg);
        List<KeyCode> shungite = new List<KeyCode>();
        if (ghghgg != "fuck")
        {
            foreach (var a in dic)
            {
                list = Converter.StringToList(a.Value);
                shungite.Clear();
                foreach (var key in list)
                {
                    shungite.Add(InputManager.namekeys[key]);
                }
                InputManager.gamekeys[a.Key] = new List<KeyCode>(shungite);
            }
        }

        if (s != null)
        {
            s.MasterVolume = float.Parse(GetString("snd_mas", "1", dict));
            s.SFXVolume = float.Parse(GetString("snd_sfx", "1", dict));
            s.MusicVolume = float.Parse(GetString("snd_mus", "1", dict));
        }
        Gamer.Instance.Highlights = float.Parse(GetString("highlights", "0.5", dict));
        Gamer.Instance.Lowlights = float.Parse(GetString("lowlights", "0.5", dict));
        var pp = Converter.StringToDictionary(GetString("vault_items", "", dict));
        GISLol.Instance.VaultItems.Clear();
        foreach(var weenor in pp)
        {
            var ppw = new GISItem();
            ppw.StringToItem(weenor.Key);
            GISLol.Instance.VaultItems.Add(ppw, int.Parse(weenor.Value));
        }
        GISLol.Instance.LogbookDiscoveries = Converter.StringToDictionary(GetString("logbook", "", dict));
        NoScroll = bool.Parse(GetString("noscroll", "False", dict));
        DashSkillShow = bool.Parse(GetString("dashskillshow", "False", dict));
        test = int.Parse(GetString("test_num", "0", dict));


        list = Converter.StringToList(GetString("quests", "", dict));
        GISLol.Instance.Quests.Clear();
        foreach (var quest in list)
        {
            if (quest == "") continue;
            GISLol.Instance.Quests.Add(new QuestProgress(quest));
        }
        Gamer.Instance.TimeOfQuest = double.Parse(GetString("questtime", "-1", dict));

        //ConsoleLol.Instance.ConsoleLog(Prefix(i) + "test_num");
        Gamer.Instance.UpdateShaders();
        Gamer.Instance.AttemptAddLogbookItem("Rock");
        LoadAllData?.Invoke();
    }
    public void SaveGame(string dict = "def")
    {
        /* Input Modes:
         * -1 = Save whatever is the currently selected file (by default is 0)
         * Any Other Value = Save curent data to a specfic file
         */
        Dictionary<string, string> dic = new Dictionary<string, string>();
        List<string> list = new List<string>();
        var s = SoundSystem.Instance;

        dic.Clear();
        foreach (var a in InputManager.gamekeys)
        {
            list.Clear();
            foreach (var b in a.Value)
            {
                list.Add(InputManager.keynames[b]);
            }
            dic.Add(a.Key, Converter.ListToString(list));
        }
        SetString("keybinds", Converter.DictionaryToString(dic), dict);
        //PlayerPrefs.SetInt("UnitySelectMonitor", index); // sets the monitor that unity uses

        if (s != null)
        {
            SetString("snd_mas", s.MasterVolume.ToString(), dict);
            SetString("snd_sfx", s.SFXVolume.ToString(), dict);
            SetString("snd_mus", s.MusicVolume.ToString(), dict);
        }
        SetString("highlights", Gamer.Instance.Highlights.ToString(), dict);
        SetString("lowlights", Gamer.Instance.Highlights.ToString(), dict);

        Dictionary<string, string> pp = new Dictionary<string, string>();
        foreach (var item in GISLol.Instance.VaultItems)
        {
            pp.Add(item.Key.ItemToString(), item.Value.ToString());
        }
        SetString("vault_items", Converter.DictionaryToString(pp), dict);



        SetString("noscroll", NoScroll.ToString(), dict);
        SetString("dashskillshow", DashSkillShow.ToString(), dict);

        SetString("logbook", Converter.DictionaryToString(GISLol.Instance.LogbookDiscoveries), dict);

        list.Clear();
        foreach(var quest in GISLol.Instance.Quests)
        {
            list.Add(quest.DataToString());
        }
        SetString("quests", Converter.ListToString(list), dict);
        SetString("questtime", Gamer.Instance.TimeOfQuest.ToString(), dict);

        SaveAllData?.Invoke();
        SaveDataToFile(dict);
    }

    public void SaveCurrentRun()
    {
        string dict = "current_run";
        SetString("Floor", Gamer.CurrentFloor.ToString(), dict);
        SetString("Health", (PlayerController.Instance.entit.Health/ PlayerController.Instance.entit.Max_Health).ToString(), dict);
        SetString("Seed", Gamer.Seed.ToString(), dict);
        SetString("Coins", PlayerController.Instance.Coins.ToString(), dict);
        var c = GISLol.Instance.All_Containers["Equips"];
        SetString("Weapon1", c.slots[0].Held_Item.ItemToString(true), dict);
        SetString("Weapon2", c.slots[1].Held_Item.ItemToString(true), dict);
        List<string> list = new List<string>();
        foreach(var a in PlayerController.Instance.Skills)
        {
            list.Add(a.SkillToString());
        }
        SetString("Skills", Converter.ListToString(list, "<SK>"), dict);

        SaveDataToFile(dict);
    }
    
    public void LoadCurrentRun()
    {
        string dict = "current_run";
        GetDataFromFile(dict);
        if (GetDict(dict).ContainsKey("Loaded"))
        {
            Gamer.CurrentFloor = int.Parse(GetString("Floor", "1", dict)) - 1;
            Gamer.Seed = int.Parse(GetString("Seed", "0", dict));

            Gamer.Instance.StartCoroutine(Gamer.Instance.StartFade("NextFloor2"));
        }
        else
        {
            Gamer.Instance.StartCoroutine(Gamer.Instance.StartFade("NextFloor"));
        }
    }
    public void LoadCurRun2()
    {
        string dict = "current_run";
        PlayerController.Instance.entit.Health = double.Parse(GetString("Health", "1", dict))* PlayerController.Instance.entit.Max_Health;
        PlayerController.Instance.Coins = long.Parse(GetString("Coins", "0", dict));
        Gamer.Instance.SaveCurrentWeapons();
        var c = GISLol.Instance.All_Containers["Equips"];
        var we = new GISItem();
        we.StringToItem(GetString("Weapon1", "", dict));
        c.slots[0].Held_Item = we;
        we = new GISItem();
        we.StringToItem(GetString("Weapon2", "", dict));
        c.slots[1].Held_Item = we;
        PlayerController.Instance.SwitchWeapon(0);
        var list = Converter.StringToList(GetString("Skills", "", dict), "<SK>");
        PlayerController.Instance.Skills.Clear();
        foreach (var a in list)
        {
            var sk = new Skill();
            sk.StringToSkill(a);
            PlayerController.Instance.Skills.Add(sk);
        }
        StartCoroutine(WaitRefersh());
    }

    public IEnumerator WaitRefersh()
    {
        yield return new WaitForFixedUpdate();
        Gamer.Instance.UpdateMenus();
        PlayerController.Instance.SetData();
    }


    public string DictNameToFilePath(string e)
    {
        var f = FileSystem.Instance;
        switch (e)
        {
            case "def": return $"{f.GameDirectory}\\Game_Data.txt";
            case "ox_profile": return $"{f.UniversalDirectory}\\Player_Data.txt";
            case "current_run": return $"{f.GameDirectory}\\Current_Run_Savestate.txt";
            case "weapons": return $"{f.GameDirectory}\\Equipped_Weapons.txt";
            default: return $"{f.GameDirectory}\\Data_{e}.txt";
        }
    }


    public Dictionary<string, string> GetDict(string name = "def")
    {
        if (HoldingData.ContainsKey(name))
        {
            return HoldingData[name];
        }
        else
        {
            HoldingData.Add(name, new Dictionary<string, string>());
            return HoldingData[name];
        }
    }

    public void ResetFile(string dict)
    {
        var f = FileSystem.Instance;
        f.AssembleFilePaths();
        f.WriteFile(DictNameToFilePath(dict), "", true);
    }

    public void SetString(string key, string data, string dict = "def")
    {
        var d = GetDict(dict);
        if (d.ContainsKey(key))
        {
            d[key] = data;
        }
        else
        {
            d.Add(key, data);
        }
    }

    public string GetString(string key, string defaul = "", string dict = "def")
    {
        var d = GetDict(dict);
        if (d.ContainsKey(key))
        {
            return d[key];
        }
        else
        {
            return defaul;
        }
    }

    public void SaveDataToFile(string dict = "def")
    {
        var f = FileSystem.Instance;
        f.AssembleFilePaths();
        var w = GetDict(dict);
        if (!w.ContainsKey("Loaded")) w.Add("Loaded", "True");
        f.WriteFile(DictNameToFilePath(dict), Converter.DictionaryToString(w, Environment.NewLine, ": "), true);
    }


    public void GetDataFromFile(string dict = "def")
    {
        var f = FileSystem.Instance;
        f.AssembleFilePaths();
        var fp = DictNameToFilePath(dict);
        var des = GetDict(dict);
        des.Clear();
        if (!File.Exists(fp))
        {
            f.WriteFile(fp, "", false);
            return;
        }
        var s = Converter.StringToList(f.ReadFile(fp), Environment.NewLine);
        foreach (var d in s)
        {
            if (d.IndexOf(": ") > -1)
            {
                des.Add(d.Substring(0, d.IndexOf(": ")), d.Substring(d.IndexOf(": ") + 2));
            }
        }
    }


    public string Prefix(int file)
    {
        return UniqueGamePrefix + "_";
    }


}
