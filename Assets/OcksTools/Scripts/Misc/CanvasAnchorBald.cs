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
        if((transform.position-e).magnitude > 0.5f)
        transform.position = e;
    }
}
