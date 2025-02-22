using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public DamageProfile profile;
    public int MaxBounces = 4;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CUM());
    }
    public IEnumerator CUM()
    {
        List<NavMeshEntity> list = new List<NavMeshEntity>();
        bool a = false;
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (a)
            {
                yield return new WaitForSeconds(2);
                Destroy(gameObject);
                yield break;
            }
            float dist = 100f;
            NavMeshEntity entity = null;
            var z = transform.position;
            z.z = 0;
            foreach (var e in Gamer.Instance.EnemiesExisting)
            {
                if(!e.HasSpawned) continue;
                var z2 = e.transform.position;
                z2.z = 0;
                var x = RandomFunctions.Instance.DistNoSQRT(z, z2);
                if (x < dist && !list.Contains(e))
                {
                    dist = x;
                    entity = e;
                }
            }
            if(entity != null)
            {
                MaxBounces--;
                transform.position = entity.transform.position;
                entity.EntityOXS.Hit(new DamageProfile(profile));
                list.Add(entity);
                if (MaxBounces <= 0)a = true;
            }
            else
            {
                yield return new WaitForSeconds(2);
                Destroy(gameObject);
                yield break;
            }
        }
    }
}
