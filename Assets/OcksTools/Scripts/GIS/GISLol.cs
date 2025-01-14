using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
    public List<EntityEffect_Data> Effects = new List<EntityEffect_Data>();
    public List<Skill_Data> Skills = new List<Skill_Data>();
    public Dictionary<string, GISItem_Data> ItemsDict = new Dictionary<string, GISItem_Data>();
    public Dictionary<string, GISMaterial_Data> MaterialsDict = new Dictionary<string, GISMaterial_Data>();
    public Dictionary<string, EntityEffect_Data> EffectsDict = new Dictionary<string, EntityEffect_Data>();
    public Dictionary<string, Skill_Data> SkillsDict = new Dictionary<string, Skill_Data>();
    public List<string> AllWeaponNames = new List<string>();
    public List<string> AllCraftables = new List<string>();
    public List<string> AllRunes = new List<string>();
    private RectTransform ballingsexnut;
    private HoverRefHolder hovercummer;
    public List<Color32> attributecolors = new List<Color32>();
    public List<Color32> lorecharcolors = new List<Color32>();
    public List<string> nonowords = new List<string>();
    public Dictionary<GISItem, int> VaultItems = new Dictionary<GISItem, int>();

    public Dictionary<string,GISContainer> All_Containers = new Dictionary<string, GISContainer>();
    public Dictionary<string, string> LogbookDiscoveries = new Dictionary<string, string>();

    public List<QuestProgress> Quests = new List<QuestProgress>();

    public TextAsset DesciptionOverrides;
    public TextAsset GeneralDesciptionOverrides;
    public TextAsset EXTRADescriptionSussyBacons;
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

    public bool AddVaultItem(GISItem item, bool CanQuestCount = false)
    {
        if(CanQuestCount) Gamer.QuestProgressIncrease("Collect", item.ItemIndex);
        for(int i = 0; i < VaultItems.Count; i++)
        {
            var a = VaultItems.ElementAt(i);
            if (a.Key.Compare(item))
            {
                VaultItems[a.Key]++;
                return true;
            }
        }
        VaultItems.Add(item, 1);
        return false;
    }
    public bool GrantItem(GISItem item, bool CanQuestCount = false)
    {
        if(CanQuestCount) Gamer.QuestProgressIncrease("Collect", item.ItemIndex);

        var x = All_Containers["Inventory"].FindEmptySlot();
        if(x > -1)
        {
            All_Containers["Inventory"].slots[x].Held_Item = item;
        }
        else
        {
            AddVaultItem(item, false);
        }

        return false;
    }

    private void Awake()
    {

        foreach (var a in Skills)
        {
            SkillsDict.Add(a.Name, a);
            if (a.Name == "Empty") continue;
            var newitem = new GISItem_Data();
            newitem.Name = a.Name;
            newitem.NameOverride = a.Name;
            newitem.Sprite = a.Image;
            newitem.LogbookOverride = true;
            //newitem.IsRune = true;
            newitem.IsSkill = true;
            Items.Add(newitem);
        }


        foreach (var a in Items)
        {
            ItemsDict.Add(a.Name, a);
            if(a.IsWeapon) AllWeaponNames.Add(a.Name);
            if(a.IsCraftable) AllCraftables.Add(a.Name);
            if(a.IsRune) AllRunes.Add(a.Name);
        }
        AllWeaponNames.Remove("Bow");
        foreach (var a in Materials)
        {
            MaterialsDict.Add(a.Name, a);
        }

        foreach (var a in Effects)
        {
            EffectsDict.Add(a.Name, a);
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
        e = Converter.StringToDictionary(EXTRADescriptionSussyBacons.text.Replace("\r", ""), "\n", ":: ");
        foreach (var we in e)
        {
            if (ItemsDict.ContainsKey(we.Key))
            {
                ItemsDict[we.Key].EXTRADescription = we.Value;
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
            Mouse_Held_Item = new GISItem("Morkite");
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
        if (InputManager.IsKeyDown(KeyCode.K, "menu"))
        {
            Mouse_Held_Item = new GISItem("Zebrium");
        }
        if (InputManager.IsKeyDown(KeyCode.J, "menu"))
        {
            Mouse_Held_Item = new GISItem("Shungite");
        }
        if (InputManager.IsKeyDown(KeyCode.H, "menu"))
        {
            Mouse_Held_Item = new GISItem("Branch");
        }
        if (InputManager.IsKeyDown(KeyCode.O, "menu"))
        {
            Mouse_Held_Item = new GISItem("Zebrium");
            Mouse_Held_Item.CustomName = $"{UnityEngine.Random.Range(-69,10101010101)}";
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

    public string GetDescription(GISItem baller, bool EXTRA = false)
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
        if (EXTRA)
        {
            e += $"<br><br>{ItemsDict[baller.ItemIndex].EXTRADescription}";
        }
        return ColorText(e);
    }

    public string ColorText(string e)
    {
        e = e.Replace("<g>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[0])}>"); //good effect
        e = e.Replace("<b>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[1])}>"); //bad effect / axel
        e = e.Replace("<e>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[2])}>"); //enemy
        e = e.Replace("<o>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[3])}>"); //object/physical-thing/item/woman
        e = e.Replace("<c>", $"<color=#{ColorUtility.ToHtmlStringRGBA(attributecolors[4])}>"); //curruption
        e = e.Replace("</>", $"</color>"); // end
        e = e.Replace("<bold>", $"<b>"); //bold override lol

        e = e.Replace("H:", $"<color=#{ColorUtility.ToHtmlStringRGBA(lorecharcolors[0])}>H:");
        e = e.Replace("F:", $"<color=#{ColorUtility.ToHtmlStringRGBA(lorecharcolors[1])}>F:");
        e = e.Replace("K:", $"<color=#{ColorUtility.ToHtmlStringRGBA(lorecharcolors[2])}>K:");
        e = e.Replace("J:", $"<color=#{ColorUtility.ToHtmlStringRGBA(lorecharcolors[3])}>J:");
        e = e.Replace("ADA:", $"<color=#{ColorUtility.ToHtmlStringRGBA(lorecharcolors[4])}>ADA:");
        e = e.Replace("God:", $"<color=#{ColorUtility.ToHtmlStringRGBA(lorecharcolors[5])}>God:");
        e = e.Replace("G:", $"<color=#{ColorUtility.ToHtmlStringRGBA(lorecharcolors[6])}>G:");
        e = e.Replace("<ch_G>", $"<color=#{ColorUtility.ToHtmlStringRGBA(lorecharcolors[6])}>");

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
    public float Luck = -1;
    public float BlockChance = 0;
    public PlayerController Player;
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
            else if (doingus.IsRune)
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
        Luck = sexnut.Luck;
        CompileItems();
        Player = sexnut.Player;
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
    public int RollLuck(float arr, bool ignoreluck = false)
    {
        if (arr > 0)
        {
            int tt2 = Mathf.FloorToInt(arr);
            var ff2 = UnityEngine.Random.Range(0f, 1f);
            var wank = (arr % 1);
            if (ff2 <= wank)
            {
                tt2++;
            }
            else if (Luck > 0 && !ignoreluck)
            {
                int times = RollLuck(Luck, true);
                if (times > 0)
                {
                    for(int i = 0; i < times; i++)
                    {
                        ff2 = UnityEngine.Random.Range(0f, 1f);
                        if (ff2 <= wank)
                        {
                            tt2++;
                            break;
                        }
                    }
                }
            }
            if (tt2 > 0)
            {
                return tt2;
            }
        }
        return 0;
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

        if (sexnut.CustomName != CustomName) comp = false;
        if (!usebase && comp)
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
            { "RunMats", "" },
        };
        return e;
    }


    public string ItemToString(bool perrun = false)
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
        if (perrun)
        {
            var mats2 = new List<string>();
            foreach (var mat in Run_Materials)
            {
                mats2.Add(mat.index == "" ? mat.itemindex : mat.index);
            }
            if (mats2.Count > 0)
                Data["RunMats"] = Converter.ListToString(mats2, "(q]");
        }



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
        if (wanker.ContainsKey("RunMats"))
        {
            var sex = Converter.StringToList(Data["RunMats"], "(q]");
            Run_Materials.Clear();
            foreach (var pp in sex)
            {
                if (pp != "")
                {
                    var weenor = GISLol.Instance.ItemsDict[pp];
                    if (weenor.IsCraftable)
                    {
                        Run_Materials.Add(new GISMaterial(pp));
                    }
                    else
                    {
                        var w = new GISMaterial();
                        w.itemindex = pp;
                        Run_Materials.Add(w);
                    }
                }
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
    public string NameOverride;
    public Sprite Sprite;
    public string Description;
    public string EXTRADescription;
    public int MaxAmount;
    public bool IsWeapon = false;
    public bool IsCraftable = false;
    public bool IsRune = false;
    public bool IsSkill = false;
    public bool CanSpawn = true;
    public bool LogbookOverride = false;
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
    public string GetDisplayName()
    {
        return NameOverride == "" ? Name : NameOverride;
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
    public Color32 OverlayColorMod = new Color32(255,255,255,255);
    public Sprite[] SwordParts;
    public Sprite[] BowParts;
    public Sprite[] SpearParts;
    public Sprite[] CrossbowParts;
    public Sprite[] DaggerParts;
    public Sprite[] SawbladeParts;
    public Sprite[] AxeParts;
    public Sprite[] BlowParts;
    public Sprite[] TDaggerParts;
    public string fallthroughmaterial = "Rock";
    public string fallthroughmaterialmian = "";
    public bool IsOverlay = false;
    public bool ignorecolorforcumimg = true;
}
[Serializable]
public class EntityEffect_Data
{
    public string Name = "Null";
    public Sprite Image;
}

public class QuestProgress
{
    public Dictionary<string, string> Data;

    private void SetDefaults()
    {
        Data = GetDefaults();
    }
    private Dictionary<string,string> GetDefaults()
    {
        return new Dictionary<string, string>()
        {
            {"Name",""},
            {"Progress","0"},
            {"Target_Type","Item"},
            {"Target_Data",""},
            {"Target_Amount",""},
            {"Reward_Type","Item"},
            {"Reward_Data",""},
            {"Reward_Amount",""},
            {"Completed","False"},
            {"Extra_Data",""},
        };
    }
    public QuestProgress(string e)
    {
        StringToData(e);
    }
    public QuestProgress()
    {
        SetDefaults();
    }
    public string DataToString()
    {
        Dictionary<string, string> w = new Dictionary<string, string>();
        Dictionary<string, string> w2 = GetDefaults();
        foreach(var a in Data)
        {
            if(!w2.ContainsKey(a.Key) || w2[a.Key] != a.Value)
            {
                w.Add(a.Key, a.Value);
            }
        }
        return Converter.DictionaryToString(w, "<x>", "<y>");
    }
    public void StringToData(string e)
    {
        SetDefaults();
        var aa = Converter.StringToDictionary(e, "<x>", "<y>");
        foreach(var a in aa)
        {
            if (Data.ContainsKey(a.Key))
            {
                Data[a.Key] = a.Value;
            }
            else
            {
                Data.Add(a.Key, a.Value);
            }
        }
    }

    public void CheckComplete()
    {
        if (Data["Completed"] == "True") return;
        if (int.Parse(Data["Progress"]) >= int.Parse(Data["Target_Amount"]))
        {
            Data["Completed"] = "True";
            switch (Data["Reward_Type"])
            {
                default:
                    var x = int.Parse(Data["Reward_Amount"]);
                    for(int i = 0; i < x; i++)
                    {
                        var item = new GISItem(Data["Reward_Data"]);
                        GISLol.Instance.GrantItem(item, true);
                    }
                    break;
            }
        }
    }
}


[Serializable]
public class Skill_Data
{
    public string Name = "Null";
    public Sprite Image;
    public float Cooldown = 5f;
    public int MaxStacks = 1;
    public bool CanHold = false;
}

[Serializable]
public class Skill
{
    public string Name;
    public float Timer = 0;
    public float MaxCooldown = 0;
    public int Stacks = 0;
    public float usecool = 0;
    public bool IsHeld = false;
    public Skill()
    {

    }
    public Skill(string name)
    {
        Name = name;
    }

    public string SkillToString()
    {
        return Name;
    }
    public void StringToSkill(string a)
    {
        Name = a;
    }

}