using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Generator : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs; // all of the cars that be spawned
    [SerializeField] private float minSpawnTime; // minimum time in seconds before a new car spawns
    [SerializeField] private float maxSpawnTime; // maximum time in seconds before a new car spawns
    private float timer = 0; // tracks how much time has passed since spawning a car
    private float spawnTime = 0; // amount of time before spawning another car; randomly generated each time

    // These two variables are accessed by the Car_Hazard scripts
    public Transform startPoint;
    public Transform endPoint;

    void Start()
    {
        SpawnCar(); // Immediately spawn a car
    }

    void Update()
    {
        timer += Time.deltaTime; // Timer increases over time
        if (timer > spawnTime)
        {
            SpawnCar(); // Once enough time has passed, spawn a new car
        }
    }

    private void SpawnCar()
    {
        int i = Random.Range(0, carPrefabs.Length); // Randomly decide which car to spawn
        Instantiate(carPrefabs[i], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform); // Spawn the car
        timer = 0; // Reset the timer
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime); // Randomly decide how much time before the next car is spawned
    }
}
