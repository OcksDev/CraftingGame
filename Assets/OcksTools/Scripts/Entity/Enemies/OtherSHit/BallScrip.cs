using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScrip : MonoBehaviour
{
    public Vector3 directionsons = Vector3.zero;
    public float speed;
    public float PullStrnehgth = 1;
    float life;
    public float ChargePOerc = 0f;
    private void Update()
    {
        transform.position += directionsons * Time.deltaTime * speed;
        if ((life += Time.deltaTime) > 12)
        {
            var a = GetComponent<EnemyHitShit>();
            a.Kill();
            //speed = 0f;
        }
    }
    private void FixedUpdate()
    {
        if (PlayerController.Instance != null)
        {
            var w = PlayerController.Instance.transform.position;
            float dist = RandomFunctions.Instance.Dist(transform.position, w);
            if(dist <= 13)
            PlayerController.Instance.rigid.velocity += (Vector2)(ChargePOerc * PullStrnehgth * (transform.position - w))/Mathf.Max(dist, 1f);
        }
    }

}
