using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTransfer : MonoBehaviour
{
    public GISDisplay dip;
    public string Type = "FromRun";
    public bool wasshun = false;

    public void Clickity()
    {
        switch (Type)
        {
            case "FromRun":
                Gamer.Instance.SpawnItemTranser(dip.item, "FromRun2");
                break;
            case "FromRun2":
                Gamer.Instance.SpawnItemTranser(dip.item, "FromRun");
                break;
        }
        Destroy(gameObject);
    }
}
