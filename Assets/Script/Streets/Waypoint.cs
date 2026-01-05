using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public enum WaypointType
    {
        PATHING,
        START,
        TWO_WAYPOINT,
        THREE_WAYPOINT
    }

    [Header("Waypoint Status")]
    public Waypoint NextWaypointA;//Will always refer to same side
    public Waypoint NextWaypointB;//Will always refer to opposite side
    public WaypointType waypointType;

    [UnityEngine.Range(0f,5f)]
    public float waypointWidth = 1.5f;

    public virtual Waypoint AdvancedNextWaypoint(){return null;}
    public virtual void IncreaseQueue(){}
    public virtual bool AwaitQueue(){return false;}

    public Waypoint GetNextWaypoint()
    {
        if(waypointType == WaypointType.PATHING || waypointType == WaypointType.START)
        {
            float swapChance = Random.Range(0f,1f);
            if(swapChance > 0.25)
            {
                if(NextWaypointB.waypointType == WaypointType.TWO_WAYPOINT || NextWaypointB.waypointType == WaypointType.THREE_WAYPOINT)
                {
                    if (!AwaitQueue())
                    {
                        return null;
                    }
                    else
                    {
                        NextWaypointB.IncreaseQueue();
                        return NextWaypointB;
                    }
                    
                }
                else
                {
                    return NextWaypointB;
                }
                
            }
            else
            {
                return NextWaypointA;
            }
        }
        else
        {
            return AdvancedNextWaypoint();
            
            
        }
    }

    public Vector3 GetRoughPosition()
    {
        Vector3 minBound = transform.position + transform.right * waypointWidth /2f;
        Vector3 maxBound = transform.position - transform.right * waypointWidth /2f;

        return Vector3.Lerp(minBound,maxBound, Random.Range(0f,1f));
    }

    public Vector3 GetExactPosition()
    {
        return transform.position;
    }

}
