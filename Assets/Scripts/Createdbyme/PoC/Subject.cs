using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class Subject : MonoBehaviour
{
    public event Action ThingHappened;
    
    public void OnMouseDown()
    {
        ThingHappened?.Invoke();
    }
    public void OnCollisionEnter(Collision collision)
    {
        ThingHappened?.Invoke();
    }

}
