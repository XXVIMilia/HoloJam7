using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Car : MonoBehaviour, ICarMoveable
{

    public CarDrivingStateMachine carDrivingStateMachine {get; set;}
    public CarGroundedState carGroundedState {get; set;} 
	public CarOverturnedState carOverturnedState {get; set;} 
	public CarAirborneState carAirborneState {get; set;}
    public AnimationCurve TorqueLookup { get; set; }

    //Suspension Details
    public float SuspensionOffset = 0.5f;
    public float SuspensionStrength = 0.5f;
    public float DamperStrength = 0.1f;

    //Steering Details
    public AnimationCurve FrontTireGrip;
    public AnimationCurve BackTireGrip;
    public enum DriveTrainType
    {
        FRONT,
        BACK,
        ALL
    }
    public DriveTrainType CarDriveTrain;
    
    //Tires: Assumes 4 Tires for all cars. No peanut or motorcycle unless implementation is changed
    [SerializeField] public Transform FR_Tire;
    [SerializeField] public Transform FL_Tire;
    [SerializeField] public Transform BL_Tire;
    [SerializeField] public Transform BR_Tire;

    //RB for tire calcs to act on
    public Rigidbody CarRB;

    //Car Specific animations. Not added rn
    public Animator carAnimator;
	


    public void Awake()
    {
        carDrivingStateMachine = new CarDrivingStateMachine();
        carGroundedState = new CarGroundedState(this, carDrivingStateMachine, carAnimator); 
		carOverturnedState = new CarOverturnedState(this, carDrivingStateMachine, carAnimator); 
		carAirborneState = new CarAirborneState(this, carDrivingStateMachine, carAnimator);

        CarRB = GetComponent<Rigidbody>();

        carDrivingStateMachine.Initialize(carGroundedState);
		
    }

     private void Update()
    {
        carDrivingStateMachine.CurrentCarDrivingState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        carDrivingStateMachine.CurrentCarDrivingState.PhysicsUpdate();
    }

    public void UpdateTireCalcs(Transform Tire)
    {
        RaycastHit hit;
        if(Physics.Raycast(Tire.transform.position,-Tire.transform.up, out hit, 1f))//Above 1f, the tire is not acting on the car body
        {
            PerformSuspensionCalc(Tire, hit);
            PerformSteeringCalc(Tire,hit);
        }
    }

    public void PerformSuspensionCalc(Transform Tire, RaycastHit TireHit)
    {
        Vector3 springDir = Tire.up;
        Vector3 TireVel = CarRB.GetPointVelocity(Tire.position);
        float offset = SuspensionOffset - TireHit.distance;
        float vel = Vector3.Dot(springDir,TireVel);
        float force = (offset * SuspensionStrength) - (vel * DamperStrength);
        CarRB.AddForceAtPosition(springDir * force, Tire.position);
    }

    public void PerformSteeringCalc(Transform Tire, RaycastHit TireHit)
    {
        Vector3 steeringDir = Tire.right;
        Vector3 tireVel = CarRB.GetPointVelocity(Tire.position);
        float steeringVel = Vector3.Dot(steeringDir,tireVel);
        if (Tire.name.StartsWith("F"))
        {
            float desireVelChange = -steeringVel * FrontTireGrip.Evaluate(steeringVel);
        }
    }

    

    public enum AnimationTriggerType
    {
        WheelSpin
    }

}