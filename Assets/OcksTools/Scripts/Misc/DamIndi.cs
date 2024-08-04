using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamIndi : MonoBehaviour
{
    float t = 1;
    public TextMeshProUGUI sex;
    public TextAnimator shexysex;
    public int critlevel = -1;
    public bool NoCLor = false;
    public float r = 0;
    float size = 2;
    float spd = 3;
    public Color[] colors = new Color[0];
    public void Start()
    {
        float ee = 1.4f;
        r = Random.Range(-ee, ee);
        transform.localScale = new Vector3(2, 2, 2);
        if(critlevel > -1)
        {
            size = 2.5f;
            spd = 2.5f;
            if (critlevel > 0)
            {
                size = 3f;
                spd = 2.5f;
                if(critlevel > 1)
                {
                    var anim = new TextAnim();
                    if ((critlevel+1) > (colors.Length-1))
                        anim.Type = "Shake";
                    else
                        anim.Type = "ShakeLess";
                    anim.startindex = 0;
                    anim.endindex = 1000000000;
                    shexysex.anims.Add(anim);
                }
            }
        }
        if (!NoCLor)
        {
            var e = colors[Mathf.Clamp(critlevel + 1, 0, colors.Length - 1)];
            sex.color = e;
        }
        sex.ForceMeshUpdate();
    }
    private void FixedUpdate()
    {
        t += Time.deltaTime * spd;
        transform.Rotate(0, 0, r);
        transform.position += transform.up * Time.deltaTime;
        float g = size / (t);
        transform.localScale = new Vector3(g, g, g);
        var e = sex.color;
        e.a = 1 - ((t-1)/3);
        sex.color = e;
        if(t >= 3.5f) Destroy(gameObject);
    }
}
