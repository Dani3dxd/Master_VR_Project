using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ExecuteTrajectories : MonoBehaviour
{
    [SerializeField] private GameObject[] axis = new GameObject[6];
    [SerializeField] private float lerpDuration = 5f; // time between a execute articulation and the next one
    [SerializeField] private AnimationCurve curve; //animation curve to simulate a smooth movement

    //List trajectories for each articulation
    [SerializeField] private List<Vector3> trajectory0 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory1 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory2 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory3 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory4 = new List<Vector3>();
    [SerializeField] private List<Vector3> trajectory5 = new List<Vector3>();

    /// <summary>
    /// When press the button this function allows to program each articulation
    /// </summary>
    public void ListMovements()
    {
        trajectory0.Add(axis[0].transform.localEulerAngles);
        trajectory1.Add(axis[1].transform.localEulerAngles);
        trajectory2.Add(axis[2].transform.localEulerAngles);
        trajectory3.Add(axis[3].transform.localEulerAngles);
        trajectory4.Add(axis[4].transform.localEulerAngles);
        trajectory5.Add(axis[5].transform.localEulerAngles);
    }
    /// <summary>
    /// When press the button this function eliminates all the previous trajectories and clear the workspace
    /// </summary>
    public void CleanMovements()
    {
        trajectory0.Clear();
        trajectory1.Clear();
        trajectory2.Clear();
        trajectory3.Clear();
        trajectory4.Clear();
        trajectory5.Clear();
    }
    /// <summary>
    /// When press the button execute the movement trajectories to all positions in the list
    /// </summary>
    public void ExecuteMovement()
    {
        if (trajectory0.Count>=2)
            StartCoroutine(lerpAngular(lerpDuration));        
    }
    
    IEnumerator lerpAngular(float lerpDuration)
    {        
        float timeElapsed = 0f;
        int currentPointIndex = 0;
        //int i = 0;
        while (timeElapsed < lerpDuration)
        {
            for (int i = 0; i < axis.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory0[currentPointIndex]), Quaternion.Euler(trajectory0[currentPointIndex+1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 1:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory1[currentPointIndex]), Quaternion.Euler(trajectory1[currentPointIndex + 1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 2:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory2[currentPointIndex]), Quaternion.Euler(trajectory2[currentPointIndex + 1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 3:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory3[currentPointIndex]), Quaternion.Euler(trajectory3[currentPointIndex + 1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 4:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory4[currentPointIndex]), Quaternion.Euler(trajectory4[currentPointIndex + 1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    case 5:
                        axis[i].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectory5[currentPointIndex]), Quaternion.Euler(trajectory5[currentPointIndex + 1]), curve.Evaluate(timeElapsed / lerpDuration));
                        break;
                    default:
                        break;
                }
            }
            
            timeElapsed += Time.deltaTime;
            if (Quaternion.Angle(axis[0].transform.localRotation, Quaternion.Euler(trajectory0[currentPointIndex + 1])) < 0.1f)
                if (Quaternion.Angle(axis[1].transform.localRotation, Quaternion.Euler(trajectory1[currentPointIndex + 1])) < 0.1f)
                    if (Quaternion.Angle(axis[2].transform.localRotation, Quaternion.Euler(trajectory2[currentPointIndex + 1])) < 0.1f)
                        if (Quaternion.Angle(axis[3].transform.localRotation, Quaternion.Euler(trajectory3[currentPointIndex + 1])) < 0.1f)
                            if (Quaternion.Angle(axis[4].transform.localRotation, Quaternion.Euler(trajectory4[currentPointIndex + 1])) < 0.1f)
                                if(Quaternion.Angle(axis[5].transform.localRotation, Quaternion.Euler(trajectory5[currentPointIndex + 1])) < 0.1f)
                                    {
                                        timeElapsed = 0;
                                        currentPointIndex = (currentPointIndex + 1) % trajectory5.Count;
                                    }
            yield return null;
        }
    }
}
