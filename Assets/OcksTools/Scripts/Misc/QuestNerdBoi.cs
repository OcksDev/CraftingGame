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
    public GISDisplay Icon;
    public GISDisplay Icon2;
    public QuestProgress quest;
    public Color32[] Cols;
    public void UpdateStuff(QuestProgress q, bool force = false)
    {
        if(quest == q && !force) return;
        quest = q;

        

        //this code is retarded
        // 0
        Title.text = GetTitle(q);
        if (quest.Data["Completed"] == "True")
        {
            Title.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(Cols[0])}>" + Title.text;
        }
        Func<string> res = () => {
            switch (quest.Data["Name"])
            {
                default: return $"Reward: {quest.Data["Reward_Amount"]} {GISLol.Instance.ItemsDict[quest.Data["Reward_Data"]].GetDisplayName()}";
            }
        };
        MainBody.text = res();
        if (quest.Data["Completed"] == "True")
        {
            MainBody.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(Cols[1])}>" + MainBody.text;
        }
        res = () => {
            if (quest.Data["Completed"] == "False")
                switch (quest.Data["Name"])
                {
                    case "Collect": return $"Collected: {quest.Data["Progress"]}/{quest.Data["Target_Amount"]}";
                    case "Kill": return $"Slain: {quest.Data["Progress"]}/{quest.Data["Target_Amount"]}";
                    case "Craft": return $"Crafted: {quest.Data["Progress"]}/{quest.Data["Target_Amount"]}";
                    case "Room": return $"Progress: {quest.Data["Progress"]}/{quest.Data["Target_Amount"]}";
                    default: return $"Incomplete";
                }
            else
                return $"Complete";
        };
        Status.text = res();
        if (quest.Data["Completed"] == "True")
        {
            Status.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(Cols[0])}>" + Status.text;
        }
        SetImageOnGISD(Icon, "Target");
        SetImageOnGISD(Icon2, "Reward");

    }

    public static string GetTitle(QuestProgress quest)
    {
        switch (quest.Data["Name"])
        {
            case "Collect": return $"Collect {quest.Data["Target_Amount"]} {GISLol.Instance.ItemsDict[quest.Data["Target_Data"]].GetDisplayName()}";
            case "Kill": return $"Kill {quest.Data["Target_Amount"]} enemies using {GISLol.Instance.ItemsDict[quest.Data["Target_Data"]].GetDisplayName()} weapons.";
            case "Craft": return $"Craft {quest.Data["Target_Amount"]} {GISLol.Instance.ItemsDict[quest.Data["Target_Data"]].GetDisplayName()} weapons, (Rock disqualifies craft)";
            case "Drug": return $"End on the 5th Floor or later with the {GISLol.Instance.ItemsDict[quest.Data["Target_Data"]].GetDisplayName()} drug active.";
            case "Room":
                switch (quest.Data["Target_Data"])
                {
                    case "Chest":
                        return $"Pay to open {quest.Data["Target_Amount"]} Dungeon Chests";
                    case "Pick Three":
                        return $"Pay to get {quest.Data["Target_Amount"]} items from \"Pick From Three\" rooms";
                    case "Shrine":
                        return $"Activate {quest.Data["Target_Amount"]} Shrines";
                    default:
                        return $"Clear {quest.Data["Target_Amount"]} \"{quest.Data["Target_Data"]}\" rooms";
                }
            default: return "";
        }
    }

    public void SetImageOnGISD(GISDisplay Icon, string boner)
    {
        if (quest.Data["Completed"] == "True")
        {
            GISItem dikl = new GISItem("Check");
            Icon.item = dikl;
            Icon.UpdateDisplay();
            return;
        }
        if(boner == "Target")
        {
            switch (quest.Data["Name"])
            {
                case "Room":
                    var sp = GISLol.Instance.ItemsDict[quest.Data[$"{boner}_Data"]].Sprite;
                    Icon.item = new GISItem();
                    Icon.UpdateDisplay();
                    Icon.displays[0].sprite = sp;
                    Icon.amnt.text = $"x{Converter.NumToRead(quest.Data[$"{boner}_Amount"])}";
                    return;
            }
        }
        switch (quest.Data[$"{boner}_Type"])
        {
            default:
                GISItem dikl = new GISItem(quest.Data[$"{boner}_Data"]);
                if (GISLol.Instance.ItemsDict[dikl.ItemIndex].IsWeapon)
                {
                    switch (quest.Data["Name"])
                    {
                        case "Craft":
                            dikl.Materials = new List<GISMaterial>()
                            {
                                new GISMaterial("Gold"),
                                new GISMaterial("Gold"),
                                new GISMaterial("Gold"),
                            };
                            break;
                        default:
                            dikl.Materials = new List<GISMaterial>()
                            {
                                new GISMaterial("Rock"),
                                new GISMaterial("Rock"),
                                new GISMaterial("Rock"),
                            };
                            break;
                    }
                }
                
                Icon.item = dikl;
                Icon.UpdateDisplay();
                if (boner=="Target")
                Icon.amnt.text = $"x{Converter.NumToRead(quest.Data[$"{boner}_Amount"])}";
                break;
        }
    }
}
