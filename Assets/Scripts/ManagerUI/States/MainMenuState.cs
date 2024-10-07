using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : IState
{
    private GameManager gameManager;

    public MainMenuState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        gameManager.articularBtn.onClick.AddListener(Articular);
        gameManager.cartesianBtn.onClick.AddListener(Cartesian);
    }
 
    public void Articular()
    {
        gameManager.stateMachine.TransitionTo(gameManager.stateMachine.articularState);
    }

    public void Cartesian()
    {
        gameManager.stateMachine.TransitionTo(gameManager.stateMachine.cartesianState);
    }

    public void Exit()
    {
        gameManager.canvasButtonInteractions.SetActive(true);
    }
}
