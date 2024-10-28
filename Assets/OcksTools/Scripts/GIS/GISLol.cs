using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GISLol : MonoBehaviour
{
    public bool UseLanguageFile = true;
    public GISItem Mouse_Held_Item;
    public GISDisplay Mouse_Displayer;
    public GameObject MouseFollower;
    public RectTransform BallFondler;
    public RectTransform NormalRender;
    private static GISLol instance;
    public List<GISItem_Data> Items = new List<GISItem_Data>();
    public List<GISMaterial_Data> Materials = new List<GISMaterial_Data>();
    public Dictionary<string, GISItem_Data> ItemsDict = new Dictionary<string, GISItem_Data>();
    public Dictionary<string, GISMaterial_Data> MaterialsDict = new Dictionary<string, GISMaterial_Data>();
    public List<string> AllWeaponNames = new List<string>();
    public List<string> AllCraftables = new List<string>();
    private RectTransform ballingsexnut;
    private HoverRefHolder hovercummer;
    public List<Color32> attributecolors = new List<Color32>();
    public List<string> nonowords = new List<string>();

    public Dictionary<string,GISContainer> All_Containers = new Dictionary<string, GISContainer>();


    public TextAsset DesciptionOverrides;
    public TextAsset GeneralDesciptionOverrides;
    public static GISLol Instance
    {
        get { return instance; }
    }

    public void LoadTempForAll()
    {
        foreach(var con in All_Containers)
        {
            if (con.Value != null) con.Value.LoadTempContents();
        }
    }

    private void Awake()
    {

        foreach (var a in Items)
        {
            ItemsDict.Add(a.Name, a);
            if(a.IsWeapon) AllWeaponNames.Add(a.Name);
            if(a.IsCraftable) AllCraftables.Add(a.Name);
        }

        foreach (var a in Materials)
        {
            MaterialsDict.Add(a.Name, a);
        }

        var e = Converter.StringToDictionary(DesciptionOverrides.text.Replace("\r", ""), "\n", ":: ");
        foreach (var we in e)
        {
            if (MaterialsDict.ContainsKey(we.Key))
            {
                MaterialsDict[we.Key].Description = we.Value;
            }
        }
        e = Converter.StringToDictionary(GeneralDesciptionOverrides.text.Replace("\r", ""), "\n", ":: ");
        foreach (var we in e)
        {
            if (ItemsDict.ContainsKey(we.Key))
            {
                ItemsDict[we.Key].Description = we.Value;
            }
        }

        if (Instance == null) instance = this;
        SaveSystem.SaveAllData += SaveAll;
        Mouse_Held_Item = new GISItem();
    }

    private void Start()
    {
        MouseFollower.SetActive(true);
        ballingsexnut = MouseFollower.GetComponent<RectTransform>();
        hovercummer = BallFondler.GetComponent<HoverRefHolder>();
        if (UseLanguageFile)
        {
            
            var l = LanguageFileSystem.Instance;
            l.UpdateGameFromFile();
            bool changed = false;
            for (int i = 0; i < Items.Count; i++)
            {
                var s = "Item_" + SaltName(Items[i].Name);
                var s2 = "ItemDesc_" + Items[i].Description;
                if (l.IndexValuePairs.ContainsKey(s))
                {
                    Items[i].Name = l.IndexValuePairs[s];
                }
                else
                {
                    changed = true;
                    l.IndexValuePairs.Add(s, Items[i].Name);
                }
                if (l.IndexValuePairs.ContainsKey(s2))
                {
                    Items[i].Description = l.IndexValuePairs[s2];
                }
                else
                {
                    changed = true;
                    l.IndexValuePairs.Add(s2, Items[i].Description);
                }
            }
            if (changed) l.UpdateTextFile();
            
        }
    }
    public bool CanHover = false;
    [HideInInspector]
    public List<RaycastResult> rcl = new List<RaycastResult>();
    public void HoverDataCooler()
    {
        if (!CanHover) return;
        CanHover = false;
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        rcl.Clear();
        EventSystem.current.RaycastAll(ped, rcl);
    }
    public bool IsHoveringReal(GameObject banana)
    {
        GISLol.Instance.HoverDataCooler();
        foreach (var ray in GISLol.Instance.rcl)
        {
            if (ray.gameObject == banana)
            {
                return true;
            }
        }


        return false;
    }
    //public static event Gamer.JustFuckingRunTheMethods checkforhover;
    public GISItem hoverballer;
    private void Update()
    {
        Mouse_Displayer.item = Mouse_Held_Item;
        var za = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        za.z = 0;
        MouseFollower.transform.position = za;
        hoverballer = null;
        CanHover = true;
        foreach (var boner in All_Containers)
        {
            if (boner.Value.gameObject.activeInHierarchy)
            {
                foreach(var bone in boner.Value.slots)
                {
                    if(bone != null && IsHoveringReal(bone.gameObject)){
                        hoverballer = bone.Held_Item;
                    }
                }
            }
            if (hoverballer != null) break;
        }

        //event call?
        if (!founddaddy && hoverballer != null && hoverballer.ItemIndex != "Empty")
        {
            HoverDohicky(new HoverType(hoverballer));
        }
        if (!founddaddy)
        {
            hovercummer.gameObject.SetActive(false);
        }
        founddaddy = false;

#if UNITY_EDITOR
        if (InputManager.IsKeyDown(KeyCode.X, "menu"))
        {
            Mouse_Held_Item = new GISItem("Rock");
        }
        if (InputManager.IsKeyDown(KeyCode.C, "menu"))
        {
            Mouse_Held_Item = new GISItem("Gold");
        }
        if (InputManager.IsKeyDown(KeyCode.V, "menu"))
        {
            Mouse_Held_Item = new GISItem("Emerald");
        }
        if (InputManager.IsKeyDown(KeyCode.B, "menu"))
        {
            Mouse_Held_Item = new GISItem("Slime");
        }
        if (InputManager.IsKeyDown(KeyCode.N, "menu"))
        {
            Mouse_Held_Item = new GISItem("Glass");
        }
        if (InputManager.IsKeyDown(KeyCode.M, "menu"))
        {
            Mouse_Held_Item = new GISItem("Angelic Ingot");
        }
        if (InputManager.IsKeyDown(KeyCode.Comma, "menu"))
        {
            Mouse_Held_Item = new GISItem("Demonic Ingot");
        }
        if (InputManager.IsKeyDown(KeyCode.Period, "menu"))
        {
            Mouse_Held_Item = new GISItem("Amethyst");
        }
        if (InputManager.IsKeyDown(KeyCode.Backslash, "menu")|| InputManager.IsKeyDown(KeyCode.Backslash, "item_menu"))
        {
            if(Mouse_Held_Item.ItemIndex != "Empty")
            {
                Mouse_Held_Item = new GISItem(Items[Items.IndexOf(ItemsDict[Mouse_Held_Item.ItemIndex])+1].Name);
            }
            else
            {
                Mouse_Held_Item = new GISItem("Rune Of Self");
            }
        }
        if (InputManager.IsKeyDown(KeyCode.P, "menu"))
        {
            var e = new GISMaterial();
            e.itemindex = "Rock";
            PlayerController.Instance.mainweapon.Materials.Add(e);
        }
        
        if (InputManager.IsKeyDown(KeyCode.L, "menu"))
        {
            Debug.LogError("Fuck you, pause the game");
        }
#endif
    }
    bool founddaddy = false;
    public void HoverDohicky(HoverType hv)
    {
        if (founddaddy) return;
        var hoverballer = hv.item;
        hovercummer.gameObject.SetActive(true);
        if (true)
        {
            hovercummer.SetMostData(hv);
            var wank = ballingsexnut.anchoredPosition;
            float xoffset = 35;
            float yoffset = 35;
            var halfsize = BallFondler.sizeDelta / 2;
            var halfnormalsize = NormalRender.sizeDelta / 2;
            wank.x += xoffset + halfsize.x;
            if (halfsize.y > yoffset)
            {
                wank.y += yoffset - halfsize.y;
            }
            var yfloor = wank.y - halfsize.y;
            var yceil = wank.y + halfsize.y;
            var xceil = wank.x + halfsize.x;
            if (yfloor <= -halfnormalsize.y)
            {
                wank.y = halfsize.y - halfnormalsize.y;
            }
            else if (yceil >= halfnormalsize.y)
            {
                wank.y = halfnormalsize.y - halfsize.y;
            }
            if (xceil >= halfnormalsize.x)
            {
                wank.x -= (xoffset + halfsize.x) * 2;
            }
            BallFondler.anchoredPosition = wank;
        }
        founddaddy = true;
    }

    public string GetDescription(GISItem baller)
    {
        var wank = ItemsDict[baller.ItemIndex];
        string e = wank.Description;
        if (wank.IsWeapon)
        {
            e = $"{MaterialsDict[baller.Materials[0].index].Description}<br>{MaterialsDict[baller.Materials[1].index].Description}<br>{MaterialsDict[baller.Materials[2].index].Description}";
        }
        if (wank.IsCraftable)
        {
            e += $"<br><br>{MaterialsDict[baller.Materials[0].index].Description}";
        }

        e = e.Replace("<g>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[0])}>"); //good effect
        e = e.Replace("<b>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[1])}>"); //bad effect / axel
        e = e.Replace("<e>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[2])}>"); //enemy
        e = e.Replace("<o>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[3])}>"); //object/physical-thing/item/woman
        e = e.Replace("</>", $"</color>"); // end


        return e;
    }
    public void SaveAll()
    {
        foreach(var c in All_Containers)
        {
            if(c.Value != null && c.Value.SaveLoadData)
            {
                c.Value.SaveContents();
            }
        }
    }


    public string SaltName(string e)
    {
        e = e.Replace(" ", "");
        e = e.Replace(":", ";");
        return e;
    }
}


