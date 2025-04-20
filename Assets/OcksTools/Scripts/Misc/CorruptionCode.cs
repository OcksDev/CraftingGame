using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CorruptionCode : MonoBehaviour
{
    Dictionary<Vector3Int, VoidTile> allnerds = new Dictionary<Vector3Int, VoidTile>();
    Dictionary<Vector3Int, VoidTile> activenerds = new Dictionary<Vector3Int, VoidTile>();
    public Tilemap TM;
    public TileBase VoidThing;

    public Color[] colors = new Color[3];

    public static CorruptionCode Instance;
    private void Awake()
    {
        Instance = this;
    }
    int exi = 0;
    private void FixedUpdate()
    {
        exi++;
        int revs = 8;
        exi %= revs;
        float timtim = Time.deltaTime * revs;
        if (Gamer.ActiveDrugs.Contains("Liquid Corruption")) timtim *= 1.5f;
        for (int i = exi; i < activenerds.Count; i+=revs)
        {
            var tingle = activenerds.ElementAt(i).Value;
            var timy = timtim;
            if (PlayerController.Instance != null && RandomFunctions.Instance.DistNoSQRT(tingle.pos, PlayerController.Instance.transform.position) >= 3000) timy *= 1.3f;
            if ((tingle.timer -= timy) <= 0)
            {
                switch (tingle.stage)
                {
                    case 0:
                        TM.SetTile(tingle.pos, VoidThing);
                        TM.SetTileFlags(tingle.pos, TileFlags.None);
                        CorruptTile(tingle.pos + new Vector3Int(1, 0, 0));
                        CorruptTile(tingle.pos + new Vector3Int(-1, 0, 0));
                        CorruptTile(tingle.pos + new Vector3Int(0, 1, 0));
                        CorruptTile(tingle.pos + new Vector3Int(0, -1, 0));
                        tingle.stage++;
                        tingle.timer = Random.Range(maxtim, maxtim+0.5f);
                        break;
                    case 3:
                    case 2:
                    case 1:
                        TM.SetColor(tingle.pos, colors[tingle.stage]);
                        tingle.stage++;
                        tingle.timer = Random.Range(maxtim, maxtim + 0.5f);
                        break;
                    default:
                        activenerds.Remove(tingle.pos);
                        i--;
                        break;
                }
            }
        }
    }

    public IEnumerator ClearAllNerds()
    {
        activenerds.Clear();
        allnerds.Clear();
        yield return null;
        TM.ClearAllTiles();
        yield break;
    }
    public string GetCurretHolder(int holder)
    {
        return $"VoidHolder{holder}";
    }

    public void CorruptTile(Vector3Int pos)
    {
        if (allnerds.ContainsKey(pos)) return;
        if (!Gamer.Instance.IsPosInBounds(pos, true, true)) return;
        var ween = new VoidTile();
        ween.pos = pos;
        ween.timer = Random.Range(mintim, maxtim);
        allnerds.Add(pos, ween);
        activenerds.Add(pos, ween);
    }

    float mintim = 0.35f;
    float maxtim = 1.7f;
    private static List<Vector2Int> nerdlocs = new List<Vector2Int>()
    {
        new Vector2Int(1,1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,-1),
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
    };
    public static Vector3 twotothree(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y, 0);
    }


}


