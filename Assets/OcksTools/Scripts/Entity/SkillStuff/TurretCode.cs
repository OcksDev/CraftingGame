using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TurretCode : MonoBehaviour
{
    public DamageProfile Damprof;
    float t = 0;

    public float StoredAtks;
    private void Start()
    {
        StoredAtks = 2 / Damprof.WeaponOfAttack.Player.AttacksPerSecond;
        t = StoredAtks;
        Instantiate(Gamer.Instance.ParticleSpawns[33], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
    }
    float life = 0;
    public void FixedUpdate()
    {
        if((t -= Time.deltaTime) <= 0)
        {
            if(Gamer.Instance.EnemiesExisting.Count > 0)
            {
                float dist = 250;
                NavMeshEntity found = null;
                foreach(var a in Gamer.Instance.EnemiesExisting)
                {
                    if (!a.HasSpawned) continue;
                    if(RandomFunctions.Instance.DistNoSQRT(a.transform.position, transform.position) < dist)
                    {
                        dist = RandomFunctions.Instance.DistNoSQRT(a.transform.position, transform.position);
                        found = a;
                    }
                }
                if(found != null)
                {
                    t = StoredAtks;
                    Damprof.WeaponOfAttack.Player.SpawnArrow(Damprof, transform.position, RandomFunctions.PointAtPoint2D(transform.position, found.transform.position, 90), 1);
                }
            }
        }
        if ((life += Time.deltaTime) >= 10f)
        {
            Instantiate(Gamer.Instance.ParticleSpawns[33], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
            Destroy(gameObject);
        }
    }
}
