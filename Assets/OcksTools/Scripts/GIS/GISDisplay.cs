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
        for (int index = 0; index < 3; index++)
        {
            var e = GISLol.Instance.Materials[ITEM.Materials[index].index];
            var fall = e.fallthroughmaterial;
            Sprite baller = null;
            Color32 beans = e.ColorMod;
            switch (ITEM.ItemIndex)
            {
                case 3:
                    if(index >= e.SwordParts.Length || e.SwordParts[index] == null)
                    {
                        baller = GISLol.Instance.Materials[fall].SwordParts[index];
                    }
                    else
                    {
                        baller = e.SwordParts[index];
                    }
                    break;
                case 4:
                    if (index >= e.BowParts.Length || e.BowParts[index] == null)
                    {
                        baller = GISLol.Instance.Materials[fall].BowParts[index];
                    }
                    else
                    {
                        baller = e.BowParts[index];
                    }
                    break;
                case 5:
                    if (index >= e.SpearParts.Length || e.SpearParts[index] == null)
                    {
                        baller = GISLol.Instance.Materials[fall].SpearParts[index];
                    }
                    else
                    {
                        baller = e.SpearParts[index];
                    }
                    break;
                case 6:
                    if (index >= e.CrossbowParts.Length || e.CrossbowParts[index] == null)
                    {
                        baller = GISLol.Instance.Materials[fall].CrossbowParts[index];
                    }
                    else
                    {
                        baller = e.CrossbowParts[index];
                    }
                    break;
                case 7:
                    if (index >= e.DaggerParts.Length || e.DaggerParts[index] == null)
                    {
                        baller = GISLol.Instance.Materials[fall].DaggerParts[index];
                    }
                    else
                    {
                        baller = e.DaggerParts[index];
                    }
                    break;
                case 8:
                    if (index >= e.SawbladeParts.Length || e.SawbladeParts[index] == null)
                    {
                        baller = GISLol.Instance.Materials[fall].SawbladeParts[index];
                    }
                    else
                    {
                        baller = e.SawbladeParts[index];
                    }
                    break;
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