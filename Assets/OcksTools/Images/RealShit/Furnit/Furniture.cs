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
        OXComponent.StoreComponent(this);
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
                    if (f < 0.01)
                    {
                        self.sprite = sprites[1];
                    }
                    else if (f < 0.75)
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
        if (self.sprite == null) Destroy(gameObject);
    }
    bool hadsexed = false;
    public void OnTouch()
    {
        if (hadsexed) return;
        switch (type)
        {
            case "Barrel":
                SoundSystem.Instance.PlaySound(8, true, 0.4f, 0.8f);
                if (self.sprite == sprites[0])
                {
                    var wank = Random.Range(0, 1f);
                    if(wank <= 0.01f)
                    {
                        Gamer.Instance.SpawnGroundItem(transform.position, Gamer.Instance.GetItemForLevel());
                    }
                    else if(wank <= 0.1f)
                    {
                        Gamer.Instance.SpawnHealers(transform.position, 1, PlayerController.Instance);
                    }
                    Instantiate(Gamer.Instance.ParticleSpawns[2], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                }
                else
                {
                    Gamer.Instance.SpawnHealers(transform.position, 2, PlayerController.Instance);
                    Instantiate(Gamer.Instance.ParticleSpawns[17], transform.position, Quaternion.identity, Tags.refs["ParticleHolder"].transform);
                }
                hadsexed = true;
                Destroy(gameObject);
                break;
        }
    }


}
