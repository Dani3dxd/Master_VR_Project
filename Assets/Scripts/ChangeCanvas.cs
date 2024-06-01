using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCanvas : MonoBehaviour
{
    [SerializeField] private GameObject canvasArticular;
    [SerializeField] private GameObject canvasCartesian;
    [SerializeField] private GameObject controllerArticular;
    [SerializeField] private GameObject controllerCartesian;
    [SerializeField] private GameObject canvasButtonInteractions;
    public void whenButtonClickedArt()
    {
        if (canvasArticular.activeInHierarchy == false)
        {
            //active all articular parts with this part of code
            canvasArticular.SetActive(true);
            controllerArticular.SetActive(true);
            canvasButtonInteractions.SetActive(true);
            //deactive all the cartesian parts with this part of code
            canvasCartesian.SetActive(false);
            controllerCartesian.SetActive(false);
            //Position the canvas
            canvasButtonInteractions.transform.position = new Vector3(1.0f, 0.9f, 0.8f);
        }
        else
        // when clicked again on button deactive all that it active before
        {
            canvasArticular.SetActive(false);
            controllerArticular.SetActive(false);
            canvasButtonInteractions.SetActive(false);
        }
    }
    public void whenButtonClickedCart()
    {
        if (canvasCartesian.activeInHierarchy == false)
        {
            //active all cartesian movements with this part of code
            canvasCartesian.SetActive(true);
            controllerCartesian.SetActive(true);
            canvasButtonInteractions.SetActive(true);
            //deactive all articular movements with this part of code
            canvasArticular.SetActive(false);
            controllerArticular.SetActive(false);
            //Position the canvas button interactions
            canvasButtonInteractions.transform.position = new Vector3(-0.4f, 0.9f, 0.8f);
        }

        else 
        // when clicked again on button deactive all that it active before
        {
            canvasCartesian.SetActive(false);
            controllerCartesian.SetActive(false);
            canvasButtonInteractions.SetActive(false);
        }
    }
}
