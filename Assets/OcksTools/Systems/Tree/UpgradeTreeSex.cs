using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeTreeSex : MonoBehaviour
{
    public string DisplayItem = "";
    public string Title = "";
    public string Desc = "";
    [HideInInspector]
    public string DescReal = "";
    public bool IsWeaponDisplay = false;
    public bool UseItemDesc = false;
    public List<ItemCost> itemCosts = new List<ItemCost>();
    public Color multcol = Color.white;
    public GISDisplay wawa;
    public TreeNode treeeee;
    public bool CanPurcahse = false;
    public static int GetTotalAmountOfItem(GISItem a)
    {
        int totalAmount = 0;

        foreach(var b in GISLol.Instance.VaultItems)
        {
            if(b.Key.Compare(a, true))
            {
                totalAmount += b.Value;
                break;
            }
        }

        totalAmount += GISLol.Instance.All_Containers["Inventory"].AmountOfItem(a, true);
        return totalAmount;
    }
    public bool MeetsRequirements()
    {
        foreach(var a in itemCosts)
        {
            var c = new GISItem(a.ItemType);
            if(GetTotalAmountOfItem(c) < a.Amount) return false;
        }
        return true;
    }
    string ee = "sex";
    private void OnEnable()
    {
        if (Time.time <= 0.1f) return;
        SetRealDesc();
        if(ree != null)
        {
            Destroy(ree.gameObject);
            ree = null;
        }
        if (ee == DisplayItem) return;
        ee = DisplayItem;
        wawa.item = new GISItem(DisplayItem);
        if (IsWeaponDisplay)
        {
            wawa.item.Materials = new List<GISMaterial> { new GISMaterial("Angelic Ingot"), new GISMaterial("Angelic Ingot"), new GISMaterial("Angelic Ingot") };
        }
        wawa.UpdateDisplay();
        foreach(var a in wawa.displays)
        {
            a.color = multcol;
        }
    }
    private void Update()
    {
        if(ree != null) ree.Particicic.transform.localScale = Vector3.one * UtreeGaming.Instance.scalem;
    }
    public void SetRealDesc()
    {
        DescReal = Desc;
        if (IsWeaponDisplay || UseItemDesc)
        {
            DescReal = GISLol.Instance.ItemsDict[DisplayItem].Description;
        }
        CanPurcahse = true;
        if (treeeee.ViewState == TreeNode.ViewStates.Obtained)
        {
            DescReal += "<br><br><c>Purchased</>";
        }
        else if(itemCosts.Count > 0)
        {
            DescReal += "<br><br>Cost:";
            foreach (var a in itemCosts)
            {
                var x = GetTotalAmountOfItem(new GISItem(a.ItemType));
                bool wee = x >= a.Amount;
                if (wee) DescReal += "<g>";
                else DescReal += "<b>";
                DescReal += $"<br>- {x}/{a.Amount} {GISLol.Instance.ItemsDict[a.ItemType].GetDisplayName()}";
                DescReal += "</>";
                if(!wee) CanPurcahse  = false;
            }
        }
        DescReal = GISLol.Instance.ColorText(DescReal);
        treeeee.UpdateDisplay();
    }
    partShitBall ree;
    public void Clity()
    {
        if (!Gamer.Instance.DevNoTreeCost)
        {
            if (!MeetsRequirements()) return;
            foreach (var a in itemCosts)
            {
                SpendItem(new GISItem(a.ItemType), a.Amount);
            }
        }

        ree = Instantiate(Gamer.Instance.ParticleSpawns[40], transform.position, Quaternion.identity, transform).GetComponent<partShitBall>();
        ree.Particicic.transform.localScale = Vector3.one * UtreeGaming.Instance.scalem;
        treeeee.Click();
        Gamer.Instance.UpdateLobbyStuff();
        foreach(var a in TreeHandler.Nodes)
        {
            a.Value.prntr.SetRealDesc();
        }
    }


    public static void SpendItem(GISItem a, int Amount)
    {
        System.Action<string> Spender = (name) =>
        {
            if (Amount <= 0) return;
            var c = GISLol.Instance.All_Containers[name];
            int times = Mathf.Clamp(c.AmountOfItem(a), 0, Amount);
            Amount -= times;
            for (int i = 0; i < times; i++)
            {
                int x = 0;
                foreach (var b in c.slots)
                {
                    if (b.Held_Item.Compare(a, true)) break;
                    x++;
                }
                c.slots[x].Held_Item = new GISItem();
            }
            if(times > 0)
            {
                c.SaveTempContents();
            }
        };
        Spender("Inventory");


        if(Amount > 0)
        {
            for(int i = 0; i < GISLol.Instance.VaultItems.Count; i++)
            {
                var aa = GISLol.Instance.VaultItems.ElementAt(i);
                if(aa.Key.Compare(a, true))
                {
                    int x = Mathf.Clamp(aa.Value, 0, Amount);
                    GISLol.Instance.VaultItems[aa.Key] -= x;
                    Amount -= x;
                    if(GISLol.Instance.VaultItems[aa.Key] <= 0)
                    {
                        GISLol.Instance.VaultItems.Remove(aa.Key);
                    }
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class ItemCost
{
    public int Amount;
    public string ItemType;
}

