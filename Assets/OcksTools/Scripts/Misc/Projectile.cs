using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    public float speed = 0.1f;
    public string Banan = "Arrow";
    [HideInInspector]
    public float life = 0f;
    float targetlife = 0.2f;
    public bool Bouncy = false;
    public Sprite[] Springles = new Sprite[0];
    public SpriteRenderer spinglerenderer;
    public CircleCollider2D cic;
    // Start is called before the first frame update
    void Start()
    {
        switch (Banan)
        {
            case "Boomerang":
                targetlife = 0.25f;
                break;
            case "Dagger":
                targetlife = 0.5f;
                break;
            default:
                targetlife = 0.2f;
                break;
        }
    }
    Dictionary<Vector2, int> bannedvectiors = new Dictionary<Vector2, int>();
    // Update is called once per frame
    int ticks = 0;
    void FixedUpdate()
    {
        ticks++;
        if (Bouncy && ticks > 2)
        {
            List<Vector2> killme = new List<Vector2> ();
            for(int i = 0; i < bannedvectiors.Count; i++) 
            {
                var wank = bannedvectiors.ElementAt(i);
                bannedvectiors[wank.Key]--;
                if (bannedvectiors[wank.Key] <= 0) killme.Add(wank.Key);
            }
            foreach(var a in killme)
            {
                bannedvectiors.Remove(a);
            }
            RaycastHit2D[] results = new RaycastHit2D[10];
            var x = cic.Cast(transform.up, results, speed);

            var dir = transform.up;
            int realx = 0;
            foreach (var a in results)
            {
                if (a.collider == null) continue;
                var tp = Gamer.Instance.GetObjectType(a.collider.gameObject);
                switch (tp.type)
                {
                    case "Wall":
                        if (!bannedvectiors.ContainsKey(a.normal))
                        {
                            realx++;
                            dir = RandomFunctions.ReflectVector(dir, a.normal);
                            bannedvectiors.Add(a.normal, 0);
                        }
                        goto end;
                        break;
                }
            }
            end:
            if(realx > 0)
            {

                transform.rotation = Point2DMod2(transform.position + dir, -90, 0);
                transform.position += transform.up * speed;
            }
            wankout:;
        }

        transform.position += transform.up * speed;


        if ((life += Time.deltaTime) > targetlife)
        {
            var a = GetComponent<HitBalls>();
            a.StartCoroutine(a.WaitForDIe());
            //speed = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Bouncy) return;
        return;
        var tp = Gamer.Instance.GetObjectType(collision.gameObject);
        switch (tp.type)
        {
            case "Hitable":
            case "Enemy":
            case "Wall":
                var dir = transform.up;
                ContactPoint2D[] contactPoints = new ContactPoint2D[5];
                int x = collision.GetContacts(contactPoints);
                Debug.Log("E: " + x);
                for (int i = 0; i < x; i++)
                {
                    Debug.Log(contactPoints[i].normal);
                    dir = RandomFunctions.ReflectVector(dir, contactPoints[i].normal);
                }
                transform.rotation = Point2DMod2(transform.position + dir, -90, 0);
                break;
        }
    }
    private Quaternion Point2DMod2(Vector3 pos, float offset2, float spread)
    {
        //returns the rotation required to make the current gameobject point at the mouse, untested in 3D.
        var offset = UnityEngine.Random.Range(-spread, spread);
        offset += offset2;
        //Debug.Log(offset);
        Vector3 difference = pos - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var sex = Quaternion.Euler(0f, 0f, rotation_z + offset);
        return sex;
    }
}
