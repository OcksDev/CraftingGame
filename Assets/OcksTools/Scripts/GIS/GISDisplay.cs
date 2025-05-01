using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GISDisplay : MonoBehaviour
{
    public string Type = "";
    public GISItem item;
    public GISSlot memeparent;
    public Image[] displays;
    public TextMeshProUGUI amnt;
    public bool AutoUpdate = true;
    private GISItem olditem;
    private Vector3 initsize = Vector3.zero;
    private void Awake()
    {
        if (initsize == Vector3.zero)
        {
            initsize = transform.localScale;
        }
        if (item == null) item = new GISItem();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (AutoUpdate && item != olditem) UpdateDisplay();
    }

    public void UpdateDisplay(string extra = "")
    {
        if (Time.time <= 0.5f) return;
        olditem = item;
        var g = GISLol.Instance.ItemsDict[item.ItemIndex];
        amnt.text = item.Amount > 0 && g.MaxAmount != 1 && item.ItemIndex != "Empty" ? "x" + item.Amount : "";
        var b = GetSprites(item);

        switch (extra)
        {
            case "logbook":
                var c = new Color32(0, 0, 0, 255);
                if (!GISLol.Instance.LogbookDiscoveries.ContainsKey(item.ItemIndex))
                {
                    displays[0].material = Gamer.Instance.sexex[4];
                    displays[1].material = Gamer.Instance.sexex[4];
                    displays[2].material = Gamer.Instance.sexex[4];
                    displays[3].material = Gamer.Instance.sexex[4];
                    displays[4].material = Gamer.Instance.sexex[4];
                    displays[5].material = Gamer.Instance.sexex[4];
                }
                else
                {
                    displays[0].material = Gamer.Instance.sexex[5];
                    displays[1].material = Gamer.Instance.sexex[5];
                    displays[2].material = Gamer.Instance.sexex[5];
                    displays[3].material = Gamer.Instance.sexex[5];
                    displays[4].material = Gamer.Instance.sexex[5];
                    displays[5].material = Gamer.Instance.sexex[5];
                }
                break;
        }
        switch (Type)
        {
            case "Specific":
                if(item.ItemIndex == "Empty")
                {
                    Sprite a = null;
                    b.colormods[0] = new Color32(255, 255, 255, 35);
                    switch (GetSwitch())
                    {
                        case 0:
                            a = GISLol.Instance.MaterialsDict["Empty"].SwordParts[0];
                            break;
                        case 1:
                            a = GISLol.Instance.MaterialsDict["Empty"].SwordParts[1];
                            break;
                        case 2:
                            a = GISLol.Instance.MaterialsDict["Empty"].SwordParts[2];
                            break;
                        case 3:
                            a = GISLol.Instance.MaterialsDict["Empty"].SwordParts[3];
                            break;
                    }

                    b.sprites[0] = a;

                }
                break;
        }
        displays[0].sprite = b.sprites[0];
        displays[1].sprite = b.sprites[1];
        displays[2].sprite = b.sprites[2];
        displays[3].sprite = b.sprites[3];
        displays[4].sprite = b.sprites[4];
        displays[5].sprite = b.sprites[5];
        displays[0].color = b.colormods[0];
        displays[1].color = b.colormods[1];
        displays[2].color = b.colormods[2];
        displays[3].color = b.colormods[3];
        displays[4].color = b.colormods[4];
        displays[5].color = b.colormods[5];
        if (initsize != Vector3.zero)
        {
            transform.localScale = initsize * g.GetSizeMulty();
        }
        else
        {
            initsize = transform.localScale;
            transform.localScale = initsize * g.GetSizeMulty();
        }
        
    }

    public int GetSwitch()
    {
        switch (memeparent.InteractFilter)
        {
            default:
                return 0;
            case "Craftable":
                return 1;
            case "Aspect":
                return 2;
            case "Empty":
                return 3;
        }
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
                var fall = TraceFallthrough(e.fallthroughmaterial);
                var fallmian = TraceFallthrough2(e.fallthroughmaterialmian, e.Name);
                Sprite baller;
                Sprite baller2 = d;
                Color32 beans = e.ColorMod;
                Color32 beans2 = e.OverlayColorMod;
                Sprite[] mysprites = null;
                Sprite[] defaultsprites = null;
                switch (ITEM.ItemIndex)
                {
                    default:
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].SwordParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].SwordParts;
                        break;
                    case "Bow":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].BowParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].BowParts;
                        break;
                    case "Spear":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].SpearParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].SpearParts;
                        break;
                    case "Crossbow":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].CrossbowParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].CrossbowParts;
                        break;
                    case "Shuriken":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].DaggerParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].DaggerParts;
                        break;
                    case "Boomerang":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].SawbladeParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].SawbladeParts;
                        break;
                    case "Axe":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].AxeParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].AxeParts;
                        break;
                    case "Blowdart":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].BlowParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].BlowParts;
                        break;
                    case "Dagger":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].TDaggerParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].TDaggerParts;
                        break;
                    case "Knife":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].KnifeParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].KnifeParts;
                        break;
                    case "Wand":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].WandParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].WandParts;
                        break;
                    case "Scythe":
                        mysprites = GISLol.Instance.MaterialsDict[fallmian].ScytheParts;
                        defaultsprites = GISLol.Instance.MaterialsDict[fall].ScytheParts;
                        break;
                }

                if (index >= mysprites.Length || mysprites[index] == null || e.IsOverlay)
                {
                    baller = defaultsprites[index];
                }
                else
                {
                    if (e.ignorecolorforcumimg) beans = ccc;
                    baller = mysprites[index];
                }
                if (e.IsOverlay)
                {
                    baller2 = mysprites[index];
                }
                boner.Add(baller);
                boner2.Add(beans);
                boner.Add(baller2);
                boner2.Add(beans2);
            }

        }
        else if (w.IsCraftable)
        {
            boner = new List<Sprite>() { w.Sprite, d, d, d, d, d };
            boner2 = new List<Color32>() { ccc, ccc, ccc, ccc, ccc, ccc };
        }
        else if (w.IsRune)
        {
            boner = new List<Sprite>() { GISLol.Instance.ItemsDict["RuneBase"].Sprite, w.Sprite, d, d, d, d };
            boner2 = new List<Color32>() { ccc, ccc, ccc, ccc, ccc, ccc };
        }
        else
        {
            boner = new List<Sprite>() { w.Sprite, d, d, d, d, d };
            boner2 = new List<Color32>() { ccc, ccc, ccc, ccc, ccc, ccc };
        }

        var a = new SpriteReturn();

        a.sprites = boner;
        a.colormods = boner2;
        return a;
    }

    public static string TraceFallthrough(string origin)
    {
        if (GISLol.Instance.MaterialsDict[origin].IsOverlay)
        {
            return TraceFallthrough(GISLol.Instance.MaterialsDict[origin].fallthroughmaterial);
        }
        else
        {
            return origin;
        }
    }
    public static string TraceFallthrough2(string origin, string main)
    {
        if (origin == "") return main;
        if (GISLol.Instance.MaterialsDict[origin].IsOverlay)
        {
            return TraceFallthrough2(GISLol.Instance.MaterialsDict[origin].fallthroughmaterialmian, origin);
        }
        else
        {
            return origin;
        }
    }


}

public class SpriteReturn
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<Color32> colormods = new List<Color32>();
}