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
        displaything.amnt.text = $"x{GISLol.Instance.VaultItems[item]}";
    }
    public void Clickity()
    {
        if(GISLol.Instance.Mouse_Held_Item.ItemIndex == "Empty")
        {
            GISLol.Instance.Mouse_Held_Item = new GISItem(item);
            GISLol.Instance.VaultItems[item]--;
            if(GISLol.Instance.VaultItems[item] <= 0)
            {
                GISLol.Instance.VaultItems.Remove(item);
            }
            Gamer.Instance.LoadVaultPage(Gamer.Instance.currentvault);
            GISLol.Instance.Mouse_Displayer.UpdateDisplay();
        }
    }

}
