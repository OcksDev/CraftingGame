using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrappHook : MonoBehaviour
{
    public PlayerController dad;
    public Skill SkillDad;
    public float speed = 5;
    public float strength = 5;
    public CircleCollider2D cic;
    public LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        cic  = GetComponent<CircleCollider2D>();
    }
    bool islatched = false;
    // Update is called once per frame
    Vector3 oldpos = Vector3.zero;
    float oldrotz = 0;
    void Update()
    {
        if (dad == null || !SkillDad.IsHeld)
        {
            End();
            return;
        }
        if (!islatched)
        {
            float dist = Time.deltaTime * speed;
            RaycastHit2D[] results = new RaycastHit2D[10];
            cic.Cast(transform.right, results, dist);

            foreach(var a in results)
            {
                if (a.collider == null) continue;
                var tp = Gamer.Instance.GetObjectType(a.collider.gameObject);
                Console.Log(tp.type);
                switch (tp.type)
                {
                    case "Enemy":
                    case "Door":
                    case "Wall":
                        islatched = true;
                        cic.enabled = false;
                        dist = a.distance + 0.5f;
                        oldpos = a.collider.transform.position;
                        oldrotz = a.collider.transform.rotation.eulerAngles.z;
                        target = a.collider.transform;
                        str += 0.3f;
                        goto wankout;
                }
            }
        wankout:;
            transform.position += transform.right *dist;
        }
    }


    private void FixedUpdate()
    {
        if (dad == null || !SkillDad.IsHeld)
        {
            End();
            return;
        }
        if (islatched)
        {
            str *= 0.90f;
            spd *= 1.15f;
            var x1 = transform.position;
            x1.z = 0;
            var x2 = dad.transform.position;
            x2.z = 0;
            var dist = (x1 - x2).magnitude;
            if (dist <= 1)
            {
                End();
                return;
            }
            var dir = ((x1 - x2).normalized * 2 + dad.moveintent) * strength;
            dad.momentum += (Vector2)dir;
        }
        else
        {
            speed *= 0.96f;
        }
        str *= 0.97f;
        spd *= 0.975f;
    }
    float life = 0;
    float spd = 30;
    float str = 1;
    Transform target;
    private void LateUpdate()
    {
        life += Time.deltaTime * spd;
        if(islatched && target == null)
        {
            End();
            return;
        }
        if (islatched)
        {
            var x = target.position;
            if(oldpos != x)
            {
                var diff = x - oldpos;
                transform.position += diff;
                oldpos = x;
            }
            var z = target.rotation.eulerAngles.z;
            if(oldrotz != z)
            {
                var diff = z - oldrotz;
                var wankoff = transform.position - x;
                transform.position = x;
                wankoff = Quaternion.Euler(0,0,diff) * wankoff;
                transform.position += wankoff;
                oldrotz = 0;
            }
        }
        var tg = Quaternion.Inverse(transform.rotation) * (dad.transform.position - line.transform.position);
        line.SetPosition(0, Vector3.zero);
        int amnt = 15;
        float total = Mathf.PI / amnt;
        //Debug.Log(total);
        for(int i = 1; i < amnt; i++)
        {
            var off = (life + i / 1.5f);
            float cum = 1;
            if (i == amnt - 1) cum = 0.5f;
            line.SetPosition(i, Vector3.Lerp(Vector3.zero, tg, (1f/amnt)*i) + Quaternion.Euler(0, 0, 90) * tg.normalized * Mathf.Sin(off) * str * cum);
        }
        line.SetPosition(amnt, tg);
    }
    public void End()
    {
        if (!SkillDad.IsHeld && !islatched)
        {
            SkillDad.Timer = 0.1f;
        }
        SkillDad.IsHeld = false;
        Destroy(gameObject);
    }
}
