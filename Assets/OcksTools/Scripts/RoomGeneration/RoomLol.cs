
using System.Collections.Generic;
using UnityEngine;

public class RoomLol : MonoBehaviour
{
    public int RoomDensity = 6;
    public float DistanceScaler = 1f;
    public Vector3 CenterSpawn = new Vector3(0, 0, 0);
    public GameObject[] RoomPrefabs;
    public List<GameObject> SpawnedRooms = new List<GameObject>();
    public List<I_Room> SpawnedRoomsDos = new List<I_Room>();
    public int[,] RoomColliders = new int[200, 200];
    private List<Room> AllRooms= new List<Room>();
    private List<Room> LeftRooms = new List<Room>();
    private List<Room> RightRooms = new List<Room>();
    private List<Room> UpRooms = new List<Room>();
    private List<Room> DownRooms = new List<Room>();
    private List<Room> EndLeftRooms = new List<Room>();
    private List<Room> EndRightRooms = new List<Room>();
    private List<Room> EndUpRooms = new List<Room>();
    private List<Room> EndDownRooms = new List<Room>();
    private List<Room> ValidStartRooms = new List<Room>();
    public GameObject Parente;

    public static RoomLol Instance;
    private void Awake()
    {
        Instance = this;
    }

    private int wseed = 0;
    public void ClearRooms()
    {
        RoomColliders = new int[200, 200];
        foreach(var r in SpawnedRooms)
        {
            Destroy(r);
        }
        SpawnedRooms.Clear();
        SpawnedRoomsDos.Clear();
    }


