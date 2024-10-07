using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ControllerBase : MonoBehaviour
{
    //This part call each part of robot joints
    [SerializeField] protected Slider[] sliders = new Slider[6];

    //This part call each part of values show in canvas to know what happen internaly with values changed on robot
    [SerializeField] protected TMP_Text[] valueTexts = new TMP_Text[6];

    //This part use each part of robot to transform the coordinates on robot
    [SerializeField] protected GameObject[] robotParts = new GameObject[6];

    protected virtual void Start()
    {
        InitializeSliders();
        foreach (var slider in sliders)
            slider.onValueChanged.AddListener((float value) => OnSliderValueChanged()); //Call the function when change the value on the slider
    }

    /// <summary>
    /// When starts the application set value for each slider on scene
    /// </summary>
    protected abstract void InitializeSliders();
    protected abstract void OnSliderValueChanged();

    /// <summary>
    /// Refresh the value for each slider
    /// </summary>
    protected void UpdateValues()
    {
        for (int i = 0; i < robotParts.Length; i++)
            valueTexts[i].text = sliders[i].value.ToString("F2");
    }
}