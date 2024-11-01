using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeWhenYourMom : MonoBehaviour
{
    public Image img;
    public Transform target;
    public float speed = 5;
    public float life = 1f;
    float time = 0;
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        transform.localScale = Vector3.one * (1 - RandomFunctions.EaseIn(time / life, 4));
        if ((time += Time.deltaTime) > life)
        {
            Destroy(gameObject);
        }
    }
}