    public int runs = 0;
    public int cycles = 0;
    public void GenerateRandomLayout()
    {
        wseed = Gamer.Seed;
        runs = 0;
        cycles = 0;
        ClearRooms();
        PopulateRooms();
        int sz = RoomColliders.GetLength(0)/2;
        var crs = GenerateFromRooms(RoomDensity, RoomColliders, -1, new Vector2(sz,sz));
        PlaceFromCoolRoom(crs, Parente);

        Debug.Log($"Build Stats: [{runs}], [{cycles}]" );

    }
    public void PopulateRooms()
    {
        //change rooms to use here
        AllRooms = new List<Room>()
        {
            new Room(0),
            new Room(1),
            new Room(2),
            new Room(3),
            new Room(4),
            new Room(5),
            new Room(6),
            new Room(6),
            new Room(7),
            new Room(8),
            new Room(9),
            new Room(10),
        };
        LeftRooms.Clear();
        RightRooms.Clear();
        UpRooms.Clear();
        DownRooms.Clear();
        EndLeftRooms.Clear();
        EndRightRooms.Clear();
        EndUpRooms.Clear();
        EndDownRooms.Clear();
        ValidStartRooms.Clear();
        //whichs rooms are chosen for certain directions
        foreach (var room in AllRooms)
        {
            if (room.HasLeftDoor && !room.IsEndpoint) LeftRooms.Add(room);
            if (room.HasRightDoor && !room.IsEndpoint) RightRooms.Add(room);
            if (room.HasTopDoor && !room.IsEndpoint) UpRooms.Add(room);
            if (room.HasBottomDoor && !room.IsEndpoint) DownRooms.Add(room);
            if (room.HasLeftDoor && room.IsEndpoint) EndLeftRooms.Add(room);
            if (room.HasRightDoor && room.IsEndpoint) EndRightRooms.Add(room);
            if (room.HasTopDoor && room.IsEndpoint) EndUpRooms.Add(room);
            if (room.HasBottomDoor && room.IsEndpoint) EndDownRooms.Add(room);
        }
        foreach (var a in LeftRooms) {ValidStartRooms.Add(a); }
        foreach (var a in RightRooms) {ValidStartRooms.Add(a); }
        foreach (var a in UpRooms) {ValidStartRooms.Add(a); }
        foreach (var a in DownRooms) {ValidStartRooms.Add(a); }
    }
    public CoolRoom GenerateFromRooms(int lvl, int[,] roomcol, int dir, Vector2 pos)
    {
        CoolRoom ret = new CoolRoom();
        var r = new System.Random(wseed);
        wseed+=169;
        runs++;
        if (lvl < 1) 
        {
            ret.WasChill = true;
            return ret;
        }
        /* dir:
         * -1 = any direction
         * 0 = top
         * 1 = bottom
         * 2 = left
         * 3 = right
         * 
         * dir is the direction the room is trying to be generated from the previous room.
         * Lists
         * 
         */
        List<Room> available_rooms;
        switch (dir)
        {
            default:
                available_rooms = new List<Room>(ValidStartRooms);
                for(int i= 0; i < available_rooms.Count; i++)
                {
                    if (available_rooms[i].IsEndpoint)
                    {
                        available_rooms.Remove(available_rooms[i]);
                        i--;
                    }
                }
                break;
            case 0:
                available_rooms = new List<Room>(lvl <=  1? EndDownRooms : DownRooms);
                break;
            case 1:
                available_rooms = new List<Room>(lvl <= 1 ? EndUpRooms : UpRooms);
                break;
            case 2:
                available_rooms = new List<Room>(lvl <= 1 ? EndRightRooms : RightRooms);
                break;
            case 3:
                available_rooms = new List<Room>(lvl <= 1 ? EndLeftRooms : LeftRooms);
                break;
        }
        bool found_place = false;
        //bool found_ever = false;
        //Debug.Log("Fuck dawg " + lvl + ", " + dir);
        while (available_rooms.Count > 0)
        {
            cycles++;
            int index = r.Next(0, available_rooms.Count);
            Room rom = available_rooms[index];
            bool keepgoing = true;
            ret.room = rom;
            var pos2 = pos;
            switch (dir)
            {
                case 0:
                    pos2 -= rom.BottomDoor;
                    pos2 -= new Vector2(0, 1);
                    break;
                case 1:
                    pos2 -= rom.TopDoor;
                    pos2 += new Vector2(0, 1);
                    break;
                case 2:
                    pos2 -= rom.RightDoor;
                    pos2 -= new Vector2(1, 0);
                    break;
                case 3:
                    pos2 -= rom.LeftDoor;
                    pos2 += new Vector2(1, 0);
                    break;
            }
            ret.pos = pos2;
            for (int i = 0; i < rom.RoomSize.x && keepgoing; i++)
            {
                for (int j = 0; j < rom.RoomSize.y && keepgoing; j++)
                {
                    if (roomcol[i + (int)pos2.x, j + (int)pos2.y]>0)
                    {
                        keepgoing = false;
                    }
                }
            }
            if (keepgoing)
            {

                if (dir != 1 && rom.HasTopDoor)
                {
                    var v = pos2 + rom.TopDoor - new Vector2(0, 1);
                    if (roomcol[(int)v.x, (int)v.y] > 0) keepgoing = false;
                }
                if (dir != 0 && rom.HasBottomDoor)
                {
                    var v = pos2 + rom.BottomDoor + new Vector2(0, 1);
                    if (roomcol[(int)v.x, (int)v.y] > 0) keepgoing = false;
                }
                if (dir != 3 && rom.HasLeftDoor)
                {
                    var v = pos2 + rom.LeftDoor - new Vector2(1, 0);
                    if (roomcol[(int)v.x, (int)v.y] > 0) keepgoing = false;
                }
                if (dir != 2 && rom.HasRightDoor)
                {
                    var v = pos2 + rom.RightDoor + new Vector2(1, 0);
                    if (roomcol[(int)v.x, (int)v.y] > 0) keepgoing = false;
                }
            }

            if (keepgoing)
            {
                for (int i = 0; i < rom.RoomSize.x && keepgoing; i++)
                {
                    for (int j = 0; j < rom.RoomSize.y && keepgoing; j++)
                    {
                        roomcol[i + (int)pos2.x, j + (int)pos2.y]++;
                    }
                }
                bool good = true;

                List<int> doors = new List<int>();
                if (dir != -1)
                {
                    if (dir != 1 && rom.HasTopDoor) doors.Add(0);
                    if (dir != 0 && rom.HasBottomDoor) doors.Add(1);
                    if (dir != 3 && rom.HasLeftDoor) doors.Add(2);
                    if (dir != 2 && rom.HasRightDoor) doors.Add(3);
                    if (doors.Count > 0) doors.RemoveAt(r.Next(0, doors.Count));
                }
                var x = lvl - 1;
                if (good && dir != 1 && rom.HasTopDoor)
                {
                    if (doors.Contains(0)) x = 1;
                    var a = GenerateFromRooms(x, roomcol, 0, pos2 + rom.TopDoor);
                    if (a.room != null && a.WasChill)
                    {
                        ret.comlpetedRooms.Add(a);
                    }
                    good = a.WasChill;
                }
                x = lvl - 1;
                if (good && dir != 0 && rom.HasBottomDoor)
                {
                    if (doors.Contains(1)) x = 1;
                    var a = GenerateFromRooms(x, roomcol, 1, pos2 + rom.BottomDoor);
                    if (a.room != null && a.WasChill)
                    {
                        ret.comlpetedRooms.Add(a);
                    }
                    good = a.WasChill;
                }
                x = lvl - 1;
                if (good && dir != 3 && rom.HasLeftDoor)
                {
                    if (doors.Contains(2)) x = 1;
                    var a = GenerateFromRooms(x, roomcol, 2, pos2 + rom.LeftDoor);
                    if (a.room != null && a.WasChill)
                    {
                        ret.comlpetedRooms.Add(a);
                    }
                    good = a.WasChill;
                }
                x = lvl - 1;
                if (good && dir != 2 && rom.HasRightDoor)
                {
                    if (doors.Contains(3)) x = 1;
                    var a = GenerateFromRooms(x, roomcol, 3, pos2 + rom.RightDoor);
                    if (a.room != null && a.WasChill)
                    {
                        ret.comlpetedRooms.Add(a);
                    }
                    good = a.WasChill;
                }
                //Debug.Log("Child Is Chill: " + lvl + ", " + good + ", " + dir);
                if (!good)
                {
                    ClearFromCoolRoom(ret);
                    available_rooms.RemoveAt(index);
                }
                else
                {
                    found_place = true;
                    break;
                }
            }
            else
            {
                available_rooms.RemoveAt(index);
            }
        }

        //Debug.Log("Shit dawg " + lvl + ", " + found_place + ", " + dir);
        ret.WasChill = found_place;
        return ret;
    }


