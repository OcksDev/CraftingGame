using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 0.1f;
    float life = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.up * speed;
        if((life += Time.deltaTime) > 5f)
        {
            Destroy(gameObject);
        }
    }
}
