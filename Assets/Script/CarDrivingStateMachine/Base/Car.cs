using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using DG.Tweening;
using Unity.Mathematics;
using TMPro;

public class Car : MonoBehaviour, ICarMoveable
{
    #region Declarations
    public CarDrivingStateMachine carDrivingStateMachine { get; set; }
    public CarGroundedState carGroundedState { get; set; }
    public CarDriftState carDriftState { get; set; }
    public CarOverturnedState carOverturnedState { get; set; }
    public CarAirborneState carAirborneState { get; set; }
    public AnimationCurve TorqueLookup { get; set; }
    public float accelInput { get; set; }
    public float brakeInput { get; set; }
    public float steeringInput { get; set; }
    public int tireAirborn { get; set; }

    [Header("Suspension")]
    public float SuspensionOffset = 0.5f;
    public float SuspensionStrength = 0.5f;
    public float DamperStrength = 0.1f;
    public float JumpForce = 5f;

    [Header("Steering")]
    public AnimationCurve FrontTireGrip;
    public AnimationCurve BackTireGrip;
    public enum DriveTrainType
    {
        FRONT,
        BACK,
        ALL
    }
    public DriveTrainType CarDriveTrain;

    [Header("Speed Rules")]
    public float topSpeed;
    public float accelForce = 5f;
    public float carSpeed;
    public AnimationCurve powerCurve;
    public AnimationCurve steeringCurve;
    public AnimationCurve brakeCurve;
    public float breakForce = 5;
    public float maxSteeringAngle;
    public float ackermanConstant;
    public float tireMass;

    [Header("Drifting")]
    [SerializeField] private float driftTractionPercent = 0.5f;
    [SerializeField] private float currentTractionPercent = 1f;

    //Tires: Assumes 4 Tires for all cars. No peanut or motorcycle unless implementation is changed
    [Header("References")]
    [SerializeField] public Transform FR_Tire;
    [SerializeField] public Transform FL_Tire;
    [SerializeField] public Transform BL_Tire;
    [SerializeField] public Transform BR_Tire;

    //RB for tire calcs to act on
    public Rigidbody CarRB;
    public BoxCollider CarCol;
    [SerializeField] public TrailRenderer BR_Trail;
    [SerializeField] public TrailRenderer BL_Trail;

    //Car Specific animations. Not added rn
    [Header("Animator")]
    public Animator carAnimator;

    public InputSystem_Actions controller;

    #endregion

    #region Debug
    [Header("Debug")]
    public TextMeshProUGUI Velocity;
    public TextMeshProUGUI Accel;


    #endregion

