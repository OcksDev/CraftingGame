using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Relay;

public class PickThingymabob : MonoBehaviour
{
    private RelayMoment relay;
    public void GoinGame()
    {
        NetworkManager.Singleton.StartClient();
    }
    public void GoinGameE()
    {
        var p = GetComponent<TMP_InputField>();
        GoinGameE2(p.text);
    }
    public async void GoinGameE2(string code)
    {
        relay = RelayMoment.Instance;

        if (relay.SignInState != "Good")
        {
            var e = await relay.SignIn();
            if (e == 0)
            {
                return;
            }
        }
        int i = await relay.JoinRelay(code);
        if (i == 1)
        {
            NetworkManager.Singleton.StartClient();
            Console.Log("Joined match with code: " + code);
            Gamer.IsMultiplayer = true;
            Gamer.Instance.checks[3] = false;
            Gamer.Instance.UpdateMenus();
        }
        else
        {
            //code goes here for if you failed to join
            Console.Log("Failed to join match with code: " + code, "\"red\"");
        }
    }

    public async void MakeGame()
    {
        var x =  await MakeGame2();
        if(x != "Error")
        {
            relay = RelayMoment.Instance;
            var p = Instantiate(relay.ServerGamerObject, relay.transform.position, relay.transform.rotation, relay.transform);
            p.GetComponent<NetworkObject>().Spawn();
            Gamer.IsMultiplayer = true;
            Gamer.Instance.StartLobby();
            Console.Log("Started match with join code: " + x);
        }
    }
    public async Task<string> MakeGame2()
    {
        relay = RelayMoment.Instance;

        if(relay.SignInState != "Good")
        {
            var e = await relay.SignIn();
            if (e == 0)
            {
                return "Error";
            }
        }

        int i = await relay.CreateRelay();
        if (i == 1)
        {
            Debug.Log(relay.Join_Code);

            NetworkManager.Singleton.StartHost();

            return relay.Join_Code;
        }
        else
        {
            return "Error";
        }
    }
}
