using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Articular Menu")]
    [SerializeField] public GameObject controllerArticular;
    [SerializeField] public GameObject canvasArticular;
    [SerializeField] public Button articularBtn;


    [Header("Cartesian Menu")]
    [SerializeField] public GameObject controllerCartesian;
    [SerializeField] public GameObject canvasCartesian;
    [SerializeField] public Button cartesianBtn;

    [Header("Main Menu")]
    [SerializeField] public GameObject canvasButtonInteractions;

    public StateMachine stateMachine;

    void Start()
    {
        stateMachine = new(this);
        stateMachine.Initialize(stateMachine.mainMenuState);
    }
}
