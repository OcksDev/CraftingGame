using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnchorBald : MonoBehaviour
{
    public GameObject target;
    private float z = 0;
    private void Awake()
    {
        z = transform.position.z;
        Gamer.Instance.RefreshUIPos += AHG;
        StartCoroutine(Fuck());
    }
    public IEnumerator Fuck()
    {
        yield return new WaitForSeconds(0.5f);
        AHG();
    }

    public void AHG()
    {
        var e = target.transform.position;
        e.z = z;
        e.x = Mathf.Ceil(e.x*16)/16;
        e.y = Mathf.Ceil(e.y*16)/16;
        if((transform.position-e).magnitude > 0.25f)
        transform.position = e;
    }
}