public class VoidTile
{
    public Vector3Int pos;
    public int stage = 0;
    public float timer = 0;
    public bool iscomplete = false;
    public bool isend = false;
    public Tile TileMe;
}
/* v1
public class CorruptionCode : MonoBehaviour
{
    Dictionary<Vector2Int, VoidTile> allnerds = new Dictionary<Vector2Int, VoidTile>();
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
        var dupe = new Dictionary<Vector2Int, VoidTile>(allnerds);
        allnerds.Clear();
        Tags.refs[GetCurretHolder(holder)].SetActive(false);
        holder += 1;
        var cd = Tags.refs[GetCurretHolder(holder %= 2)].transform;
        cd.gameObject.SetActive(true);

        foreach (var a in dupe)
        {
            if (a.Value.iscomplete && !a.Value.isend)
            {
                yield return new WaitForFixedUpdate();
                a.Value.VoidObject.transform.position = new Vector3(0, 987540987, 4795433);
                a.Value.VoidObject.transform.parent = cd;
                poolpps.Add(a.Value.VoidObject, 1);
            }
        }

        yield break;
    }
    int holder = 0;
    public string GetCurretHolder(int holder)
    {
        return $"VoidHolder{holder}";
    }

    public void CorruptTile(Vector2Int pos)
    {
        StartCoroutine(cortile(pos));
    }

    float mintim = 0.35f;
    float maxtim = 2f;
    private static List<Vector2Int> nerdlocs = new List<Vector2Int>()
    {
        new Vector2Int(1,1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,-1),
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
    };
    private IEnumerator cortile(Vector2Int pos)
    {
        if (allnerds.ContainsKey(pos)) goto wa;
       // if(!Gamer.Instance.IsPosInBounds(twotothree(pos), true, true)) goto wa;
        var ween = new VoidTile();
        ween.pos = pos;
        ween.timer = Random.Range(mintim, maxtim);
        allnerds.Add(pos, ween);

        yield return new WaitForSeconds(ween.timer);
        ween.VoidObject = PullFromPool(pos);
        ween.iscomplete = true;
        var nerdlocs2 = new List<Vector2Int>(nerdlocs);
        for(int i = 0; i < nerdlocs2.Count; i++)
        {
            nerdlocs2[i] += pos;
        }
        CorruptTile(nerdlocs2[4]);
        CorruptTile(nerdlocs2[5]);
        CorruptTile(nerdlocs2[6]);
        CorruptTile(nerdlocs2[7]);

        yield return new WaitForSeconds(8);
        if (ween.isend) goto wa; 
        bool e = true;
        foreach(var popo in nerdlocs2)
        {
            if (!TileExists(popo))
            {
                e = false; break;
            }
        }
        if (e)
        {
            foreach (var popo in nerdlocs2)
            {
                allnerds[popo].VoidObject.GetComponent<SpriteRenderer>().color = Color.blue;
                ShadowRealm(allnerds[popo]);
            }
            ween.VoidObject.GetComponent<SpriteRenderer>().color = Color.green;
            ween.VoidObject.transform.localScale = Vector3.one * 3; 
        }


    wa:;
    }
    public bool TileExists(Vector2Int pos)
    {
        return allnerds.ContainsKey(pos) && allnerds[pos].iscomplete && !allnerds[pos].isend;
    }

    public static Vector3 twotothree(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y, 0);
    }

    public void CompleteCorruption(Vector2Int pos)
    {
        var ween = allnerds[pos];
        ween.VoidObject = PullFromPool(pos);
        CorruptTile(pos+new Vector2Int(0,1));
        CorruptTile(pos+new Vector2Int(0,-1));
        CorruptTile(pos+new Vector2Int(1,0));
        CorruptTile(pos+new Vector2Int(-1,0));
    }

    Dictionary<GameObject, int> poolpps = new Dictionary<GameObject, int>();

    public GameObject PullFromPool(Vector2Int pos)
    {
        if(poolpps.Count > 0)
        {
            var pp = poolpps.ElementAt(0);
            poolpps.Remove(pp.Key);
            pp.Key.transform.position = twotothree(pos);
            pp.Key.transform.localScale = Vector3.one;
            return pp.Key;
        }
        return Instantiate(VoidObject, twotothree(pos), Quaternion.identity, Tags.refs[GetCurretHolder(holder)].transform);
    }
    public void ShadowAndErase(VoidTile vt)
    {
        allnerds.Remove(vt.pos);
        ShadowRealm(vt);
    }

    public void ShadowRealm(VoidTile vt)
    {
        vt.VoidObject.transform.position = new Vector3(0,987540987,4795433);
        vt.iscomplete = true;
        vt.isend = true;
        poolpps.Add(vt.VoidObject, 0);
    }


}


public class VoidTile
{
    public Vector2Int pos;
    public float timer = 0;
    public bool iscomplete = false;
    public bool isend = false;
    public GameObject VoidObject;
}
*/