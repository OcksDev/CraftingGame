using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrafterWeenorUsus : MonoBehaviour
{
    public Button CumCraft;
    // Update is called once per frame
    void FixedUpdate()
    {
        CumCraft.interactable = Gamer.Instance.CanCurrentCraft();
    }
}
