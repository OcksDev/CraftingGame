using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDingle : MonoBehaviour
{
    public string type = "";
    public VaultitemDisplay vuny;
    void Update()
    {
        var wan = GISLol.Instance.IsHoveringReal(gameObject);
        if (wan)
        {
            var hv = new HoverType(type);
            switch (type)
            {
                case "weenor":
                    GISLol.Instance.HoverDohicky(new HoverType(vuny.item));
                    break;
                default:
                    hv.type = "TitleAndDesc";
                    hv.data = "Craft";
                    hv.data2 = "A weapon requires<br>three materials and a name";
                    GISLol.Instance.HoverDohicky(hv);
                    break;
            }
        }
    }
}
