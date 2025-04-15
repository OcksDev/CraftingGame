using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquippersSex : MonoBehaviour
{
    GISContainer cc;
    public List<GISDisplay> aa;
    public List<Image> aa2;
    public List<TextMeshProUGUI> aa3;
    private void Start()
    {
        cc = GISLol.Instance.All_Containers["Equips"];
    }
    private void FixedUpdate()
    {
        if(cc==null) return;
        aa[0].item = cc.slots[0].Held_Item;
        aa[1].item = cc.slots[1].Held_Item;
        aa[0].UpdateDisplay();
        aa[1].UpdateDisplay();

        aa3[0].text = InputManager.keynames[InputManager.gamekeys["switch1"][0]];
        aa3[1].text = InputManager.keynames[InputManager.gamekeys["switch2"][0]];
    }

    int a = -69;
    // Update is called once per frame
    void Update()
    {
        if(PlayerController.Instance==null) return;
        if(a != PlayerController.Instance.selecteditem)
        {
            a = PlayerController.Instance.selecteditem;
            aa2[a].color = new Color32(200, 200, 200, 200);
            aa2[1-a].color = new Color32(178, 178, 178, 128);
            aa2[a+2].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            aa2[(1-a)+2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
    }
}