    #region Monobehavior

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
        carDriftState = new CarDriftState(this, carDrivingStateMachine, carAnimator);
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
        controller.Car.Drift.performed += SetDrift;
        controller.Car.Drift.canceled += CancelDrift;




    }

    private void Update()
    {
        carDrivingStateMachine.CurrentCarDrivingState.FrameUpdate();
        if (carDrivingStateMachine.CurrentCarDrivingState == carGroundedState && currentTractionPercent < 1)
        {
            LerpTraction(1f);
        }
        else if (carDrivingStateMachine.CurrentCarDrivingState == carDriftState && currentTractionPercent > driftTractionPercent)
        {
            LerpTraction(driftTractionPercent);
        }
    }

    private void FixedUpdate()
    {
        carDrivingStateMachine.CurrentCarDrivingState.PhysicsUpdate();
    }

    #endregion


    #region TireCalcs

    public void UpdateTireCalcs(Transform Tire)
    {
        RaycastHit hit;
        if (Physics.Raycast(Tire.transform.position, -Tire.transform.up, out hit, 0.75f))//Above 1f, the tire is not acting on the car body
        {
            PerformSuspensionCalc(Tire, hit);
            PerformSteeringCalc(Tire, hit);
            CheckSpeed();
            Velocity.text = carSpeed.ToString();

            if (accelInput > 0f)
            {
                PerformAccelerationCalc(Tire, hit);
            }
            PerformBreakCalc(Tire, hit);
        }

    }

    public void PerformSuspensionCalc(Transform Tire, RaycastHit TireHit)
    {
        Vector3 springDir = Tire.up;
        float force;
        Vector3 TireVel = CarRB.GetPointVelocity(Tire.position);
        float offset = SuspensionOffset - TireHit.distance;
        float vel = Vector3.Dot(springDir, TireVel);
        if (Tire.name.StartsWith("F"))
        {
            force = Mathf.Abs(Mathf.Clamp((offset * SuspensionStrength) - (vel * DamperStrength), -JumpForce, 10000f));//Negative suspension is mostly removed
        }
        else
        {
            force = Mathf.Clamp((offset * SuspensionStrength) - (vel * DamperStrength), -JumpForce, 10000f);//Negative suspension is mostly removed
        }
        Debug.DrawRay(Tire.position, springDir * force, Color.green);
        CarRB.AddForceAtPosition(springDir * force, Tire.position);
    }

    public void PerformSteeringCalc(Transform Tire, RaycastHit TireHit)
    {
        Vector3 steeringDir = -Tire.forward;
        Vector3 tireVel = CarRB.GetPointVelocity(Tire.position);
        float steeringVel = Vector3.Dot(tireVel, steeringDir);
        float steeringRatio = Mathf.Clamp01(Vector3.Angle(tireVel, steeringDir) / 90f);


        if (Tire.name.StartsWith("F"))
        {
            float desireVelChange = -steeringVel * FrontTireGrip.Evaluate(steeringRatio) * currentTractionPercent;
            float desiredAccel = desireVelChange / Time.fixedDeltaTime;
            Debug.DrawRay(Tire.position, steeringDir * tireMass * desiredAccel, Color.red);
            CarRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel, Tire.position);


        }
        else if (Tire.name.StartsWith("B"))
        {
            float desireVelChange = -steeringVel * BackTireGrip.Evaluate(steeringRatio) * currentTractionPercent;
            float desiredAccel = desireVelChange / Time.fixedDeltaTime;
            // print(Tire.name + " Tire grip: " + desiredAccel);
            CarRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel, Tire.position);


        }
        else
        {
            // print("Tire Naming error, using 0.3 grip");
            float desireVelChange = -steeringVel * 0.3f;
            float desiredAccel = desireVelChange / Time.fixedDeltaTime;
            CarRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel, Tire.position);
        }

        if (CarDriveTrain == DriveTrainType.BACK)
        {
            if (Tire.name.StartsWith("FL"))
            {
                if (steeringInput < 0f)
                {
                    // print(Tire.name + "AckermanOffset: " + ackermanConstant * steeringInput);
                    Tire.localRotation = Quaternion.AngleAxis(ackermanConstant * steeringInput * maxSteeringAngle, transform.up);
                }
                else
                {
                    Tire.localRotation = Quaternion.AngleAxis(steeringInput * maxSteeringAngle, transform.up);
                }
            }
            else if (Tire.name.StartsWith("FR"))
            {
                if (steeringInput > 0f)
                {
                    // print(Tire.name + "AckermanOffset: " + ackermanConstant * steeringInput);
                    Tire.localRotation = Quaternion.AngleAxis(ackermanConstant * steeringInput * maxSteeringAngle, transform.up);
                }
                else
                {
                    Tire.localRotation = Quaternion.AngleAxis(steeringInput * maxSteeringAngle, transform.up);
                }
            }
        }
    }

    public void PerformAccelerationCalc(Transform Tire, RaycastHit TireHit)
    {
        Vector3 accelDir = Tire.right;
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);
        // print(normalizedSpeed);
        float availableTorque = powerCurve.Evaluate(normalizedSpeed) * accelForce * accelInput;
        Accel.text = availableTorque.ToString();
        Debug.DrawRay(Tire.position, accelDir * availableTorque, Color.blue);
        CarRB.AddForceAtPosition(accelDir * availableTorque, Tire.position);
    }

    public void PerformBreakCalc(Transform Tire, RaycastHit TireHit)
    {
        Vector3 accelDir = -1 * Tire.right;
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);
        float availableTorque = brakeCurve.Evaluate(normalizedSpeed) * breakForce * brakeInput;
        Debug.DrawRay(Tire.position, accelDir * availableTorque, Color.purple);
        CarRB.AddForceAtPosition(accelDir * availableTorque, Tire.position);
    }

    private void LerpTraction(float target)
    {
        if (target > currentTractionPercent)
        {
            currentTractionPercent += Time.deltaTime / 3;
            if (currentTractionPercent >= target)
            {
                currentTractionPercent = target;
            }
        }
        else if (target < currentTractionPercent)
        {
            currentTractionPercent -= Time.deltaTime ;
            if (currentTractionPercent <= target)
            {
                currentTractionPercent = target;
            }
        }
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

    public void SetDrift(InputAction.CallbackContext context)
    {
        if (CheckAirborne())
        {
            carDrivingStateMachine.ChangeState(carDriftState);
        }
    }

    public void CancelDrift(InputAction.CallbackContext context)
    {
        carDrivingStateMachine.ChangeState(carDrivingStateMachine.PreviousCarDrivingState);
    }

    public bool CheckAirborne()
    {
        //Vector3 YOffset = new Vector3(0f,CarCol.size.y + 0.666f,0f);
        Debug.DrawRay(transform.position, -transform.up * 0.65f, Color.orange);
        return Physics.Raycast(transform.position, -transform.up, 0.75f, LayerMask.NameToLayer("Player"));

    }

    private void CheckSpeed()
    {
        carSpeed = Vector3.Dot(transform.right, CarRB.linearVelocity);
    }

    #endregion

    public enum AnimationTriggerType
    {
        WheelSpin
    }

}