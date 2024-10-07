using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ExecuteTrajectories : MonoBehaviour
{
    [Header("Trajectories Settings")]
    [SerializeField] private GameObject[] axis = new GameObject[6];
    [SerializeField] private float trajectoriesDuration = 5f; // time between a execute articulation and the next one
    [SerializeField] private AnimationCurve curve; //animation curve to simulate a smooth movement

    //List trajectories for each articulation
    [SerializeField] private List<List<Vector3>> trajectories = new List<List<Vector3>>()
    {
        new List<Vector3>(),
        new List<Vector3>(),
        new List<Vector3>(),
        new List<Vector3>(),
        new List<Vector3>(),
        new List<Vector3>()
    };

    private int trajectoriesCount=0;
    
    /// <summary>
    /// When press the button this function allows to program save current angle for each articulation
    /// </summary>
    public void ListMovements()
    {
        for (int i = 0; i < trajectories.Count; i++)
            trajectories[trajectoriesCount].Add(axis[i].transform.localEulerAngles);
        trajectoriesCount++;
    }
    /// <summary>
    /// When press the button this function eliminates all the previous trajectories and clear the workspace
    /// </summary>
    public void CleanMovements()
    {
        for (int i = 0; i < trajectories.Count; i++)
            trajectories[i].Clear();
        trajectoriesCount = 0;
    }
    /// <summary>
    /// When press the button execute the movement trajectories to all positions in the list when there is more than one
    /// </summary>
    public void ExecuteMovement()
    {
        if (trajectories.Count>=2)
            StartCoroutine(AngularAxisMovement(trajectoriesDuration));
    }
    IEnumerator AngularAxisMovement(float totalTime)
    {        
        float timeElapsed = 0f;
        int currentTrajectoryIndex = 0;
        
        while (timeElapsed < totalTime)
        {
            for (int j = 0; j < trajectories.Count; j++)
                axis[j].transform.localRotation = Quaternion.Lerp(Quaternion.Euler(trajectories[currentTrajectoryIndex][j]), Quaternion.Euler(trajectories[currentTrajectoryIndex + 1][j]), curve.Evaluate(timeElapsed / totalTime));

            timeElapsed += Time.deltaTime;
            bool allAnglesCompleted = true;

            for (int pointIndex = 0; pointIndex < trajectories.Count; pointIndex++)
                if (Quaternion.Angle(axis[pointIndex].transform.localRotation, Quaternion.Euler(trajectories[currentTrajectoryIndex + 1][pointIndex])) >= 0.1f)
                {
                    allAnglesCompleted = false;
                    break;
                }

            if (allAnglesCompleted)
            {
                timeElapsed = 0;
                currentTrajectoryIndex = (currentTrajectoryIndex + 1) % trajectories.Count;
            }

            yield return null;
        }
    }
}
