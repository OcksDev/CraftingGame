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
    public bool AutoUpdate = true;
    private void Awake()
    {
        if (item == null) item = new GISItem();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (AutoUpdate) UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        var g = GISLol.Instance.ItemsDict[item.ItemIndex];
        amnt.text = item.Amount > 0 && g.MaxAmount != 1 ? "x" + item.Amount : "";
        var b = GetSprites(item);
        displays[0].sprite = b.sprites[0];
        displays[1].sprite = b.sprites[1];
        displays[2].sprite = b.sprites[2];
        displays[0].color = b.colormods[0];
        displays[1].color = b.colormods[1];
        displays[2].color = b.colormods[2];
    }
    public static SpriteReturn GetSprites(GISItem ITEM)
    {
        List<Sprite> boner = new List<Sprite>();
        List<Color32> boner2 = new List<Color32>();
        var ccc = new Color32(255, 255, 255, 255);
        var d = GISLol.Instance.ItemsDict["Empty"].Sprite;
        var w = GISLol.Instance.ItemsDict[ITEM.ItemIndex];
        if (w.IsWeapon)
        {
            for (int index = 0; index < 3; index++)
            {
                var e = GISLol.Instance.MaterialsDict[ITEM.Materials[index].index];
                var fall = e.fallthroughmaterial;
                Sprite baller = null;
                Color32 beans = e.ColorMod;
                Sprite[] mysprites = null;
                Sprite[] defaultsprites = null;
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
                    case "Axe":
                        mysprites = e.AxeParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].AxeParts;
                        break;
                    case "Blowdart":
                        mysprites = e.BlowParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].BlowParts;
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

        }
        else if (w.IsCraftable)
        {
            boner = new List<Sprite>() { w.Sprite, d, d };
            boner2 = new List<Color32>() { ccc, ccc, ccc };
        }
        else if (w.IsRune)
        {
            boner = new List<Sprite>() { GISLol.Instance.ItemsDict["RuneBase"].Sprite, w.Sprite, d };
            boner2 = new List<Color32>() { ccc, ccc, ccc };
        }
        else
        {
            boner = new List<Sprite>() { w.Sprite, d, d };
            boner2 = new List<Color32>() { ccc, ccc, ccc };
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