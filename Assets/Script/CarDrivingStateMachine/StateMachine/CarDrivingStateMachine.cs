using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDrivingStateMachine
{
    public CarDrivingState CurrentCarDrivingState { get; set; }
    public CarDrivingState PreviousCarDrivingState { get; set; }

    public void Initialize(CarDrivingState startingState)
    {
        PreviousCarDrivingState = startingState;
        CurrentCarDrivingState = startingState;
        CurrentCarDrivingState.EnterState();
    }

    public void ChangeState(CarDrivingState newState)
    {
        CurrentCarDrivingState.ExitState();
        PreviousCarDrivingState = CurrentCarDrivingState;
        CurrentCarDrivingState = newState;
        CurrentCarDrivingState.EnterState();
    }
}