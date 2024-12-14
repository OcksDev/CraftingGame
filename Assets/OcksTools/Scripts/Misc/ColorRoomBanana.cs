using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRoomBanana : MonoBehaviour
{
    public List<Color32> Color32s = new List<Color32>();
    public List<INteractable> refs = new List<INteractable>();
    public List<int> ints = new List<int>();
    public void ClickityMe(INteractable meme)
    {

    }

    public void Start()
    {
        foreach (var r in refs)
        {
            int x = Random.Range(0, Color32s.Count);
            r.GetComponent<SpriteRenderer>().color = Color32s[x];
            Color32s.RemoveAt(x);
        }
        for(int i = 0; i < 8; i++)
        {
            ints.Add(Random.Range(0,refs.Count));
        }
    }
}
