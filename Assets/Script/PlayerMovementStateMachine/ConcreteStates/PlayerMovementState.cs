using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState
{

    protected Player player;
    protected PlayerMovementStateMachine playerMovementStateMachine;
    

    public PlayerMovementState(Player player, PlayerMovementStateMachine playerMovementStateMachine)
    {
        this.player = player;
        this.playerMovementStateMachine = playerMovementStateMachine;
    }
    
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void CollisionEvent(Collision col) { }
    public virtual void TriggerEvent(Collider col, bool isEntering) { }


}