    public void ClearFromCoolRoom(CoolRoom cr)
    {

        for (int i = 0; i < cr.room.RoomSize.x; i++)
        {
            for (int j = 0; j < cr.room.RoomSize.y; j++)
            {
                RoomColliders[i + (int)cr.pos.x, j + (int)cr.pos.y]--;
            }
        }
        foreach (var c in cr.comlpetedRooms)
        {
            ClearFromCoolRoom(c);
        }
        cr.comlpetedRooms.Clear();
    }
    public void PlaceFromCoolRoom(CoolRoom cr, GameObject parent, int fard = 0, CoolRoom parenroom = null)
    {
        fard++;
        float z = 0;
        int sz = RoomColliders.GetLength(0) / 2;
        var sp = RoomPrefabs[cr.room.RoomIndexToPrefabIndex(cr.room.RoomID)];
        var gm = Instantiate(sp, CenterSpawn + ((new Vector3(cr.pos.x, cr.pos.y, z) - new Vector3(sz, sz, 0)) * DistanceScaler) + (new Vector3((sp.transform.localScale.x / 2) - 0.5f, (sp.transform.localScale.y / 2), 0)), parent.transform.rotation, parent.transform);
        var s = gm.GetComponent<I_Room>();
        s.gm = gm;
        s.room = cr.room;
        if(parenroom != null)
        {
            s.parent_room = parenroom.room;
        }
        else
        {
            s.parent_room = null;
        }
        s.level = fard;
        SpawnedRooms.Add(gm);
        SpawnedRoomsDos.Add(s);
        parenroom = cr;
        foreach (var c in cr.comlpetedRooms)
        {
            PlaceFromCoolRoom(c, parent, fard, parenroom);
        }
    }
}



