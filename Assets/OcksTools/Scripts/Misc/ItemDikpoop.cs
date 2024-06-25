using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDikpoop : MonoBehaviour
{
    public string Name = "";
    public Image Shirt;
    public TextMeshProUGUI TShirt;  
    public void UpdateDisplay()
    {
        int c = PlayerController.Instance.GetItem(Name);
        gameObject.SetActive(c>0);
        if (c > 0)
        {
            Shirt.sprite = Gamer.Instance.ItemShitDick[Name].Image;
            TShirt.text = $"x{c}";
        }
    }
}
