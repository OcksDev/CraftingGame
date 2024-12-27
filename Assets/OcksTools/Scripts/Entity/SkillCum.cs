using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCum : MonoBehaviour
{
    public int SkillIndex = 0;
    public Image SkillIcon;
    public Image Cooldownsex;
    public TextMeshProUGUI Counter;
    public TextMeshProUGUI Charges;
    public TextMeshProUGUI Keybindn;

    public void UpdateRare()
    {
        if(PlayerController.Instance == null) return;
        var me = PlayerController.Instance.Skills[SkillIndex];
        bool a = me.Name != "Empty";
        gameObject.SetActive(a);
        if (a)
        {
            SkillIcon.sprite = GISLol.Instance.SkillsDict[me.Name].Image;
            switch (SkillIndex)
            {
                case 0:
                    Keybindn.text = InputManager.keynames[InputManager.gamekeys[$"dash"][0]].ToUpper();
                    break;
                default:
                    Keybindn.text = InputManager.keynames[InputManager.gamekeys[$"skill{SkillIndex}"][0]].ToUpper();
                    break;
            }
            UpdateCommon();
        }
    }

    private void FixedUpdate()
    {
        UpdateCommon();
    }

    public void UpdateCommon()
    {
        var me = PlayerController.Instance.Skills[SkillIndex];
        switch (SkillIndex)
        {
            case 0:
                var e = PlayerController.Instance;
                var total = (e.DashCoolDown) / (e.MaxDashCooldown);
                var perc = 1 - (total % 1);
                if(total >= 3) perc = 0;
                Cooldownsex.fillAmount = perc;
                Charges.text = total > 1 ? $"{(int)total}" : "";
                break;
            default:
                Cooldownsex.fillAmount = me.Timer / me.MaxCooldown;
                Charges.text = me.Stacks > 0 ?$"{me.Stacks}":"";
                break;
        }
    }



}
