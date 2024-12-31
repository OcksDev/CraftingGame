using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceMover : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0.3f,0.5f).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * speed * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var wank = Gamer.Instance.GetObjectType(collision.gameObject);
        switch (wank.type)
        {
            case "Enemy":
            case "Player":
            case "Wall":
                ContactPoint2D[] contactPoints = new ContactPoint2D[5];
                int x = collision.GetContacts(contactPoints);
                for (int i = 0; i < x; i++)
                {
                    velocity = RandomFunctions.ReflectVector(velocity, contactPoints[i].normal);
                }
                break;
        }
    }
}
