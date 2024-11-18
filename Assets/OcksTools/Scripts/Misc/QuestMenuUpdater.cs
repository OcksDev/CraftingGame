using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMenuUpdater : MonoBehaviour
{
    List<QuestNerdBoi> QuestNerds = new List<QuestNerdBoi>();

    // Update is called once per frame
    void FixedUpdate()
    {
        /* seems reasonable
         * current amount of normal enemies: 13 I think (?)
         * "what is your fucking issue, what is your fucking problem" crazy stuff
         */

        Gamer.Instance.UpdateCurrentQuests();
        for(int i = 0; i < GISLol.Instance.Quests.Count; i++)
        {
            QuestNerds[i].UpdateStuff(GISLol.Instance.Quests[i]);
        }
    }
}
