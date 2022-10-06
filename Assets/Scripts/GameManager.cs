using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public FloatVariable PlayerHealth;
    public int KillCounter;
    public int EnergyCounter;
    public GameObject Energy;
    public int minSpawnEnergy;
    public int maxSpawnEnergy;

    [Header("Linked UI Text")]
    public TextMeshProUGUI KillCounterTMP;
    public TextMeshProUGUI EnergyCounterTMP;
    public TextMeshProUGUI HealthTMP;

    private void Start()
    {
        HealthTMP.text = PlayerHealth.value.ToString();
    }

    // knowing which enemy will determine which drops and how many
    // knowing where the enemy died will determine where to drop them items
    public void EnemyKill(string enemy_tag, Vector3 enemy_pos)
    {
        KillCounter++;
        KillCounterTMP.text = "x" + KillCounter;
        if(enemy_tag == "Enemy")
        {
            var numToSpawn = UnityEngine.Random.Range(minSpawnEnergy, maxSpawnEnergy);
            for (var i = 0; i < numToSpawn; i++)
            {
                GameObject toSpawn = Instantiate(Energy, enemy_pos, Quaternion.identity);
            }
        }
    }

    public void EnergyCollected()
    {
        EnergyCounter++;
        EnergyCounterTMP.text = "x" + EnergyCounter;
    }

    public void HealthChanged()
    {
        HealthTMP.text = PlayerHealth.value.ToString();
    }
}