[System.Serializable]
public class GISItem
{
    /*
     * This class is for each item in the container, specifying individual data such as durability or amount
     * 
     * When adding new attributes for items, make sure to update the below functions:
     * GISItem.GISItem()
     * GISItem.GISItem(GISItem)
     * GISItem.Compare(GISItem)
     * GISContainer.LoadContents()
     */

    public string ItemIndex;
    public int Amount;
    public string CustomName;  
    public GISContainer Container;
    public List<GISMaterial> Materials = new List<GISMaterial>();
    public List<GISMaterial> Run_Materials = new List<GISMaterial>();
    public List<GISContainer> Interacted_Containers = new List<GISContainer>();
    public Dictionary<string, int> AmountOfItems = new Dictionary<string, int>();
    public GISItem()
    {
        setdefaultvals();
        CompileItems();
    }
    public GISItem(string base_type)
    {
        setdefaultvals();
        ItemIndex = base_type;
        SetDefaultsBasedOnIndex();
        CompileItems();
    }

    public void CompileItems()
    {
        AmountOfItems.Clear();
        foreach (var a in Materials)
        {
            if (AmountOfItems.ContainsKey(a.GetName()))
            {
                AmountOfItems[a.GetName()]++;
            }
            else
            {
                AmountOfItems.Add(a.GetName(), 1);
            }
        }
        foreach (var a in Run_Materials)
        {
            if (AmountOfItems.ContainsKey(a.GetName()))
            {
                AmountOfItems[a.GetName()]++;
            }
            else
            {
                AmountOfItems.Add(a.GetName(), 1);
            }
        }
    }
    public int ReadItemAmount(string weenor)
    {
        if (AmountOfItems.ContainsKey(weenor))
        {
            return AmountOfItems[weenor];
        }
        else
        {
            return 0;
        }
    }
    public void SetDefaultsBasedOnIndex()
    {
        if(GISLol.Instance.ItemsDict.TryGetValue(ItemIndex, out GISItem_Data doingus))
        {
            if (GISLol.Instance.MaterialsDict.ContainsKey(ItemIndex))
            {
                Materials.Add(new GISMaterial(ItemIndex));
                Amount = 1;
            }
            else if (doingus.IsWeapon)
            {
                Amount = 1;
            }
        }
    }

