using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugSex : MonoBehaviour
{
    public Transform startp;
    public Transform selp;
    public GameObject Spawn;
    public List<string> NeedsToBeUnlocked = new List<string>();
    public void DoAll()
    {
        List<string> offers = new List<string>();
        List<string> onners = new List<string>();
        var aa = startp.GetComponentsInChildren<DrugDealing>();
        foreach(var a in aa)
        {
            Destroy(a.gameObject);
        }
        aa = selp.GetComponentsInChildren<DrugDealing>();
        foreach(var a in aa)
        {
            Destroy(a.gameObject);
        }

        foreach(var a in GISLol.Instance.Drugs)
        {
            if (NeedsToBeUnlocked.Contains(a.Name) && !TreeHandler.CurrentOwnerships.ContainsKey(a.Name)) continue;
            if (Gamer.ActiveDrugs.Contains(a.Name))
            {
                onners.Add(a.Name);
            }
            else
            {
                offers.Add(a.Name);
            }
        }
        System.Action<List<string>, Transform> ree = (x, y) =>
        {
            foreach (var a in x)
            {
                var e = Instantiate(Spawn, transform.position, Quaternion.identity, y).GetComponent<DrugDealing>();
                e.MainCont.item = new GISItem(a);
                e.MainCont.UpdateDisplay();
                e.tits.text = GISLol.Instance.ItemsDict[a].GetDisplayName();
                e.dick.text = GISLol.Instance.ItemsDict[a].Description;
                e.Parent = this;
                e.tatee = y;
            }
        };
        ree(offers, startp);
        ree(onners, selp);
    }

    public void FlipFlop(DrugDealing aa)
    {
        bool ree = aa.tatee == startp;
        if (ree)
        {
            aa.transform.parent = selp;
            aa.tatee = selp;
            Gamer.ActiveDrugs.Add(aa.MainCont.item.ItemIndex);
        }
        else
        {
            aa.transform.parent = startp;
            aa.tatee = startp;
            Gamer.ActiveDrugs.Remove(aa.MainCont.item.ItemIndex);
        }
    }

}
