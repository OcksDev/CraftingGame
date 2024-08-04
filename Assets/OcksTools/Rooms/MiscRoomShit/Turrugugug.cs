using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrugugug : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Shart(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Shart(collision.gameObject);
    }


    public void Shart(GameObject a)
    {

        var e = Gamer.Instance.GetObjectType(a);
        if (e.type == "Player" || e.type == "Enemy")
        {
            e.gm.transform.position += transform.rotation * new Vector3(0, 2.5f, 0);
        }
    }
}
