using UnityEngine;

public class T_Waypoint : Waypoint
{
    public Waypoint NextWaypointC;
    int queue;

    void Start()
    {
        queue = 0;
    }

    public override bool AwaitQueue()
    {
        if(queue != 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void IncreaseQueue()
    {
        queue+=1;
    }

    public override Waypoint AdvancedNextWaypoint()
    {
        queue--;
        return NextWaypointC;

    }
}
