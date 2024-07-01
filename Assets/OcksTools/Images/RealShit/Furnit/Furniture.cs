using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    private SpriteRenderer self;
    public List<Sprite> sprites = new List<Sprite>();
    public bool CanNotSpawn = false;

    private void Start()
    {
        self = GetComponent<SpriteRenderer>();
        if (CanNotSpawn) sprites.Add(null);
        SetFurniture();
    }

    public void SetFurniture()
    {
        try
        {
            self.sprite = sprites[Random.Range(0, sprites.Count)];
        }
        catch
        {
            Destroy(self.gameObject);
        }
    }

}
