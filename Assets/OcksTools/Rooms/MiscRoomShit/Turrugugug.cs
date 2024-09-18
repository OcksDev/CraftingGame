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
            var wank = e.gm.transform.position;
            var offsex = transform.rotation * new Vector3(0, 2.5f, 0);
            if(e.type== "Enemy")
            {
                e.entity.beans.nextPosition = PlayerController.Instance.transform.position - new Vector3(0,10,10);
            }
            else
            {
                e.gm.transform.position = wank + offsex;
            }
            //e.entity.beans.destination = e.gm.transform.position;
        }
    }
}
