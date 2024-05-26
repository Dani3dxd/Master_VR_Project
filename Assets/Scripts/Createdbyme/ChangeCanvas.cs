using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCanvas : MonoBehaviour
{
    [SerializeField] private GameObject canvasArticular;
    [SerializeField] private GameObject canvasCartesian;
    [SerializeField] private GameObject controllerArticular;
    [SerializeField] private GameObject controllerCartesian;
    public void whenButtonClickedArt()
    {
        if (canvasArticular.activeInHierarchy == false)
        {
            //active all articular parts with this part of code
            canvasArticular.SetActive(true);
            controllerArticular.SetActive(true);
            //deactive all the cartesian parts with this part of code
            canvasCartesian.SetActive(false);
            controllerCartesian.SetActive(false);
        }
        else
            canvasArticular.SetActive(false);
    }
    public void whenButtonClickedCart()
    {
        if(canvasCartesian.activeInHierarchy==false)
        {
            //active all cartesian movements with this part of code
            canvasCartesian.SetActive(true);
            controllerCartesian.SetActive(true);
            //deactive all articular movements with this part of code
            canvasArticular.SetActive(false);
            controllerArticular.SetActive(false);
        }
            
        else 
            canvasCartesian.SetActive(false);
    }
}
