using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (a.Value.VoidObject != null)
            a.Value.VoidObject.transform.position = new Vector3 (0, 1000000000, 10000);
        }
        var we = new Dictionary<Vector2Int, VoidTile>(allnerds);
        allnerds.Clear();
        int amtper = 1;
        int i = 0;
        foreach(var a in we)
        {
            i++;
            KillObj(a.Key, a.Value.VoidObject);
            if (i >= amtper)
            {
                i = 0;
                yield return new WaitForFixedUpdate();
            }
        }

    }
    public void KillObj(Vector2Int pos, GameObject VoidObject)
    {
        if(VoidObject != null)
            VoidObject.SetActive(true);
        if (allnerds.ContainsKey(pos)) allnerds.Remove(pos);
    }

    public void CorruptTile(Vector2Int pos)
    {
        StartCoroutine(cortile(pos));
    }
    float maxtim = 2f;
    float mintim = 0.35f;
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
        if (!allnerds.ContainsKey(pos))
        {
            goto wa;
        }
        CompleteCorruption(pos);
        goto skibidi;
        yield return new WaitForSeconds(maxtim);
        if (!allnerds.ContainsKey(pos))
        {
            KillObj(pos, ween.VoidObject);
            goto wa;
        }
        float minfade = 0.7f;
        float maxfade = 3f;
        var aaa = ween.VoidObject.GetComponent<SpriteRenderer>();
        var c = aaa.color;
        c *= 0.92f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(Random.Range(minfade, maxfade));
        if (!allnerds.ContainsKey(pos))
        {
            KillObj(pos, ween.VoidObject);
            goto wa;
        }
        c = aaa.color;
        c *= 0.92f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(Random.Range(minfade, maxfade));
        if (!allnerds.ContainsKey(pos))
        {
            KillObj(pos, ween.VoidObject);
            goto wa;
        }
        c = aaa.color;
        c *= 0.90f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(Random.Range(minfade, maxfade));
        if (!allnerds.ContainsKey(pos))
        {
            KillObj(pos, ween.VoidObject);
            goto wa;
        }
        c = aaa.color;
        c *= 0.87f;
        c.a = 1;
        aaa.color = c;
        yield return new WaitForSeconds(Random.Range(minfade, maxfade));
        if (!allnerds.ContainsKey(pos))
        {
            KillObj(pos, ween.VoidObject);
            goto wa;
        }
        c = aaa.color;
        c *= 0.84f;
        c.a = 1;
        aaa.color = c;
        skibidi:
        ween.iscomplete = true;
        yield return new WaitForSeconds(8);
        if (!allnerds.ContainsKey(pos))
        {
            KillObj(pos, ween.VoidObject);
            goto wa;
        }
        List<Vector2Int> possy = new List<Vector2Int>() 
        {
            pos + new Vector2Int(1, 1),
            pos + new Vector2Int(-1, 1),
            pos + new Vector2Int(1, -1),
            pos + new Vector2Int(-1, -1),
            pos + new Vector2Int(1, 0),
            pos + new Vector2Int(-1, 0),
            pos + new Vector2Int(0, 1),
            pos + new Vector2Int(0, -1),
        };
        if (TileExists(possy[0]) && TileExists(possy[1]) && TileExists(possy[2]) && TileExists(possy[3]) && TileExists(possy[4]) && TileExists(possy[5]) && TileExists(possy[6]) && TileExists(possy[7]))
        {
            foreach(var p in possy)
            {
                KillObj(p, allnerds[p].VoidObject);
                allnerds.Remove(p);
            }
            ween.VoidObject.transform.localScale = new Vector3(3,3,1);

        }


    wa:;
    }

    public bool TileExists(Vector2Int pos)
    {
        return allnerds.ContainsKey(pos);
    }
    Dictionary<GameObject, int> sussypool = new Dictionary<GameObject, int>();

    public GameObject PullFromPool(Vector2Int pos)
    {
        if(sussypool.Count > 0)
        {
            var a = sussypool.ElementAt(0);
            a.Key.transform.localScale = Vector3.one;
            sussypool.Remove(a.Key);
            a.Key.SetActive(true);
            return a.Key;
        }
        return Instantiate(VoidObject, twotothree(pos), Quaternion.identity, Tags.refs["VoidHolder"].transform);
    }

    public static Vector3 twotothree(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y, 0);
    }

    public void CompleteCorruption(Vector2Int pos)
    {
        activenerds.Remove(pos);
        var ween = allnerds[pos];
        ween.VoidObject = PullFromPool(pos);
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
