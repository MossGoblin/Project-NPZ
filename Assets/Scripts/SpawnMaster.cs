using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaster : MonoBehaviour
{
    // References
    public Conductor conductor;
    
    // EnemyPrefabStore
    public GameObject[] enemies;

    // globals
    [SerializeField] private int spawnPointNumber;
    [SerializeField] private int enemyTypesNumber;
    [SerializeField] public float spawnDelay;


    // SpawnPoints
    public GameObject[] spawnPoints; // an array of all spawn points
    [SerializeField] private bool[] spawnPointVacancy; // keeps track of the available spawn points
    [SerializeField] private float[] spawnPointTimers;

    internal void SpawnBulk(int spawnNumber)
    {
        for (int count = 0; count < spawnNumber; count++)
        {
            Spawn();
        }
    }

    // ExtantEnemyDB
    private List<GameObject> population; // holds all the objects from the current population
    private int[] typePopulations; // holds count of all the current types
    private List<int> pValues; // used for calculating the probability table for spawning a new enemy


    // Start is called before the first frame update
    void Awake()
    {
        // init values
        enemyTypesNumber = 3;
        spawnPointNumber = 9;
        spawnPointTimers = new float[spawnPointNumber];

        population = new List<GameObject>();
        typePopulations = new int[enemyTypesNumber];
        for (int count = 0; count < enemyTypesNumber; count++)
        {
            typePopulations[count] = 0;
        }
        spawnPointVacancy = new bool[spawnPointNumber];
        for (int count = 0; count < spawnPointNumber; count++)
        {
            spawnPointVacancy[count] = true;
        }
        for (int count = 0; count < spawnPointNumber; count++)
        {
            spawnPointTimers[count] = 0;
        }

        // initialize p values
        pValues = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        // update spawn delays
        UpdateSpawnDelayMap();
    }

    private void UpdateSpawnDelayMap()
    {
        for (int count = 0; count < spawnPointNumber; count++)
        {
            if (spawnPointTimers[count] > 0)
            {
                spawnPointTimers[count] = Math.Max(spawnPointTimers[count] - Time.deltaTime, 0);
            }
        }
    }

    public void Spawn()
    {
        // CHECK if there are available spawn points
        bool havePoint = false;
        for (int count = 0; count < spawnPointNumber; count++)
        {
            if (spawnPointVacancy[count])
            {
                havePoint = true;
                break;
            }
        }
        if (havePoint)
        {
            // Choose spawnPoint
            int chosenSpawnPointIndex = GetSpawnPoint();

            // Spawn Enemy
            int typeSpawn = CreateInstance(chosenSpawnPointIndex);
        }
    }

    private int CreateInstance(int chosenSpawnPointIndex)
    {
        for (int count = 0; count < enemyTypesNumber; count++)
        {
            for (int inner = 0; inner < enemyTypesNumber - typePopulations[count]; inner++)
            {
                pValues.Add(count);
            }
        }

        // select random type from the list
        System.Random rnd = new System.Random();
        int rndTypeIndex = rnd.Next(pValues.Count - 1);
        int typeSpawn = pValues[rndTypeIndex];

        GameObject chosenSpawnPoint = spawnPoints[chosenSpawnPointIndex];
        // get coordinates of spawn point
        Vector3 spawnCoordinates = chosenSpawnPoint.transform.position;

        GameObject newEnemy = Instantiate(enemies[typeSpawn], spawnCoordinates, Quaternion.identity);
        // send information to the enemy about the parent spawning point
        newEnemy.GetComponent<EnemyController>().spawnPointIndex = chosenSpawnPointIndex;
        newEnemy.GetComponent<EnemyController>().selfType = typeSpawn;

        // log the new enemy in the population list
        population.Add(newEnemy);
        // mark the population type increase
        typePopulations[typeSpawn] += 1;

        return typeSpawn;
    }

    private int GetSpawnPoint()
    {
        // build list of viables
        List<int> viablePoints = new List<int>();
        for (int count = 0; count < spawnPointNumber; count++)
        {
            if (spawnPointVacancy[count])
            {
                viablePoints.Add(count);
            }
        }

        System.Random rnd = new System.Random();
        int rndSPNumber = rnd.Next(viablePoints.Count);
        int rndSPIndex = viablePoints[rndSPNumber];
        spawnPointVacancy[rndSPIndex] = false;
        return rndSPIndex;
    }

    public void RemoveEnemy(GameObject enemy, int enemyType, int spawnPointIndex)
    {
        // remove enemy from the population
        population.Remove(enemy);
        // adjust enemy type populaition array
        typePopulations[enemyType] -= 1;
        // adjust spawn point vacancy array
        spawnPointVacancy[spawnPointIndex] = true;
        spawnPointTimers[spawnPointIndex] = spawnDelay;
    }
}
