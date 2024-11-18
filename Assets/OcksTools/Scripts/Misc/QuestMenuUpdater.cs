using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMenuUpdater : MonoBehaviour
{
    public List<QuestNerdBoi> QuestNerds = new List<QuestNerdBoi>();

    // Update is called once per frame
    public void FixedUpdate()
    {

        Gamer.Instance.UpdateCurrentQuests();
        for(int i = 0; i < GISLol.Instance.Quests.Count; i++)
        {
            QuestNerds[i].UpdateStuff(GISLol.Instance.Quests[i]);
        }
    }
}
