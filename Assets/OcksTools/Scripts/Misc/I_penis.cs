using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_penis : MonoBehaviour
{
    public GISDisplay GISDisplay;


    public void CLICK_MY_MASSIVE_BOOTY_CEEKS()
    {
        if (!GISLol.Instance.LogbookDiscoveries.ContainsKey(GISDisplay.item.ItemIndex)) return;
        var a = Tags.refs["LogbookSubmenu"].GetComponent<I_PENISTWO>();
        a.GISDisplay.item = GISDisplay.item;
        a.GISDisplay.UpdateDisplay();
        a.title.text = a.GISDisplay.item.ItemIndex;
        a.description.text = GISLol.Instance.GetDescription(a.GISDisplay.item, true);
        Gamer.Instance.checks[13] = true;
        Gamer.Instance.UpdateMenus();
    }
}
