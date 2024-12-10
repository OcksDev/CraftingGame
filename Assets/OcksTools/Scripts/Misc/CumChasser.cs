using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumChasser : MonoBehaviour
{
    public I_Room iroom;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = GetRandomPos();
    }

    public Vector3 GetRandomPos()
    {
        float sz = 7;
        var e = new Vector3 (Random.Range(-sz,sz), Random.Range(-sz, sz), 0) + iroom.transform.position;
        bool can = RandomFunctions.Instance.Dist(e, transform.position) > 5;
        if(can) can = Gamer.Instance.IsPosInBounds(e);
        if (can)
        {
            return e;
        }
        else
        {
            return GetRandomPos();
        }
    }

}
