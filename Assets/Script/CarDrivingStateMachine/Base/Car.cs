using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Car : MonoBehaviour, IMoveable
{

    public CarDrivingStateMachine carDrivingStateMachine {get; set;}
    public CarGroundedState carGroundedState {get; set;} 
	public CarOverturnedState carOverturnedState {get; set;} 
	public CarAirborneState carAirborneState {get; set;} 
    public Animator carAnimator;
	


    public void Awake()
    {
        carDrivingStateMachine = new CarDrivingStateMachine();
        carGroundedState = new CarGroundedState(this, carDrivingStateMachine, carAnimator); 
		carOverturnedState = new CarOverturnedState(this, carDrivingStateMachine, carAnimator); 
		carAirborneState = new CarAirborneState(this, carDrivingStateMachine, carAnimator); 
		
    }

     private void Update()
    {
        carDrivingStateMachine.CurrentCarDrivingState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        carDrivingStateMachine.CurrentCarDrivingState.PhysicsUpdate();
    }


    public enum AnimationTriggerType
    {
        WheelSpin
    }

}