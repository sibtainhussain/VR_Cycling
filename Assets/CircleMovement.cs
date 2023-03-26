using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{

    public GameObject anchor;
    public GameObject DataCanvas;
    private DisplayInputData  speedSource;
    private float radius;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        radius = (transform.position - anchor.transform.position).magnitude;
        speedSource = DataCanvas.GetComponent<DisplayInputData>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(anchor.transform.position, Vector3.up, speedSource.velocity * 20 * Time.deltaTime);
        //transform.RotateAround(anchor.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
