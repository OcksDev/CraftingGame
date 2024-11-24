using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFollower : MonoBehaviour
{
    public Color32[] cols; 
    public int Amount = 6;
    public GameObject thing;
    public List<Transform> Followers = new List<Transform>();
    public List<SpriteRenderer> sPIRTETET = new List<SpriteRenderer>();
    public List<bool> wankisor = new List<bool>();
    public List<CircleCollider2D> FollowersHitboxes = new List<CircleCollider2D>();
    public ParticleSystem IUNgoundidnd;
    public float FollowerDist = 1;
    private void Start()
    {
        Followers.Insert(0, transform);
        sPIRTETET.Insert(0, GetComponent<SpriteRenderer>());
        wankisor.Add(true);
    }
    public void SpawnNerds()
    {
        for(int i = 0; i < Amount; i++)
        {
            var a = Instantiate(thing, transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), Quaternion.identity, Tags.refs["ParticleHolder"].transform).transform;
            Followers.Add(a);
            var b = a.GetComponent<SpriteRenderer>();
            sPIRTETET.Add(b);
            b.sortingOrder = -(i+2);
            var weenor = gameObject.AddComponent<CircleCollider2D>();
            weenor.radius = 0.55f;
            weenor.isTrigger = true;
            FollowersHitboxes.Add(weenor);
        }
    }
    void Update()
    {
        for(int i = 1; i < Followers.Count; i++)
        {
            var me = Followers[i].transform.position;
            Vector3 targ = Followers[i-1].position;
            Vector3 targdir = (me-targ).normalized;
            var newd = targ + (targdir * FollowerDist);
            Followers[i].transform.position = newd;
            Followers[i].transform.rotation = RandomFunctions.PointAtPoint2D(newd, targ, 0);
            FollowersHitboxes[i-1].offset = newd- Followers[0].position;
            wankisor.Add(true);
        }
    }
    public void FixedUpdate()
    {
        for (int i = 0; i < sPIRTETET.Count; i++)
        {
            bool wankis = Gamer.Instance.IsPosInBounds(Followers[i].position);
            if(wankis != wankisor[i])
            {
                wankisor[i] = wankis;
                sPIRTETET[i].color = wankis ? cols[0] : cols[1];
                Instantiate(Gamer.Instance.ParticleSpawns[22], Followers[i].position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                if (i == 0)
                {
                    if (wankis)
                    {
                        IUNgoundidnd.Stop();
                    }
                    else
                    {
                        IUNgoundidnd.Play();
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 1; i < Followers.Count; i++)
        {
            Destroy(Followers[i].gameObject);
        }
    }
}
