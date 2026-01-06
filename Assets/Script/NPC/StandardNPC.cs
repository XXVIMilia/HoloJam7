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
            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,50f * Time.deltaTime);
            transform.Translate(Vector3.forward * 5f * Time.deltaTime);
            Debug.DrawRay(distanceToWaypoint.position,dirToDrive.normalized,Color.black);
        }
        

    }
    
}
