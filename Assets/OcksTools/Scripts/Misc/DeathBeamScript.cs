using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBeamScript : MonoBehaviour
{
    public bool IsFrog = false;
    public Transform Player;
    public Transform SorceNerd;
    public Vector3 offset = Vector3.zero;
    public BoxCollider2D farddingle;
    public SpriteRenderer fardd;
    float timealive = 0;
    public Color32 sexl;
    public Transform yongle;
    float maxlive = 3f;
    private void Start()
    {
        if (IsFrog) maxlive = 1f;
    }

    private void FixedUpdate()
    {
        timealive += Time.deltaTime*(IsFrog?2f: 1.3f);
        if(farddingle == null)
        {
            Destroy(gameObject);
            return;
        }
        if (IsFrog)
        {
            farddingle.enabled = true;
            fardd.color = sexl;
        }
        else
        {
            farddingle.enabled = timealive >= 1.2f;
            fardd.color = timealive >= 1.2f ? Color.white : sexl;
        }
        UpdatePos();
        if(timealive >= maxlive+1)
        {
            Destroy(gameObject);
        }
    }

    Vector3 sex;

    public void UpdatePos()
    {
        if(SorceNerd != null) sex = SorceNerd.position + offset;
        if (yongle == null) yongle = fardd.transform;
        float sz = 0.04f;
        float size = 0;
        if(timealive <= maxlive)
        {
            size = Mathf.Sin(Mathf.Clamp01(timealive) * Mathf.PI / 2);
        }
        else
        {
            size = Mathf.Sin(Mathf.Clamp01(1-(timealive- maxlive)) * Mathf.PI / 2);
        }
        transform.localScale = new Vector3(IsFrog?size:1, size, 1);
        transform.position = sex;
        if (timealive >= 1.2f || IsFrog)
        {
            transform.rotation = RotateLock(transform.rotation, PointAtPoint2D(Player.position, 0), IsFrog? 1f: 0.3f);
            if(!IsFrog)transform.position += new Vector3(Random.Range(-sz, sz), Random.Range(-sz, sz), 0);
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
