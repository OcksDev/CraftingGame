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
    public int SaveFile = 0;
    //idk how needed this is tbh
    private string UniqueGamePrefix = "oxt";
    public int test = 0;

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
    public void ChangeFile(int i = 0)
    {
        SaveFile = i;
        LoadGame(i);
    }
    public void LoadGame(int i  = -1)
    {
        /* Input Modes:
         * -1 = Load whatever was the last used file (if being used for the first time it defaults to 0)
         * Any Other Value = Load the data of a specific file
         */

        LoadedData = true;

        InputManager.AssembleTheCodes();
        List<string> list = new List<string>();



        if (i == -1)
        {
            var xx = PlayerPrefs.GetInt("SaveFile", -1);
            if (xx != -1)
            {
                SaveFile = xx;
            }
            else
            {
                SaveFile = 0;
            }
        }
        else
        {
            SaveFile = i;
        }
        var s = SoundSystem.Instance;

        int x = 0;


        GetDataFromFile();

        var ghghgg = PlayerPrefs.GetString("keybinds", "fuck");
        list = StringToList(ghghgg);
        if (ghghgg != "fuck")
        {
            foreach (var a in list)
            {
                try
                {
                    var sseexx = StringToList(a, "<K>");
                    InputManager.gamekeys[sseexx[0]] = InputManager.namekeys[sseexx[1]];
                    x++;
                }
                catch
                {
                }
            }
        }
        x = 0;

        if(s != null)
        {
            s.MasterVolume = PlayerPrefs.GetFloat("snd_mas", 1);
            s.SFXVolume = PlayerPrefs.GetFloat("snd_sfx", 1);
            s.MusicVolume = PlayerPrefs.GetFloat("snd_mus", 1);
        }

        test = int.Parse(GetString("test_num", "0"));
        //ConsoleLol.Instance.ConsoleLog(Prefix(i) + "test_num");


        SaveFile = i;
    }
    public void SaveGame(int i = -1)
    {
        /* Input Modes:
         * -1 = Save whatever is the currently selected file (by default is 0)
         * Any Other Value = Save curent data to a specfic file
         */


        List<string> list = new List<string>();
        if (i == -1)
        {
            PlayerPrefs.SetInt("SaveFile", SaveFile);
        }
        else
        {
            PlayerPrefs.SetInt("SaveFile", i);
            SaveFile = i;
        }
        var s = SoundSystem.Instance;

        list.Clear();
        foreach (var a in InputManager.gamekeys)
        {
            list.Add(a.Key + "<K>" + InputManager.keynames[a.Value]);
        }
        PlayerPrefs.SetString("keybinds", ListToString(list));
        //PlayerPrefs.SetInt("UnitySelectMonitor", index); // sets the monitor that unity uses

        if (s != null)
        {
            PlayerPrefs.SetFloat("snd_mas", s.MasterVolume);
            PlayerPrefs.SetFloat("snd_sfx", s.SFXVolume);
            PlayerPrefs.SetFloat("snd_mus", s.MusicVolume);
        }

        SetString("test_num", test.ToString());
        GISLol.Instance.SaveAll();
        SaveDataToFile();
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

    public void SaveDataToFile(int path = 1, string dict = "def")
    {
        var f = FileSystem.Instance;
        f.AssembleFilePaths();
        f.WriteFile(f.FileLocations[path], DictionaryToString(GetDict(dict), Environment.NewLine, ": "), true);
    }


    public void GetDataFromFile(int path = 1, string dict = "def")
    {
        var f = FileSystem.Instance;
        f.AssembleFilePaths();
        var fp = f.FileLocations[path];
        var des = GetDict(dict);
        des.Clear();
        if (!File.Exists(fp))
        {
            f.WriteFile(fp, "", false);
            return;
        }
        var s = StringToList(f.ReadFile(fp), Environment.NewLine);
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
        if (file == -1) file = SaveFile;
        return UniqueGamePrefix + "#" + file + "_";
    }


    public int BoolToInt(bool a)
    {
        return a ? 1 : 0;
    }
    public bool IntToBool(int a)
    {
        return a == 1;
    }

    public string ListToString(List<string> eee, string split = ", ")
    {
        return String.Join(split, eee);
    }

    public List<string> StringToList(string eee, string split = ", ")
    {
        return eee.Split(split).ToList();
    }

    public string DictionaryToString(Dictionary<string, string> dic, string splitter = ", ", string splitter2 = "<K>")
    {
        List<string> list = new List<string>();
        foreach (var a in dic)
        {
            list.Add(a.Key + splitter2 + a.Value);
        }
        return ListToString(list, splitter);
    }
    public Dictionary<string, string> StringToDictionary(string e, string splitter = ", ", string splitter2 = "<K>")
    {
        var dic = new Dictionary<string, string>();
        var list = StringToList(e, splitter);
        foreach (var a in list)
        {
            try
            {
                int i = a.IndexOf(splitter2);
                List<string> sseexx = new List<string>()
                {
                    a.Substring(0, i),
                    a.Substring(i + splitter2.Length),
                };
                if (dic.ContainsKey(sseexx[0]))
                {
                    dic[sseexx[0]] = dic[sseexx[1]];
                }
                else
                {
                    dic.Add(sseexx[0], sseexx[1]);
                }
            }
            catch
            {
            }
        }
        return dic;
    }

}
