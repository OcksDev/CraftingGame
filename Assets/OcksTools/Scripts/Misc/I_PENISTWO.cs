using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class I_PENISTWO : MonoBehaviour
{
    public GISDisplay GISDisplay;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public void ENDME()
    {
        Gamer.Instance.checks[13] = false;
        Gamer.Instance.UpdateMenus();
    }
}
