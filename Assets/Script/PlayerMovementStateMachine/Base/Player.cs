using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IMoveable
{

    public PlayerMovementStateMachine playerMovementStateMachine {get; set;}
    public PlayerGroundedState playerGroundedState {get; set;} 
	public PlayerAirborneState playerAirborneState {get; set;} 
	public PlayerCrouchingState playerCrouchingState {get; set;} 
	public PlayerDashingState playerDashingState {get; set;}
    public float MoveInput { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Awake()
    {
        playerMovementStateMachine = new PlayerMovementStateMachine();
        playerGroundedState = new PlayerGroundedState(this, playerMovementStateMachine); 
		playerAirborneState = new PlayerAirborneState(this, playerMovementStateMachine); 
		playerCrouchingState = new PlayerCrouchingState(this, playerMovementStateMachine); 
		playerDashingState = new PlayerDashingState(this, playerMovementStateMachine); 
		
    }

     private void Update()
    {
        playerMovementStateMachine.CurrentPlayerMovementState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        playerMovementStateMachine.CurrentPlayerMovementState.PhysicsUpdate();
    }

    public void SetMoveInput(float moveValue)
    {
        throw new NotImplementedException();
    }


    //Overrides will be generated after regeneration and adding into the unity project

}