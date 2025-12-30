using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WARNING: Confirm This interface doesn't already exist before adding to unity!

public interface ICarMoveable
{
    //The public interface the state machine will be able to interact with
    //Variables and functions added here will be forced to implement in the base class
    


    

    //Acceleration Details
    AnimationCurve TorqueLookup {get; set;}


    //Suspension Details
    
    void UpdateTireCalcs(Transform Tire);
    void PerformSuspensionCalc(Transform Tire, RaycastHit TireHit);
    void PerformSteeringCalc(Transform Tire, RaycastHit TireHit);



    //Steering Details
    

}