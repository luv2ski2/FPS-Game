using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Transform timer;

    private bool isPlaying = true;

    private double time;

    private double startTime;

    // public getter for time, variable is called GameTime
    public double GameTime => time;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            time = Math.Round(Time.time, 2);
            time -= startTime;
            time = Math.Round(time, 2);

            timer.GetComponent<TextMeshProUGUI>().text = "Timer: " + time;
        }
    }

    public void stopPlaying()
    {
        isPlaying = false;
    }

    public void startPlaying()
    {
        isPlaying = true;
    }
    
}
