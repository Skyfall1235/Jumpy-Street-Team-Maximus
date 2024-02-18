using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Generator : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    private float timer = 0;
    private float spawnTime = 0;

    public Transform startPoint;
    public Transform endPoint;

    void Start()
    {
        SpawnCar();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnTime)
        {
            SpawnCar();
        }
    }

    private void SpawnCar()
    {
        int i = Random.Range(0, carPrefabs.Length);
        Instantiate(carPrefabs[i], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        timer = 0;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
