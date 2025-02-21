using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public SpriteRenderer[] sexylady;
    public GISItem sexyballer;
    // Start is called before the first frame update
    void Start()
    {
        if (sexyballer != null)
        {
            FixMe();
        }
    }
    public void FixMe()
    {
        var w = GISDisplay.GetSprites(sexyballer);
        sexylady[0].sprite = w.sprites[0];
        sexylady[1].sprite = w.sprites[1];
        sexylady[2].sprite = w.sprites[2];
        sexylady[0].color = w.colormods[0];
        sexylady[1].color = w.colormods[1];
        sexylady[2].color = w.colormods[2];
    }


    float life = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        life += Time.deltaTime;
        sexylady[0].transform.localPosition = new Vector3(0, Mathf.Sin(life*2.5f)*0.1f, 0);
    }
    
    public void AttemptPickup()
    {
        if (Gamer.Instance.checks[5]) return;
        Gamer.Instance.PickupItemCrossover = sexyballer;
        sexyballer.IAMSPECIL = this;
        Gamer.Instance.itemshite = gameObject;
        Gamer.Instance.ToggleItemPickup();
    }
}