    public GISItem(GISItem sexnut)
    {
        setdefaultvals();
        Amount = sexnut.Amount;
        ItemIndex = sexnut.ItemIndex;
        Container = sexnut.Container;
        CustomName = sexnut.CustomName;
        Materials = new List<GISMaterial>(sexnut.Materials);
        Run_Materials = new List<GISMaterial>(sexnut.Run_Materials);
        CompileItems();
    }
    private void setdefaultvals()
    {
        Data = GetDefaultData();
        Amount = 0;
        ItemIndex = "Empty";
        Container = null;
        CustomName = "";
        Materials = new List<GISMaterial>();
        Run_Materials = new List<GISMaterial>();
    }
    public void Solidify()
    {
        AddConnection(Container);
        foreach (var c in Interacted_Containers)
        {
            if (c != null) c.SaveTempContents();
        }
    }

    public bool Compare(GISItem sexnut, bool usebase = false)
    {
        /* returns:
         * false - not the same
         * true - are the same
         */
        bool comp = ItemIndex == sexnut.ItemIndex;
        if( Materials.Count != sexnut.Materials.Count)
        {
            comp = false;
        }
        else
        {
            for (int i = 0; i < Materials.Count; i++)
            {
                if (Materials[i].index != sexnut.Materials[i].index) { comp = false; break; }
            }
        }

        if (!usebase && !comp)
        {
            //code to further compare goes here
        }
        return comp;
    }

    public void AddConnection(GISContainer gis)
    {
        if (!Interacted_Containers.Contains(gis))
        {
            Interacted_Containers.Add(gis);
        }
    }
    public void SetContainer(GISContainer gis)
    {
        if (gis == null || gis.CanRetainItems)
        {
            Container = gis;
        }
        //if(!gis.CanRetainItems) Tainted = true;
    }

