using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsChecker : MonoBehaviour
{
    public bool IMINBOUNDS = false;
    void Update()
    {
        IMINBOUNDS = Gamer.Instance.IsPosInBounds(transform.position);
    }
}
