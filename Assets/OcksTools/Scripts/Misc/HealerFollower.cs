using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerFollower : MonoBehaviour
{
    public PlayerController SexChaser;
    public List<GameObject> others = new List<GameObject>();
    public SpriteRenderer sex2;
    public Vector3 vel;
    public float sexer;
    // Update is called once per frame

    private void Start()
    {
        transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        Gamer.Instance.healers.Add(gameObject);
        sexer = 0.5f;
        var a = transform.position;
        a.z = 0;
        var b = SexChaser.transform.position;
        b.z = 0;
        vel = (a - b).normalized * 0.2f;
    }


    void FixedUpdate()
    {
        sexer += Time.deltaTime;
        if (hadsexualencounterwithplayerandwasrapedfrombehindthensenttocounselingfortraumaandlackofballs)
        {
            if (sexer >= 5f) Destroy(gameObject);
        }
        else
        {
            if (SexChaser == null)
            {
                SexMyNuts();
                return;
            }
            vel *= 0.98f;
            Vector3 sex = Vector3.MoveTowards(transform.position, SexChaser.transform.position, 0.1f * sexer);
            sex += vel;
            others = Gamer.Instance.healers;
            foreach (GameObject obj in others)
            {
                if (obj == null || obj == gameObject) continue;
                var e = sex - obj.transform.position;
                e = ((e.normalized) / 100) / e.magnitude;
                sex += e;
            }

            transform.position = sex;
            if ((transform.position - SexChaser.transform.position).magnitude <= 0.5f) SexMyNuts();
        }
    }
    private bool hadsexualencounterwithplayerandwasrapedfrombehindthensenttocounselingfortraumaandlackofballs = false;
    public void SexMyNuts()
    {
        if (SexChaser != null)
        {
            SexChaser.entit.Heal(5);
            SoundSystem.Instance.PlaySound(5, true, 0.3f);
        }
        sexer = 0f;
        hadsexualencounterwithplayerandwasrapedfrombehindthensenttocounselingfortraumaandlackofballs = true;
        sex2.enabled = false;
        Gamer.Instance.healers.Remove(gameObject);
    }
}
