using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestNerdBoi : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI MainBody;
    public TextMeshProUGUI Status;
    public Image Icon;
    public QuestProgress quest;
    public void UpdateStuff(QuestProgress q)
    {
        if(quest == q) return;
        quest = q;

        

        //this code is retarded
        // 0
        Func<string> res = () => {
            switch (quest.Data["Name"])
            {
                case "Collect": return $"Collect {quest.Data["Target_Amount"]} {GISLol.Instance.ItemsDict[quest.Data["Target_Data"]].GetDisplayName()}";
                case "Kill": return $"Kill {quest.Data["Target_Amount"]} enemies using weapons of type {GISLol.Instance.ItemsDict[quest.Data["Target_Data"]].GetDisplayName()}";
                default: return "";
            }
        };
        Title.text = res();
        res = () => {
            switch (quest.Data["Name"])
            {
                default: return $"Reward: {quest.Data["Reward_Amount"]} {GISLol.Instance.ItemsDict[quest.Data["Reward_Data"]].GetDisplayName()}";
            }
        };
        MainBody.text = res();
        res = () => {
            if (quest.Data["Completed"] == "False")
                switch (quest.Data["Name"])
                {
                    case "Collect": return $"Collected: {quest.Data["Progress"]}/{quest.Data["Target_Amount"]}";
                    case "Kill": return $"Slain: {quest.Data["Progress"]}/{quest.Data["Target_Amount"]}";
                    default: return $"Incomplete";
                }
            else
                return $"Complete";
        };
        Status.text = res();

    }

}
