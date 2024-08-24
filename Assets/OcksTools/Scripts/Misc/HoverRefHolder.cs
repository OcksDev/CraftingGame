using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverRefHolder : MonoBehaviour
{
    public TextMeshProUGUI ItemName;
    public RectTransform NameTransform;
    public RectTransform ParentTransform;
    public RectTransform LineTransform;
    public TextMeshProUGUI DescMesh;
    public RectTransform DescTransform;
    public RectTransform OutlineTransform;
    GISItem oldsex = null;
    public void SetMostData(GISItem hover)
    {
        if (oldsex == hover) return;
        oldsex = hover;

        float edgespace = 16f;
        float xexpand = 0;
        ItemName.text = hover.ItemIndex;
        DescMesh.text = GISLol.Instance.GetDescription(hover);
        var layoutr = ItemName.GetComponent<ContentSizeFitter>();
        layoutr.SetLayoutHorizontal();
        layoutr.SetLayoutVertical();
        var halftitle = NameTransform.sizeDelta / 2;
        var x = halftitle.x + edgespace;
        NameTransform.anchoredPosition = new Vector2(x, -(halftitle.y+edgespace));
        if(NameTransform.sizeDelta.x > xexpand) xexpand = NameTransform.sizeDelta.x;

        float totalYchange = -NameTransform.sizeDelta.y-edgespace;
        totalYchange -= 8f;
        LineTransform.sizeDelta = new Vector2(NameTransform.sizeDelta.x, 3);
        LineTransform.anchoredPosition = new Vector2(x, totalYchange+1.5f);

        layoutr = DescMesh.GetComponent<ContentSizeFitter>();
        layoutr.SetLayoutHorizontal();
        layoutr.SetLayoutVertical();
        totalYchange -= 10;
        var halftdesc = DescTransform.sizeDelta / 2;
        x = halftdesc.x + edgespace;
        DescTransform.anchoredPosition = new Vector2(x, totalYchange - halftdesc.y);
        if (DescTransform.sizeDelta.x > xexpand) xexpand = DescTransform.sizeDelta.x;
        totalYchange -= DescTransform.sizeDelta.y;


        ParentTransform.sizeDelta = new Vector2((edgespace * 2) + xexpand, edgespace - totalYchange);
        OutlineTransform.sizeDelta = ParentTransform.sizeDelta - new Vector2(6,6);
        OutlineTransform.anchoredPosition = new Vector2(0, 0);
    }

}
