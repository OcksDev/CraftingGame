using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.Progress;

public class ColorRoomBanana : MonoBehaviour
{
    public List<Color32> Color32s = new List<Color32>();
    public List<INteractable> refs = new List<INteractable>();
    public List<int> ints = new List<int>();
    public GameObject displaysegsmcnugget;
    public int ptrog = 0;
    public bool ClickityMe(INteractable meme)
    {
        int mememe = refs.IndexOf(meme);


        if (ints[ptrog] == mememe)
        {
            ptrog++;
            if (ptrog == ints.Count)
            {
                StartCoroutine(Complete());
            }
            else
            {
                var cd = OXComponent.GetComponent<SpriteRenderer>(meme.gameObject).color;
                SpawnParticle(meme.transform.position, cd);
            }
            return true;
        }
        else
        {
            return false;
        }

    }
    public IEnumerator Complete()
    {
        foreach (INteractable me in refs)
        {
            var cd = OXComponent.GetComponent<SpriteRenderer>(me.gameObject).color;
            SpawnParticle(me.transform.position, cd);
            Destroy(me.gameObject);
        }
        yield return null;  
    }
    public void SrartThing()
    {
        foreach(var t in refs)
        {
            t.CanInteract = true;
        }
        StartCoroutine(PasscodeShow());
    }
    public IEnumerator PasscodeShow()
    {
        float wa = 0.4f;
        float wa2 = 0.4f;
        var disck = displaysegsmcnugget.GetComponent<SpriteRenderer>();
        foreach (var item in ints)
        {
            disck.color = new Color32(255, 255, 255, 30);
            yield return new WaitForSeconds(wa);
            disck.color = OXComponent.GetComponent<SpriteRenderer>(refs[item].gameObject).color;
            yield return new WaitForSeconds(wa2);
        }
    }

    public void SpawnParticle(Vector3 pos, Color32 cl)
    {
        var w = Instantiate(Gamer.Instance.ParticleSpawns[30], pos, Quaternion.identity, Tags.refs["ParticleHolder"].transform).GetComponent<partShitBall>();
        w.Particicic.GetComponent<ParticleSystem>().startColor = cl;
    }


    public void Start()
    {
        foreach (var r in refs)
        {
            int x = Random.Range(0, Color32s.Count);
            OXComponent.StoreComponent(r.GetComponent<SpriteRenderer>());
            OXComponent.GetComponent<SpriteRenderer>(r.gameObject).color = Color32s[x];
            Color32s.RemoveAt(x);
        }
        for (int i = 0; i < 8; i++)
        {
            ints.Add(Random.Range(0, refs.Count));
        }
    }
}
