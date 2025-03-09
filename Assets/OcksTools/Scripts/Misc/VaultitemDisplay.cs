using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultitemDisplay : MonoBehaviour
{
    public GISItem item;
    public GISDisplay displaything;
    public void UpdateDisplay()
    {
        displaything.item = item;
        displaything.UpdateDisplay();
        displaything.amnt.text = $"x{Converter.NumToRead(GISLol.Instance.VaultItems[item].ToString())}";
    }
    public void Clickity()
    {
        bool ctrl = InputManager.IsKey("item_quick");
        if (ctrl)
        {
            GISLol.Instance.GrantItem(new GISItem(item));
            GISLol.Instance.VaultItems[item]--;
            if (GISLol.Instance.VaultItems[item] <= 0)
            {
                GISLol.Instance.VaultItems.Remove(item);
                Gamer.Instance.LoadVaultPage(Gamer.Instance.currentvault);
            }
            else
            {
                UpdateDisplay();
            }
        }
        else
        {
            if (GISLol.Instance.Mouse_Held_Item.ItemIndex == "Empty")
            {
                GISLol.Instance.Mouse_Held_Item = new GISItem(item);
                GISLol.Instance.VaultItems[item]--;
                if (GISLol.Instance.VaultItems[item] <= 0)
                {
                    GISLol.Instance.VaultItems.Remove(item);
                    Gamer.Instance.LoadVaultPage(Gamer.Instance.currentvault);
                }
                else
                {
                    UpdateDisplay();
                }
                GISLol.Instance.Mouse_Displayer.UpdateDisplay();
            }
            else if (item.Compare(GISLol.Instance.Mouse_Held_Item))
            {
                var p = GISLol.Instance.Mouse_Held_Item;
                GISLol.Instance.Mouse_Held_Item = new GISItem();
                p.Solidify();
                displaything.item = item;
                displaything.UpdateDisplay();
                displaything.amnt.text = $"x{++GISLol.Instance.VaultItems[item]}";
            }
        }
    }

}
