using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("SpawnRoom", 0.5f);
    }

    private void SpawnRoom()
    {
        if (!spawned)
        {
            switch (openingDirection)
            {
                // spawn a room with a Bottom door
                case 1:
                    rand = Random.Range(0, templates.B.Length);
                    Instantiate(templates.B[rand], transform.position, Quaternion.identity);
                    break;
                // spawn a room with a Left door
                case 2:
                    rand = Random.Range(0, templates.L.Length);
                    Instantiate(templates.L[rand], transform.position, Quaternion.identity);
                    break;
                // spawn a room with a Top door
                case 3:
                    rand = Random.Range(0, templates.T.Length);
                    Instantiate(templates.T[rand], transform.position, Quaternion.identity);
                    break;
                // spawn a room with a Right door
                case 4:
                    rand = Random.Range(0, templates.R.Length);
                    Instantiate(templates.R[rand], transform.position, Quaternion.identity);
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("SpawnPoint"))
        {
            var otherSpawner = other.GetComponent<RoomSpawner>();
            if(otherSpawner != null)
            {
                if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
                {
                    // spawn walls to block openings
                    Instantiate(templates.Wall, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                spawned = true;
            }
        }
    }
}
