using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartesianState : IState
{
    private GameManager gameManager;

    public CartesianState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    
    public void Enter()
    {
        gameManager.articularBtn.onClick.AddListener(Articular);
        gameManager.controllerCartesian.SetActive(true);
        gameManager.canvasCartesian.SetActive(true);
        gameManager.canvasButtonInteractions.transform.position = new Vector3(-0.4f, 0.9f, 0.8f);
    }

    public void Articular()
    {
        gameManager.stateMachine.TransitionTo(gameManager.stateMachine.articularState);
    }


    public void Exit()
    {
        gameManager.controllerCartesian.SetActive(false);
        gameManager.canvasCartesian.SetActive(false);
    }
}
