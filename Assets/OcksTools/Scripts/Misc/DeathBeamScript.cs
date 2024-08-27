using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBeamScript : MonoBehaviour
{
    public Transform Player;
    public Transform SorceNerd;
    public BoxCollider2D farddingle;
    public SpriteRenderer fardd;
    float timealive = 0;
    public Color32 sexl;
    public Transform yongle;
    private void FixedUpdate()
    {
        timealive += Time.deltaTime*1.3f;
        if(farddingle == null)
        {
            Destroy(gameObject);
            return;
        }
        farddingle.enabled = timealive >= 1.2f;
        fardd.color = timealive >= 1.2f?Color.white:sexl;
        UpdatePos();
        if(timealive >= 4)
        {
            Destroy(gameObject);
        }
    }

    Vector3 sex;

    public void UpdatePos()
    {
        if(SorceNerd != null) sex = SorceNerd.position;
        if (yongle == null) yongle = fardd.transform;
        float sz = 0.04f;
        float size = 0;
        if(timealive <= 3f)
        {
            size = Mathf.Sin(Mathf.Clamp01(timealive) * Mathf.PI / 2);
        }
        else
        {
            size = Mathf.Sin(Mathf.Clamp01(1-(timealive-3)) * Mathf.PI / 2);
        }
        transform.localScale = new Vector3(1, size, 1);
        transform.position = sex;
        if (timealive >= 1.2f)
        {
            transform.rotation = RotateLock(transform.rotation, PointAtPoint2D(Player.position, 0), 0.3f);
            transform.position += new Vector3(Random.Range(-sz, sz), Random.Range(-sz, sz), 0);
        }

    }
    private Quaternion PointAtPoint2D(Vector3 location, float spread)
    {
        // a different version of PointAtPoint with some extra shtuff
        //returns the rotation the gameobject requires to point at a specific location
        var offset = UnityEngine.Random.Range(-spread, spread);

        //Debug.Log(offset);
        Vector3 difference = NoZ(location) - NoZ(transform.position);
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset);
        return sex;
    }
    public Vector3 NoZ(Vector3 s)
    {
        s.z = 0;
        return s;
    }
    private Quaternion RotateLock(Quaternion start_rot, Quaternion target, float max_speed)
    {
        return Quaternion.RotateTowards(start_rot, target, max_speed);
    }


}
