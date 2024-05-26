using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvKinematics : Robot
{
    private string valueKin = "J4+J5+J6";
    private void OnEnable()
    {
        Debug.Log(valueKin);
    }
}
