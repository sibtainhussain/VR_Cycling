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
    public TextMeshProUGUI bpmDisplay;

    private InputData _inputData;
    public float velocity = 0f;

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
            velocity += gripData * Time.deltaTime * 5;
        }
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerData))
        {
            triggerDisplay.text = triggerData.ToString("F2");
            velocity -= triggerData * Time.deltaTime * 10;
        }
        velocity -= Time.deltaTime * 1;
        velocity = Mathf.Min(Mathf.Max(velocity, 0f), 30f);
        speedDisplay.text = velocity.ToString("F2");
        bpmDisplay.text = velocity.ToString("F2");
    }
}
