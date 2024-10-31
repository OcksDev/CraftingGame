using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour
{
    public PlayerController controller;
    public SpriteRenderer[] displays = new SpriteRenderer[3];
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GISLol.Instance.All_Containers.ContainsKey("Equips"))
        {
            UpdateDisplay();
        }
    }

    public void UpdateDisplay()
    {
        //var WeaponItem = GISLol.Instance.All_Containers["Equips"].slots[controller.selecteditem].Held_Item;
        var WeaponItem = controller.mainweapon;
        var c = new Color32(255, 255, 255, 255);
        displays[0].color = c;
        displays[1].color = c;
        displays[2].color = c;
        if (WeaponItem != null && WeaponItem.ItemIndex != "Empty")
        {
            var d = GISDisplay.GetSprites(WeaponItem);
            displays[0].sprite = d.sprites[0];
            displays[1].sprite = d.sprites[1];
            displays[2].sprite = d.sprites[2];
            displays[3].sprite = d.sprites[3];
            displays[4].sprite = d.sprites[4];
            displays[5].sprite = d.sprites[5];
            displays[0].color = d.colormods[0];
            displays[1].color = d.colormods[1];
            displays[2].color = d.colormods[2];
            displays[3].color = d.colormods[3];
            displays[4].color = d.colormods[4];
            displays[5].color = d.colormods[5];
        }
        else
        {
            var e = GISLol.Instance.Items[0].Sprite;
            displays[0].sprite = e;
            displays[1].sprite = e;
            displays[2].sprite = e;
            displays[3].sprite = e;
            displays[4].sprite = e;
            displays[5].sprite = e;
        }
    }
}
