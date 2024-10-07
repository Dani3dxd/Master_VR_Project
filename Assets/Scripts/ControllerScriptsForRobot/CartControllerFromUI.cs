using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CartControllerFromUI : ControllerBase
{
    private UR3Solver robotSolverIK = new UR3Solver();
    private float x, y, z, phi, theta, psi;

    protected override void Start()
    {
        base.Start();
        InitialPosEndEffector();
    }

    protected override void InitializeSliders()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            switch(i)
            {
                case 0 or 2:
                    sliders[i].minValue = -9;
                    sliders[i].maxValue = 9;
                    break;
                case 1:
                    sliders[i].minValue = 0;
                    sliders[i].maxValue = 8;
                    break;
                case 4:
                    sliders[i].minValue = -180;
                    sliders[i].maxValue = 180;
                    break;
                default:
                    sliders[i].minValue = -90;
                    sliders[i].maxValue = 90;
                    break;
            }
        }
    }

    protected override void OnSliderValueChanged()
    {
        FollowEndEffector();
        IKMovement();
    }

    private void FollowEndEffector()
    {
        x = sliders[0].value;
        y = sliders[1].value;
        z = sliders[2].value;
        phi = sliders[3].value;
        theta = sliders[4].value;
        psi = sliders[5].value;
    }

    /// <summary>
    /// This solves the inverse kinematics system from positions and orientations in final articulation to get the quaternion angle for each rotation
    /// </summary>
    private void IKMovement()
    {
        robotSolverIK.Solve(x, y, z, phi * Mathf.Deg2Rad, theta * Mathf.Deg2Rad, psi * Mathf.Deg2Rad);
        UpdateValues();

        for (int j = 0; j < robotParts.Length; j++)
            robotParts[j].transform.localRotation = j switch
            {
                0 or 4 or 5=> Quaternion.Euler(0, robotSolverIK.solutionArray[j], 0),
                _ => Quaternion.Euler(robotSolverIK.solutionArray[j], 0, 0),
            };
    }

    /// <summary>
    /// Start the cartesian movement with a determinated position
    /// </summary>
    private void InitialPosEndEffector()
    {
        for (int j = 0; j < robotParts.Length; j++)
            sliders[j].value = j switch
            {
                0 => -5,
                1 => 4,
                2 => 0,
                _ => 0f,
            };
    }
}
