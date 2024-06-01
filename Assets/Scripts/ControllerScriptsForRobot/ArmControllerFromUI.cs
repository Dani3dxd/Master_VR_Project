using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmControllerFromUI : MonoBehaviour
{
    //This part call each part of robot joints
    [SerializeField] private Slider[] articularSliders = new Slider[6];

    //This part call each part of values show in canvas to know what happen internaly with values changed on robot
    [SerializeField] private TMP_Text[] valueArticulations = new TMP_Text[6]; //Try to improve this part for call one time like the function before
    
    //This part use each part of robot to transform the coordinates on robot
    [SerializeField] private GameObject[] robotArticulations = new GameObject[6];
   
    void Start()
    {
        DefineInitialValues();
        foreach (var slides in articularSliders)
        {
            slides.onValueChanged.AddListener((float value) =>
            {
                ProcessMovement(); //Call the function when change the value on the slider
            });        
        }
        
    }
    /// <summary>
    /// when starts the application set value for each slider on scene
    /// </summary>
    void DefineInitialValues()
    {
        for (int i = 0; i < articularSliders.Length; i++)
        {
            if (i == 0 || i == 5)
            {
                articularSliders[i].minValue = 0;
                articularSliders[i].maxValue = 360;
            }
            else if (i == 1)
            {
                articularSliders[i].minValue = -180;
                articularSliders[i].maxValue = 0;
            }
            else
            {
                articularSliders[i].minValue = -90;
                articularSliders[i].maxValue = 90;
            }
        }
    }
    /// <summary>
    /// Execute the movement and it depends from the slider value
    /// </summary>
    void ProcessMovement()
    {
        for (int i = 0; i < robotArticulations.Length; i++)
        {
            valueArticulations[i].text = articularSliders[i].value.ToString("F2");
            if (i == 0 || i == 4)
                robotArticulations[i].transform.localRotation = Quaternion.Euler(0, 0, articularSliders[i].value);
            else
                robotArticulations[i].transform.localRotation = Quaternion.Euler(0, articularSliders[i].value, 0);
        }
    }

}
