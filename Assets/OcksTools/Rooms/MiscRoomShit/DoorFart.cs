using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFart : MonoBehaviour
{
    public BoxCollider2D boxcol;
    public BoxCollider2D teleboxcol;
    public SpriteRenderer visual;
    public SpriteRenderer visual2electricsex;
    public bool IsGood = true;
    private void Start()
    {
        if (!IsGood) return;
        Gamer.Instance.StupidAssDoorDoohickies.Add(gameObject);
        StartCoroutine(fartmyballs());
    }

    public IEnumerator fartmyballs()
    {
        boxcol.enabled = true;
        teleboxcol.enabled = true;
        var c = visual.color;
        c.a = 0;
        visual.color = c;
        var cc = 0.01f * Mathf.PI;
        var v0 = Vector3.zero;
        var v1 = new Vector3(0,-3,0);
        for (int i = 0; i <= 50; i++)
        {
            float f = i * cc;
            c.a += 0.03f;
            visual.transform.localPosition = Vector3.Lerp(v1, v0, Mathf.Sin(f));
            visual.color = c;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitUntil(() => { return !Gamer.Instance.InRoom; });
        yield return new WaitForSeconds(0.5f);
        boxcol.enabled = false;
        teleboxcol.enabled = false;
        visual2electricsex.enabled = false;
        for (int i = 50; i <= 100; i++)
        {
            float f = i * cc;
            c.a -= 0.04f;
            visual.transform.localPosition = Vector3.Lerp(v1, v0, Mathf.Sin(f));
            visual.color = c;
            yield return new WaitForFixedUpdate();
        }
        Gamer.Instance.StupidAssDoorDoohickies.Remove(gameObject);
        Destroy(gameObject);
    }
}
