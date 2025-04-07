using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public string type = "";
    private SpriteRenderer self;
    public List<Sprite> sprites = new List<Sprite>();
    public List<GameObject> miscrefs = new List<GameObject>();
    public bool CanNotSpawn = false;

    private void Start()
    {
        self = GetComponent<SpriteRenderer>();
        if (CanNotSpawn) sprites.Add(null);
        OXComponent.StoreComponent(this);
        SetFurniture();
    }
    Coroutine cum;
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
                    if (self.sprite == null) Destroy(gameObject);
                    break;
                case "Torch":
                    if (Random.Range(0, 1f) <= 0.5f)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    float initpos = Random.Range(0, 1f);

                    var a = miscrefs[1].GetComponent<Animator>();
                    a.Play("New Animation", 0, initpos);

                    cum = StartCoroutine(OXLerp.LinearInfniteLooped((x) =>
                    {
                        miscrefs[0].transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one*1.3f, (Mathf.Sin(2*Mathf.PI*(x+initpos))+1)/2)*1.2f;
                    }, 5));
                    break;
                case "Rock":
                    if (Random.Range(0, 1f) <= 0.5f)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    self.sprite = sprites[Random.Range(0, sprites.Count)];
                    if (self.sprite == null) Destroy(gameObject);
                    break;
                case "Grate":
                    if (Random.Range(0, 1f) <= 0.75f)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    self.sprite = sprites[Random.Range(0, sprites.Count)];
                    break;
                case "Plank":
                    if (Random.Range(0, 1f) <= 0.60f)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    self.sprite = sprites[Random.Range(0, sprites.Count)];
                    if (self.sprite == null) Destroy(gameObject);
                    break;
                case "Chains":
                    if (Random.Range(0, 1f) <= 0.40f)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    self.sprite = sprites[Random.Range(0, sprites.Count)];
                    if (Random.Range(0, 1f) <= 0.5f)
                    {
                        self.flipX = true;
                    }
                    if (self.sprite == null) Destroy(gameObject);
                    break;
                default:
                    self.sprite = sprites[Random.Range(0, sprites.Count)];
                    if (self.sprite == null) Destroy(gameObject);
                    break;
            }
        }
        catch
        {
            Destroy(self.gameObject);
        }
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
            case "Torch":
                SoundSystem.Instance.PlaySound(20, true, 0.4f);
                hadsexed = true;
                Destroy(miscrefs[0]);
                StopCoroutine(cum);
                miscrefs[1].GetComponent<Animator>().enabled = false;
                miscrefs[2].GetComponent<ParticleSystem>().Stop();
                self.sprite = sprites[0];
                break;
        }
    }


}
