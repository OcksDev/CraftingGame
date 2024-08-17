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
        var g = GISLol.Instance.Items[item.ItemIndex];
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
            var e = GISLol.Instance.Materials[ITEM.Materials[index].index];
            var fall = e.fallthroughmaterial;
            Sprite baller = null;
            Color32 beans = e.ColorMod;
            Sprite[] mysprites=null;
            Sprite[] defaultsprites=null;
            switch (ITEM.ItemIndex)
            {
                default:
                    mysprites = e.SwordParts;
                    defaultsprites = GISLol.Instance.Materials[fall].SwordParts;
                    break;
                case 4:
                    mysprites = e.BowParts;
                    defaultsprites = GISLol.Instance.Materials[fall].BowParts;
                    break;
                case 5:
                    mysprites = e.SpearParts;
                    defaultsprites = GISLol.Instance.Materials[fall].SpearParts;
                    break;
                case 6:
                    mysprites = e.CrossbowParts;
                    defaultsprites = GISLol.Instance.Materials[fall].CrossbowParts;
                    break;
                case 7:
                    mysprites = e.DaggerParts;
                    defaultsprites = GISLol.Instance.Materials[fall].DaggerParts;
                    break;
                case 8:
                    mysprites = e.SawbladeParts;
                    defaultsprites = GISLol.Instance.Materials[fall].SawbladeParts;
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

    public static Sprite[] GetBaseSprites(int itemindex)
    {
        var e = GISLol.Instance.Materials[0];
        switch (itemindex)
        {
            default:
                return e.SwordParts;
            case 4:
                return e.BowParts;
            case 5:
                return e.SpearParts;
            case 6:
                return e.CrossbowParts;
            case 7:
                return e.DaggerParts;
            case 8:
                return e.SawbladeParts;
        }
    }

}

public class SpriteReturn
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<Color32> colormods = new List<Color32>();
}