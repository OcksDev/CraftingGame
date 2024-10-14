using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRendnen : MonoBehaviour
{
    public float initTime;
    public ParticleSystem PE;
    private void OnEnable()
    {
        PE.Simulate(initTime);
        PE.Play();
    }
}
