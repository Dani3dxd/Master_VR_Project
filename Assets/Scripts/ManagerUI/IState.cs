using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter()
    {
        // begin state
    }

    public void Update()
    {
        // code per frame and finish state condition
    }

    public void Exit()
    {
        // execute when exit state
    }
}

