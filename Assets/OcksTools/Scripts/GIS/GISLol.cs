using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GISLol : MonoBehaviour
{
    public bool UseLanguageFile = true;
    public GISItem Mouse_Held_Item;
    public GISDisplay Mouse_Displayer;
    public GameObject MouseFollower;
    private static GISLol instance;
    public List<GISItem_Data> Items = new List<GISItem_Data>();
    public List<GISMaterial_Data> Materials = new List<GISMaterial_Data>();


    public Dictionary<string,GISContainer> All_Containers = new Dictionary<string, GISContainer>();


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
        if (Instance == null) instance = this;
        SaveSystem.SaveAllData += SaveAll;
        Mouse_Held_Item = new GISItem();
    }

    private void Start()
    {
        MouseFollower.SetActive(true);

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

    private void Update()
    {
        Mouse_Displayer.item = Mouse_Held_Item;
        var za = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        za.z = 0;
        MouseFollower.transform.position = za;

        if (InputManager.IsKeyDown(KeyCode.X))
        {
            Mouse_Held_Item = new GISItem(1);
            Mouse_Held_Item.ItemType = "Craftable";
            Mouse_Held_Item.Materials.Add(new GISMaterial(0));
            Mouse_Held_Item.Amount = 1;
        }
        if (InputManager.IsKeyDown(KeyCode.C))
        {
            Mouse_Held_Item = new GISItem(2);
            Mouse_Held_Item.ItemType = "Craftable";
            Mouse_Held_Item.Materials.Add(new GISMaterial(1));
            Mouse_Held_Item.Amount = 1;
        }
        if (InputManager.IsKeyDown(KeyCode.V))
        {
            Mouse_Held_Item = new GISItem(9);
            Mouse_Held_Item.ItemType = "Craftable";
            Mouse_Held_Item.Materials.Add(new GISMaterial(2));
            Mouse_Held_Item.Amount = 1;
        }
        if (InputManager.IsKeyDown(KeyCode.B))
        {
            Mouse_Held_Item = new GISItem(10);
            Mouse_Held_Item.ItemType = "Craftable";
            Mouse_Held_Item.Materials.Add(new GISMaterial(3));
            Mouse_Held_Item.Amount = 1;
        }
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

    public int ItemIndex;
    public string ItemType = "";
    public int Amount;
    public GISContainer Container;
    public List<GISMaterial> Materials = new List<GISMaterial>();
    public List<GISContainer> Interacted_Containers = new List<GISContainer>();

    public GISItem()
    {
        setdefaultvals();
    }
    public GISItem(int base_type)
    {
        setdefaultvals();
        ItemIndex = base_type;
    }
    public GISItem(GISItem sexnut)
    {
        setdefaultvals();
        Amount = sexnut.Amount;
        ItemIndex = sexnut.ItemIndex;
        Container = sexnut.Container;
        Materials = new List<GISMaterial>(sexnut.Materials);
        ItemType = sexnut.ItemType;
    }
    private void setdefaultvals()
    {
        Amount = 0;
        ItemIndex = 0;
        Container = null;
        ItemType = "";
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
        if (ItemType != sexnut.ItemType) comp = false;
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

    public string ItemToString()
    {
        string e = "";
        var s = "~|~";
        e += ItemIndex + s + Amount + s + ItemType + s;
        int i = 0;
        foreach (var mat in Materials)
        {
            if (i > 0) e += "(q]";
            i++;
            e += mat.index;
        }

        return e;
    }
    public void StringToItem(string e)
    {
        var s = RandomFunctions.Instance.StringToList(e, "~|~");
        ItemIndex = int.Parse(s[0]);
        Amount = int.Parse(s[1]);
        ItemType = s[2];
        var sex = RandomFunctions.Instance.StringToList(s[3], "(q]");
        Materials.Clear();
        foreach(var pp in sex)
        {
            if(pp != "") Materials.Add(new GISMaterial(int.Parse(pp)));
        }

    }
    public bool CanCraft()
    {
        bool t = ItemIndex != 0;
        if (ItemType != "Craftable") t = false;
        return t;
    }
}

[Serializable]
public class GISItem_Data
{
    //this is what holds all of the base data for a general item of it's type.
    //EX: All "coal" items refer back to this for things like icon and name
    public Sprite Sprite;
    public string Name;
    public string Description;
    public int MaxAmount;
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
    public int index;
    public GISMaterial(int index)
    {
        this.index = index;
    }
}
[Serializable]
public class GISMaterial_Data
{
    //this is what holds all of the base data for a general material
    public Color32 ColorMod = new Color32(255,255,255,255);
    public string Name = "Null";
    public Sprite[] SwordParts;
    public Sprite[] BowParts;
    public Sprite[] SpearParts;
    public Sprite[] CrossbowParts;
    public Sprite[] DaggerParts;
    public Sprite[] SawbladeParts;
}