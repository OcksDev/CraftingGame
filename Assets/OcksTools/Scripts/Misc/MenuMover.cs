using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMover : MonoBehaviour
{
    public List<Transform> nerds = new List<Transform>();
    public List<Image> nerds_img = new List<Image>();
    [HideInInspector]
    public List<Vector3> nerds_orig = new List<Vector3>();
    [HideInInspector]
    public List<Color> nerds_img_orig = new List<Color>();
    bool hassex = false;
    public void Initial()
    {
        if (hassex) return;
        hassex = true;
        nerds_orig.Clear();
        foreach (Transform t in nerds)
        {
            nerds_orig.Add(t.localPosition);
        }
        nerds_img_orig.Clear();
        foreach (var t in nerds_img)
        {
            nerds_img_orig.Add(t.color);
        }
    }
}
