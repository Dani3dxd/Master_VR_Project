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
    [SerializeField] private float trajectoriesDuration = 10f; // time between a execute articulation and the next one
    [SerializeField] private AnimationCurve curve; //animation curve to simulate a smooth movement
    
    [Header("Arduino Connection")]
    [SerializeField] private ArduinoController arduinoController; //Reference to send controller the information to arduino
    [SerializeField] private List<string> dataListArduino = new List<string>(); //List the information in a string
    private int[] motorAngles = new int[6]; //Prepare the data for save the information of each angle

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
        //Vector3 angleVector = new Vector3();
        
        for (int i = 0; i < trajectories.Count; i++)
            trajectories[trajectoriesCount].Add(axis[i].transform.localEulerAngles);

        for (int j = 0; j < motorAngles.Length; j++)
            switch (j)
            {
                case 0 or 5:
                    motorAngles[j] = Mathf.RoundToInt(trajectories[trajectoriesCount][j].y);
                    break;
                case 2:
                    motorAngles[j] = Mathf.RoundToInt(MapAngle(trajectories[trajectoriesCount][j].x, trajectories[trajectoriesCount][j].y, j));                   
                    break;
                case 4:
                    motorAngles[j] = Mathf.RoundToInt(MapAngle(trajectories[trajectoriesCount][j].y, trajectories[trajectoriesCount][j].y, j));

                    break;
                default: //1 or 3
                    motorAngles[j] = Mathf.RoundToInt(MapAngle(trajectories[trajectoriesCount][j].x, trajectories[trajectoriesCount][j].y, j));
                    break;
            }

        if (trajectoriesCount != 0)    //if trajectory is not the first one save in other case ignore
        {
            if (arduinoController.enableMotors) //if the button that enables the value of the 3 last motors
                dataListArduino.Add(string.Join("*", motorAngles) + "\n"); //save and prepare information like a String to send later to arduino
            else
                dataListArduino.Add("*"+motorAngles[0]+"*"+motorAngles[1]+"*"+motorAngles[2]+"*181*181*0\n"); // in other case send a error value to disable the last 3 motors
        }

        
        trajectoriesCount++;
    }

    /// <summary>
    /// When press the button this function eliminates all the previous trajectories and clear the workspace
    /// </summary>
    public void CleanMovements()
    {
        for (int i = 0; i < trajectories.Count; i++)
            trajectories[i].Clear();
        dataListArduino.Clear();
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
        sendDataToArduino(currentTrajectoryIndex);
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
                sendDataToArduino(currentTrajectoryIndex);
            }

            yield return null;
        }
    }

    /// <summary>
    /// Send information to arduino for pose in a correctly way each articulation
    /// </summary>
    public void sendDataToArduino(int current)
    {
        if (arduinoController != null && arduinoController.isConnected)
            arduinoController.SendData(dataListArduino[current]);
        else
            Debug.LogWarning("Arduino no está conectado o no se encontró el controlador.");
    }

    float MapAngle(float xAngle, float yAngle, int iteration)
    {
        float newAngle = 0f;

        switch (iteration)
        {
            case 1:
                if (yAngle == 0f)
                    newAngle = (xAngle == 0f) ? 0f : (xAngle - 360f) * -1f;

                else if (yAngle == 180f)
                    newAngle = (xAngle < 1f) ? 180f : xAngle - 180f;
                break;

            case 2:
                if (yAngle == 0f)
                {

                    if (xAngle >= 270 && xAngle < 360)
                        newAngle = Mathf.Lerp(90, 180, Mathf.InverseLerp(270, 360, xAngle));

                    else if (xAngle >= 0 && xAngle <= 90)
                        newAngle = Mathf.Lerp(180, 270, Mathf.InverseLerp(0, 90, xAngle));
                }
                else if (yAngle == 180f)
                {

                    if (xAngle >= 270 && xAngle < 360)
                        newAngle = Mathf.Lerp(90, 0, Mathf.InverseLerp(270, 360, xAngle));

                    else if (xAngle >= 0 && xAngle <= 90)
                        newAngle = Mathf.Lerp(360, 270, Mathf.InverseLerp(0, 90, xAngle));
                }
                break;

            case 3:
                if (yAngle == 180f)
                    newAngle = (xAngle < 1f) ? 0f : (xAngle - 360f) * -1f;

                else if (yAngle == 0f)
                    newAngle = (xAngle < 1f) ? 180f : xAngle - 180f;
                break;

            case 4:
                if (xAngle >= 270 && xAngle < 360)
                    newAngle = Mathf.Lerp(180, 90, Mathf.InverseLerp(270, 360, xAngle));

                else if (xAngle >= 0 && xAngle <= 90)
                    newAngle = Mathf.Lerp(90, 0, Mathf.InverseLerp(0, 90, xAngle));
                break;

            default:
                Debug.LogWarning("Iteration value is not valid.");
                break;
        }

        return newAngle;
    }
}
