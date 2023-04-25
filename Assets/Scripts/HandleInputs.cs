using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;

[RequireComponent(typeof(GetInputs))]
public class HandleInputs : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public Image speedBar;
    public TextMeshProUGUI bpmText;
    public Image bpmBar;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI HoursText;
    public TextMeshProUGUI DistanceText;
    public TextMeshProUGUI TotalText;
    public Image progressBar;
    public SplineFollow player;
    public Material textMaterial;
    public Material textOutlineMaterial;
    public PauseController pauseController;
    
    private GetInputs _inputData;
    public float velocity;
    private float power;
    private float heartRate = 80;
    private float time = 0f;

    public float maxSpeed = 30f;
    public float minSpeed = 0f;
    public float maxHR = 194f;
    public float minHR = 80f;
    public float unitsPerMile = 1000;
    public bool pause = false;
    private bool held = false;
    
    private void Start()
    {
        _inputData = GetComponent<GetInputs>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.menuButton, out bool clicked))
        {
            Debug.Log("Pause Clicked: " + clicked);
            if(held && !clicked) {
                pause = !pause;
                if(pause) {
                    pauseController.Pause();
                    Time.timeScale = 0;
                }
                else {
                    pauseController.Resume();
                    Time.timeScale = 1;
                }
            }
            held = clicked;
        }
        time += Time.deltaTime;
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.grip, out float gripData))
        {
            // calculate velocity when accelerating
            velocity += gripData * Time.deltaTime * 6;

            // calculate power depending on effort
            power = (511 * gripData);
            
            // calculate heart rate depending
            if (heartRate <= maxHR && heartRate >= 110)
            {
                heartRate = heartRate + (((2 * gripData) - 1) * Time.deltaTime);
            }
            if (heartRate < 110 && heartRate >= minHR)
            {
                heartRate = heartRate + (((4 * gripData) - 0.5f) * Time.deltaTime);
            }
        }
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerData))
        {
            // calculate velocity when breaking
            velocity -= triggerData * Time.deltaTime * 6f;
        }

        velocity -= Time.deltaTime * 3;
        velocity = Mathf.Min(Mathf.Max(velocity, minSpeed), maxSpeed);
        heartRate = Mathf.Min(Mathf.Max(heartRate, minHR), maxHR);
        player.speed = velocity;

        int minutes = Mathf.FloorToInt(time/60);
        int seconds = Mathf.FloorToInt(time % 60);
        int hours = Mathf.FloorToInt(minutes/60);
        minutes = Mathf.FloorToInt(minutes % 60);

        if(hours > 0) {
            HoursText.fontSharedMaterial = textMaterial;
        }

        speedText.text = velocity.ToString("F0");
        speedBar.fillAmount = (velocity-minSpeed)/(maxSpeed-minSpeed);
        bpmText.text = heartRate.ToString("F0");
        bpmBar.fillAmount = (heartRate-minHR)/(maxHR-minHR);
        TimeText.text = ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        HoursText.text = hours.ToString("D2");
        DistanceText.text = (player.distanceTraveled/unitsPerMile).ToString("F1");
        TotalText.text = (player.path.pathLength/unitsPerMile).ToString("F1");
        progressBar.fillAmount = player.pathPosition/player.path.pathLength;
    }
}
