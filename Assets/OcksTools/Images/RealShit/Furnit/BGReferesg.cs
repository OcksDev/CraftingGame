using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGReferesg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var ween = GetComponent<SpriteRenderer>();
        ween.material = new Material( ween.material );
    }
}
