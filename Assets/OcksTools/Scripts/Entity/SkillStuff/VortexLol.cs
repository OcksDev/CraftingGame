using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexLol : MonoBehaviour
{
    public Transform rotato;
    public Transform rotato2;
    public float Rotsped = 90;
    public float Initspeed = 1f;
    public ParticleSystem[] weewees;
    public GameObject weem;
    // Update is called once per frame

    private void Awake()
    {
        foreach(var a in weewees)
        {
            a.Simulate(0.125f);
            a.Play();
        }
    }
    float life = 1;
    void FixedUpdate()
    {
        if (wanklife) return;
        life += Time.deltaTime;
        transform.position += transform.up * Initspeed;
        foreach (var a in weewees)
        {
            var b = a.main;
            b.simulationSpeed = (Initspeed) + 1;
        }
        Initspeed *= 0.90f;
        rotato.rotation *= Quaternion.Euler(0, Rotsped * Time.deltaTime, 0);
        //rotato2.rotation *= Quaternion.Euler(0, -Rotsped * Time.deltaTime, 0);
        if(life > 6f)
        {
            StartCoroutine(endme());
        }
        if(life > 1f)
        {
            foreach(var a in Gamer.Instance.EnemiesExisting)
            {
                if(!a.HasSpawned) continue;
                var dist = RandomFunctions.Instance.Dist(transform.position, a.transform.position);
                if(dist <= 6.5)
                {
                    a.sex.velocity += (Vector2)((transform.position - a.transform.position).normalized);
                }
            }
        }
    }
    bool wanklife = false;
    public IEnumerator endme()
    {
        wanklife = true;
        weem.SetActive(false);
        foreach (var a in weewees)
        {
            var b = a.emission;
            var c = a.trails;
            b.rateOverTime = 0;
            c.attachRibbonsToTransform = false;
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
