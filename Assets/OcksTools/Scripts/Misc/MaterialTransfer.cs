using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTransfer : MonoBehaviour
{
    public GISDisplay dip;
    public string Type = "FromRun";
    public bool wasshun = false;
    private void Start()
    {
        if(!wasshun)dip.item = new GISItem(GISLol.Instance.AllCraftables[Random.Range(0, GISLol.Instance.AllCraftables.Count)]);
    }

    public void Clickity()
    {
        MaterialTransfer transfer = null;
        switch (Type)
        {
            case "FromRun":
                transfer = Instantiate(Gamer.Instance.ItemTranser, transform.position, transform.rotation, Tags.refs["RightTrans"].transform).GetComponent < MaterialTransfer>();
                transfer.dip.item = dip.item;
                transfer.Type = "FromRun2";
                transfer.wasshun = true;
                transfer.dip.UpdateDisplay();
                break;
            case "FromRun2":
                transfer = Instantiate(Gamer.Instance.ItemTranser, transform.position, transform.rotation, Tags.refs["LeftTrans"].transform).GetComponent < MaterialTransfer>();
                transfer.dip.item = dip.item;
                transfer.wasshun = true;
                transfer.dip.UpdateDisplay();
                break;
        }
        Destroy(gameObject);
    }
}
