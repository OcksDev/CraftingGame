using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Room : MonoBehaviour
{
    public Room room;
    public Room parent_room;
    public GameObject gm;
    public float dist = 94385740398;
    public bool isused = false;
    public int level = 0;
    public bool hasbeensexed = false;
    public void Start()
    {
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        var pos = PlayerController.Instance.transform.position;
        var mypos = transform.position;
        var x = (room.RoomSize)*30f;
        x -= new Vector2(2, 2);
        var x2 = x / 2;
        if (!isused && !hasbeensexed && !Gamer.Instance.InRoom)
        {
            //Debug.Log("Running check " + name);
            if (pos.y > (mypos.y - x2.y) && pos.y < (mypos.y + x2.y) && pos.x > (mypos.x - x2.x) && pos.x < (mypos.x + x2.x))
            {
                SetActiveRoom();
            }
        }
    }


    public void SetActiveRoom()
    {
        var g = Gamer.Instance;
        g.InRoom = true;
        hasbeensexed = true;
        g.CurrentRoom = this;
        Debug.Log("Room Started");
        g.StartCoroutine(g.StartRoom());
    }
}
