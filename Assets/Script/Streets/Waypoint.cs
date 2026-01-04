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
