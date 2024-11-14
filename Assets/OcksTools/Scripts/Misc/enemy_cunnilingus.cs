using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class enemy_cunnilingus : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUGUI;
    private void Awake()
    {
        OXComponent.StoreComponent(this);
    }

    private void FixedUpdate()
    {
        TextMeshProUGUI.text = Gamer.Instance.EnemiesExisting.Count.ToString();
    }
}
