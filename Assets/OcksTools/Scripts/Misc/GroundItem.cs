using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public SpriteRenderer sexylady;
    public GISItem sexyballer;
    // Start is called before the first frame update
    void Start()
    {
        if (sexyballer != null) 
        sexylady.sprite = GISLol.Instance.Items[sexyballer.ItemIndex].Sprite;
    }
    float life = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        life += Time.deltaTime;
        sexylady.transform.localPosition = new Vector3(0, Mathf.Sin(life*2.5f)*0.1f, 0);
    }

    public void AttemptPickup()
    {
        var inv = GISLol.Instance.All_Containers["Inventory"];
        var open = inv.FindEmptySlot();
        if(open > -1)
        {
            inv.slots[open].Held_Item = sexyballer;

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("NO SPACE");
        }
    }
}
