using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerVOidoidod : MonoBehaviour
{
    public PlayerController playerController;
    public void OnTriggerStay2D(Collider2D collision)
    {
        playerController.CorruptTim(collision);
    }
}
