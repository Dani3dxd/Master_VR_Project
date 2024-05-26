using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmControllerFromUI : MonoBehaviour
{
    //This part call each part of robot joints
    [SerializeField] private Slider[] robotSlides = new Slider[6];

    //This part call each part of values show in canvas to know what happen internaly with values changed on robot
    [SerializeField] private TMP_Text[] valueArticulations = new TMP_Text[6]; //Try to improve this part for call one time like the function before
    
    //This part use each part of robot to transform the coordinates on robot
    [SerializeField] private GameObject[] robotArticulations = new GameObject[6];
   
    void Start()
    {
        DefineInitialValues();
        //define initial values
        foreach (var slides in robotSlides)
        {
            slides.onValueChanged.AddListener((float value) =>
            {
                ProcessMovement();
            });        
        }
        
    }

    void DefineInitialValues()
    {
        for (int i = 0; i < robotSlides.Length; i++)
        {
            if (i == 0 || i == 5)
            {
                robotSlides[i].minValue = -180;
                robotSlides[i].maxValue = 180;
            }
            else if (i == 1)
            {
                robotSlides[i].minValue = -180;
                robotSlides[i].maxValue = 0;
            }
            else
            {
                robotSlides[i].minValue = -90;
                robotSlides[i].maxValue = 90;
            }
        }
    }

    void ProcessMovement()
    {
        for (int i = 0; i < robotArticulations.Length; i++)
        {
            valueArticulations[i].text = robotSlides[i].value.ToString("F2");
            if (i == 0 || i == 4)
                robotArticulations[i].transform.localRotation = Quaternion.Euler(0, 0, robotSlides[i].value);
            else
                robotArticulations[i].transform.localRotation = Quaternion.Euler(0, robotSlides[i].value, 0);
        }
    }

    /*
    */

}
