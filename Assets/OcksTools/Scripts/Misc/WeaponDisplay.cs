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
            //var WeaponItem = GISLol.Instance.All_Containers["Equips"].slots[controller.selecteditem].Held_Item;
            var WeaponItem = controller.mainweapon;
            if (WeaponItem != null && WeaponItem.ItemIndex != 0)
            {
                displays[0].sprite = GISDisplay.GetSprite(WeaponItem, 0);
                displays[1].sprite = GISDisplay.GetSprite(WeaponItem, 1);
                displays[2].sprite = GISDisplay.GetSprite(WeaponItem, 2);
            }
            else
            {
                var e = GISLol.Instance.Items[0].Sprite;
                displays[0].sprite = e;
                displays[1].sprite = e;
                displays[2].sprite = e;
            }
        }
    }
}
