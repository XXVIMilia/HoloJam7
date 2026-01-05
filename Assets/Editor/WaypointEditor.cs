using UnityEngine;
using UnityEditor;


[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(Waypoint waypoint,GizmoType gizmoType)
    {
        if(waypoint.waypointType == Waypoint.WaypointType.START)
        {
            if((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.red * 0.5f;
            }
        }
        else if(waypoint.waypointType == Waypoint.WaypointType.PATHING)
        {
            if((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.blue * 0.5f;
            }
        }
        else if(waypoint.waypointType == Waypoint.WaypointType.THREE_WAYPOINT)
        {
            if((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.orange;
            }
            else
            {
                Gizmos.color = Color.orange * 0.5f;
            }
        }
        else if(waypoint.waypointType == Waypoint.WaypointType.TWO_WAYPOINT)
        {
            if((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.yellow * 0.5f;
            }
        }
        

        Gizmos.DrawSphere(waypoint.transform.position, -0.5f );
        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + waypoint.transform.right * waypoint.waypointWidth /2f,waypoint.transform.position - waypoint.transform.right * waypoint.waypointWidth/2f); 

        //Need to add waypoint type check here as well
        Gizmos.color = Color.purple;
        if(waypoint.NextWaypointA != null)
        {
            Gizmos.DrawLine(waypoint.transform.position,waypoint.NextWaypointA.transform.position);
        }

        if(waypoint.NextWaypointB != null)
        {
            Gizmos.DrawLine(waypoint.transform.position,waypoint.NextWaypointB.transform.position);
        }

        if (waypoint.GetComponent<Plus_Waypoint>() != null)
        {
            Gizmos.DrawLine(waypoint.transform.position,waypoint.GetComponent<Plus_Waypoint>().NextWaypointC.transform.position);
            Gizmos.DrawLine(waypoint.transform.position,waypoint.GetComponent<Plus_Waypoint>().NextWaypointD.transform.position);
        }

        if (waypoint.GetComponent<T_Waypoint>() != null)
        {
            Gizmos.DrawLine(waypoint.transform.position,waypoint.GetComponent<T_Waypoint>().NextWaypointC.transform.position);
        }
    
    }
}
