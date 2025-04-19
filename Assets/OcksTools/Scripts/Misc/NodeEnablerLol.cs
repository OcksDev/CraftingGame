using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEnablerLol : MonoBehaviour
{
    public List<FunnyThing> aa = new List<FunnyThing>();
    public void Reebaka()
    {
        foreach(var a in aa)
        {
            a.gm.SetActive(TreeHandler.CurrentOwnerships.ContainsKey(a.Name));
        }
    }
}

[System.Serializable]
public class FunnyThing
{
    public string Name;
    public GameObject gm;
}