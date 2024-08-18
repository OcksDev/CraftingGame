using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 0.1f;
    public string Banan = "Arrow";
    [HideInInspector]
    public float life = 0f;
    float targetlife = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        switch (Banan)
        {
            case "Boomerang":
                targetlife = 0.3f;
                break;
            default:
                targetlife = 0.2f;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.up * speed;
        if((life += Time.deltaTime) > targetlife)
        {
            var a = GetComponent<HitBalls>();
            a.StartCoroutine(a.WaitForDIe());
            //speed = 0f;
        }
    }
}
