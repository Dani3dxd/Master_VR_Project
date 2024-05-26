using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UR3Controller : MonoBehaviour
{
    public GameObject RobotBase; // Este se puede borrar
    public GameObject controlCube;
    public float[] jointValues = new float[6];
    public float x = 0, y = 6, z = 3, phi = 0, theta = 0, psi;
    public float oldX, oldY, oldZ, oldPhi, oldTheta, oldPsi;
    //[Range(-10.0f, 10.0f)]
    public string stringX, stringY, stringZ, stringPhi, stringTheta, stringPsi;
    public bool userHasHitOk = false;

    private int counter = 1;
    private int frameCount = 200;
    private float floatX, floatY, floatZ, floatPhi, floatTheta, floatPsi; //Procurar utilizar verdaderos flotantes ya que este programa solo reconoce enteros
    private GameObject[] jointList = new GameObject[6]; // Este es el que sirve para operar
    private UR3Solver Robot1 = new UR3Solver();
    private UR3Solver Robot11
    {
        get
        {
            return Robot1;
        }

        set
        {
            Robot1 = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        initializeJoints();
        initializeCube();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        controlCube.transform.position = new Vector3(floatX, floatY, floatZ);
        controlCube.transform.eulerAngles = new Vector3(floatPhi, floatTheta, floatPsi);

        oldX = x;
        oldY = y;
        oldZ = z;
        oldPhi = phi;
        oldTheta = theta;
        oldPsi = psi;

        if (userHasHitOk)
        {
            if (counter >= frameCount)
            {
                counter = 1;
                userHasHitOk = false;
            }

            x = oldX + (counter * (controlCube.transform.position.x - oldX) / frameCount);
            y = oldY + (counter * (controlCube.transform.position.y - oldY) / frameCount);
            z = oldZ + (counter * (controlCube.transform.position.z - oldZ) / frameCount);
            phi = controlCube.transform.eulerAngles.x * Mathf.Deg2Rad;
            theta = controlCube.transform.eulerAngles.y * Mathf.Deg2Rad;
            psi = controlCube.transform.eulerAngles.z * Mathf.Deg2Rad;
            Debug.Log("psi = " + psi);

            Robot11.Solve(x, y, z, phi, theta, psi);

            for (int j = 0; j < 6; j++)
            {
                //modificar sobre este dependiendo del caso para cada eje
                Vector3 currentRotation = jointList[j].transform.localEulerAngles;
                if (j == 0 || j == 4)
                {
                    currentRotation.z = Robot11.solutionArray[j];
                    jointList[j].transform.localEulerAngles = currentRotation;
                }
                else
                {
                    currentRotation.y = Robot11.solutionArray[j];
                    jointList[j].transform.localEulerAngles = currentRotation;
                }
            }

            counter++;
        }
    }

    void initializeCube()
    {
        controlCube = GameObject.Find("Target");
        controlCube.transform.position = new Vector3(0f, 6f, 3f); //note, this is in format x, y, z - but y is up
        controlCube.transform.localScale = new Vector3(.3f, 1f, .3f);
        controlCube.transform.eulerAngles = new Vector3(0f, 0f, 0f); //in degrees

        floatX = 3f;
        floatY = 4f;
        floatZ = 3f;
        floatPhi = 0f;
        floatTheta = 0f;
        floatPsi = 0f;
    }

    void OnGUI() // esta parte quitarla y cambiarla por los sliders
    {
        GUI.color = Color.black;

        if (userHasHitOk)
        {
            //Esta parte permite convertir el valor ingresado en la interfaz en flotantes
            floatX = float.Parse(stringX);
            floatY = float.Parse(stringY);
            floatZ = float.Parse(stringZ);
            floatPhi = float.Parse(stringPhi);
            floatTheta = float.Parse(stringTheta);
            floatPsi = float.Parse(stringPsi);

            //Debug.Log("floatZ = " + floatZ + " and floatY = " + floatY);
        }
        else
        {
            //En esta parte se visualiza la interfaz grafica
            GUI.Label(new Rect(10, 10, 100, 30), "Enter X:");
            stringX = GUI.TextField(new Rect(150, 10, 50, 25), stringX, 40);
            GUI.Label(new Rect(10, 70, 150, 30), "Enter Psi (Degrees):");
            stringPsi = GUI.TextField(new Rect(150, 70, 50, 25), stringPsi, 40);
            GUI.Label(new Rect(10, 130, 100, 30), "Enter Y:");
            stringY = GUI.TextField(new Rect(150, 130, 50, 25), stringY, 40);
            GUI.Label(new Rect(10, 190, 150, 30), "Enter Theta (Degrees):");
            stringTheta = GUI.TextField(new Rect(150, 190, 50, 25), stringTheta, 40);
            GUI.Label(new Rect(10, 250, 100, 30), "Enter Z:");
            stringZ = GUI.TextField(new Rect(150, 250, 50, 25), stringZ, 40);
            GUI.Label(new Rect(10, 310, 150, 30), "Enter Phi (Degrees):");
            stringPhi = GUI.TextField(new Rect(150, 310, 50, 25), stringPhi, 40);



        }

        if (GUI.Button(new Rect(10, 370, 50, 30), "Enter"))
            userHasHitOk = true;
    }

    // Create the list of GameObjects that represent each joint of the robot
    void initializeJoints()//Reemplazar esto para utilizar un array que permita cada conjunto de eje ponerlo aqui
    {
        var RobotChildren = RobotBase.GetComponentsInChildren<Transform>();
        for (int i = 0; i < RobotChildren.Length; i++)
        {
            if (RobotChildren[i].name == "Joint1")
            {
                jointList[0] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "Joint2")
            {
                jointList[1] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "Joint3")
            {
                jointList[2] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "Joint4")
            {
                jointList[3] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "Joint5")
            {
                jointList[4] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "Joint6")
            {
                jointList[5] = RobotChildren[i].gameObject;
            }
        }
    }
}
