using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverSexBalls : MonoBehaviour
{
    public float speed;
    public float rotationspeed;
    public Transform myballs;
    public float HomeStrength = 0;
    public NavMeshEntity homie;
    private GameObject homie2;

    private void Start()
    {
        var wank = GetComponent<EnemyHitShit>();
        if(wank!=null)homie = wank.sexballs;
        if(homie!=null)homie2 = homie.target;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        if (myballs != null) myballs.rotation *= Quaternion.Euler(0,0,rotationspeed);
        if(HomeStrength != 0)
        {
            transform.rotation = RotateLock(transform.rotation, Point2DMod2(homie2.transform.position, 0, 0), HomeStrength);
        }
    }
    private Quaternion RotateLock(Quaternion start_rot, Quaternion target, float max_speed)
    {
        return Quaternion.RotateTowards(start_rot, target, max_speed);
    }
    private Quaternion Point2DMod2(Vector3 pos, float offset2, float spread)
    {
        //returns the rotation required to make the current gameobject point at the mouse, untested in 3D.
        var offset = UnityEngine.Random.Range(-spread, spread);
        offset += offset2;
        //Debug.Log(offset);
        Vector3 difference = pos - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset);
        return sex;
    }
}
