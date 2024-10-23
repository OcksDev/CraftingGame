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
        bool edge= true;
        switch (Type)
        {
            case "FromRun":
                var w = Tags.refs["RightTrans"].transform.childCount;
                if(w < Gamer.CurrentFloor)
                {
                    Gamer.Instance.SpawnItemTranser(dip.item, "FromRun2");
                }
                else
                {
                    edge = false;
                }
                break;
            case "FromRun2":
                Gamer.Instance.SpawnItemTranser(dip.item, "FromRun");
                break;
        }
        if(edge)
        Destroy(gameObject);
    }

    private void Update()
    {
        var wee =  GISLol.Instance.IsHoveringReal(gameObject);
        if (wee)
        {
            GISLol.Instance.HoverDohicky(new HoverType(dip.item));
        }
    }

}
