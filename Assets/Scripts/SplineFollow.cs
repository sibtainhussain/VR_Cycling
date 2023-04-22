using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SplineFollow : MonoBehaviour
{
    [SerializeField] SplinePath path;
    [SerializeField] float speed = 2f, yOffset = 2, rotationSpeed = 5f;
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
        //Debug.Log(pathPosition);
        OrientedPoint point = path.GetPointByDistance(pathPosition);
        point.pos += transform.up * yOffset;
        transform.position = point.pos;
        transform.rotation = Quaternion.Lerp(transform.rotation, point.rot, rotationSpeed * Time.deltaTime);
    }

    void OnValidate()
    {
        OrientedPoint point = path.GetPointAtPosition(0f);
        point.pos += transform.up * yOffset;
        transform.position = point.pos;
    }

}
