using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
[System.Serializable]
public class Tags : MonoBehaviour
{
    public static Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();
    public static Dictionary<GameObject, string> gameobject_dict = new Dictionary<GameObject, string>();
    public static Dictionary<string, SpawnData> SDs = new Dictionary<string, SpawnData>();
    public static List<string> tag1 = new List<string>();
    [HideInInspector]
    [DoNotSerialize]
    public static Dictionary<string, GameObject> refs = new Dictionary<string, GameObject>();
    public static Dictionary<string, Dictionary<string, string>> customdata = new Dictionary<string, Dictionary<string, string>>();

    /*
     * Tags Help:
     * To check if a gameobject has a certain tag: see if the tag list contains the string ID of the gameobject(s)
     * string ID of a gamobject should be stored in SpawnData.Data[0]
     * 
     */

    public static Tags Instance;

    public void Awake()
    {
        if(Instance == null) Instance= this;
    }


    public static void ClearAllOf(string key)
    {
        //should go and clear any instance of the ID found in any tag
        if (dict.ContainsKey(key) && gameobject_dict.ContainsKey(dict[key])) gameobject_dict.Remove(dict[key]);
        if (dict.ContainsKey(key)) dict.Remove(key);
        if (SDs.ContainsKey(key)) SDs.Remove(key);
        if (tag1.Contains(key)) tag1.Remove(key);
    }
    public static void GarbageCleanup()
    {
        //should locate any null gameobject references and remove them.
        int i = 0;
        while(i < dict.Count)
        {
            var d = dict.ElementAt(i);
            if (d.Value == null)
            {
                ClearAllOf(d.Key);
                i--;
            }
            i++;
        }
    }

    public static void DefineReference(GameObject boner, string id)
    {
        if (!dict.ContainsKey(id)) dict.Add(id, boner);
        if (!gameobject_dict.ContainsKey(boner)) gameobject_dict.Add(boner, id);
    }

    public static void SetRef(string name, GameObject ob)
    {
        if (refs.ContainsKey(name))
        {
            refs[name] = ob;
        }
        else
        {
            refs.Add(name, ob);
        }
    }
}

[Serializable]
public class OcksNetworkVar
{
    public string Data = "";
    public string ID = "";
    public string ObjectID;
    public OcksNetworkVar(string iD, string objectID)
    {
        ID = iD;
        ObjectID = objectID;
    }
    public OcksNetworkVar()
    {
    }
    public void SetCreds(string objectid, string id)
    {
        ObjectID = objectid;
        ID = id;
    }
    public string GetValue()
    {
        var p = ObjectID;
        Data = Tags.customdata[p][ID];
        return Data;
    }
    public void Query()
    {
        var p = ObjectID;
        var c = "WAITING FOR DATA";
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
        ServerGamer.Instance.MessageServerRpc(RandomFunctions.Instance.ClientID, "var", ObjectID, ID);
    }
    public void SetValue(string c)
    {
        if(ServerGamer.Instance != null)
        {
            Data = c;
            var p = ObjectID;
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
            ServerGamer.Instance.MessageServerRpc(RandomFunctions.Instance.ClientID, "var", ObjectID, ID, c);
        }
        else
        {
            Gamer.Backup.Add(new List<string>() { ObjectID, ID, c });
        }
    }
}
