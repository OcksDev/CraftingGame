using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineKiller : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform coolpos;
    public Transform coolerpos;
    public Color banana;
    float life;
    Vector3[] poses = new Vector3[2];
    // Update is called once per frame
    void FixedUpdate()
    {
        life += Time.deltaTime;
        banana.a -= 0.04f;
        lineRenderer.startColor = banana;
        lineRenderer.endColor = banana;
        if (coolpos != null)
            poses[0] = coolpos.position;
        if (coolerpos != null)
            poses[1] = coolerpos.position;
        lineRenderer.SetPosition(0, poses[0]);
        lineRenderer.SetPosition(4, poses[1]);
        float sz = 0.3f;
        lineRenderer.SetPosition(1, Vector3.Lerp(poses[0], poses[1], 0.25f) + new Vector3(Random.Range(sz, -sz), Random.Range(sz, -sz),0));
        lineRenderer.SetPosition(2, Vector3.Lerp(poses[0], poses[1], 0.50f) + new Vector3(Random.Range(sz, -sz), Random.Range(sz, -sz),0));
        lineRenderer.SetPosition(3, Vector3.Lerp(poses[0], poses[1], 0.75f) + new Vector3(Random.Range(sz, -sz), Random.Range(sz, -sz),0));
        if (life > 1) Destroy(gameObject);
    }
}
