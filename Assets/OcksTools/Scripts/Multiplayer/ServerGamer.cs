using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerGamer : NetworkBehaviour
{
    private static ServerGamer instance;

    // public NetworkVariable<int> PlayerNum = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // FixedString128Bytes
    public static ServerGamer Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (Instance == null) instance = this;
    }


    /* working code, commented out to prevent error messages when importing oxtools*/
    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectServerRpc(int refe, Vector3 pos, Quaternion rot, string id, string data, string hdata)
    {
        SpawnObjectClientRpc(refe, pos, rot, id, data, hdata);
    }

    [ClientRpc]
    public void SpawnObjectClientRpc(int refe, Vector3 pos, Quaternion rot, string id, string data = "", string hdata = "")
    {
        if (id == RandomFunctions.Instance.ClientID) return;

        RandomFunctions.Instance.SpawnObject(refe, gameObject, pos, rot, false, data, hdata);
    }

    private bool ShowLogs = true;

    [ServerRpc(RequireOwnership = false)]
    public void MessageServerRpc(string id, string type, string data, string data2 = "", string data3 = "")
    {
        if (ShowLogs) Console.Log($"Sent DM: {id.Substring(0,4)}, {type}, {data}, {data2}, {data3}");
        RecieveMessageClientRpc(id, type, data, data2, data3);
    }

    [ClientRpc]
    public void RecieveMessageClientRpc(string id, string type, string data, string data2, string data3)
    {
        var cid = RandomFunctions.Instance.ClientID;
        if (id == cid) return;
        if (ShowLogs) Console.Log($"Got DM: {id.Substring(0, 4)}, {type}, {data}, {data2}, {data3}");
        string ID = "";
        string c = "";
        string p = "";
        switch (type)
        {
            case "initattack":
                Tags.dict[data].GetComponent<PlayerController>().StartAttack();
                break;
            case "WhoAmI":
                if (Gamer.Instance.IsHost)
                {
                    var id2 = ulong.Parse(data);
                    foreach (var e in Gamer.Instance.Players)
                    {
                        if (e.sexer.NetworkObjectId == id2)
                        {
                            var epe = RandomFunctions.Instance.GenerateBlankHiddenData();
                            e.spawnData.Hidden_Data = epe;
                            e.spawnData.FardStart();
                            MessageServerRpc(cid, "FoundWhoAm", data, RandomFunctions.Instance.ListToString(epe));
                            break;
                        }
                    }
                }
                break;
            case "WhoTheFuckIsThisYo":
                if (Gamer.Instance.IsHost)
                {
                    var id2 = ulong.Parse(data);
                    foreach (var e in Gamer.Instance.Players)
                    {
                        if (e.sexer.NetworkObjectId == id2)
                        {
                            MessageServerRpc(cid, "FoundWhoAm", data, RandomFunctions.Instance.ListToString(e.spawnData.Hidden_Data));
                            break;
                        }
                    }
                }
                break;
            case "PAtt":
                var pp = Tags.dict[data].GetComponent<PlayerController>();
                pp.StartAttack();
                break;
            case "FoundWhoAm":
                if (!Gamer.Instance.IsHost)
                {
                    foreach (var e in Gamer.Instance.Players)
                    {
                        if (e.sexer.NetworkObjectId.ToString() == data)
                        {
                            if (e.spawnData.Hidden_Data.Count != 0) return;
                            var epe = RandomFunctions.Instance.StringToList(data2);
                            e.spawnData.Hidden_Data = epe;
                            e.spawnData.FardStart();
                            break;
                        }
                    }
                }
                break;
            case "Qvar":
                p = data;
                ID = data2;
                c = "";
                if (Tags.customdata.ContainsKey(p))
                {
                    if (Tags.customdata[p].ContainsKey(ID))
                    {
                        c = Tags.customdata[p][ID];
                    }
                    else
                    {
                        Tags.customdata[p].Add(ID, c);
                    }
                }
                else
                {
                    Tags.customdata.Add(p, new Dictionary<string, string>() { { ID, c } });
                }
                MessageServerRpc(cid, "var", p, ID, c);
                break;
            case "var":
                p = data;
                ID = data2;
                c = data3;
                if (Tags.customdata.ContainsKey(p))
                {
                    if (Tags.customdata[p].ContainsKey(ID))
                    {
                        Tags.customdata[p][ID] = c;
                    }
                    else
                    {
                        Tags.customdata[p].Add(ID, c);
                    }
                }
                else
                {
                    Tags.customdata.Add(p, new Dictionary<string, string>() { { ID, c } });
                }
                break;
            default:
                break;
        }
    }


    //chat related method
    [ServerRpc(RequireOwnership = false)]
    public void SendChatMessageServerRpc(string id, string message, string hex)
    {
        RecieveChatMessageClientRpc(id, message, hex);
    }

    //chat related method
    [ClientRpc]
    public void RecieveChatMessageClientRpc(string id, string message, string hex)
    {
        if (id == RandomFunctions.Instance.ClientID) return;

        ChatLol.Instance.WriteChat(message, hex);
    }

}
