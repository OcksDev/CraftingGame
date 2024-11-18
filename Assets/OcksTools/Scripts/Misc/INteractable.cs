using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class INteractable : MonoBehaviour
{
    public string Type = "Crafter";
    public float IneteractDistance = 3;
    public float TextOffsetDist = 3;
    public TextMeshProUGUI DisplaySegsmcnugget;

    private void OnDisable()
    {
        if (DisplaySegsmcnugget != null && DisplaySegsmcnugget.gameObject != null)
        {
            Destroy(DisplaySegsmcnugget.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Gamer.Instance.CanInteractThisFrame || DisplaySegsmcnugget == null) return;
        var e = transform.position;
        if(PlayerController.Instance != null)
        {
            var e2 = PlayerController.Instance.transform.position;
            e.z = 0;
            e2.z = 0;
            bool wanker = (e - e2).magnitude <= IneteractDistance + transform.localScale.x;
            DisplaySegsmcnugget.gameObject.SetActive(wanker);
            if (wanker)
            {
                DisplaySegsmcnugget.transform.position = transform.position + new Vector3(0, TextOffsetDist, 0);
                if (InputManager.IsKeyDown("interact", "player"))
                {
                    Interact();
                }
            }
        }
    }
    private void OnDestroy()
    {
        if(DisplaySegsmcnugget != null && DisplaySegsmcnugget.gameObject != null)
        {
            Destroy(DisplaySegsmcnugget.gameObject);
        }
    }
    public void OnEnable()
    {
        if (Time.time < 0.2f) return;
        var w = Instantiate(Gamer.Instance.textShuingite, transform.position, Quaternion.identity, Tags.refs["DIC"].transform);
        var e = w.GetComponent<TextMeshProUGUI>();
        e.text = InputManager.keynames[InputManager.gamekeys["interact"][0]];
        DisplaySegsmcnugget = e;
    }

    public GISItem cuum;
    public void Interact()
    {
        var g = Gamer.Instance;
        g.CanInteractThisFrame = false;
        if (g.IsFading) return;
        switch(Type)
        {
            case "Crafter":
                g.checks[1] = !g.checks[1];
                g.checks[0] = g.checks[1];
                g.checks[2] = false;
                g.checks[11] = false;
                g.cuumer.Open();
                g.UpdateMenus();
                break;
            case "Vault":
                g.checks[11] = true;
                g.checks[1] = false;
                g.checks[0] = true;
                g.checks[2] = false;
                g.UpdateMenus();
                Gamer.Instance.LoadVaultPage(0);
                break;
            case "StartGame":
                Gamer.Instance.StartCoroutine(Gamer.Instance.StartFade("NextFloor"));
                break;
            case "ContinueRun":
                SaveSystem.Instance.LoadCurrentRun();
                break;
            case "Quest":
                Gamer.Instance.ToggleQuests();
                break;
            case "NextShop":
                Gamer.Instance.StartCoroutine(Gamer.Instance.StartFade("NextShop"));
                break;
            case "Item":
                GetComponent<GroundItem>().AttemptPickup();
                break;
            case "Chest":
                Gamer.Instance.SpawnGroundItem(transform.position, cuum);
                Destroy(gameObject);
                break;
        }
    }
}
