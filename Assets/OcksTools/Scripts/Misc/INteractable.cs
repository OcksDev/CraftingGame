using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INteractable : MonoBehaviour
{
    public string Type = "Crafter";
    public float IneteractDistance = 3;
    // Update is called once per frame
    void Update()
    {
        var e = transform.position;
        if(PlayerController.Instance != null)
        {
            var e2 = PlayerController.Instance.transform.position;
            e.z = 0;
            e2.z = 0;
            if (InputManager.IsKeyDown(InputManager.gamekeys["interact"]) && (e - e2).magnitude <= IneteractDistance + transform.localScale.x)
            {
                Interact();
            }
        }
    }

    public void Interact()
    {
        var g = Gamer.Instance;
        switch(Type)
        {
            case "Crafter":
                g.checks[1] = !g.checks[1];
                g.checks[0] = g.checks[1];
                g.checks[2] = false;
                g.cuumer.Open();
                g.UpdateMenus();
                break;
            case "StartGame":
                StartCoroutine(Gamer.Instance.StartFade("NextFloor"));
                break;
        }
    }
}
