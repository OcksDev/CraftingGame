using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeTreeSex : MonoBehaviour
{
    public List<ItemCost> itemCosts = new List<ItemCost>();
    public static int GetTotalAmountOfItem(GISItem a)
    {
        int totalAmount = GISLol.Instance.All_Containers["Vault"].AmountOfItem(a, true);
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
                    if(GISLol.Instance.VaultItems[aa.Key] <= 0)
                    {
                        GISLol.Instance.VaultItems.Remove(aa.Key);
                    }
                    break;
                }
            }
        }
        if(Amount > 0)
        {
            Debug.Log("HOW?");
        }
    }
}

[System.Serializable]
public class ItemCost
{
    public int Amount;
    public string ItemType;
}

