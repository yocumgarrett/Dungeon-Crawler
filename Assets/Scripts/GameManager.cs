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
    public FloatVariable PlayerCurrentProjectiles;
    public FloatVariable PlayerMaxProjectiles;
    public GameObject ProjectileDrop;

    [Header("Linked UI")]
    public TextMeshProUGUI KillCounterTMP;
    public TextMeshProUGUI EnergyCounterTMP;
    public TextMeshProUGUI HealthTMP;
    public GameObject[] ProjectileImages;

    [Header("GameControls")]
    public bool paused;

    private void Start()
    {
        HealthTMP.text = PlayerHealth.value.ToString();
        paused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseUnpauseGame();
        }
    }

    // knowing which enemy will determine which drops and how many
    // knowing where the enemy died will determine where to drop them items
    public void EnemyKill(string enemy_tag, Vector3 enemy_pos)
    {
        KillCounter++;
        KillCounterTMP.text = "x" + KillCounter;
        if(enemy_tag == "Enemy")
        {
            StartCoroutine(DropEnergyCoroutine(enemy_pos));

            var chance_to_spawn = (PlayerMaxProjectiles.value - PlayerCurrentProjectiles.value) / (PlayerMaxProjectiles.value * 2f);
            var outcome = UnityEngine.Random.value;
            if (outcome <= chance_to_spawn)
            {
                GameObject toSpawn = Instantiate(ProjectileDrop, enemy_pos, Quaternion.identity);
            }
        }
    }

    IEnumerator DropEnergyCoroutine(Vector3 enemy_pos)
    {
        var numToSpawn = UnityEngine.Random.Range(minSpawnEnergy, maxSpawnEnergy);
        for (var i = 0; i < numToSpawn; i++)
        {
            GameObject toSpawn = Instantiate(Energy, enemy_pos, Quaternion.identity);
            yield return new WaitForSeconds(0.075f);
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

    public void NumberProjectilesChanged()
    {
        for (int i = 0; i < PlayerMaxProjectiles.value; i++)
        {
            if (i < PlayerCurrentProjectiles.value)
                ProjectileImages[i].SetActive(true);
            else
                ProjectileImages[i].SetActive(false);
        }
    }

    private void PauseUnpauseGame()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
        }
        else if (!paused)
        {
            Time.timeScale = 0;
            paused = true;
        }
    }
}
