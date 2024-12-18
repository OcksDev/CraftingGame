using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HoverRefHolder : MonoBehaviour
{
    public TextMeshProUGUI ItemName;
    public RectTransform NameTransform;
    public RectTransform ParentTransform;
    public RectTransform LineTransform;
    public RectTransform LineTransform2;
    public TextMeshProUGUI DescMesh;
    public RectTransform DescTransform;
    public RectTransform OutlineTransform;
    public RectTransform[] MaterialDisplays;
    public RectTransform[] MaterialDisplayBackgrounds;
    GISItem oldsex = null;

    public void SetMostData(HoverType hoverr)
    {
        var hover = hoverr.item;
        float edgespace = 16f;
        float halfedge = edgespace / 2;
        float xexpand = 0;
        float totalYchange = 0;

        switch (hoverr.type)
        {
            case "Item": break;
            default:
                oldsex = null;
                for (int i = 0; i < 3; i++)
                {
                    MaterialDisplays[i].gameObject.SetActive(false);
                    MaterialDisplayBackgrounds[i].gameObject.SetActive(false);
                }
                LineTransform.gameObject.SetActive(false);
                LineTransform2.gameObject.SetActive(false);
                break;
        }
        switch (hoverr.type)
        {
            case "TitleAndDesc":
                LineTransform.gameObject.SetActive(true);
                if (ItemName.text == hoverr.data && DescMesh.text == hoverr.data2) return;
                ItemName.text = hoverr.data;
                DescMesh.text = hoverr.data2;
                var layoutr2 = ItemName.GetComponent<ContentSizeFitter>();
                layoutr2.SetLayoutHorizontal();
                layoutr2.SetLayoutVertical();
                var halftitle2 = NameTransform.sizeDelta / 2;
                var x2 = halftitle2.x + edgespace;
                NameTransform.anchoredPosition = new Vector2(x2, -(halftitle2.y + edgespace));
                if (NameTransform.sizeDelta.x > xexpand) xexpand = NameTransform.sizeDelta.x;

                totalYchange = -NameTransform.sizeDelta.y - edgespace;

                totalYchange -= 5 + 3f;
                LineTransform.anchoredPosition = new Vector2(0, totalYchange + 1.5f);


                layoutr2 = DescMesh.GetComponent<ContentSizeFitter>();
                layoutr2.SetLayoutHorizontal();
                layoutr2.SetLayoutVertical();
                totalYchange -= 10;
                var halftdesc2 = DescTransform.sizeDelta / 2;
                x2 = halftdesc2.x + edgespace;
                DescTransform.anchoredPosition = new Vector2(x2, totalYchange - halftdesc2.y);
                if (DescTransform.sizeDelta.x > xexpand) xexpand = DescTransform.sizeDelta.x;
                totalYchange -= DescTransform.sizeDelta.y;
                break;
            default:
                LineTransform.gameObject.SetActive(true);
                LineTransform2.gameObject.SetActive(true);
                if (oldsex == hover) return;
                oldsex = hover;



                var itembase = GISLol.Instance.ItemsDict[hover.ItemIndex];
                if (hover.CustomName != "")
                {
                    ItemName.text = hover.CustomName;
                }
                else
                {
                    ItemName.text = itembase.GetDisplayName();
                }


                DescMesh.text = GISLol.Instance.GetDescription(hover);
                var layoutr = ItemName.GetComponent<ContentSizeFitter>();
                layoutr.SetLayoutHorizontal();
                layoutr.SetLayoutVertical();
                var halftitle = NameTransform.sizeDelta / 2;
                var x = halftitle.x + edgespace;
                NameTransform.anchoredPosition = new Vector2(x, -(halftitle.y + edgespace));
                if (NameTransform.sizeDelta.x > xexpand) xexpand = NameTransform.sizeDelta.x;

                totalYchange = -NameTransform.sizeDelta.y - edgespace;

                totalYchange -= 5 + 3f;
                LineTransform.anchoredPosition = new Vector2(0, totalYchange + 1.5f);


                layoutr = DescMesh.GetComponent<ContentSizeFitter>();
                layoutr.SetLayoutHorizontal();
                layoutr.SetLayoutVertical();
                totalYchange -= 10;
                var halftdesc = DescTransform.sizeDelta / 2;
                x = halftdesc.x + edgespace;
                DescTransform.anchoredPosition = new Vector2(x, totalYchange - halftdesc.y);
                if (DescTransform.sizeDelta.x > xexpand) xexpand = DescTransform.sizeDelta.x;
                totalYchange -= DescTransform.sizeDelta.y;

                totalYchange -= 10 + 3f;
                LineTransform2.anchoredPosition = new Vector2(0, totalYchange + 1.5f);


                for (int i = 0; i < 3; i++)
                {
                    MaterialDisplays[i].gameObject.SetActive(itembase.IsWeapon);
                    MaterialDisplayBackgrounds[i].gameObject.SetActive(itembase.IsWeapon);
                }

                if (itembase.IsWeapon)
                {
                    totalYchange -= 10;
                    float size = 37.5f;
                    float halfsize = size / 2;
                    float xadd = -halfedge - halfsize;
                    totalYchange -= halfsize;
                    for (int i = 0; i < 3; i++)
                    {
                        xadd += size + halfedge;
                        var dic = MaterialDisplays[i].GetComponent<GISDisplay>();
                        dic.item = new GISItem(hover.Materials[i].index);
                        dic.UpdateDisplay();
                        var wanksex = new Vector2(edgespace + xadd, totalYchange);
                        MaterialDisplays[i].anchoredPosition = wanksex;
                        MaterialDisplayBackgrounds[i].anchoredPosition = wanksex;
                    }
                    xadd += halfsize;
                    totalYchange -= halfsize;
                    if (xadd > xexpand) xexpand = xadd;
                }

                break;
        }
        //line sizing
        float wanksize = xexpand / 2 + edgespace;
        LineTransform.sizeDelta = new Vector2(xexpand, 3);
        LineTransform.anchoredPosition = new Vector2(wanksize, LineTransform.anchoredPosition.y);
        LineTransform2.sizeDelta = new Vector2(xexpand, 3);
        LineTransform2.anchoredPosition = new Vector2(wanksize, LineTransform2.anchoredPosition.y);

        //the end, do not put new elements below this
        ParentTransform.sizeDelta = new Vector2((edgespace * 2) + xexpand, edgespace - totalYchange);
        OutlineTransform.sizeDelta = ParentTransform.sizeDelta - new Vector2(6,6);
        OutlineTransform.anchoredPosition = new Vector2(0, 0);
    }
}

public class HoverType
{
    public GISItem item;
    public string type = "Item";
    public string data = "";
    public string data2 = "";
    public HoverType(GISItem it)
    {
        item = it;
        type = "Item";
    }
    public HoverType(string ty)
    {
        type = ty;
    }
}