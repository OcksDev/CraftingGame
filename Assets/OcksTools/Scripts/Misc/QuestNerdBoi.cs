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
                case "Collect": return $"Collect {quest.Data["Target_Amount"]}x of {quest.Data["Target_Data"]}";
                case "Kill": return $"Kill {quest.Data["Target_Amount"]} enemies using a {quest.Data["Target_Data"]} weapon";
                default: return "";
            }
        };
        Title.text = res();
        res = () => {
            switch (quest.Data["Name"])
            {
                default: return $"Reward: {quest.Data["Reward_Amount"]}x of {quest.Data["Reward_Data"]}"; ;
            }
        };
        MainBody.text = res();

    }

}
