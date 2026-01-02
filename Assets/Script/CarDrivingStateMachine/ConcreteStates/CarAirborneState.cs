using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAirborneState : CarDrivingState
{
    public CarAirborneState(Car car, CarDrivingStateMachine carDrivingStateMachine, Animator drivingAnimator) : base(car, carDrivingStateMachine, drivingAnimator)
    {
    }

    //Overrides will be generated after regeneration and adding into the unity project
    public override void EnterState()
    {
        base.EnterState();
        car.JumpForce = 0f;
        Debug.Log("Entering Airborn State");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        car.CarRB.angularVelocity = new Vector3(0f,car.steeringInput,0f);
        if (car.CheckAirborne())
        {
            carDrivingStateMachine.ChangeState(car.carGroundedState);
        }


    }

}