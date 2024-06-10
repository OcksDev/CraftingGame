using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Room : MonoBehaviour
{
    public Room room;
    public GameObject gm;
    public float dist = 94385740398;
    public bool isused = false;
    public int level = 0;

    public void Start()
    {
        transform.rotation = Quaternion.identity;
    }

}
