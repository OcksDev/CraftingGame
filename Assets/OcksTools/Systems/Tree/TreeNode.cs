using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeNode : MonoBehaviour
{
    public string Name;
    public List<string> Prerequisites = new List<string>();
    [HideInInspector]
    public List<string> RelateNodes;
    [HideInInspector]
    public List<string> RelatedUpdates;
    public ViewReq ViewRequirement = ViewReq.AtLeastOne;
    public List<string> LockPrerequisites = new List<string>();
    public ViewReq UnlockRequirement = ViewReq.AtLeastOne;
    public ViewStates StartState = ViewStates.Hidden;
    [HideInInspector]
    public ViewStates ViewState = ViewStates.Hidden;
    public GameObject LineObject;

    public Button clicky;
    public Image sexing;
    public Image sexing_overlat;

    public UpgradeTreeSex prntr;
    public Dictionary<string, LineSex> lines = new Dictionary<string, LineSex>();
    private void Awake()
    {
        InitializeNode();
    }
    bool hasinit = false;
    public void InitializeNode()
    {
        //Debug.Log("INITIITIT");
        if (hasinit) return;
        hasinit = true;
        TreeHandler.Nodes.Add(Name, this);
        ViewState = StartState;
        RelateNodes = new List<string>(Prerequisites);
        TreeHandler.LoadCurrentState.Append(Name, UpdateState);
        TreeHandler.SpawnLines.Append(Name, SpawnLines);
        TreeHandler.UpdateLines.Append(Name, UpdateAllLines);
    }


    public void UpdateState()
    {
        var t = TreeHandler.Instance;
        bool lockedview = false;
        if (t.MeetsReqs(RelateNodes, ViewRequirement))
        {
            switch (ViewState)
            {
                case ViewStates.Locked:
                    lockedview = true;
                    if(t.MeetsReqs(LockPrerequisites, UnlockRequirement))
                    {
                        ViewState = ViewStates.Available;
                        goto Ragg;
                    }
                    break;
                case ViewStates.Hidden:
                    ViewState = ViewStates.Available;
                    goto Ragg;
                case ViewStates.Seeable:
                    ViewState = ViewStates.Available;
                    goto Ragg;
                case ViewStates.Available:
                Ragg:
                    if (TreeHandler.CurrentOwnerships.ContainsKey(Name))
                    {
                        ViewState = ViewStates.Obtained;
                    }
                    break;
            }
        }
        else
        {
            ViewState = StartState;
        }
         canseeme = false;
        switch (ViewState)
        {
            case ViewStates.Locked:
                canseeme = lockedview;
                break;
            case ViewStates.Seeable:
            case ViewStates.Available:
            case ViewStates.Obtained:
                canseeme = true;
                break;
            case ViewStates.Hidden:
                break;
        }
        switch (ViewState)
        {
            case ViewStates.Locked:
            case ViewStates.Seeable:
            case ViewStates.Obtained:
            case ViewStates.Hidden:
                clicky.interactable = false;
                break;
            case ViewStates.Available:
                clicky.interactable = true;
                break;
        }
        UpdateDisplay();
        gameObject.SetActive(canseeme);
    }

    public void UpdateDisplay()
    {

        switch (ViewState)
        {
            case ViewStates.Obtained:
                sexing.color = new Color32(148, 0, 255, 255);
                transform.localScale = Vector3.one;
                sexing_overlat.enabled = true;
                sexing_overlat.color = new Color32(148, 3, 252, 70);
                break;
            case ViewStates.Locked:
            case ViewStates.Seeable:
            case ViewStates.Hidden:
                sexing.color = new Color32(113, 113, 113, 255);
                transform.localScale = Vector3.one * 0.8f;
                sexing_overlat.enabled = true;
                break;
            case ViewStates.Available:
                sexing.color = new Color32(240, 100, 100, 255);
                if(prntr.CanPurcahse) sexing.color = new Color32(100, 240, 100, 255);
                transform.localScale = Vector3.one;
                sexing_overlat.enabled = false;
                break;
        }
    }



    [HideInInspector]
    public bool canseeme = false;

    public void PropogatedUpdate()
    {
        UpdateState();
        foreach(var a in RelateNodes)
        {
            TreeHandler.Nodes[a].UpdateState();
        }
        foreach(var a in RelatedUpdates)
        {
            TreeHandler.Nodes[a].UpdateState();
        }
        UpdateAllLines();
        foreach(var a in RelateNodes)
        {
            TreeHandler.Nodes[a].UpdateAllLines();
            TreeHandler.Nodes[a].UpdatePrereqLines(Name);
        }
    }
    public void UpdateAllLines()
    {
        foreach (var a in lines)
        {
            UpdateLineStatus(a);
        }
    }
    public void UpdatePrereqLines(string exludeme)
    {
        foreach (var a in Prerequisites)
        {
            if (a == exludeme) continue;
            TreeHandler.Nodes[a].UpdateAllLines();
        }
    }
    public void Click()
    {
        var t = TreeHandler.Instance;
        if (!t.MeetsReqs(RelateNodes, ViewRequirement))
        {
            return;
        }
        switch(ViewState)
        {
            case ViewStates.Obtained:
                //clicked an already obtained thing
                break;
            case ViewStates.Locked:
                //clicked something that is locked
                break;
            case ViewStates.Available:
                TreeHandler.CurrentOwnerships.Add(Name, "");
                PropogatedUpdate();
                break;
        }
    }

    public void SpawnLines()
    {
        foreach(var a in RelateNodes)
        {
            if (Prerequisites.Contains(a)) continue;
            var li = Instantiate(LineObject, transform.position, Quaternion.identity, TreeHandler.Instance.LineParent.transform);
            lines.Add(a, li.GetComponent<LineSex>());
        }
        foreach (var a in lines)
        {
            UpdateLinePos(a);
        }
    }

    private void OnEnable()
    {
        if (Time.time <= 0.1f) return;
        UpdateState();
    }

    public void UpdateLinePos(KeyValuePair<string, LineSex> s)
    {
        var targ = TreeHandler.Nodes[s.Key].transform.position;
        s.Value.ree.position = Vector3.Lerp(transform.position, targ, 0.5f);
        s.Value.ree.rotation = RandomFunctions.PointAtPoint2D(transform.position, targ, 0);
        s.Value.ree.localScale = new Vector3(RandomFunctions.Instance.Dist(transform.position, targ), 1, 1);
    }
    public void UpdateLineStatus(KeyValuePair<string, LineSex> s)
    {
        if (!TreeHandler.CurrentOwnerships.ContainsKey(Name))
        {
            s.Value.baka.color = new Color32(255, 255, 255, 25);
            if (TreeHandler.CurrentOwnerships.ContainsKey(s.Key))
            {
                s.Value.baka.color = new Color32(255, 255, 255, 65);
            }
            return;
        }
        else if (!TreeHandler.CurrentOwnerships.ContainsKey(s.Key))
        {
            s.Value.baka.color = new Color32(255, 255, 255, 65);
            return;
        }


        if (TreeHandler.Nodes[s.Key].canseeme)
        {
            s.Value.baka.color = new Color32(148, 3, 252, 130);
            return;
        }
        s.Value.baka.color = new Color32(255,255,255,25);
    }

    public enum ViewReq
    {
        AtLeastOne,
        AllOf,
    }
    public enum ViewStates
    {
        Hidden,
        Locked,
        Available,
        Obtained,
        Seeable,
    }

}
