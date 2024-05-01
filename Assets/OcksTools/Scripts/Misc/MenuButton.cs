using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public string type = "";
    public void Click()
    {
        switch (type)
        {
            case "Host":
                Gamer.Instance.GetComponent<PickThingymabob>().MakeGame();
                break;
            case "Single":
                Gamer.Instance.StartLobby();
                break;
            case "Join":
                Gamer.Instance.GetComponent<PickThingymabob>().GoinGameE2(GetComponent<TMP_InputField>().text);
                break;
            case "MainMenu":
                Gamer.Instance.MainMenu();
                break;
        }
    }
}
