using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

[RequireComponent(typeof(InputData))]
public class DisplayInputData : MonoBehaviour
{
    public TextMeshProUGUI gripDisplay;
    public TextMeshProUGUI triggerDisplay;
    public TextMeshProUGUI speedDisplay;
    public TextMeshProUGUI powerDisplay;
    public TextMeshProUGUI bpmDisplay;

    private InputData _inputData;
    public float velocity;
    private float power;
    private float heartRate = 80;

    private void Start()
    {
        _inputData = GetComponent<InputData>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.grip, out float gripData))
        {
            gripDisplay.text = gripData.ToString("F2");

            // calculate velocity when accelerating
            velocity += gripData * Time.deltaTime * 6;

            // calculate power depending on effort
            power = (511 * gripData);
            
            // calculate heart rate depending
            if (heartRate <= 194 && heartRate >= 110)
            {
                heartRate = heartRate + (((2 * gripData) - 1) * Time.deltaTime);
            }
            if (heartRate < 110 && heartRate >= 80)
            {
                heartRate = heartRate + (((4 * gripData) - 0.5f) * Time.deltaTime);
            }
        }
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerData))
        {
            triggerDisplay.text = triggerData.ToString("F2");

            // calculate velocity when breaking
            velocity -= triggerData * Time.deltaTime * 6f;
        }

        velocity -= Time.deltaTime * 3;
        velocity = Mathf.Min(Mathf.Max(velocity, 0f), 30f);
        heartRate = Mathf.Min(Mathf.Max(heartRate, 80f), 194f);

        speedDisplay.text = velocity.ToString("F2");
        bpmDisplay.text = heartRate.ToString("F2");
        powerDisplay.text = power.ToString("F2");
    }
}
