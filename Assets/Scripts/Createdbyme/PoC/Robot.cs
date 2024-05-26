using InverseKinematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private ArmControllerFromUI armController;
    [SerializeField] private JointController cartController;

    private void Start()
    {
        armController = GetComponent<ArmControllerFromUI>();
        cartController = GetComponent<JointController>();
    }
}
