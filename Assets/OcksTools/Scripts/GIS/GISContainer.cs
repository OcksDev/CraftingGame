using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GISContainer : MonoBehaviour
{
    public string Name = "RandomThingIDK";
    public bool SaveLoadData = true;
    [Tooltip("Can cause item destruction when trying to return items to a container with no open slots")]
    public bool CanRetainItems = true;
    public bool CanShiftClickItems = true;
    [Tooltip("Uses the GISItems however doesn't use slots")]
    public bool IsAbstract = false;
    public bool GenerateRandomItems = false;
    public bool GenerateSlotObjects = true;
    public int GenerateXSlots = 20;
    public GameObject SlotPrefab;
    public List<GISSlot> slots= new List<GISSlot>();
    [HideInInspector]
    public bool LoadedData = false;

    public List<GISItem> saved_items = new List<GISItem>();
    // Start is called before the first frame update
    public void Start()
    {
        if (LoadedData) return;
        var myass = GetComponentsInChildren<GISSlot>();
        GISLol.Instance.All_Containers.Add(Name, this);
        if (!IsAbstract)
        {
            if (GenerateSlotObjects)
            {
                foreach (var pp in myass)
                {
                    Destroy(pp.gameObject);
                }

                for (int i = 0; i < GenerateXSlots; i++)
                {
                    var h = Instantiate(SlotPrefab, transform.position, transform.rotation, transform);
                    var h2 = h.GetComponent<GISSlot>();
                    h2.Conte = this;
                    h2.Held_Item = new GISItem();
                    slots.Add(h2);
                }
            }
            else
            {
                foreach (var pp in myass)
                {
                    pp.Conte = this;
                    slots.Add(pp);
                }
            }
        }
        else
        {
            slots.Clear();
        }



        if (SaveLoadData)
        {
            if(gameObject.active)StartCoroutine(WaitForSaveSystem());
        }
        else if(GenerateRandomItems)
        {
            //this is some debug shit for creating a bunch of randomly generated new containers.
            foreach(var s in slots)
            {
                s.Held_Item = new GISItem(UnityEngine.Random.Range(0, 5));
                s.Held_Item.Amount = 69;
                s.Held_Item.Container = this;
                if (s.Held_Item.ItemIndex == 0)
                {
                    s.Held_Item.Amount = 0;
                }
            }
        }
    }
    public IEnumerator WaitForSaveSystem()
    {
        yield return new WaitUntil(() =>{return SaveSystem.Instance.LoadedData; });
        LoadContents();
    }
    public void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var x = slots[FindEmptySlot()];
            x.Held_Item = new GISItem();
            x.Held_Item.Amount = 50;
            x.Held_Item.ItemIndex = UnityEngine.Random.Range(1, 5);
            x.Held_Item.Container = this;
            x.Held_Item.Solidify();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveContents();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            LoadContents();
        }*/
    }

    private void OnApplicationQuit()
    {
        SaveContents();
    }

    public bool SaveTempContents()
    {
        if(CanRetainItems && GISLol.Instance.Mouse_Held_Item.ItemIndex == 0)
        {
            saved_items.Clear();
            foreach (var h in slots)
            {
                saved_items.Add(new GISItem(h.Held_Item));
            }
            return true;
        }
        return false;
    }
    public void LoadTempContents()
    {
        if (CanRetainItems)
        {
            int i = 0;
            if (IsAbstract)
            {
                slots.Clear();
                foreach (var h in saved_items)
                {
                    var pp = new GISSlot(this);
                    pp.Held_Item = new GISItem(h);
                    slots.Add(pp);
                }
            }
            else
            {
                foreach (var h in saved_items)
                {
                    slots[i].Held_Item = new GISItem(h);
                    i++;
                }
            }
        }
        else
        {
            
            foreach (var h in slots)
            {
                h.Held_Item = new GISItem();
            }
        }
        if (GISLol.Instance.Mouse_Held_Item.Container == this) GISLol.Instance.Mouse_Held_Item = new GISItem();
    }
    public int FindEmptySlot()
    {
        int i = -1;
        int k = 0;
        foreach (var j in slots)
        {
            if (j.Held_Item.ItemIndex == 0)
            {
                i = k;
                break;
            }
            k++;
        }

        return i;
    }

    private string ContainerPath = "";
    public void SaveContents()
    {
        if (SaveLoadData)
        {
            if(!IsAbstract) GISLol.Instance.LoadTempForAll();
            List<string> a = new List<string>();

            foreach (var p in slots)
            {
                a.Add(p.Held_Item.ItemToString());
            }

            SaveSystem.Instance.SetString("cnt_" + Name, ListToString(a, "+=+"));
        }
    }
    

    public void LoadContents()
    {
        Debug.Log("Loading " + Name);
        LoadedData = true;
        if (SaveLoadData)
        {
            if (IsAbstract)
            {
                slots.Clear();
            }
            List<string> a = new List<string>();
            List<string> b = new List<string>();
            var gg = SaveSystem.Instance.GetString("cnt_" + Name, "fuck");
            if (gg != "fuck")
            {
                Debug.Log("Found Data " + Name);
                a = StringToList(gg, "+=+");
                int i = 0;
                GISItem ghj = null;
                foreach (var st in a)
                {
                    if (st != "")
                    {
                        ghj = new GISItem();
                        ghj.StringToItem(st);
                        ghj.Container = this;

                        if (IsAbstract)
                        {
                            AbstractAdd(ghj);
                        }
                        else
                        {
                            slots[i].Held_Item = ghj;
                        }
                        i++;
                        if (!IsAbstract && i >= slots.Count) break;
                    }
                }

            }
            else
            {
                Debug.LogWarning("WHAT, FAILED TO LOAD CONTAINER DATA FOR " + Name + "!!!!!!!!!!!!!!");
            }
            SaveTempContents();
        }
        if(Name == "Equips")
        {
            if(PlayerController.Instance != null)PlayerController.Instance.SetData();
        }
    }

    public int AmountOfItem(GISItem item, bool usebase = false)
    {
        int amnt = 0;

        foreach(var st in slots)
        {
            if(st.Held_Item.Compare(item, usebase))
            {
                amnt += st.Held_Item.Amount;
            }
        }

        return amnt;
    }

    public bool ReturnItem(GISItem Held_Item)
    {
        bool left = true;
        var a = new GISItem(Held_Item);
        int i = left ? 0 : slots.Count - 1;
        bool found = false;
        foreach (var item in slots)
        {
            var x = slots[i].Held_Item;
            if (x.Compare(a))
            {
                int max = GISLol.Instance.Items[a.ItemIndex].MaxAmount;
                int t = x.Amount + a.Amount;
                if (max <= 0)
                {
                    x.Amount = t;
                    found = true;
                    break;
                }
                else
                {
                    int z = Mathf.Clamp(t, 0, max);
                    x.Amount = z;
                    a.Amount = t - z;
                    if (a.Amount == 0)
                    {
                        found = true;
                        break;
                    }
                }
            }
            else if (x.ItemIndex == 0)
            {
                found = true;
                slots[i].Held_Item = a;
                break;
            }
            if(!found)
            {
                i += left ? 1 : -1;
            }
        }
        if (found)
        {
            slots[i].Held_Item.AddConnection(this);
            slots[i].Held_Item.Solidify();
        }
        return found;
    }
    //any method prefixed with "Abstract" should only be used if the container is abstract.
    public void AbstractAdd(GISItem item)
    {
        var f = GISLol.Instance.Items[item.ItemIndex].MaxAmount;
        foreach (var s in slots)
        {
            if (item.Compare(s.Held_Item))
            {
                int z = s.Held_Item.Amount;
                z += item.Amount;
                if (f > 0)
                {
                    s.Held_Item.Amount = Math.Clamp(z, 0, f);
                    if (z > f)
                    {
                        z -= f;
                    }
                    else
                    {
                        z = 0;
                        item.Amount = z;
                        break;
                    }
                    item.Amount = z;
                }
                else
                {
                    s.Held_Item.Amount = z;
                    z = 0;
                    item.Amount = z;
                    break;
                }
            }
        }
        if(item.Amount > 0)
        {
            if (f > 0)
            {
                while(item.Amount > f)
                {
                    Debug.Log("sex: " +item.Amount);
                    var ns = new GISSlot(this);
                    var pp = new GISItem(item);
                    pp.Amount = f;
                    ns.Held_Item = pp;
                    slots.Add(ns);
                    item.Amount -= f;
                }
                if(item.Amount > 0)
                {
                    var ns = new GISSlot(this);
                    ns.Held_Item = item;
                    slots.Add(ns);
                }
            }
            else
            {
                var ns = new GISSlot(this);
                ns.Held_Item = item;
                slots.Add(ns);
            }
        }
    }
    public int IndexOf(GISItem item, bool truecompare = false)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (truecompare ? item.Compare(slots[i].Held_Item): slots[i].Held_Item == item)
            {
                return i;
            }
        }
        return -1;
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
}
