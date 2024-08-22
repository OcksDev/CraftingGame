using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GISDisplay : MonoBehaviour
{
    public GISItem item;
    public Image[] displays;
    public TextMeshProUGUI amnt;

    private void Awake()
    {
        if (item == null) item = new GISItem();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        var g = GISLol.Instance.ItemsDict[item.ItemIndex];
        amnt.text = item.Amount > 0 && g.MaxAmount != 1?"x" + item.Amount:"";
        var e = GISLol.Instance.Items[0].Sprite;
        var c = new Color32(255, 255, 255, 255);
        displays[0].color = c;
        displays[1].color = c;
        displays[2].color = c;
        if (item.ItemType == "Craftable")
        {
            displays[0].sprite = g.Sprite;
            displays[1].sprite = e;
            displays[2].sprite = e;
        }
        else if (item.ItemType == "Made")
        {
            var b = GetSprites(item);
            displays[0].sprite = b.sprites[0];
            displays[1].sprite = b.sprites[1];
            displays[2].sprite = b.sprites[2];
            displays[0].color = b.colormods[0];
            displays[1].color = b.colormods[1];
            displays[2].color = b.colormods[2];
        }
        else
        {
            displays[0].sprite = e;
            displays[1].sprite = e;
            displays[2].sprite = e;
        }
    }


    public static SpriteReturn GetSprites(GISItem ITEM)
    {
        List<Sprite> boner = new List<Sprite>();
        List<Color32> boner2 = new List<Color32>();
        var ccc = new Color32(255,255, 255,255);
        for (int index = 0; index < 3; index++)
        {
            var e = GISLol.Instance.MaterialsDict[ITEM.Materials[index].index];
            var fall = e.fallthroughmaterial;
            Sprite baller = null;
            Color32 beans = e.ColorMod;
            Sprite[] mysprites=null;
            Sprite[] defaultsprites=null;
            switch (ITEM.ItemIndex)
            {
                default:
                    mysprites = e.SwordParts;
                    defaultsprites = GISLol.Instance.MaterialsDict[fall].SwordParts;
                    break;
                case "Bow":
                    mysprites = e.BowParts;
                    defaultsprites = GISLol.Instance.MaterialsDict[fall].BowParts;
                    break;
                case "Spear":
                    mysprites = e.SpearParts;
                    defaultsprites = GISLol.Instance.MaterialsDict[fall].SpearParts;
                    break;
                case "Crossbow":
                    mysprites = e.CrossbowParts;
                    defaultsprites = GISLol.Instance.MaterialsDict[fall].CrossbowParts;
                    break;
                case "Shuriken":
                    mysprites = e.DaggerParts;
                    defaultsprites = GISLol.Instance.MaterialsDict[fall].DaggerParts;
                    break;
                case "Boomerang":
                    mysprites = e.SawbladeParts;
                    defaultsprites = GISLol.Instance.MaterialsDict[fall].SawbladeParts;
                    break;
            }


            if (index >= mysprites.Length || mysprites[index] == null)
            {
                baller = defaultsprites[index];
            }
            else
            {
                if (e.ignorecolorforcumimg) beans = ccc;
                baller = mysprites[index];
            }

            boner.Add(baller);
            boner2.Add(beans);
        }

        var a = new SpriteReturn();
        a.sprites = boner;
        a.colormods = boner2;
        return a;
    }


}

public class SpriteReturn
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<Color32> colormods = new List<Color32>();
}