using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGroundedState : CarDrivingState
{
    public CarGroundedState(Car car, CarDrivingStateMachine carDrivingStateMachine, Animator drivingAnimator) : base(car, carDrivingStateMachine, drivingAnimator)
    {
    }

    //Overrides will be generated after regeneration and adding into the unity project

    public override void EnterState()
    {
        base.EnterState();
        car.JumpForce = 2f;
        Debug.Log("Entering Grounded State");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        car.UpdateTireCalcs(car.FR_Tire);
        car.UpdateTireCalcs(car.FL_Tire);
        car.UpdateTireCalcs(car.BR_Tire);
        car.UpdateTireCalcs(car.BL_Tire);
        if (!car.CheckAirborne())
        {
            carDrivingStateMachine.ChangeState(car.carAirborneState);
        }
    }

    public override void AnimationTriggerEvent(Car.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }



}