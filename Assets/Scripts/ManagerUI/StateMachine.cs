using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class StateMachine
{
    public IState CurrentState { get; private set; }

    public MainMenuState mainMenuState;
    public ArticularState articularState;
    public CartesianState cartesianState;
    
    public StateMachine(GameManager gameManager)
    {
        this.mainMenuState = new MainMenuState(gameManager);
        this.articularState = new ArticularState(gameManager);
        this.cartesianState = new CartesianState(gameManager);
    }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}