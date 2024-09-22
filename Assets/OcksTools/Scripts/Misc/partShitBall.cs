using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partShitBall : MonoBehaviour
{
    public string type = "Null";
    public float lifetime = 3f;

    public SpriteRenderer spspsp;
    public Transform Particicic;
    private void Start()
    {
        switch (type)
        {
            case "Blood":
                StartCoroutine(bloodyy());
                break;
        }
    }
    public IEnumerator bloodyy()
    {
        spspsp.transform.localScale = new Vector3(0, 0.75f, 1);
        var c = spspsp.color;
        c.a = 0f;
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForFixedUpdate();
            c.a += 0.1f;
            spspsp.transform.localScale += new Vector3(0.14f, 0, 0);
            c.a = Mathf.Clamp(c.a, 0, 0.9f);
            spspsp.color = c;
        }
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForFixedUpdate();
            c.a -= 0.02f;
            spspsp.transform.localScale -= new Vector3(0.02f, 0, 0);
            spspsp.color = c;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }

    }
}
