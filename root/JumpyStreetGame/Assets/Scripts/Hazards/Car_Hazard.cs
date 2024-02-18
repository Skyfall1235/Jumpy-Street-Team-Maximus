using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Hazard : MonoBehaviour
{
    [SerializeField] private float speed = 0.25f;

    [SerializeField] private float lerpRatio = 0;

    private Transform startPoint;
    private Transform endPoint;

    void Start()
    {
        startPoint = gameObject.GetComponentInParent<Car_Generator>().startPoint;
        endPoint = gameObject.GetComponentInParent<Car_Generator>().endPoint;
    }

    void Update()
    {
        lerpRatio += (Time.deltaTime * speed);
        gameObject.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpRatio);
        if (lerpRatio >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
