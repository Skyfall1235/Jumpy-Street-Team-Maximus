using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Car_Generator : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs; // all of the cars that be spawned
    [SerializeField] private float minSpawnTime; // minimum time in seconds before a new car spawns
    [SerializeField] private float maxSpawnTime; // maximum time in seconds before a new car spawns
    private float timer = 0; // tracks how much time has passed since spawning a car
    private float spawnTime = 0; // amount of time before spawning another car; randomly generated each time
    public bool facingLeft = false;

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
        Vector3 givenPosition = gameObject.transform.position;
        Quaternion givenRotation = gameObject.transform.rotation;
        //if the cars are going from right to left, flip the rotation and endpoints
        if (facingLeft)
        {
            givenPosition = endPoint.position;
            givenRotation = Quaternion.LookRotation(transform.forward * -1f);
        }
        Instantiate(carPrefabs[i], givenPosition, givenRotation, transform); // Spawn the car
        timer = 0; // Reset the timer
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime); // Randomly decide how much time before the next car is spawned
    }
}
