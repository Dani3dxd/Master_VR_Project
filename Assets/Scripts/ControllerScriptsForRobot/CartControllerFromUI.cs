using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartControllerFromUI : MonoBehaviour

{
    [SerializeField] private Slider[] cartesianSliders = new Slider[6];
    [SerializeField] private TMP_Text[] valueCartesian = new TMP_Text[6];
    [SerializeField] private GameObject[] robotCartesian = new GameObject[6];

    public GameObject controlCube;
    public float x = 0, y = 6, z = 3, phi = 0, theta = 0, psi;
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

    void Start()
    {
        initializeValues();
        initializeCube();
        
        foreach (var slides in cartesianSliders)
        {
            slides.onValueChanged.AddListener((float value) => //Call it when change the value
            {
                followCube();
                IKMovement();
            });
        }
    }

    void followCube()
    {
        x = cartesianSliders[0].value;
        y = cartesianSliders[1].value;
        z = cartesianSliders[2].value;
        phi = cartesianSliders[3].value;
        theta = cartesianSliders[4].value;
        psi = cartesianSliders[5].value;

        controlCube.transform.position = new Vector3(x, y, z);
        controlCube.transform.eulerAngles = new Vector3(phi, theta, psi);
    }
    /// <summary>
    /// This solves the inverse kinematics system from positions and orientations in final articulation to get the quaternion angle for each rotation
    /// </summary>
    void IKMovement()
    {
        
        Robot11.Solve(x, y, z, phi * Mathf.Deg2Rad, theta * Mathf.Deg2Rad, psi * Mathf.Deg2Rad);

        for (int j = 0; j < robotCartesian.Length; j++)
        {
            valueCartesian[j].text = cartesianSliders[j].value.ToString("F2");
            if (j == 0 || j == 4)
                robotCartesian[j].transform.localRotation = Quaternion.Euler(0, 0, Robot11.solutionArray[j]);
            else
                robotCartesian[j].transform.localRotation = Quaternion.Euler(0, Robot11.solutionArray[j], 0);
        }

    }
    /// <summary>
    /// when starts the application set value for each slider on scene
    /// </summary>
    void initializeValues()
    {
        for (int i = 0; i < cartesianSliders.Length; i++)
        {
            if (i == 3||i ==5)
            {
                cartesianSliders[i].minValue = -90;
                cartesianSliders[i].maxValue = 90;
            }
            else if(i == 4)
            {
                cartesianSliders[i].minValue = -180;
                cartesianSliders[i].maxValue = 180;
            }
            else if(i == 1)
            {
                cartesianSliders[i].minValue = 0;
                cartesianSliders[i].maxValue = 6;
            }
            else
            {
                cartesianSliders[i].minValue = -5;
                cartesianSliders[i].maxValue = 5;
            }
        }
    }
    /// <summary>
    /// Start the cartesian movement with a determinated position
    /// </summary>
    void initializeCube()
    {
        controlCube = GameObject.Find("Target");
        controlCube.transform.position = new Vector3(2f, 1f, 0f); //note, this is in format x, y, z - but y is up
        controlCube.transform.localScale = new Vector3(.3f, 1f, .3f);
        controlCube.transform.eulerAngles = new Vector3(0f, 0f, 0f); //in degrees
        cartesianSliders[0].value = controlCube.transform.position.x; cartesianSliders[1].value = controlCube.transform.position.y; cartesianSliders[2].value = controlCube.transform.position.z;
    }
}
