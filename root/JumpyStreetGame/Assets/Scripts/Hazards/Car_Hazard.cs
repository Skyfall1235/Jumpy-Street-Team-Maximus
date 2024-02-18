using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Hazard : MonoBehaviour
{
    [SerializeField] private float speed = 0.25f; // how fast the cars move

    [SerializeField] private float lerpRatio = 0; // controls how far along the car is: 0 - startpoint 1 - endpoint

    private Transform startPoint; // point where the car spawns
    private Transform endPoint; // point where the car despawns

    void Start()
    {
        // The car is spawned as a child of the Car Spawner object
        // The Car_Generator script contains variables for the start and end points
        startPoint = gameObject.GetComponentInParent<Car_Generator>().startPoint;
        endPoint = gameObject.GetComponentInParent<Car_Generator>().endPoint;
    }

    void Update()
    {
        // the lerpRatio variable increases over time and is used the move the car to the endpoint
        lerpRatio += (Time.deltaTime * speed);
        gameObject.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpRatio);
        // Once the car reaches the endpoint, it destroys itself
        if (lerpRatio >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
