using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public List<string> ItemIndexes = new List<string>();
    public List<GameObject> gm = new List<GameObject>();
    public GameObject prefabsex;
   
    public void Start()
    {
        Open();
    }
    public void Open()
    {
        if (gm.Count > 0) 
        {
            gm[0].GetComponent<ItemHolder>().Click();
            return;
            };
        for(int i = 0; i < ItemIndexes.Count; i++)
        {
            var e = new GISItem(ItemIndexes[i]);
            e.Materials.Add(new GISMaterial("Rock"));
            e.Materials.Add(new GISMaterial("Rock"));
            e.Materials.Add(new GISMaterial("Rock"));
            var pp = GISDisplay.GetSprites(e).sprites;
            var rs = Instantiate(prefabsex, transform.position, transform.rotation, transform).GetComponent<ItemHolder>();
            rs.one.sprite = pp[0];
            rs.two.sprite = pp[2];
            rs.trhee.sprite = pp[4];
            rs.sexindex = i;
            rs.nerdl = e;
            gm.Add(rs.gameObject);
            if (i == 0) rs.Click();
        }
    }
}
