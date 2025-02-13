using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Networking;
using UnityEngine;

public class PLayerNet : NetworkBehaviour
{
    public NetworkVariable<int> PlayerNum = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<FixedString128Bytes> PlayerName = new NetworkVariable<FixedString128Bytes>("Sex", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        StartCoroutine(Testy());
    }
    public IEnumerator Testy()
    {
        yield return new WaitForSeconds(0.5f);
        if (GetComponent<NetworkObject>().IsOwner)
        {
            PlayerNum.Value = Random.Range(0, 6969);
            PlayerName.Value = "Cumrag" + PlayerNum.Value;
            Debug.Log("SET NAME");
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Name: " + PlayerName.Value);
        }
        else
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("Name: " + PlayerName.Value);
        }
    }
}
