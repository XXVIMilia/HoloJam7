using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Unity.Mathematics;

public class Car : MonoBehaviour, ICarMoveable
{

    public CarDrivingStateMachine carDrivingStateMachine {get; set;}
    public CarGroundedState carGroundedState {get; set;} 
	public CarOverturnedState carOverturnedState {get; set;} 
	public CarAirborneState carAirborneState {get; set;}
    public AnimationCurve TorqueLookup { get; set; }
    public float accelInput { get; set; }
    public float brakeInput { get; set; }
    public float steeringInput { get; set; }

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

    public float topSpeed;
    public AnimationCurve powerCurve;
    public AnimationCurve steeringCurve;
    public float maxSteeringAngle;
    public float tireMass;
    
    //Tires: Assumes 4 Tires for all cars. No peanut or motorcycle unless implementation is changed
    [SerializeField] public Transform FR_Tire;
    [SerializeField] public Transform FL_Tire;
    [SerializeField] public Transform BL_Tire;
    [SerializeField] public Transform BR_Tire;

    //RB for tire calcs to act on
    public Rigidbody CarRB;

    //Car Specific animations. Not added rn
    public Animator carAnimator;
	
    public InputSystem_Actions controller;

    void OnEnable()
    {
        controller.Enable();
    }

    void OnDisable()
    {
        controller.Disable();
    }


    public void Awake()
    {
        carDrivingStateMachine = new CarDrivingStateMachine();
        carGroundedState = new CarGroundedState(this, carDrivingStateMachine, carAnimator); 
		carOverturnedState = new CarOverturnedState(this, carDrivingStateMachine, carAnimator); 
		carAirborneState = new CarAirborneState(this, carDrivingStateMachine, carAnimator);

        CarRB = GetComponent<Rigidbody>();

        carDrivingStateMachine.Initialize(carGroundedState);

        controller = new InputSystem_Actions();
        controller.Car.Acceleration.performed += accelCTX => SetAccelInput(accelCTX.ReadValue<float>());
        controller.Car.Acceleration.canceled += _ => SetAccelInput(0);
        controller.Car.Brake.performed += brakeCTX => SetBrakeInput(brakeCTX.ReadValue<float>());
        controller.Car.Brake.canceled += _ => SetBrakeInput(0);
        controller.Car.Steering.performed += steeringCTX => SetSteeringInput(steeringCTX.ReadValue<float>());
        controller.Car.Steering.canceled += _ => SetSteeringInput(0f);


		
    }

     private void Update()
    {
        carDrivingStateMachine.CurrentCarDrivingState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        carDrivingStateMachine.CurrentCarDrivingState.PhysicsUpdate();
    }


#region TireCalcs

    public void UpdateTireCalcs(Transform Tire)
    {
        RaycastHit hit;
        if(Physics.Raycast(Tire.transform.position,-Tire.transform.up, out hit, 1f))//Above 1f, the tire is not acting on the car body
        {
            PerformSuspensionCalc(Tire, hit);
            PerformSteeringCalc(Tire, hit);
            if(accelInput > 0.0f)
            {
                PerformAccelerationCalc(Tire, hit);
            }
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
        Vector3 steeringDir = Tire.forward;
        Vector3 tireVel = CarRB.GetPointVelocity(Tire.position);
        float steeringVel = Vector3.Dot(steeringDir,tireVel);
        float steeringRatio = Mathf.Clamp01(Vector3.Angle(steeringDir,tireVel)/180f);
        if (Tire.name.StartsWith("F"))
        {
            
            float desireVelChange = -steeringVel * FrontTireGrip.Evaluate(steeringRatio);
            float desiredAccel = desireVelChange / Time.fixedDeltaTime;
            // print(Tire.name + " Tire grip: " + desiredAccel);
            CarRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel,Tire.position);
        }
        else if (Tire.name.StartsWith("B"))
        {
            float desireVelChange = -steeringVel * BackTireGrip.Evaluate(steeringRatio);
            float desiredAccel = desireVelChange / Time.fixedDeltaTime;
            // print(Tire.name + " Tire grip: " + desiredAccel);
            CarRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel,Tire.position);
        }
        else
        {
            // print("Tire Naming error, using 0.3 grip");
            float desireVelChange = -steeringVel * 0.3f;
            float desiredAccel = desireVelChange / Time.fixedDeltaTime;
            CarRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel,Tire.position);
        }

        if(CarDriveTrain == DriveTrainType.BACK)
        {
            
            if (Tire.name.StartsWith("F"))
            {
                
                Tire.localRotation = Quaternion.AngleAxis(steeringInput*maxSteeringAngle,Tire.up);
            }
        }


        
    }

    public void PerformAccelerationCalc(Transform Tire, RaycastHit TireHit)
    {
        Vector3 accelDir = Tire.right;
        float carSpeed = Vector3.Dot(transform.forward, CarRB.linearVelocity);
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed)/topSpeed);
        float availableTorque = powerCurve.Evaluate(normalizedSpeed) * accelInput;
        CarRB.AddForceAtPosition(accelDir * availableTorque, Tire.position);
    }

#endregion

#region User Input

    public void SetAccelInput(float accelVal)
    {
        // print("Accel: " + accelVal);
        accelInput = accelVal;
    }

    public void SetBrakeInput(float brakeVal)
    {
        // print("Brake: " + brakeVal);
        brakeInput = brakeVal;
    }

    public void SetSteeringInput(float steeringVal)
    {
        // print("Steering: " + steeringVal);
        steeringInput = steeringVal;
    }

#endregion

    public enum AnimationTriggerType
    {
        WheelSpin
    }

}