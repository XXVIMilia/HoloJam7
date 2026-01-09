using System;
using Unity.Mathematics;
using UnityEngine;

public class StandardNPC : NPCBaseClass
{
    


    public override void DriveNPC()
    {
        base.DriveNPC();
        if(target != null)
        {
            Vector3 dirToDrive = target.GetExactPosition() - distanceToWaypoint.position;
            dirToDrive.y = 0f;
            Quaternion targetRotation =  Quaternion.LookRotation(dirToDrive);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,turnSpeed* Time.deltaTime);
            transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);
            Debug.DrawRay(distanceToWaypoint.position,dirToDrive.normalized,Color.black);
        }
        

    }
    
}
