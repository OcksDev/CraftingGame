using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    public Image one;
    public Image two;
    public Image trhee;
    public int sexindex = 0;
    public Color32[] sex;
    public void Click()
    {
        var e = transform.parent.GetComponent<Selector>();
        foreach(var s in e.gm)
        {
            s.GetComponent<Image>().color = sex[0];
        }
        GetComponent<Image>().color = sex[1];
        Gamer.Instance.CraftSex = e.ItemIndexes[sexindex];
    }
}
