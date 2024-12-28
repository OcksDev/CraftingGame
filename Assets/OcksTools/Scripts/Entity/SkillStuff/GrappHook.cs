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
        if(islatched)
        {
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
            var dir = ((x1 - x2).normalized*2 + dad.moveintent) * strength;
            dad.momentum += (Vector2)dir;
        }
    }

    Transform target;
    private void LateUpdate()
    {
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
    }
    public void End()
    {
        SkillDad.IsHeld = false;
        Destroy(gameObject);
    }
}
