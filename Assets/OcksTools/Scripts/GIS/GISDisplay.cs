using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        if (item.ItemType == "Craftable")
        {
            displays[0].sprite = g.Sprite;
            displays[1].sprite = e;
            displays[2].sprite = e;
        }
        else if (item.ItemType == "Made")
        {
            displays[0].sprite = GetSprite(item, 0);
            displays[1].sprite = GetSprite(item, 1); 
            displays[2].sprite = GetSprite(item, 2); 
        }
        else
        {
            displays[0].sprite = e;
            displays[1].sprite = e;
            displays[2].sprite = e;
        }
    }


    public static Sprite GetSprite(GISItem ITEM, int index)
    {
        var e = GISLol.Instance.Materials[ITEM.Materials[index].index];
        switch (ITEM.ItemIndex)
        {
            case 3:
                return e.SwordParts[index];
            case 4:
                return e.BowParts[index];
            case 5:
                return e.SpearParts[index];
            case 6:
                return e.CrossbowParts[index];
            case 7:
                return e.DaggerParts[index];
            case 8:
                return e.SawbladeParts[index];
        }


        return GISLol.Instance.Materials[ITEM.Materials[0].index].SwordParts[0];
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
