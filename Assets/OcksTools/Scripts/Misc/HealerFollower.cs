using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerFollower : MonoBehaviour
{
    public PlayerController SexChaser;
    public List<GameObject> others = new List<GameObject>();
    public Vector3 vel;
    public float sexer;
    // Update is called once per frame

    private void Start()
    {
        transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        Gamer.Instance.healers.Add(gameObject);
        sexer = 0.5f;
        vel = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
    }


    void FixedUpdate()
    {
        sexer += Time.deltaTime;
        if(SexChaser == null)
        {
            SexMyNuts();
            return;
        }
        vel *= 0.98f;
        Vector3 sex = Vector3.MoveTowards(transform.position, SexChaser.transform.position, 0.1f * sexer);
        sex += vel;
        others = Gamer.Instance.healers;
        foreach (GameObject obj in others)
        {
            if (obj == null || obj==gameObject) continue;
            var e = sex - obj.transform.position;
            e = ((e.normalized) / 100) / e.magnitude;
            sex += e;
        }

        transform.position = sex;
        if ((transform.position - SexChaser.transform.position).magnitude <= 0.5f) SexMyNuts();
    }

    public void SexMyNuts()
    {
        if(SexChaser != null)
        {
            SexChaser.entit.Health += 10f;
        }
        Destroy(gameObject);
    }

}
