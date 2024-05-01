using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public List<int> ItemIndexes = new List<int>();
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
            var pp = GISDisplay.GetBaseSprites(ItemIndexes[i]);
            var rs = Instantiate(prefabsex, transform.position, transform.rotation, transform).GetComponent<ItemHolder>();
            rs.one.sprite = pp[0];
            rs.two.sprite = pp[1];
            rs.trhee.sprite = pp[2];
            rs.sexindex = i;
            gm.Add(rs.gameObject);
            if (i == 0) rs.Click();
        }
    }
}
