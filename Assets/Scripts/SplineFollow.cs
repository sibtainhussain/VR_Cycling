using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineFollow : MonoBehaviour
{
    [SerializeField] SplinePath path;
    [SerializeField] float speed = 2f, rotationSpeed = 5f;
    [SerializeField] bool loop = true;
    [SerializeField] float pathPosition;
    public float distanceTraveled;


    void Start()
    {
        pathPosition = 0f;
        distanceTraveled = 0f;
    }

    void Update()
    {
        distanceTraveled += speed * Time.deltaTime;
        if(!loop && distanceTraveled > path.pathLength) {
            return;
        }
        pathPosition = Mathf.Repeat(distanceTraveled, path.pathLength);
        Debug.Log(pathPosition);
        OrientedPoint point = path.GetPointAtPosition(pathPosition);
        transform.position = point.pos;
        transform.rotation = Quaternion.Lerp(transform.rotation, point.rot, rotationSpeed * Time.deltaTime);
    }
}
