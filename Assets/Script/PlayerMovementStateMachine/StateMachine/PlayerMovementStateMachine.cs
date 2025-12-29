using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine
{
    public PlayerMovementState CurrentPlayerMovementState { get; set; }
    public PlayerMovementState PreviousPlayerMovementState { get; set; }

    public void Initialize(PlayerMovementState startingState)
    {
        PreviousPlayerMovementState = startingState;
        CurrentPlayerMovementState = startingState;
        CurrentPlayerMovementState.EnterState();
    }

    public void ChangeState(PlayerMovementState newState)
    {
        CurrentPlayerMovementState.ExitState();
        PreviousPlayerMovementState = CurrentPlayerMovementState;
        CurrentPlayerMovementState = newState;
        CurrentPlayerMovementState.EnterState();
    }
}