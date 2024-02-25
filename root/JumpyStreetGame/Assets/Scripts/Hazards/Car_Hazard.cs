using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Car_Hazard : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // how fast the cars move

    private Rigidbody rb;

    private Transform startPoint; // point where the car spawns
    private Transform endPoint; // point where the car despawns

    void Start()
    {
        // The car is spawned as a child of the Car Spawner object
        // The Car_Generator script contains variables for the start and end points
        rb = GetComponent<Rigidbody>();
        startPoint = gameObject.transform.parent;
        endPoint = gameObject.GetComponentInParent<Car_Generator>().endPoint;

        // Determines if the car will move left or right
        if (!gameObject.GetComponentInParent<Car_Generator>().facingLeft)
        {
            rb.velocity = Vector3.right * speed;
        }
        else
        {
            rb.velocity = Vector3.left * speed;
        }
    }

    void Update()
    {
        // the distance between the car and its endpoint
        float diff;
        if(!gameObject.GetComponentInParent<Car_Generator>().facingLeft)
        {
            diff = Mathf.Abs(gameObject.transform.position.x - endPoint.position.x);
        }
        else
        {
            diff = Mathf.Abs(gameObject.transform.position.x - startPoint.position.x);
        }

        // Once the car reaches the endpoint, it destroys itself
        if (diff < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
