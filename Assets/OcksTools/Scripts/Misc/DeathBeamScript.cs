using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBeamScript : MonoBehaviour
{
    public bool IsFrog = false;
    public bool IsBossrock = false;
    public Transform Player;
    public Transform SorceNerd;
    public Vector3 offset = Vector3.zero;
    public BoxCollider2D farddingle;
    public SpriteRenderer fardd;
    float timealive = 0;
    public Color32 sexl;
    public Transform yongle;
    float maxlive = 3f;
    Color ocolor;
    public int multdir = 1;
    private void Start()
    {
        ocolor = fardd.color;
        if (IsFrog) maxlive = 0.7f;
    }

    private void FixedUpdate()
    {
        timealive += Time.deltaTime*(IsFrog?2f: 1.3f);
        if(farddingle == null)
        {
            Destroy(gameObject);
            return;
        }
        if (IsBossrock)
        {
            farddingle.enabled = timealive >= 1.2f;
            fardd.color = timealive >= 1.2f ? ocolor : sexl;
        }
        else
        {
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
        if (IsBossrock)
        {
            if (timealive >= 1.2f)
            {
                transform.rotation = transform.rotation * Quaternion.Euler(0,0,30*Time.deltaTime*multdir);
            }
        }
        else
        {
            if (timealive >= 1.2f || IsFrog)
            {
                if (Player == null) return;
                transform.rotation = RotateLock(transform.rotation, PointAtPoint2D(Player.position, 0), IsFrog ? 0.4f * RandomFunctions.EaseIn(timealive / maxlive) : 0.25f);
                if (!IsFrog) transform.position += new Vector3(Random.Range(-sz, sz), Random.Range(-sz, sz), 0);
            }
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
