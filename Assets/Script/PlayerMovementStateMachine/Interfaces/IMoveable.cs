using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WARNING: Confirm This interface doesn't already exist before adding to unity!

public interface IMoveable
{
    //The public interface the state machine will be able to interact with
    //Variables and functions added here will be forced to implement in the base class
    

    //Example for a movable interface
    float MoveInput { get; set; }
    void SetMoveInput(float moveValue);

    //Example for a damageable interface
    //void Damage(int damageAmount);
    //void Die();
    //int MaxHealth { get; set; }
    //int CurrentHealth { get; set; }

}