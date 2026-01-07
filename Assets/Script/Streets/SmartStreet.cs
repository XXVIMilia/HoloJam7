using UnityEngine;



public class SmartStreet : MonoBehaviour
{
    public Waypoint[] Waypoints;


    void Start()
    {
        Waypoints = GetComponentsInChildren<Waypoint>();
        print("Waypoints found in " + name + ": " +  Waypoints.Length);
    }

    public Waypoint GetWaypoint()
    {
        if(Waypoints.Length > 0)
        {
            return Waypoints[Random.Range(0, Waypoints.Length)];
        }
        else
        {
            return null;
        }
    }






}
