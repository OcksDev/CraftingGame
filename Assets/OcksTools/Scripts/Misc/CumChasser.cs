using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CumChasser : MonoBehaviour
{
    public I_Room iroom;
    public GameObject notif;
    public int amnt = 0;
    Transform weenk;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int max = 15;
        var wank = Instantiate(notif, transform.position, Quaternion.identity, Tags.refs["RanNot"].transform).GetComponent<DamIndi>();
        amnt++;
        wank.NoCLor = true;
        wank.sex.color = new Color32(100,255,100,255);
        wank.sex.text = $"{amnt}/{max}";
        wank.rmod = 0.5f;
        wank.lifemod = 0.75f;
        if(amnt >= max)
        {
            StartCoroutine(Cumplete());
        }
        else
        {
            transform.position = GetRandomPos();
            weenk.transform.position = transform.position;
        }
    }
    private void Start()
    {
        weenk = Instantiate(Gamer.Instance.ParticleSpawns[29], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform).transform;
    }
    bool stap = false;
    public IEnumerator Cumplete()
    {
        if (stap) goto endme;
        stap = true;
        Gamer.Instance.SpawnGroundItem(iroom.transform.position, Gamer.Instance.GetItemForLevel());
        weenk.transform.position = iroom.transform.position;
        GetComponent<SpriteRenderer>().enabled = false;
        Gamer.QuestProgressIncrease("Room", "Chase The Orb");
        Gamer.Instance.SpawnCoins(iroom.transform.position, 2, PlayerController.Instance);
        yield return new WaitForSeconds(0.6f);
        Destroy(weenk.gameObject);
        Destroy(gameObject);
    endme:;
    }


    public Vector3 GetRandomPos()
    {
        float sz = 8;
        var e = new Vector3 (Random.Range(-sz,sz), Random.Range(-sz, sz), 0) + iroom.transform.position;
        bool can = RandomFunctions.Instance.Dist(e, transform.position) > 7;
        if(can) can = Gamer.Instance.IsPosInBounds(e);
        if (can)
        {
            return e;
        }
        else
        {
            return GetRandomPos();
        }
    }

}
