using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtControllerFromUI : ControllerBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void InitializeSliders()
    {
        for (int i = 0; i < sliders.Length; i++)
            switch (i)
            {
                case 0 or 5:
                    sliders[i].minValue = 0;
                    sliders[i].maxValue = 360;
                    break;
                
                case 1 or 3:
                    sliders[i].minValue = -180;
                    sliders[i].maxValue = 0;
                break;
                
                default:
                    sliders[i].minValue = -90;
                    sliders[i].maxValue = 90;
                break;
            }
    }

    protected override void OnSliderValueChanged()
    {
        
        UpdateValues();

        // Execute the movement and it depends from the slider value
        for (int j = 0; j < robotParts.Length; j++)
            robotParts[j].transform.localRotation = j switch
            {
                0 or 4 or 5 => Quaternion.Euler(0, sliders[j].value, 0),
                _ => Quaternion.Euler(sliders[j].value, 0, 0),
            };
    }
}
