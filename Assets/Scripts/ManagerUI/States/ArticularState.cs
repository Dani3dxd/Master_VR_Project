using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticularState :IState
{
    private GameManager gameManager;

    public ArticularState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        gameManager.cartesianBtn.onClick.AddListener(Cartesian);
        gameManager.controllerArticular.SetActive(true);
        gameManager.canvasArticular.SetActive(true);
        gameManager.canvasButtonInteractions.transform.position = new Vector3(1.0f, 0.9f, 0.8f);
    }

    public void Cartesian()
    {
        gameManager.stateMachine.TransitionTo(gameManager.stateMachine.cartesianState);
    }


    public void Exit()
    {
        gameManager.controllerArticular.SetActive(false);
        gameManager.canvasArticular.SetActive(false);
    }
}
