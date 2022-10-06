using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // this will make the manager accessible everywhere and doesn't get destroyed on scene change
        if (Instance == null)
            Instance = this; 
        else if (Instance != null)
            Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    [Header("Combat")]
    public int Kills;
    public GameObject Energy;

    // knowing which enemy will determine which drops and how many
    // knowing where the enemy died will determine where to drop them items
    public void EnemyKill(string enemy_tag, Vector3 enemy_pos)
    {
        Kills++;
        Debug.Log(enemy_tag);
        //Instantiate(Energy, enemy_pos, Quaternion.identity);
    }
}
