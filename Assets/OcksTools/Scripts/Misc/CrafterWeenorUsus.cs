using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrafterWeenorUsus : MonoBehaviour
{
    public string type = "Craft";
    public Button CumCraft;
    // Update is called once per frame
    void FixedUpdate()
    {
        switch (type)
        {
            default:
                CumCraft.interactable = Gamer.Instance.CanCurrentCraft();
                break;
            case "Graft":
                CumCraft.interactable = Gamer.Instance.CanCurrentGraft();
                break;
            case "Aspect":
                CumCraft.interactable = Gamer.Instance.CanCurrentAspect();
                break;
            case "Repair":
                CumCraft.interactable = Gamer.Instance.CanCurrentRepair();
                break;
            case "Transmute2":
                CumCraft.interactable = Gamer.Instance.CanCurrentTrans2();
                break;
        }
        
    }
}
