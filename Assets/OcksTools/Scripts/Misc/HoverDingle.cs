using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDingle : MonoBehaviour
{
    public string type = "";
    public GISDisplay guny;
    public VaultitemDisplay vuny;
    void Update()
    {
        var g = GISLol.Instance;
        var wan = g.IsHoveringReal(gameObject);
        if (wan)
        {
            var hv = new HoverType(type);
            switch (type)
            {
                case "weenor":
                    g.HoverDohicky(new HoverType(vuny.item));
                    break;
                case "logbook":
                    if (Gamer.Instance.checks[13]) return;
                    if (g.LogbookDiscoveries.ContainsKey(guny.item.ItemIndex))
                    {
                        g.HoverDohicky(new HoverType(guny.item));
                    }
                    else
                    {
                        hv.type = "TitleAndDesc";
                        hv.data = "???";
                        hv.data2 = "Discover this item in a run to unlock";
                        g.HoverDohicky(hv);
                    }
                    break;
                default:
                    hv.type = "TitleAndDesc";
                    hv.data = "Craft";
                    hv.data2 = "A weapon requires<br>three materials and a name";
                    g.HoverDohicky(hv);
                    break;
            }
        }
    }
}
