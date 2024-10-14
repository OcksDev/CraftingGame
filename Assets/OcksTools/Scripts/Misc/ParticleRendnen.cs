using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRendnen : MonoBehaviour
{
    public float initTime;
    public ParticleSystem PE;
    void Start()
    {
        PE.Simulate(initTime);
        PE.Play();
    }
}
