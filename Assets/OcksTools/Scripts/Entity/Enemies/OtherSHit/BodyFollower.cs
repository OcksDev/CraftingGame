using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFollower : MonoBehaviour
{
    public int Amount = 6;
    public GameObject thing;
    public List<Transform> Followers = new List<Transform>();
    public List<BoxCollider2D> FollowersHitboxes = new List<BoxCollider2D>();
    public float FollowerDist = 1;
    private void Start()
    {
        Followers.Insert(0, transform);
    }
    public void SpawnNerds()
    {
        for(int i = 0; i < Amount; i++)
        {
            var a = Instantiate(thing, transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), Quaternion.identity, Tags.refs["ParticleHolder"].transform).transform;
            Followers.Add(a);
            a.GetComponent<SpriteRenderer>().sortingOrder = -(i+2);
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
