using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class What : MonoBehaviour
{
    public GISLol huh;
    private void Awake()
    {
        huh.enabled = true;
        GISLol.Instance = huh;
    }

}
