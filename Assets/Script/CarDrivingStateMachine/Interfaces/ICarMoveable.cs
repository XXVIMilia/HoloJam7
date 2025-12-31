using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WARNING: Confirm This interface doesn't already exist before adding to unity!

public interface ICarMoveable
{
    //The public interface the state machine will be able to interact with
    //Variables and functions added here will be forced to implement in the base class
    float accelInput {get; set;}
    float brakeInput {get; set;}
    float steeringInput {get; set;}


    




  
    
    void UpdateTireCalcs(Transform Tire);
    void PerformSuspensionCalc(Transform Tire, RaycastHit TireHit);
    void PerformSteeringCalc(Transform Tire, RaycastHit TireHit);
    void PerformAccelerationCalc(Transform Tire, RaycastHit TireHit);
    void PerformBreakCalc(Transform Tire, RaycastHit TireHit);


    void SetAccelInput(float accelVal);
    void SetBrakeInput(float brakeVal);
    void SetSteeringInput(float steeringVal);
    
    

}