    public Dictionary<string,string> Data = new Dictionary<string,string>();

    public Dictionary<string, string> GetDefaultData()
    {
        var e = new Dictionary<string, string>()
        {
            { "Index", "Empty" },
            { "Count", "0" },
            { "Name", "" },
            { "Mats", "" },
        };
        return e;
    }


    public string ItemToString()
    {
        string e = "";
        var def = GetDefaultData();

        Data["Index"] = ItemIndex.ToString();
        Data["Count"] = Amount.ToString();
        Data["Name"] = CustomName;


        List<string> mats = new List<string>();
        int x = 0;
        foreach (var mat in Materials)
        {
            mats.Add(mat.index);
            x++;
            if (x >= 3) break;
        }
        if(mats.Count > 0)
        Data["Mats"] = Converter.ListToString(mats, "(q]");



        Dictionary<string, string> bb = new Dictionary<string, string>();
        foreach(var dat in Data)
        {
            if(!def.ContainsKey(dat.Key) || dat.Value != def[dat.Key])
            {
                bb.Add(dat.Key, dat.Value);
            }
        }
        if (bb.ContainsKey("Count") && bb["Count"] == "1") bb.Remove("Count");
        if (bb.ContainsKey("Index"))
        {
            if (bb.ContainsKey("Mats") && mats[0] == bb["Index"])
            {
                bb.Remove("Mats");
            }
        }
        var gs = GISLol.Instance.nonowords;
        for(int i = 0; i < bb.Count; i++)
        {
            var sex = bb.ElementAt(i);
            bb[sex.Key] = Converter.EscapeString(bb[sex.Key], gs);
        }
        e = Converter.DictionaryToString(bb, "~|~", "~o~");

        return e;
    }
    public void StringToItem(string e)
    {
        setdefaultvals();
        var gs = GISLol.Instance.nonowords;
        var wanker = Converter.StringToDictionary(e, "~|~", "~o~");
        foreach(var ae in wanker)
        {
            if (Data.ContainsKey(ae.Key))
            {
                Data[ae.Key] = Converter.UnescapeString(ae.Value, gs);
            }
            else
            {
                Data.Add(ae.Key, Converter.UnescapeString(ae.Value, gs));
            }
        }

        ItemIndex = Data["Index"];
        SetDefaultsBasedOnIndex();
        if (wanker.ContainsKey("Count"))
        {
            Amount = int.Parse(Data["Count"]);
        }
        else
        {
            Amount = ItemIndex != "Empty" ? 1 : 0;
        }
        CustomName = Data["Name"];
        if (wanker.ContainsKey("Mats"))
        {
            var sex = Converter.StringToList(Data["Mats"], "(q]");
            Materials.Clear();
            foreach (var pp in sex)
            {
                if (pp != "") Materials.Add(new GISMaterial(pp));
            }
        }

        CompileItems();
    }
    public bool CanCraft()
    {
        bool t = ItemIndex != "Empty";
        return t;
    }
}

[Serializable]
public class GISItem_Data
{
    //this is what holds all of the base data for a general item of it's type.
    //EX: All "coal" items refer back to this for things like icon and name
    public string Name;
    public Sprite Sprite;
    public string Description;
    public int MaxAmount;
    public bool IsWeapon = false;
    public bool IsCraftable = false;
    public bool IsRune = false;
    public bool CanSpawn = true;
    public GISItem_Data()
    {
        Sprite = null;
        Name = "Void";
        Description = "Nothing";
        MaxAmount = 0;
    }
    public GISItem_Data(GISItem_Data data)
    {
        Sprite = data.Sprite;
        Name = data.Name;
        Description = data.Description;
        MaxAmount = data.MaxAmount;
    }
}
[Serializable]
public class GISMaterial
{
    public string index = "";
    public string itemindex = "";
    public GISMaterial(string index)
    {
        this.index = index;
    }
    public GISMaterial()
    {
    }

    public string GetName()
    {
        return index==""?itemindex:index;
    }
}
[Serializable]
public class GISMaterial_Data
{
    //this is what holds all of the base data for a general material
    public string Name = "Null";
    public string Description;
    public Color32 ColorMod = new Color32(255,255,255,255);
    public Sprite[] SwordParts;
    public Sprite[] BowParts;
    public Sprite[] SpearParts;
    public Sprite[] CrossbowParts;
    public Sprite[] DaggerParts;
    public Sprite[] SawbladeParts;
    public Sprite[] AxeParts;
    public Sprite[] BlowParts;
    public string fallthroughmaterial = "Rock";
    public bool ignorecolorforcumimg = true;
}