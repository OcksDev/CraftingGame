using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public List<string> ItemIndexes = new List<string>();
    public List<GameObject> gm = new List<GameObject>();
    public GameObject prefabsex;
   
    public void OnEnable()
    {
        if(Time.time > 0.1f)
        Open();
    }
    public void Open()
    {
        if (gm.Count > 0)
        {
            foreach(var a in gm)
            {
                Destroy(a.gameObject);
            }
            gm.Clear();
        }
        for(int i = 0; i < ItemIndexes.Count; i++)
        {
            if (i > 1 && !TreeHandler.CurrentOwnerships.ContainsKey(ItemIndexes[i])) continue;
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
