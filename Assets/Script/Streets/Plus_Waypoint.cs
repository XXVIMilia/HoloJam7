using UnityEngine;

public class Plus_Waypoint : Waypoint
{
    public Waypoint NextWaypointC;
    public Waypoint NextWaypointD;
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
        float diceRoll = Random.Range(0f, 1f);
        if(diceRoll > 0.1)
        {
            return NextWaypointC;
        }
        else
        {
            return NextWaypointD;
        }

    }
}
