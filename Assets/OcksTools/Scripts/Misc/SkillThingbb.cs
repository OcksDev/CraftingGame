using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThingbb : MonoBehaviour
{
    public GISItem inititem;
    public int initindex;
    public GISDisplay gup;
    public GISDisplay[] skills;
    public void Insert(int me)
    {
        if(gup.item.ItemIndex != "Empty")
        {
            skills[me].item = gup.item;
            skills[me].UpdateDisplay();
            gup.item = new GISItem("Empty");
            gup.UpdateDisplay();
        }
    }
    public void Swapper(int me)
    {
        var temp = skills[me].item;
        skills[me].item = skills[me+1].item;
        skills[me+1].item = temp;
        skills[me].UpdateDisplay();
        skills[me+1].UpdateDisplay();
    }
    public void Confirm()
    {
        if(gup.item.ItemIndex == "Empty")
        {
            for(int i = 0; i < skills.Length; i++)
            {
                PlayerController.Instance.Skills[i + 1] = new Skill(skills[i].item.ItemIndex);
            }
            Gamer.Instance.AttemptAddLogbookItem(inititem.ItemIndex);
            PlayerController.Instance.Coins -= 10;
            PlayerController.Instance.SetData();
            var ggg = Tags.refs["SkillBuyMenu"].GetComponent<GAMBLING>();
            ggg.displays[initindex].item = new GISItem();
            ggg.displays[initindex].UpdateDisplay();
            Gamer.Instance.ToggleSkillSubMenu();
        }
    }
}
