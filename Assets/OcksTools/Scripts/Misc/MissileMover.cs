using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMover : MonoBehaviour
{
    public HitBalls hitbal;
    public GameObject target;
    public float speed = 5f;
    public float turnspeed = 45f;
    int offs;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.up * Time.deltaTime * speed;
        if(target == null || (offs+=1)>10)
        {
            offs = 0;
            float dist = 100000;
            foreach(var e in Gamer.Instance.EnemiesExisting)
            {
                var d = RandomFunctions.Instance.Dist(transform.position, e.transform.position);
                if (d < dist)
                {
                    target = e.gameObject;
                    dist = d;
                }
            }
        }
        if(target != null)
        {
            transform.rotation = RotateLock(transform.rotation, PointFromTo2D(target.transform.position, transform.position, -90),  turnspeed * Time.deltaTime);
        }
    }
    private Quaternion PointFromTo2D(Vector3 from_pos, Vector3 to_pos, float offset2)
    {
        //returns the rotation required to make the current gameobject point at the mouse, this method is 2D only.
        Vector3 difference = from_pos - to_pos;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset2);
        return sex;
    }
    public static Quaternion RotateLock(Quaternion start_rot, Quaternion target, float max_speed)
    {
        return Quaternion.RotateTowards(start_rot, target, max_speed);
    }

}
