using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public string type = "";
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
            switch (type)
            {
                case "Barrel":
                    self.sprite = sprites[0];
                    var f = Random.Range(0, 1f);
                    if (f < 0.25)
                    {
                        self.sprite = sprites[1];
                    }
                    if (f < 0.15)
                    {
                        self.sprite = sprites[2];
                    }
                    break;
                default:
                    self.sprite = sprites[Random.Range(0, sprites.Count)];
                    break;
            }
        }
        catch
        {
            Destroy(self.gameObject);
        }
    }

    public void OnTouch()
    {
        switch (type)
        {
            case "Barrel":
                Destroy(gameObject);
                break;
        }
    }


}