public class Room
{
    public bool IsEndpoint = false;
    public Vector2 LeftDoor = new Vector2(-6969, -6969);
    public Vector2 RightDoor = new Vector2(-6969, -6969);
    public Vector2 TopDoor = new Vector2(-6969, -6969);
    public Vector2 BottomDoor = new Vector2(-6969, -6969);
    public Vector2 RoomSize = new Vector2(1, 1);
    public bool HasLeftDoor = false;
    public bool HasRightDoor = false;
    public bool HasTopDoor = false;
    public bool HasBottomDoor = false;
    public int RoomID = -1;
    public Room(int roomid)
    {
        RoomID = roomid;
        switch (roomid)
        {
            case 0:
                RoomSize = new Vector2(2, 1);
                HasLeftDoor = true;
                HasRightDoor = true;
                LeftDoor = new Vector2(0, 0);
                RightDoor = new Vector2(1, 0);
                break;
            case 1:
                RoomSize = new Vector2(1, 2);
                HasTopDoor = true;
                HasBottomDoor = true;
                TopDoor = new Vector2(0, 0);
                BottomDoor = new Vector2(0, 1);
                break;
            case 2:
                RoomSize = new Vector2(1, 1);
                HasBottomDoor = true;
                HasTopDoor = true;
                HasLeftDoor = true;
                HasRightDoor = true;
                LeftDoor = new Vector2(0, 0);
                RightDoor = new Vector2(0, 0);
                TopDoor = new Vector2(0, 0);
                BottomDoor = new Vector2(0, 0);
                break;
            case 3:
                RoomSize = new Vector2(1, 1);
                //HasBottomDoor = true;
                //HasTopDoor = true;
                //HasLeftDoor = true;
                HasRightDoor = true;
                //LeftDoor = new Vector2(0, 0);
                RightDoor = new Vector2(0, 0);
                //TopDoor = new Vector2(0, 0);
                //BottomDoor = new Vector2(0, 0);
                IsEndpoint = true;
                break;
            case 4:
                RoomSize = new Vector2(1, 1);
                //HasBottomDoor = true;
                HasTopDoor = true;
                //HasLeftDoor = true;
                //HasRightDoor = true;
                //LeftDoor = new Vector2(0, 0);
                //RightDoor = new Vector2(0, 0);
                TopDoor = new Vector2(0, 0);
                //BottomDoor = new Vector2(0, 0);
                IsEndpoint = true;
                break;
            case 5:
                RoomSize = new Vector2(1, 1);
                HasBottomDoor = true;
                //HasTopDoor = true;
                //HasLeftDoor = true;
                //HasRightDoor = true;
                //LeftDoor = new Vector2(0, 0);
                //RightDoor = new Vector2(0, 0);
                //TopDoor = new Vector2(0, 0);
                BottomDoor = new Vector2(0, 0);
                IsEndpoint = true;
                break;
            case 6:
                RoomSize = new Vector2(1, 1);
                //HasBottomDoor = true;
                //HasTopDoor = true;
                HasLeftDoor = true;
                //HasRightDoor = true;
                LeftDoor = new Vector2(0, 0);
                //RightDoor = new Vector2(0, 0);
                //TopDoor = new Vector2(0, 0);
                //BottomDoor = new Vector2(0, 0);
                IsEndpoint = true;
                break;
            default:
                RoomSize = new Vector2(1, 1);
                HasBottomDoor = true;
                HasTopDoor = true;
                HasLeftDoor = true;
                HasRightDoor = true;
                IsEndpoint = true;
                LeftDoor = new Vector2(0,0);
                RightDoor = new Vector2(0,0);
                TopDoor = new Vector2(0,0);
                BottomDoor = new Vector2(0,0);
                break;
            case 9:
                RoomSize = new Vector2(1, 1);
                //HasBottomDoor = true;
                HasTopDoor = true;
                HasLeftDoor = true;
                //HasRightDoor = true;
                LeftDoor = new Vector2(0, 0);
                //RightDoor = new Vector2(0, 0);
                TopDoor = new Vector2(0, 0);
                //BottomDoor = new Vector2(0, 0);
                break;
            case 8:
                RoomSize = new Vector2(1, 1);
                //HasBottomDoor = true;
                HasTopDoor = true;
                //HasLeftDoor = true;
                HasRightDoor = true;
                //LeftDoor = new Vector2(0, 0);
                RightDoor = new Vector2(0, 0);
                TopDoor = new Vector2(0, 0);
                //BottomDoor = new Vector2(0, 0);
                break;
            case 7:
                RoomSize = new Vector2(1, 1);
                HasBottomDoor = true;
                //HasTopDoor = true;
                //HasLeftDoor = true;
                HasRightDoor = true;
                //LeftDoor = new Vector2(0, 0);
                RightDoor = new Vector2(0, 0);
                //TopDoor = new Vector2(0, 0);
                BottomDoor = new Vector2(0, 0);
                break;
            case 10:
                RoomSize = new Vector2(1, 1);
                HasBottomDoor = true;
                //HasTopDoor = true;
                HasLeftDoor = true;
                //HasRightDoor = true;
                LeftDoor = new Vector2(0, 0);
                //RightDoor = new Vector2(0, 0);
                //TopDoor = new Vector2(0, 0);
                BottomDoor = new Vector2(0, 0);
                break;
        }
    }
    public int RoomIndexToPrefabIndex(int i)
    {
        int j = i;
        switch (i)
        {
            //special cases where room index doesnt match prefab id
        }
        return j;
    }
}
public class CoolRoom
{
    public bool WasChill;
    public Room room;
    public Vector2 pos;
    public List<CoolRoom> comlpetedRooms = new List<CoolRoom>();
}
