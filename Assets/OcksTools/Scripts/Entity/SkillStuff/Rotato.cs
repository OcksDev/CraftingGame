using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotato : MonoBehaviour
{
    public PlayerController controller;

    public float rotund = 1;
    public float rotunddist = 5;
    public List<GameObject> seg = new List<GameObject>();
    //public BoxCollider2D b = false;
    float life = 0;
    float rot = 0;
    private void Start()
    {
        rotund = (Random.Range(0,2)*2)-1;
        rot = Random.Range(0, 360f);
        rotunddist = Random.Range(3, 6f);
        var ww = Quaternion.Euler(0, 0, rot);
        Update();
    }
    private void Update()
    {
        rot += Time.deltaTime * 120 * rotund;
        life += Time.deltaTime;
        if(controller == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            var ww = Quaternion.Euler(0, 0, rot);
            transform.position = controller.transform.position + (ww * new Vector3(0,rotunddist + Mathf.Sin(life*2),0));
            transform.rotation = ww * Quaternion.Euler(0,0,45);
        }
    }
}
