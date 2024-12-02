using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CorruptionCode : MonoBehaviour
{
    Dictionary<Vector2Int, VoidTile> allnerds = new Dictionary<Vector2Int, VoidTile>();
    Dictionary<Vector2Int, VoidTile> activenerds = new Dictionary<Vector2Int, VoidTile>();
    public GameObject VoidObject;
    Thread head;
    public static CorruptionCode Instance;
    private void Awake()
    {
        Instance = this;
        head = Thread.CurrentThread;
    }

    public IEnumerator ClearAllNerds()
    {
        foreach(var a in allnerds)
        {
            a.Value.VoidObject.SetActive(false);
        }
        var we = new Dictionary<Vector2Int, VoidTile>(allnerds);
        allnerds.Clear();
        int amtper = 3;
        int i = 0;
        foreach(var a in we)
        {
            i++;
            Destroy(a.Value.VoidObject);
            if (i >= amtper)
            {
                i = 0;
                yield return new WaitForFixedUpdate();
            }
        }

    }


    public void CorruptTile(Vector2Int pos)
    {
        StartCoroutine(cortile(pos));
    }
    float maxtim = 3.5f;
    float mintim = 1.2f;
    private IEnumerator cortile(Vector2Int pos)
    {
        if (allnerds.ContainsKey(pos)) goto wa;
        if(!Gamer.Instance.IsPosInBounds(twotothree(pos), true, true)) goto wa;
        var ween = new VoidTile();
        ween.timer = Random.Range(mintim, maxtim)*0.9f;
        ween.pos = pos;
        ween.iscomplete = false;
        allnerds.Add(pos, ween);
        activenerds.Add(pos, ween);
        yield return new WaitForSeconds(ween.timer);

        CompleteCorruption(pos);
        yield return new WaitForSeconds(maxtim);
        float minfade = 0.7f;
        float maxfade = 3f;
        var aaa = ween.VoidObject.GetComponent<SpriteRenderer>();
        var c = aaa.color;
        c *= 0.94f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(Random.Range(minfade, maxfade));
        c = aaa.color;
        c *= 0.93f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(Random.Range(minfade, maxfade));
        c = aaa.color;
        c *= 0.92f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(Random.Range(minfade, maxfade));
        c = aaa.color;
        c *= 0.90f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(Random.Range(minfade, maxfade));
        c = aaa.color;
        c *= 0.88f;
        c.a = 1;
        aaa.color = c;
        goto wa;
        yield return new WaitForSeconds(0.3f);
        c = aaa.color;
        c *= 0.6f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(0.3f);
        c = aaa.color;
        c *= 0.5f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(0.3f);
        c = aaa.color;
        c *= 0.4f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(0.3f);
        aaa.color = Color.black;
    wa:;
    }

    public static Vector3 twotothree(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y, 0);
    }

    public void CompleteCorruption(Vector2Int pos)
    {
        activenerds.Remove(pos);
        var ween = allnerds[pos];
        ween.VoidObject= Instantiate(VoidObject, twotothree(pos), Quaternion.identity, Tags.refs["VoidHolder"].transform);
        ween.iscomplete=true;
        CorruptTile(pos+new Vector2Int(0,1));
        CorruptTile(pos+new Vector2Int(0,-1));
        CorruptTile(pos+new Vector2Int(1,0));
        CorruptTile(pos+new Vector2Int(-1,0));
    }

}


public class VoidTile
{
    public Vector2Int pos;
    public float timer = 0;
    public bool iscomplete = false;
    public GameObject VoidObject;
}
