using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDrivingState
{

    protected Car car;
    protected CarDrivingStateMachine carDrivingStateMachine;
    

    public CarDrivingState(Car car, CarDrivingStateMachine carDrivingStateMachine, Animator drivingAnimator)
    {
        this.car = car;
        this.carDrivingStateMachine = carDrivingStateMachine;//This Comment can be deleted
        this.drivingAnimator = drivingAnimator;
    }
    
    
    protected Animator drivingAnimator;
    protected string CurrentAnimation;
    public void ChangeAnimation(string AnimationKey) 
    {
        if (CurrentAnimation == AnimationKey)
            return;
        
        if(AnimationKey == "None")
        {
            CurrentAnimation = "None";
            return;
        }

        drivingAnimator.StopPlayback();
        drivingAnimator.Play(AnimationKey);
        CurrentAnimation = AnimationKey;
    }


    public virtual void AnimationTriggerEvent(Car.AnimationTriggerType triggerType) { }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void CollisionEvent(Collision col) { }
    public virtual void TriggerEvent(Collider col, bool isEntering) { }


}