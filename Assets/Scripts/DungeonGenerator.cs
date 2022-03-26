using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] RoomTemplates;

    void Start()
    {
        List<GameObject> Rooms = new List<GameObject>();
        if(RoomTemplates.Length > 0)
        {
            GameObject room = Instantiate(RoomTemplates[2]);
            room.transform.parent = gameObject.transform;
            Rooms.Add(room);

            //Rooms.Add(Instantiate(RoomTemplates[Random.Range(0, RoomTemplates.Length)]));
            //Rooms[1].transform.parent = gameObject.transform;
            //Rooms[1].transform.position += new Vector3(0, -9.6f, 0);
        }

    }
}
