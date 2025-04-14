using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraLol : MonoBehaviour
{
    public static CameraLol instance;
    public Vector3 targetpos = new Vector3(0, 0, 0);
    public Vector3 ppos = new Vector3(0, 0, 0);
    public List<List<float>> shakeo = new List<List<float>>();
    private List<Vector4> shoveo = new List<Vector4>();
    public float SmoothMod = 1f;
    // Start is called before the first frame update
    public static CameraLol Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (Instance == null) instance = this;
    }
    private void FixedUpdate()
    {
        int i = -1;
        foreach(var shake in shakeo)
        {
            i++;
            shake[0] *= shake[1];
        }
        while (i>=0)
        {
            if (shakeo.Count > 0 && shakeo[i][0] <= 0.01f)
            {
                shakeo.RemoveAt(i);
                //i++;
            }
            i--;
        }
    }


    public float offset(float x)
    {
        var y = Mathf.Round(x * 16) / 16;
        x -= y;
        return x;
    }


    Vector3 oldp = Vector3.zero;
    private void LateUpdate()
    {
        /* some an example for what hurting a player could be like
        if (Input.GetKeyDown(KeyCode.N))
        {
            ppos.y += 1f;
            Shake(0.4f, 0.8f);
        }
        */
        transform.position = ppos;
        var a = new Vector3(offset(ppos.x) * SmoothMod, offset(ppos.y) * SmoothMod, 0);
        Tags.refs["RenderHolder"].transform.position = Tags.refs["NormalRender"].transform.position + a;
        if (Gamer.GameState != "Main Menu")
        {
            //handles getting the mouse position and making the camera adjust to move to it
            Vector3 p = Vector3.zero;
            if (Gamer.WithinAMenu)
            {
                p = oldp;
            }
            else
            {
                p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                p -= transform.position;
                p /= 3.5f;
                p *= Gamer.Instance.CameraMouseMult;
                oldp = p;
            }
            // "pos" is the location the camera tries to get to
            p += targetpos;
            p.z = -10;
            var z = Vector3.MoveTowards(transform.position, p, Dist(p, transform.position) * 8 * Time.deltaTime);
            /* zz can be the max size the camera can go to, remove/change as needed
            float zz = 99999;
            z.x = Mathf.Clamp(z.x, -zz, zz);
            z.y = Mathf.Clamp(z.y, -zz, zz);
            */
            ppos = z;
            Vector3 ss = ppos;
            if(Time.timeScale > 0)
            foreach (var shake in shakeo)
            {
                float f1 = 1;
                float f2 = 1;
                if (shake[2] != 0 || shake[3] != 0)
                {
                    f1 = shake[2];
                    f2 = shake[3];
                }
                float ff1 = UnityEngine.Random.Range(-f1, f1) * shake[0];
                float ff2 = UnityEngine.Random.Range(-f2, f2) * shake[0];
                ss.x += ff1;
                ss.y += ff2;
            }
            transform.position = ss;
        }
    }

    public void Shake(float shake, float falloff, int dir1 = 0, int dir2 = 0)
    {
        List<float> balls = new List<float>
        {
            shake,
            falloff,
            dir1,
            dir2,
        };
        shakeo.Add(balls);
    }

    private float Dist(Vector3 p1, Vector3 p2)
    {
        float distance = Mathf.Sqrt(
                Mathf.Pow(p2.x - p1.x, 2f) +
                Mathf.Pow(p2.y - p1.y, 2f) +
                Mathf.Pow(p2.z - p1.z, 2f));
        return distance;
    }

}
